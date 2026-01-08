using System;
using System.Windows.Forms;
using Styx.CommonBot;

namespace LogMeOut.Forms
{
    public partial class FrmTimer : Form
    {
        /// <summary>
        /// Détermine si le timer est en pause.
        /// </summary>
        private bool _blnIsInPause;
        /// <summary>
        /// Conserve le moment de la dernière mise à jour de l'heure du démarrage du bot.
        /// </summary>
        private DateTime _dtLastUpdatePause;

        public FrmTimer()
        {
            //Crée les contrôles
            InitializeComponent();
            //Bind l'événement lors de l'appui sur le bouton Start et Stop du bot
            BotEvents.OnBotStarted += BotEvents_OnBotStarted;
            BotEvents.OnBotStopped += BotEvents_OnBotStopped;
            //Vérifie que le plugin soit activé
            if (LogMeOut.Instance != null)
                timerRafraichissement.Enabled = true;
        }

        void BotEvents_OnBotStarted(EventArgs args)
        {
            //Démarre le timer
            timerRafraichissement.Start();
            //Active le bouton d'état
            btnChangeState.Enabled = true;
        }

        void BotEvents_OnBotStopped(EventArgs args)
        {
            //Arrête le timer
            timerRafraichissement.Stop();
            //Désactive le bouton d'état
            btnChangeState.Enabled = false;
            btnChangeState.Text = "Pause";
            _blnIsInPause = false;
        }

        private void TimerRafraichissementTick(object sender, EventArgs e)
        {
            //Si le timer est actif
            if (!_blnIsInPause)
            {
                // Si le plugin est désactivé
                if (!LogMeOut.Instance.IsEnabled())
                {
                    labTimer.Text = "--";
                    //Désactive le bouton pour mettre en pause/réactiver le timer
                    btnChangeState.Enabled = false;
                }
                //Rafraichît le timer si le trigger est actif
                else if (LogMeOutSettings.Instance.AlertOnTimeElapsed && TreeRoot.IsRunning && !LogMeOut.Instance.IsLoggingOut)
                {
                    //Détermine le temps restant
                    var tsRemaining = (LogMeOut.Instance.StartingBot.AddHours(LogMeOutSettings.Instance.HoursElapsed).AddMinutes(LogMeOutSettings.Instance.MinutesElapsed) - DateTime.Now);
                    labTimer.Text = tsRemaining.Hours.ToString("00") + ":" + tsRemaining.Minutes.ToString("00") + ":" + tsRemaining.Seconds.ToString("00");
                    //Active le bouton pour mettre en pause/réactiver le timer
                    btnChangeState.Enabled = true;
                }
                else if (LogMeOut.Instance.IsLoggingOut)
                {
                    labTimer.Text = "Now";
                    //Désactive le bouton pour mettre en pause/réactiver le timer
                    btnChangeState.Enabled = false;
                }
                else
                {
                    labTimer.Text = "00:00:00";
                    //Désactive le bouton pour mettre en pause/réactiver le timer
                    btnChangeState.Enabled = false;
                }
            }
            else
            {
                //Si le timer est en pause
                //Rajoute le temps écoulé depuis le dernier cycle de ce timer à l'heure de début du bot
                LogMeOut.Instance.StartingBot = LogMeOut.Instance.StartingBot.AddMilliseconds((DateTime.Now - _dtLastUpdatePause).TotalMilliseconds);
                //Met à jour l'heure du cycle
                _dtLastUpdatePause = DateTime.Now;
            }
        }

        private void BtnChangeStateClick(object sender, EventArgs e)
        {
            if (!_blnIsInPause)
            {
                //Stop le timer
                btnChangeState.Text = "Resume";
                _blnIsInPause = true;
                //Enregistre le moment où le bot a été arreté
                _dtLastUpdatePause = DateTime.Now;
                //Ecriture dans la console Log
                //Détermine le temps restant
                var tsRemaining = (LogMeOut.Instance.StartingBot.AddHours(LogMeOutSettings.Instance.HoursElapsed).AddMinutes(LogMeOutSettings.Instance.MinutesElapsed) - DateTime.Now);
                LogMeOut.WriteLog("Timer paused on " + tsRemaining.Hours.ToString("00") + ":" + tsRemaining.Minutes.ToString("00") + ":" + tsRemaining.Seconds.ToString("00"));
            }
            else
            {
                //Démarre le timer
                btnChangeState.Text = "Pause";
                _blnIsInPause = false;
                //Ecriture dans la console Log
                LogMeOut.WriteLog("Timer resumed");
            }
        }
    }
}
