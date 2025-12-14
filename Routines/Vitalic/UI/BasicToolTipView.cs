using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing.Text;

namespace VitalicRotation.UI
{
    public partial class BasicToolTipView : Form
    {
        // Extended window styles for click-through, tool window, no-activate
        private const int WS_EX_TOOLWINDOW = 0x00000080;
        private const int WS_EX_TRANSPARENT = 0x00000020;
        private const int WS_EX_NOACTIVATE = 0x08000000;
        // ====== Constantes "HTML-like" (v.zip) ======
        // <table width=175 class=htmltooltipbackground cellspacing=5 cellpadding=0>
        private const int TABLE_CONTENT_WIDTH = 175;  // largeur du contenu (px)
        private const int TABLE_CELLSPACING = 5;    // marge interne autour du contenu
        private const int TABLE_BORDER_PX = 1;    // bordure noire autour de la "table"
        private const int OUTER_MIN_W = 56;   // sécurité comme v.zip
        private const int OUTER_MIN_H = 32;   // sécurité comme v.zip
        private const int DEFAULT_OFFSET_Y = 20;   // décalage vertical lors du placement
        private const float TEXT_PT = 9f;   // police 9pt (comme v.zip)

        // ====== Win32 (no-activate + topmost) ======
        [DllImport("user32.dll")] private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll")] private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        private const uint SWP_NOSIZE = 0x0001, SWP_NOMOVE = 0x0002, SWP_NOACTIVATE = 0x0010;
        private const int SW_SHOWNOACTIVATE = 4;

        // ====== Animation ======
        public enum AnimMode { None, Fade, Slide, FadeAndSlide }
        private readonly Timer _timer = new Timer();
        private int _durationMs = 200;      // ~200ms par défaut (v.zip)
        private int _stepsRemaining;
        private double _opacityStep;
        private int _slideStep;
        private int _slideRemainder;
        private AnimMode _mode = AnimMode.Fade;

        // ====== Contenu ======
        private string _text = string.Empty;
        private Color _textColor = Color.Black;
        private bool _singleLineWithIcon; // compat API; ici non utilisé dans le rendu "HTML"
        private Image _icon;               // compat API; ici non utilisé dans le rendu "HTML"

        // Couleurs (fond / bord / texte)
        public Color BackFill = Color.White;
        public Color Border = Color.FromArgb(28, 28, 28);

        // Propriétés (C#5)
        public AnimMode Mode
        {
            get { return _mode; }
            set { _mode = value; }
        }
        public int DurationMs
        {
            get { return _durationMs; }
            set { _durationMs = Math.Max(1, value); }
        }

        public BasicToolTipView()
        {
            InitializeComponent();

            SetStyle(ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.SupportsTransparentBackColor, true);
            DoubleBuffered = true;
            BackColor = Color.White;
            ForeColor = _textColor;
            Opacity = 0.0;

            _timer.Interval = 13; // ~77 FPS
            _timer.Tick += OnTick;

            Hide(); // démarre masqué
        }

        // Ensure the tooltip never grabs focus and is click-through to avoid flicker
        protected override bool ShowWithoutActivation { get { return true; } }

        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= WS_EX_TOOLWINDOW | WS_EX_TRANSPARENT | WS_EX_NOACTIVATE;
                return cp;
            }
        }

        // ====== API "compat v.zip" ======

        // imethod_2(string): info (bleu)
        public void imethod_2(string string_0)
        {
            _singleLineWithIcon = true;
            _icon = null; // pas utilisé en mode "HTML"
            _textColor = Color.SteelBlue;
            SetTextAndResize(string_0);
        }

        // imethod_3(object): multi-ligne (noir)
        public void imethod_3(object object_1)
        {
            _singleLineWithIcon = false;
            _icon = null;
            _textColor = this.ForeColor;
            SetTextAndResize((object_1 != null) ? object_1.ToString() : string.Empty);
        }

        // imethod_4(Exception): erreur (rouge)
        public void imethod_4(Exception exception_0)
        {
            _singleLineWithIcon = true;
            _icon = null;
            _textColor = Color.Firebrick;
            SetTextAndResize((exception_0 != null && exception_0.Message != null) ? exception_0.Message : "Erreur");
        }

        // imethod_0(Point, Size): positionne et affiche
        public void imethod_0(Point point_0, Size size_0)
        {
            // calcule la taille finale (bord + cellspacing + contenu)
            Size outer = MeasureOuter();
            this.Size = outer;

            // place au-dessus et centré, avec corrections écran
            int x = point_0.X - outer.Width / 2;
            int y = point_0.Y - outer.Height + DEFAULT_OFFSET_Y;
            Rectangle wa = Screen.FromPoint(point_0).WorkingArea;
            x = Math.Max(wa.Left, Math.Min(x, wa.Right - outer.Width));
            y = Math.Max(wa.Top, Math.Min(y, wa.Bottom - outer.Height));
            this.Location = new Point(x, y);

            // affiche sans focus et topmost
            ShowWindow(this.Handle, SW_SHOWNOACTIVATE);
            SetWindowPos(this.Handle, HWND_TOPMOST, this.Left, this.Top, this.Width, this.Height, SWP_NOACTIVATE);

            StartAnimation();
        }

        // imethod_1(): cache
        public void imethod_1()
        {
            if (_stepsRemaining > 0) _timer.Stop();
            this.Hide();
            if (this.IsHandleCreated)
                SetWindowPos(this.Handle, HWND_TOPMOST, this.Left, this.Top, this.Width, this.Height,
                             SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE);
        }

        // ====== Mesure & mise en page ======
        private void SetTextAndResize(string text)
        {
            _text = text ?? string.Empty;
            this.Size = MeasureOuter();
            this.panel.Invalidate();
        }

        private Size MeasureOuter()
        {
            // Mesure du texte à largeur fixe (TABLE_CONTENT_WIDTH), sans padding supplémentaire (NoPadding)
            Size content = MeasureContentText(_text, TABLE_CONTENT_WIDTH);
            int w = content.Width + 2 * TABLE_CELLSPACING + 2 * TABLE_BORDER_PX;
            int h = content.Height + 2 * TABLE_CELLSPACING + 2 * TABLE_BORDER_PX;

            w = Math.Max(w, OUTER_MIN_W);
            h = Math.Max(h, OUTER_MIN_H);
            return new Size(w, h);
        }

        private Size MeasureContentText(string text, int contentWidth)
        {
            // Remplace \r\n par \n puis convertit en espace insécable + wordwrap équivalent
            string t = (text ?? string.Empty).Replace("\r\n", "\n").Replace("\r", "\n");
            // En HTML, les sauts de lignes deviendraient <br/> ; ici TextRenderer sait couper par mots avec WordBreak.
            TextFormatFlags flags = TextFormatFlags.WordBreak | TextFormatFlags.NoPadding;
            using (var f = new Font(this.Font.FontFamily, TEXT_PT, FontStyle.Regular, GraphicsUnit.Point))
            {
                Size sz = TextRenderer.MeasureText(t, f, new Size(contentWidth, int.MaxValue), flags);
                // NB: MeasureText renvoie déjà la hauteur multi-ligne; largeur = contentWidth dans la plupart des cas
                sz.Width = contentWidth;
                return sz;
            }
        }

        // ====== Rendu ======
        private void panel_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            g.Clear(BackFill);

            // Rectangle extérieur = bordure de la "table"
            Rectangle outer = new Rectangle(0, 0, panel.Width - 1, panel.Height - 1);
            using (var pen = new Pen(Border, TABLE_BORDER_PX))
                g.DrawRectangle(pen, outer);

            // Rectangle contenu = outer - (bordure + cellspacing)
            Rectangle content = Rectangle.Inflate(outer, -(TABLE_BORDER_PX + TABLE_CELLSPACING), -(TABLE_BORDER_PX + TABLE_CELLSPACING));

            // Texte (couleur = _textColor, police 9pt)
            TextFormatFlags flags = TextFormatFlags.WordBreak | TextFormatFlags.NoPadding | TextFormatFlags.Left | TextFormatFlags.Top;
            using (var f = new Font(this.Font.FontFamily, TEXT_PT, FontStyle.Regular, GraphicsUnit.Point))
            {
                TextRenderer.DrawText(g, (_text ?? string.Empty), f, content, _textColor, flags);
            }
        }

        // ====== Animation ======
        private void StartAnimation()
        {
            _stepsRemaining = Math.Max(1, _durationMs / Math.Max(1, _timer.Interval));

            if (_mode == AnimMode.Fade || _mode == AnimMode.FadeAndSlide)
            {
                this.Opacity = 0.0;
                _opacityStep = 1.0 / _stepsRemaining;
            }
            else
            {
                this.Opacity = 1.0;
                _opacityStep = 0.0;
            }

            if (_mode == AnimMode.Slide || _mode == AnimMode.FadeAndSlide)
            {
                // slide depuis le haut de l'écran jusqu'à Top actuel
                int finalTop = this.Top;
                _slideStep = (finalTop + this.Height) / _stepsRemaining;
                _slideRemainder = (finalTop + this.Height) % _stepsRemaining;
                this.Location = new Point(this.Left, -this.Height);
            }
            else
            {
                _slideStep = 0;
                _slideRemainder = 0;
            }

            _timer.Start();
        }

        private void OnTick(object sender, EventArgs e)
        {
            int targetTop = this.Top;
            if (_slideRemainder > 0)
            {
                targetTop += _slideRemainder;
                _slideRemainder = 0;
            }
            targetTop += _slideStep;
            this.Location = new Point(this.Left, targetTop);

            if (_opacityStep > 0.0)
                this.Opacity = Math.Min(1.0, this.Opacity + _opacityStep);

            if (--_stepsRemaining <= 0)
                _timer.Stop();
        }
    }
}
