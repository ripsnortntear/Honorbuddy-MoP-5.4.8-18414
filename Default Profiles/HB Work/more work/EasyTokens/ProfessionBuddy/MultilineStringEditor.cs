//------------------------------------------------------------------------------
// <copyright file="MultilineStringEditor.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------------


// Note: I ripped this from System.Design.dll because the dll isn't included in the .net client install.

using System.Security;
using System.Text;

namespace System.ComponentModel.Design
{
    using Microsoft.Win32;
    using System;
    using Collections;
    using ComponentModel;
    using System.Design;
    using Diagnostics;
    using Drawing;
    using Drawing.Design;
    using Globalization;
    using Runtime.InteropServices;
    using Security.Permissions;
    using Text;
    using Windows.Forms;
    using Windows.Forms.Design;

    public sealed class MultilineStringEditor : UITypeEditor
    {
        private MultilineStringEditorUI _editorUI;

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (provider != null)
            {
                IWindowsFormsEditorService editorService = (IWindowsFormsEditorService) provider.GetService(typeof (IWindowsFormsEditorService));
                if (editorService == null)
                {
                    return value;
                }
                if (this._editorUI == null)
                {
                    this._editorUI = new MultilineStringEditorUI();
                }
                this._editorUI.BeginEdit(editorService, value);
                editorService.DropDownControl(this._editorUI);
                object obj2 = this._editorUI.Value;
                if (this._editorUI.EndEdit())
                {
                    value = obj2;
                }
            }
            return value;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) { return UITypeEditorEditStyle.DropDown; }

        public override bool GetPaintValueSupported(ITypeDescriptorContext context) { return false; }

        #region Nested type: MultilineStringEditorUI

        private class MultilineStringEditorUI : RichTextBox
        {
            private const int _caretPadding = 3;
            private const int _workAreaPadding = 0x10;
            private readonly StringFormat _watermarkFormat;
            private bool _contentsResizedRaised;
            private bool _ctrlEnterPressed;
            private bool _editing;
            private IWindowsFormsEditorService _editorService;
            private bool _escapePressed;
            private Hashtable _fallbackFonts;
            private Size _minimumSize = Size.Empty;
            private SolidBrush _watermarkBrush;
            private Size _watermarkSize = Size.Empty;

            internal MultilineStringEditorUI()
            {
                this.InitializeComponent();
                this._watermarkFormat = new StringFormat();
                this._watermarkFormat.Alignment = StringAlignment.Center;
                this._watermarkFormat.LineAlignment = StringAlignment.Center;
                this._fallbackFonts = new Hashtable(2);
            }

            private Size ContentSize
            {
                get
                {
                    NativeMethods.RECT lpRect = new NativeMethods.RECT();
                    HandleRef hDC = new HandleRef(null, UnsafeNativeMethods.GetDC(NativeMethods.NullHandleRef));
                    HandleRef hObject = new HandleRef(null, this.Font.ToHfont());
                    HandleRef ref4 = new HandleRef(null, SafeNativeMethods.SelectObject(hDC, hObject));
                    try
                    {
                        SafeNativeMethods.DrawText(hDC, this.Text, this.Text.Length, ref lpRect, 0x400);
                    }
                    finally
                    {
                        NativeMethods.ExternalDeleteObject(hObject);
                        SafeNativeMethods.SelectObject(hDC, ref4);
                        UnsafeNativeMethods.ReleaseDC(NativeMethods.NullHandleRef, hDC);
                    }
                    return new Size((lpRect.right - lpRect.left) + 3, lpRect.bottom - lpRect.top);
                }
            }

            public override Font Font { get { return base.Font; } set { } }

            public override Size MinimumSize
            {
                get
                {
                    if (this._minimumSize == Size.Empty)
                    {
                        Rectangle workingArea = Screen.GetWorkingArea(this);
                        this._minimumSize = new Size((int) Math.Min(Math.Ceiling((this.WatermarkSize.Width*1.75)), (workingArea.Width/4)), Math.Min((this.Font.Height*10), (workingArea.Height/4)));
                    }
                    return this._minimumSize;
                }
            }

            private bool ShouldShowWatermark
            {
                get
                {
                    if (this.Text.Length != 0)
                    {
                        return false;
                    }
                    return (this.WatermarkSize.Width < base.ClientSize.Width);
                }
            }

            public override string Text
            {
                get
                {
                    if (!base.IsHandleCreated)
                    {
                        return "";
                    }
                    StringBuilder lpString = new StringBuilder(SafeNativeMethods.GetWindowTextLength(new HandleRef(this, base.Handle)) + 1);
                    UnsafeNativeMethods.GetWindowText(new HandleRef(this, base.Handle), lpString, lpString.Capacity);
                    if (!this._ctrlEnterPressed)
                    {
                        return lpString.ToString();
                    }
                    string str = lpString.ToString();
                    int startIndex = str.LastIndexOf("\r\n");
                    return str.Remove(startIndex, 2);
                }
                set { base.Text = value; }
            }

            internal object Value { get { return this.Text; } }

            private Brush WatermarkBrush
            {
                get
                {
                    if (this._watermarkBrush == null)
                    {
                        Color window = SystemColors.Window;
                        Color windowText = SystemColors.WindowText;
                        Color color = Color.FromArgb((short) ((windowText.R*0.3) + (window.R*0.7)), (short) ((windowText.G*0.3) + (window.G*0.7)), (short) ((windowText.B*0.3) + (window.B*0.7)));
                        this._watermarkBrush = new SolidBrush(color);
                    }
                    return this._watermarkBrush;
                }
            }

            private Size WatermarkSize
            {
                get
                {
                    if (this._watermarkSize == Size.Empty)
                    {
                        SizeF ef;
                        using (Graphics graphics = base.CreateGraphics())
                        {
                            ef = graphics.MeasureString("Press Enter to begin a new line.\nPress Ctrl+Enter to accept Text.", this.Font);
                        }
                        this._watermarkSize = new Size((int) Math.Ceiling(ef.Width), (int) Math.Ceiling(ef.Height));
                    }
                    return this._watermarkSize;
                }
            }

            internal void BeginEdit(IWindowsFormsEditorService editorService, object value)
            {
                this._editing = true;
                this._editorService = editorService;
                this._minimumSize = Size.Empty;
                this._watermarkSize = Size.Empty;
                this._escapePressed = false;
                this._ctrlEnterPressed = false;
                this.Text = (string) value;
            }

            [SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
            protected override object CreateRichEditOleCallback() { return new OleCallback(this); }

            protected override void Dispose(bool disposing)
            {
                if (disposing && (this._watermarkBrush != null))
                {
                    this._watermarkBrush.Dispose();
                    this._watermarkBrush = null;
                }
                base.Dispose(disposing);
            }

            internal bool EndEdit()
            {
                this._editing = false;
                this._editorService = null;
                this._ctrlEnterPressed = false;
                this.Text = null;
                return !this._escapePressed;
            }

            private void InitializeComponent()
            {
                base.RichTextShortcutsEnabled = false;
                base.WordWrap = false;
                base.BorderStyle = BorderStyle.None;
                this.Multiline = true;
                base.ScrollBars = RichTextBoxScrollBars.Both;
                base.DetectUrls = false;
            }

            protected override bool IsInputKey(Keys keyData) { return (((((keyData & Keys.KeyCode) == Keys.Enter) && this.Multiline) && ((keyData & Keys.Alt) == Keys.None)) || base.IsInputKey(keyData)); }

            protected override void OnContentsResized(ContentsResizedEventArgs e)
            {
                this._contentsResizedRaised = true;
                this.ResizeToContent();
                base.OnContentsResized(e);
            }

            protected override void OnKeyDown(KeyEventArgs e)
            {
                if (this.ShouldShowWatermark)
                {
                    base.Invalidate();
                }
                if ((e.Control && (e.KeyCode == Keys.Enter)) && (e.Modifiers == Keys.Control))
                {
                    this._editorService.CloseDropDown();
                    this._ctrlEnterPressed = true;
                }
            }

            protected override void OnTextChanged(EventArgs e)
            {
                if (!this._contentsResizedRaised)
                {
                    this.ResizeToContent();
                }
                this._contentsResizedRaised = false;
                base.OnTextChanged(e);
            }

            protected override void OnVisibleChanged(EventArgs e)
            {
                if (base.Visible)
                {
                    this.ProcessSurrogateFonts(0, this.Text.Length);
                    base.Select(this.Text.Length, 0);
                }
                this.ResizeToContent();
                base.OnVisibleChanged(e);
            }

            protected override bool ProcessDialogKey(Keys keyData)
            {
                if ((keyData & (Keys.Alt | Keys.Shift)) == Keys.None)
                {
                    Keys keys = keyData & Keys.KeyCode;
                    if ((keys == Keys.Escape) && ((keyData & Keys.Control) == Keys.None))
                    {
                        this._escapePressed = true;
                    }
                }
                return base.ProcessDialogKey(keyData);
            }

            public void ProcessSurrogateFonts(int start, int length)
            {
                string text = this.Text;
                if (text != null)
                {
                    int[] numArray = StringInfo.ParseCombiningCharacters(text);
                    if (numArray.Length != text.Length)
                    {
                        for (int i = 0; i < numArray.Length; i++)
                        {
                            if ((numArray[i] >= start) && (numArray[i] < (start + length)))
                            {
                                string str2 = null;
                                char ch = text[numArray[i]];
                                char ch2 = '\0';
                                if ((numArray[i] + 1) < text.Length)
                                {
                                    ch2 = text[numArray[i] + 1];
                                }
                                if (((ch >= 0xd800) && (ch <= 0xdbff)) && ((ch2 >= 0xdc00) && (ch2 <= 0xdfff)))
                                {
                                    int num2 = ((ch/'@') - 0x360) + 1;
                                    Font font = this._fallbackFonts[num2] as Font;
                                    if (font == null)
                                    {
                                        using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\LanguagePack\SurrogateFallback"))
                                        {
                                            if (key != null)
                                            {
                                                str2 = (string) key.GetValue("Plane" + num2);
                                                if (!string.IsNullOrEmpty(str2))
                                                {
                                                    font = new Font(str2, base.Font.Size, base.Font.Style);
                                                }
                                                this._fallbackFonts[num2] = font;
                                            }
                                        }
                                    }
                                    if (font != null)
                                    {
                                        int num3 = (i == (numArray.Length - 1)) ? (text.Length - numArray[i]) : (numArray[i + 1] - numArray[i]);
                                        base.Select(numArray[i], num3);
                                        base.SelectionFont = font;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            private void ResizeToContent()
            {
                if (base.Visible)
                {
                    Size contentSize = this.ContentSize;
                    contentSize.Width += SystemInformation.VerticalScrollBarWidth;
                    contentSize.Width = Math.Max(contentSize.Width, this.MinimumSize.Width);
                    Rectangle workingArea = Screen.GetWorkingArea(this);
                    int num = base.PointToScreen(base.Location).X - workingArea.Left;
                    int num2 = Math.Min(contentSize.Width - base.ClientSize.Width, num);
                    base.ClientSize = new Size(base.ClientSize.Width + num2, this.MinimumSize.Height);
                }
            }

            protected override void WndProc(ref Message m)
            {
                base.WndProc(ref m);
                if ((m.Msg == 15) && this.ShouldShowWatermark)
                {
                    using (Graphics graphics = base.CreateGraphics())
                    {
                        graphics.DrawString(
                            "Press Enter to begin a new line.\nPress Ctrl+Enter to accept Text.",
                            this.Font,
                            this.WatermarkBrush,
                            new RectangleF(0f, 0f, base.ClientSize.Width, base.ClientSize.Height),
                            this._watermarkFormat);
                    }
                }
            }
        }

        #endregion

        #region Nested type: OleCallback

        private class OleCallback : UnsafeNativeMethods.IRichTextBoxOleCallback
        {
            private static TraceSwitch richTextDbg;
            private RichTextBox owner;
            private bool unrestricted;

            internal OleCallback(RichTextBox owner) { this.owner = owner; }

            private static TraceSwitch RichTextDbg
            {
                get
                {
                    if (richTextDbg == null)
                    {
                        richTextDbg = new TraceSwitch("RichTextDbg", "Debug info about RichTextBox");
                    }
                    return richTextDbg;
                }
            }

            #region IRichTextBoxOleCallback Members

            public int ContextSensitiveHelp(int fEnterMode) { return -2147467263; }

            public int DeleteObject(IntPtr lpoleobj) { return 0; }

            public int GetClipboardData(NativeMethods.CHARRANGE lpchrg, int reco, IntPtr lplpdataobj) { return -2147467263; }

            public int GetContextMenu(short seltype, IntPtr lpoleobj, NativeMethods.CHARRANGE lpchrg, out IntPtr hmenu)
            {
                TextBox box = new TextBox
                                  {
                                      Visible = true
                                  };
                ContextMenu contextMenu = box.ContextMenu;
                if ((contextMenu == null) || !this.owner.ShortcutsEnabled)
                {
                    hmenu = IntPtr.Zero;
                }
                else
                {
                    hmenu = contextMenu.Handle;
                }
                return 0;
            }

            public int GetDragDropEffect(bool fDrag, int grfKeyState, ref int pdwEffect)
            {
                pdwEffect = 0;
                return 0;
            }

            public int GetInPlaceContext(IntPtr lplpFrame, IntPtr lplpDoc, IntPtr lpFrameInfo) { return -2147467263; }

            public int GetNewStorage(out UnsafeNativeMethods.IStorage storage)
            {
                UnsafeNativeMethods.ILockBytes iLockBytes = UnsafeNativeMethods.CreateILockBytesOnHGlobal(NativeMethods.NullHandleRef, true);
                storage = UnsafeNativeMethods.StgCreateDocfileOnILockBytes(iLockBytes, 0x1012, 0);
                return 0;
            }

            public int QueryAcceptData(Runtime.InteropServices.ComTypes.IDataObject lpdataobj, IntPtr lpcfFormat, int reco, int fReally, IntPtr hMetaPict)
            {
                if (reco != 0)
                {
                    return -2147467263;
                }
                DataObject obj2 = new DataObject(lpdataobj);
                if ((obj2 == null) || (!obj2.GetDataPresent(DataFormats.Text) && !obj2.GetDataPresent(DataFormats.UnicodeText)))
                {
                    return -2147467259;
                }
                return 0;
            }

            public int QueryInsertObject(ref Guid lpclsid, IntPtr lpstg, int cp)
            {
                if (!this.unrestricted)
                {
                    string str;
                    Guid pclsid = new Guid();
                    if (!NativeMethods.Succeeded(UnsafeNativeMethods.ReadClassStg(new HandleRef(null, lpstg), ref pclsid)))
                    {
                        return 1;
                    }
                    if (pclsid == Guid.Empty)
                    {
                        pclsid = lpclsid;
                    }
                    if (((str = pclsid.ToString().ToUpper(CultureInfo.InvariantCulture)) == null) ||
                        ((!(str == "00000315-0000-0000-C000-000000000046") && !(str == "00000316-0000-0000-C000-000000000046")) &&
                         (!(str == "00000319-0000-0000-C000-000000000046") && !(str == "0003000A-0000-0000-C000-000000000046"))))
                    {
                        return 1;
                    }
                }
                return 0;
            }

            public int ShowContainerUI(int fShow) { return 0; }

            #endregion
        }

        #endregion
    }
}

namespace System.Design
{
    using System;
    using Runtime.InteropServices;
    using Runtime.InteropServices.ComTypes;

    internal class NativeMethods
    {
        public const int DT_CALCRECT = 0x400;
        public static HandleRef NullHandleRef = new HandleRef(null, IntPtr.Zero);

        [DllImport("gdi32.dll", EntryPoint = "DeleteObject", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool ExternalDeleteObject(HandleRef hObject);

        public static bool Succeeded(int hr) { return (hr >= 0); }

        #region Nested type: CHARRANGE

        [StructLayout(LayoutKind.Sequential)]
        public class CHARRANGE
        {
            public int cpMin;
            public int cpMax;
        }

        #endregion

        #region Nested type: CommonHandles

        public sealed class CommonHandles
        {
            public static readonly int HDC = Internal.HandleCollector.RegisterType("HDC", 100, 2);
        }

        #endregion

        #region Nested type: FILETIME

        [StructLayout(LayoutKind.Sequential)]
        public class FILETIME
        {
            public int dwLowDateTime;
            public int dwHighDateTime;
        }

        #endregion

        #region Nested type: RECT

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;

            public RECT(int left, int top, int right, int bottom)
            {
                this.left = left;
                this.top = top;
                this.right = right;
                this.bottom = bottom;
            }
        }

        #endregion

        #region Nested type: STATSTG

        [StructLayout(LayoutKind.Sequential)]
        public class STATSTG
        {
            [MarshalAs(UnmanagedType.LPWStr)] public string pwcsName;
            public int type;
            [MarshalAs(UnmanagedType.I8)] public long cbSize;
            [MarshalAs(UnmanagedType.I8)] public long mtime;
            [MarshalAs(UnmanagedType.I8)] public long ctime;
            [MarshalAs(UnmanagedType.I8)] public long atime;
            [MarshalAs(UnmanagedType.I4)] public int grfMode;
            [MarshalAs(UnmanagedType.I4)] public int grfLocksSupported;
            public int clsid_data1;
            [MarshalAs(UnmanagedType.I2)] public short clsid_data2;
            [MarshalAs(UnmanagedType.I2)] public short clsid_data3;
            [MarshalAs(UnmanagedType.U1)] public byte clsid_b0;
            [MarshalAs(UnmanagedType.U1)] public byte clsid_b1;
            [MarshalAs(UnmanagedType.U1)] public byte clsid_b2;
            [MarshalAs(UnmanagedType.U1)] public byte clsid_b3;
            [MarshalAs(UnmanagedType.U1)] public byte clsid_b4;
            [MarshalAs(UnmanagedType.U1)] public byte clsid_b5;
            [MarshalAs(UnmanagedType.U1)] public byte clsid_b6;
            [MarshalAs(UnmanagedType.U1)] public byte clsid_b7;
            [MarshalAs(UnmanagedType.I4)] public int grfStateBits;
            [MarshalAs(UnmanagedType.I4)] public int reserved;
        }

        #endregion
    }

    [SuppressUnmanagedCodeSecurity]
    internal static class SafeNativeMethods
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int DrawText(HandleRef hDC, string lpszString, int nCount, ref NativeMethods.RECT lpRect, int nFormat);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowTextLength(HandleRef hWnd);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr SelectObject(HandleRef hDC, HandleRef hObject);
    }

    internal class UnsafeNativeMethods
    {
        [DllImport("ole32.dll", PreserveSig = false)]
        public static extern ILockBytes CreateILockBytesOnHGlobal(HandleRef hGlobal, bool fDeleteOnRelease);

        public static IntPtr GetDC(HandleRef hWnd) { return Internal.HandleCollector.Add(IntGetDC(hWnd), NativeMethods.CommonHandles.HDC); }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowText(HandleRef hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", EntryPoint = "GetDC", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr IntGetDC(HandleRef hWnd);

        [DllImport("user32.dll", EntryPoint = "ReleaseDC", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern int IntReleaseDC(HandleRef hWnd, HandleRef hDC);

        [DllImport("ole32.dll")]
        public static extern int ReadClassStg(HandleRef pStg, [In, Out] ref Guid pclsid);

        public static int ReleaseDC(HandleRef hWnd, HandleRef hDC)
        {
            Internal.HandleCollector.Remove((IntPtr) hDC, NativeMethods.CommonHandles.HDC);
            return IntReleaseDC(hWnd, hDC);
        }

        [DllImport("ole32.dll", PreserveSig = false)]
        public static extern IStorage StgCreateDocfileOnILockBytes(ILockBytes iLockBytes, int grfMode, int reserved);

        #region Nested type: ILockBytes

        [ComImport, Guid("0000000A-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface ILockBytes
        {
            void ReadAt([In, MarshalAs(UnmanagedType.U8)] long ulOffset, [Out] IntPtr pv, [In, MarshalAs(UnmanagedType.U4)] int cb, [Out, MarshalAs(UnmanagedType.LPArray)] int[] pcbRead);
            void WriteAt([In, MarshalAs(UnmanagedType.U8)] long ulOffset, IntPtr pv, [In, MarshalAs(UnmanagedType.U4)] int cb, [Out, MarshalAs(UnmanagedType.LPArray)] int[] pcbWritten);
            void Flush();
            void SetSize([In, MarshalAs(UnmanagedType.U8)] long cb);
            void LockRegion([In, MarshalAs(UnmanagedType.U8)] long libOffset, [In, MarshalAs(UnmanagedType.U8)] long cb, [In, MarshalAs(UnmanagedType.U4)] int dwLockType);
            void UnlockRegion([In, MarshalAs(UnmanagedType.U8)] long libOffset, [In, MarshalAs(UnmanagedType.U8)] long cb, [In, MarshalAs(UnmanagedType.U4)] int dwLockType);
            void Stat([Out] NativeMethods.STATSTG pstatstg, [In, MarshalAs(UnmanagedType.U4)] int grfStatFlag);
        }

        #endregion

        #region Nested type: IRichTextBoxOleCallback

        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("00020D03-0000-0000-C000-000000000046")]
        public interface IRichTextBoxOleCallback
        {
            [PreserveSig]
            int GetNewStorage(out IStorage ret);

            [PreserveSig]
            int GetInPlaceContext(IntPtr lplpFrame, IntPtr lplpDoc, IntPtr lpFrameInfo);

            [PreserveSig]
            int ShowContainerUI(int fShow);

            [PreserveSig]
            int QueryInsertObject(ref Guid lpclsid, IntPtr lpstg, int cp);

            [PreserveSig]
            int DeleteObject(IntPtr lpoleobj);

            [PreserveSig]
            int QueryAcceptData(IDataObject lpdataobj, IntPtr lpcfFormat, int reco, int fReally, IntPtr hMetaPict);

            [PreserveSig]
            int ContextSensitiveHelp(int fEnterMode);

            [PreserveSig]
            int GetClipboardData(NativeMethods.CHARRANGE lpchrg, int reco, IntPtr lplpdataobj);

            [PreserveSig]
            int GetDragDropEffect(bool fDrag, int grfKeyState, ref int pdwEffect);

            [PreserveSig]
            int GetContextMenu(short seltype, IntPtr lpoleobj, NativeMethods.CHARRANGE lpchrg, out IntPtr hmenu);
        }

        #endregion

        #region Nested type: IStorage

        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("0000000B-0000-0000-C000-000000000046")]
        public interface IStorage
        {
            [return: MarshalAs(UnmanagedType.Interface)]
            IStream CreateStream(
                [In, MarshalAs(UnmanagedType.BStr)] string pwcsName,
                [In, MarshalAs(UnmanagedType.U4)] int grfMode,
                [In, MarshalAs(UnmanagedType.U4)] int reserved1,
                [In, MarshalAs(UnmanagedType.U4)] int reserved2);

            [return: MarshalAs(UnmanagedType.Interface)]
            IStream OpenStream([In, MarshalAs(UnmanagedType.BStr)] string pwcsName, IntPtr reserved1, [In, MarshalAs(UnmanagedType.U4)] int grfMode, [In, MarshalAs(UnmanagedType.U4)] int reserved2);

            [return: MarshalAs(UnmanagedType.Interface)]
            IStorage CreateStorage(
                [In, MarshalAs(UnmanagedType.BStr)] string pwcsName,
                [In, MarshalAs(UnmanagedType.U4)] int grfMode,
                [In, MarshalAs(UnmanagedType.U4)] int reserved1,
                [In, MarshalAs(UnmanagedType.U4)] int reserved2);

            [return: MarshalAs(UnmanagedType.Interface)]
            IStorage OpenStorage(
                [In, MarshalAs(UnmanagedType.BStr)] string pwcsName,
                IntPtr pstgPriority,
                [In, MarshalAs(UnmanagedType.U4)] int grfMode,
                IntPtr snbExclude,
                [In, MarshalAs(UnmanagedType.U4)] int reserved);

            void CopyTo(int ciidExclude, [In, MarshalAs(UnmanagedType.LPArray)] Guid[] pIIDExclude, IntPtr snbExclude, [In, MarshalAs(UnmanagedType.Interface)] IStorage stgDest);

            void MoveElementTo(
                [In, MarshalAs(UnmanagedType.BStr)] string pwcsName,
                [In, MarshalAs(UnmanagedType.Interface)] IStorage stgDest,
                [In, MarshalAs(UnmanagedType.BStr)] string pwcsNewName,
                [In, MarshalAs(UnmanagedType.U4)] int grfFlags);

            void Commit(int grfCommitFlags);
            void Revert();
            void EnumElements([In, MarshalAs(UnmanagedType.U4)] int reserved1, IntPtr reserved2, [In, MarshalAs(UnmanagedType.U4)] int reserved3, [MarshalAs(UnmanagedType.Interface)] out object ppVal);
            void DestroyElement([In, MarshalAs(UnmanagedType.BStr)] string pwcsName);
            void RenameElement([In, MarshalAs(UnmanagedType.BStr)] string pwcsOldName, [In, MarshalAs(UnmanagedType.BStr)] string pwcsNewName);
            void SetElementTimes([In, MarshalAs(UnmanagedType.BStr)] string pwcsName, [In] NativeMethods.FILETIME pctime, [In] NativeMethods.FILETIME patime, [In] NativeMethods.FILETIME pmtime);
            void SetClass([In] ref Guid clsid);
            void SetStateBits(int grfStateBits, int grfMask);
            void Stat([Out] NativeMethods.STATSTG pStatStg, int grfStatFlag);
        }

        #endregion

        #region Nested type: IStream

        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("0000000C-0000-0000-C000-000000000046"), SuppressUnmanagedCodeSecurity]
        public interface IStream
        {
            int Read(IntPtr buf, int len);
            int Write(IntPtr buf, int len);

            [return: MarshalAs(UnmanagedType.I8)]
            long Seek([In, MarshalAs(UnmanagedType.I8)] long dlibMove, int dwOrigin);

            void SetSize([In, MarshalAs(UnmanagedType.I8)] long libNewSize);

            [return: MarshalAs(UnmanagedType.I8)]
            long CopyTo([In, MarshalAs(UnmanagedType.Interface)] IStream pstm, [In, MarshalAs(UnmanagedType.I8)] long cb, [Out, MarshalAs(UnmanagedType.LPArray)] long[] pcbRead);

            void Commit(int grfCommitFlags);
            void Revert();
            void LockRegion([In, MarshalAs(UnmanagedType.I8)] long libOffset, [In, MarshalAs(UnmanagedType.I8)] long cb, int dwLockType);
            void UnlockRegion([In, MarshalAs(UnmanagedType.I8)] long libOffset, [In, MarshalAs(UnmanagedType.I8)] long cb, int dwLockType);
            void Stat([Out] NativeMethods.STATSTG pStatstg, int grfStatFlag);

            [return: MarshalAs(UnmanagedType.Interface)]
            IStream Clone();
        }

        #endregion
    }
}

namespace System.Internal
{
    using System;
    using Threading;

    internal sealed class HandleCollector
    {
        private static int handleTypeCount;
        private static HandleType[] handleTypes;
        private static object internalSyncObject = new object();
        private static int suspendCount;

        internal static event HandleChangeEventHandler HandleAdded;

        internal static event HandleChangeEventHandler HandleRemoved;

        internal static IntPtr Add(IntPtr handle, int type)
        {
            handleTypes[type - 1].Add(handle);
            return handle;
        }

        internal static int RegisterType(string typeName, int expense, int initialThreshold)
        {
            lock (internalSyncObject)
            {
                if ((handleTypeCount == 0) || (handleTypeCount == handleTypes.Length))
                {
                    HandleType[] destinationArray = new HandleType[handleTypeCount + 10];
                    if (handleTypes != null)
                    {
                        Array.Copy(handleTypes, 0, destinationArray, 0, handleTypeCount);
                    }
                    handleTypes = destinationArray;
                }
                handleTypes[handleTypeCount++] = new HandleType(typeName, expense, initialThreshold);
                return handleTypeCount;
            }
        }

        internal static IntPtr Remove(IntPtr handle, int type) { return handleTypes[type - 1].Remove(handle); }

        internal static void ResumeCollect()
        {
            bool flag = false;
            lock (internalSyncObject)
            {
                if (suspendCount > 0)
                {
                    suspendCount--;
                }
                if (suspendCount == 0)
                {
                    for (int i = 0; i < handleTypeCount; i++)
                    {
                        lock (handleTypes[i])
                        {
                            if (handleTypes[i].NeedCollection())
                            {
                                flag = true;
                            }
                        }
                    }
                }
            }
            if (flag)
            {
                GC.Collect();
            }
        }

        internal static void SuspendCollect()
        {
            lock (internalSyncObject)
            {
                suspendCount++;
            }
        }

        #region Nested type: HandleChangeEventHandler

        internal delegate void HandleChangeEventHandler(string handleType, IntPtr handleValue, int currentHandleCount);

        #endregion

        #region Nested type: HandleType

        private class HandleType
        {
            private readonly int deltaPercent;
            internal readonly string name;
            private int handleCount;
            private int initialThreshHold;
            private int threshHold;

            internal HandleType(string name, int expense, int initialThreshHold)
            {
                this.name = name;
                this.initialThreshHold = initialThreshHold;
                this.threshHold = initialThreshHold;
                this.deltaPercent = 100 - expense;
            }

            internal void Add(IntPtr handle)
            {
                if (handle != IntPtr.Zero)
                {
                    bool flag = false;
                    int currentHandleCount = 0;
                    lock (this)
                    {
                        this.handleCount++;
                        flag = this.NeedCollection();
                        currentHandleCount = this.handleCount;
                    }
                    lock (internalSyncObject)
                    {
                        if (HandleAdded != null)
                        {
                            HandleAdded(this.name, handle, currentHandleCount);
                        }
                    }
                    if (flag && flag)
                    {
                        GC.Collect();
                        int millisecondsTimeout = (100 - this.deltaPercent)/4;
                        Thread.Sleep(millisecondsTimeout);
                    }
                }
            }

            internal int GetHandleCount()
            {
                lock (this)
                {
                    return this.handleCount;
                }
            }

            internal bool NeedCollection()
            {
                if (suspendCount <= 0)
                {
                    if (this.handleCount > this.threshHold)
                    {
                        this.threshHold = this.handleCount + ((this.handleCount*this.deltaPercent)/100);
                        return true;
                    }
                    int num = (100*this.threshHold)/(100 + this.deltaPercent);
                    if ((num >= this.initialThreshHold) && (this.handleCount < ((int) (num*0.9f))))
                    {
                        this.threshHold = num;
                    }
                }
                return false;
            }

            internal IntPtr Remove(IntPtr handle)
            {
                if (handle != IntPtr.Zero)
                {
                    int currentHandleCount = 0;
                    lock (this)
                    {
                        this.handleCount--;
                        if (this.handleCount < 0)
                        {
                            this.handleCount = 0;
                        }
                        currentHandleCount = this.handleCount;
                    }
                    lock (internalSyncObject)
                    {
                        if (HandleRemoved != null)
                        {
                            HandleRemoved(this.name, handle, currentHandleCount);
                        }
                    }
                }
                return handle;
            }
        }

        #endregion
    }
}