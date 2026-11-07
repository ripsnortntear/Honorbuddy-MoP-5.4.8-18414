using System;
using Styx.CommonBot;
using Styx.CommonBot.Routines;

namespace Singular
{
    public enum SingularBotEvent
    {
        BotChanged,
        BotStart,
        BotStarted,
        BotStop,
        BotStopped
    }

    public class SingularBotEventArg : EventArgs
    {
        public readonly SingularBotEvent Event;

        public SingularBotEventArg(SingularBotEvent ev)
        {
            Event = ev;
        }
    }

    partial class SingularRoutine
    {
        /// <summary>
        /// Added to encapsulate any context validation (like are we current routine)
        /// .. at point of raising event rather than by every subscriber
        /// </summary>
        public static event EventHandler<SingularBotEventArg> OnBotEvent;

        private static bool _botEventSubscribed;
        private static Action<EventArgs> _botChangedHandler;
        private static Action<EventArgs> _botStartHandler;
        private static Action<EventArgs> _botStartedHandler;
        private static Action<EventArgs> _botStopHandler;
        private static Action<EventArgs> _botStoppedHandler;

        private void SingularBotEventInitialize()
        {
            if (_botEventSubscribed)
                return;

            _botEventSubscribed = true;
            _botChangedHandler = e => SingularRaiseBotEvent(SingularBotEvent.BotChanged);
            _botStartHandler = e => SingularRaiseBotEvent(SingularBotEvent.BotStart);
            _botStartedHandler = e => SingularRaiseBotEvent(SingularBotEvent.BotStarted);
            _botStopHandler = e => SingularRaiseBotEvent(SingularBotEvent.BotStop);
            _botStoppedHandler = e => SingularRaiseBotEvent(SingularBotEvent.BotStopped);

            BotEvents.OnBotChanged += _botChangedHandler;
            BotEvents.OnBotStart += _botStartHandler;
            BotEvents.OnBotStarted += _botStartedHandler;
            BotEvents.OnBotStop += _botStopHandler;
            BotEvents.OnBotStopped += _botStoppedHandler;
        }

        private void SingularBotEventShutdown()
        {
            if (!_botEventSubscribed)
                return;

            try
            {
                if (_botChangedHandler != null) BotEvents.OnBotChanged -= _botChangedHandler;
                if (_botStartHandler != null) BotEvents.OnBotStart -= _botStartHandler;
                if (_botStartedHandler != null) BotEvents.OnBotStarted -= _botStartedHandler;
                if (_botStopHandler != null) BotEvents.OnBotStop -= _botStopHandler;
                if (_botStoppedHandler != null) BotEvents.OnBotStopped -= _botStoppedHandler;
            }
            catch { }

            _botChangedHandler = _botStartHandler = _botStartedHandler = _botStopHandler = _botStoppedHandler = null;
            _botEventSubscribed = false;
        }

        public void SingularRaiseBotEvent(SingularBotEvent ev)
        {
            try
            {
                if (_botEventSubscribed && RoutineManager.Current != null && RoutineManager.Current.Name == Name && OnBotEvent != null)
                    OnBotEvent(this, new SingularBotEventArg(ev));
            }
            catch
            {
            }
        }
    }
}
