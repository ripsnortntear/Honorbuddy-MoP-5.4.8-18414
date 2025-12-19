using System;
using System.Collections.Generic;
using System.Media;
using VitalicRotation.Settings;

namespace VitalicRotation.Helpers
{
    public enum AlertSound
    {
        Ready,
        Interrupt,
        Event,
        Alarm
    }

    public static class AudioBus
    {
        private static readonly Dictionary<string, SoundPlayer> _cache = new Dictionary<string, SoundPlayer>(StringComparer.OrdinalIgnoreCase);
        private static bool _initialized;

        public static void Initialize()
        {
            if (_initialized) return;
            _initialized = true;
        }

        public static void Shutdown()
        {
            foreach (var kv in _cache)
            {
                try { kv.Value.Stop(); } catch { }
            }
            _cache.Clear();
            _initialized = false;
        }

        private static string Map(AlertSound sound)
        {
            switch (sound)
            {
                case AlertSound.Ready:     return "ready.wav";
                case AlertSound.Interrupt: return "interrupt.wav";
                case AlertSound.Event:     return "event.wav";
                case AlertSound.Alarm:     return "bigwigs_alarm.wav";
                default:                   return null;
            }
        }

        public static void Play(AlertSound sound)
        {
            var name = Map(sound);
            if (!string.IsNullOrEmpty(name))
                Play(name);
        }

        public static void Play(string wavName)
        {
            if (!_initialized) Initialize();
            if (!VitalicSettings.Instance.SoundAlertsEnabled) return;
            try
            {
                SoundPlayer sp;
                if (!_cache.TryGetValue(wavName, out sp) || sp == null)
                {
                    var s = ResourceAccessor.GetAudio(wavName);
                    if (s == null)
                    {
                        try { if (VitalicSettings.Instance.DiagnosticMode) Logger.Write("[Diag][Audio] Missing resource: " + wavName); } catch { }
                        return;
                    }
                    sp = new SoundPlayer(s);
                    _cache[wavName] = sp;
                }
                // Ensure the stream is rewound before each Play when using a Stream source
                try { if (sp.Stream != null && sp.Stream.CanSeek) sp.Stream.Position = 0; } catch { }
                try { if (VitalicSettings.Instance.DiagnosticMode) Logger.Write("[Diag][Audio] Play '" + wavName + "'"); } catch { }
                sp.Play();
            }
            catch { }
        }

        public static void Notify(string message)
        {
            if (!VitalicSettings.Instance.SoundAlertsEnabled) return;
            Play("notify.wav");
            try { Logger.Write(message); } catch { }
        }

        public static void PlayInterrupt()
        {
            if (!VitalicSettings.Instance.SoundAlertsEnabled) return;
            Play(AlertSound.Interrupt);
        }

        public static void PlayEvent()
        {
            if (!VitalicSettings.Instance.SoundAlertsEnabled) return;
            Play(AlertSound.Event);
        }

        public static void PlayAlarm()
        {
            if (!VitalicSettings.Instance.SoundAlertsEnabled) return;
            Play(AlertSound.Alarm);
        }

        public static void PlayReady()
        {
            if (!VitalicSettings.Instance.SoundAlertsEnabled) return;
            Play(AlertSound.Ready);
        }
    }
}
