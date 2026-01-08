using System.Windows.Forms;

namespace TakeControl.Gui
{
    public partial class Settings : Form
    {
        /// <summary>
        /// State if the user is binding a key.
        /// </summary>
        private bool _isBinding;
        
        public Settings()
        {
            // Load components
            InitializeComponent();
            // Write the version number
            labVersion.Text += TakeControl.StaticVersion.Major + "." + TakeControl.StaticVersion.Minor;
            // Load the data
            LoadForm();
        }

        #region General
        /// <summary>
        /// The texte to write on buttons where the user is binding a key.
        /// </summary>
        private string BindingText
        {
            get { return "Press on key or click to unbind..."; }
        }
        /// <summary>
        /// The text to write on buttons where there is no bind.
        /// </summary>
        private string NoBindingText
        {
            get { return "Press to bind"; }
        }
        /// <summary>
        /// Load the form with data.
        /// </summary>
        private void LoadForm()
        {
            // Blacklist all objects
            btnBlAllObjects.Text = NoBindingText;
            if (!string.IsNullOrEmpty(TakeControlSettings.Get(s => s.KeyBlObjects)))
            {
                btnBlAllObjects.Text = string.Format("Key bound: {0}", TakeControlSettings.Get(s => s.KeyBlObjects));
            }
            numBlAllObjectsRadius.Value = TakeControlSettings.Get(s => s.BlAllObjectsRadius);
            numBlAllObjectsTime.Value = TakeControlSettings.Get(s => s.BlAllObjectsTime);

            // Blacklist current target
            btnBlTarget.Text = NoBindingText;
            if (!string.IsNullOrEmpty(TakeControlSettings.Get(s => s.KeyBlTarget)))
            {
                btnBlTarget.Text = string.Format("Key bound: {0}", TakeControlSettings.Get(s => s.KeyBlTarget));
            }
            numBlTargetTime.Value = TakeControlSettings.Get(s => s.BlTargetTime);

            // HonorBuddy pause
            btnHbPause.Text = NoBindingText;
            if (!string.IsNullOrEmpty(TakeControlSettings.Get(s => s.KeyPause)))
            {
                btnHbPause.Text = string.Format("Key bound: {0}", TakeControlSettings.Get(s => s.KeyPause));
            }

            // HonorBuddy start
            btnHbStartOrStop.Text = NoBindingText;
            if (!string.IsNullOrEmpty(TakeControlSettings.Get(s => s.KeyStartOrStop)))
            {
                btnHbStartOrStop.Text = string.Format("Key bound: {0}", TakeControlSettings.Get(s => s.KeyStartOrStop));
            }

            // HonorBuddy restart
            btnHbRestart.Text = NoBindingText;
            if (!string.IsNullOrEmpty(TakeControlSettings.Get(s => s.KeyRestart)))
            {
                btnHbRestart.Text = string.Format("Key bound: {0}", TakeControlSettings.Get(s => s.KeyRestart));
            }
        }
        #endregion

        #region BlackList all objects
        private void BtnBlAllObjectsClick(object sender, System.EventArgs e)
        {
            _isBinding = !_isBinding;
            if (_isBinding)
            {
                LoadForm();
                btnBlAllObjects.Text = BindingText;
            }
            else
            {
                TakeControlSettings.Do(s => s.KeyBlObjects = string.Empty);
                LoadForm();
                Logic.ApplyBinding();
            }
            
        }

        private void BtnBlAllObjectsKeyDown(object sender, KeyEventArgs e)
        {
            if (_isBinding)
            {
                _isBinding = false;
                TakeControlSettings.Do(s => s.KeyBlObjects = e.KeyData.ToString());
                Logic.ApplyBinding();
                LoadForm();
            }
        }
        private void NumBlAllObjectsRadiusValueChanged(object sender, System.EventArgs e)
        {
            TakeControlSettings.Do(s => s.BlAllObjectsRadius = (int)numBlAllObjectsRadius.Value);
        }

        private void NumBlAllObjectsTimeValueChanged(object sender, System.EventArgs e)
        {
            TakeControlSettings.Do(s => s.BlAllObjectsTime = (int)numBlAllObjectsTime.Value);
        }
        #endregion

        #region Blacklist current target
        private void BtnBlTargetClick(object sender, System.EventArgs e)
        {
            _isBinding = !_isBinding;
            if (_isBinding)
            {
                LoadForm();
                btnBlTarget.Text = BindingText;
            }
            else
            {
                TakeControlSettings.Do(s => s.KeyBlTarget = string.Empty);
                LoadForm();
                Logic.ApplyBinding();
            }
        }
        private void BtnBlTargetKeyDown(object sender, KeyEventArgs e)
        {
            if (_isBinding)
            {
                _isBinding = false;
                TakeControlSettings.Do(s => s.KeyBlTarget = e.KeyData.ToString());
                Logic.ApplyBinding();
                LoadForm();
            }
        }
        private void NumBlTargetTimeValueChanged(object sender, System.EventArgs e)
        {
            TakeControlSettings.Do(s => s.BlTargetTime = (int)numBlTargetTime.Value);
        }
        #endregion

        #region HonorBuddy Pause
        private void BtnHbControlBindingClick(object sender, System.EventArgs e)
        {
            _isBinding = !_isBinding;
            if (_isBinding)
            {
                LoadForm();
                btnHbPause.Text = BindingText;
            }
            else
            {
                TakeControlSettings.Do(s => s.KeyPause = string.Empty);
                LoadForm();
                Logic.ApplyBinding();
            }
        }

        private void BtnHbControlBindingKeyDown(object sender, KeyEventArgs e)
        {
            if (_isBinding)
            {
                _isBinding = false;
                TakeControlSettings.Do(s => s.KeyPause = e.KeyData.ToString());
                Logic.ApplyBinding();
                LoadForm();
            }
        }
        #endregion

        #region HonorBuddy Start/Stop
        private void BtnHbStartOrStopClick(object sender, System.EventArgs e)
        {
            _isBinding = !_isBinding;
            if (_isBinding)
            {
                LoadForm();
                btnHbStartOrStop.Text = BindingText;
            }
            else
            {
                TakeControlSettings.Do(s => s.KeyStartOrStop = string.Empty);
                LoadForm();
                Logic.ApplyBinding();
            }
        }
        private void BtnHbStartOrStopKeyDown(object sender, KeyEventArgs e)
        {
            if (_isBinding)
            {
                _isBinding = false;
                TakeControlSettings.Do(s => s.KeyStartOrStop = e.KeyData.ToString());
                Logic.ApplyBinding();
                LoadForm();
            }
        }
        #endregion

        #region HonorBuddy Restart
        private void BtnHbRestartClick(object sender, System.EventArgs e)
        {
            _isBinding = !_isBinding;
            if (_isBinding)
            {
                LoadForm();
                btnHbRestart.Text = BindingText;
            }
            else
            {
                TakeControlSettings.Do(s => s.KeyRestart = string.Empty);
                LoadForm();
                Logic.ApplyBinding();
            }
        }
        private void BtnHbRestartKeyDown(object sender, KeyEventArgs e)
        {
            if (_isBinding)
            {
                _isBinding = false;
                TakeControlSettings.Do(s => s.KeyRestart = e.KeyData.ToString());
                Logic.ApplyBinding();
                LoadForm();
            }
        }
        #endregion
    }
}