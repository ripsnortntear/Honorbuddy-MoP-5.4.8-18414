using System.IO;
using Styx;
using Styx.Helpers;

namespace JarvisPlugin
{
    public class JarvisSettings : Settings
    {
        public static readonly JarvisSettings Instance = new JarvisSettings();

        public JarvisSettings()
            : base(GetPath())
        {
        }

        private static string GetPath()
        {
            string name = "Global";
            try { if (StyxWoW.Me != null && !string.IsNullOrEmpty(StyxWoW.Me.Name)) name = StyxWoW.Me.Name; } catch { }
            string baseDir = Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            return Path.Combine(baseDir, string.Format(@"Settings\Jarvis\Jarvis-{0}.xml", name));
        }

        // Master opt-in. When false, Jarvis never sends data externally.
        [Setting, DefaultValueAttribute(false)]
        public bool ExternalAiEnabled { get; set; }

        // On-demand Ask button in the Jarvis dialog (requires ExternalAiEnabled + valid endpoint/model).
        [Setting, DefaultValueAttribute(true)]
        public bool AllowAskButton { get; set; }

        // Periodic background suggestions (opt-in; requires ExternalAiEnabled).
        [Setting, DefaultValueAttribute(false)]
        public bool AllowPeriodicSuggestions { get; set; }

        [Setting, DefaultValueAttribute(15)]
        public int PeriodicMinutes { get; set; }

        // OpenAI-compatible chat completions endpoint.
        // Local examples: http://localhost:11434/v1/chat/completions (Ollama), http://localhost:1234/v1/chat/completions (LM Studio)
        // Cloud example: https://api.openai.com/v1/chat/completions
        [Setting, DefaultValueAttribute("http://localhost:11434/v1/chat/completions")]
        public string EndpointUrl { get; set; }

        [Setting, DefaultValueAttribute("")]
        public string ApiKey { get; set; }

        [Setting, DefaultValueAttribute("llama3.1")]
        public string Model { get; set; }

        [Setting, DefaultValueAttribute(30)]
        public int TimeoutSeconds { get; set; }

        [Setting, DefaultValueAttribute(0.2f)]
        public float Temperature { get; set; }

        [Setting, DefaultValueAttribute(500)]
        public int MaxTokens { get; set; }

        [Setting, DefaultValueAttribute(true)] public bool SendCharacter { get; set; }
        [Setting, DefaultValueAttribute(true)] public bool SendLocation { get; set; }
        [Setting, DefaultValueAttribute(true)] public bool SendProfessions { get; set; }
        [Setting, DefaultValueAttribute(true)] public bool SendInventorySummary { get; set; }
        [Setting, DefaultValueAttribute(true)] public bool SendBotProfile { get; set; }
        [Setting, DefaultValueAttribute(true)] public bool SendRecentLogSummary { get; set; }

        [Setting, DefaultValueAttribute("You are Jarvis, a helpful assistant for Honorbuddy (WoW MoP 5.4.8). Give concise, practical, safe suggestions. Do not suggest cheating, exploits, or unsafe automation changes. If data is missing, say so.")]
        public string SystemPrompt { get; set; }
    }
}
