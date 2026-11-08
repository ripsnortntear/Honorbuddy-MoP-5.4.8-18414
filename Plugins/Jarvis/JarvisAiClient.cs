using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Styx.Common;

namespace JarvisPlugin
{
    internal static class JarvisAiClient
    {
        public sealed class AiResult
        {
            public bool Success;
            public string Message;
            public string Error;
        }

        public static void RequestAsync(JarvisSettings settings, string userQuestion, string contextSnapshot, Action<AiResult> onComplete)
        {
            if (onComplete == null) return;
            ThreadPool.QueueUserWorkItem(delegate
            {
                try
                {
                    if (settings == null || !settings.ExternalAiEnabled)
                    {
                        onComplete(new AiResult { Success = false, Error = "External AI is disabled (opt-in required)." });
                        return;
                    }
                    string endpoint = (settings.EndpointUrl ?? "").Trim();
                    string model = (settings.Model ?? "").Trim();
                    if (string.IsNullOrEmpty(endpoint) || string.IsNullOrEmpty(model))
                    {
                        onComplete(new AiResult { Success = false, Error = "Jarvis AI endpoint/model is not configured." });
                        return;
                    }

                    string system = settings.SystemPrompt ?? "";
                    string user = BuildUserMessage(userQuestion, contextSnapshot);
                    string body = BuildChatCompletionsJson(model, system, user, settings.Temperature, Math.Max(64, settings.MaxTokens));
                    string response = PostJson(endpoint, settings.ApiKey, body, Math.Max(5, settings.TimeoutSeconds));
                    string content = ExtractAssistantContent(response);
                    if (string.IsNullOrEmpty(content))
                        onComplete(new AiResult { Success = false, Error = "AI response contained no assistant content.", Message = Truncate(response, 500) });
                    else
                        onComplete(new AiResult { Success = true, Message = content.Trim() });
                }
                catch (Exception ex)
                {
                    Logging.WriteDiagnostic("[Jarvis] AI request failed: " + ex);
                    onComplete(new AiResult { Success = false, Error = ex.Message });
                }
            });
        }

        private static string BuildUserMessage(string question, string context)
        {
            var sb = new StringBuilder();
            if (!string.IsNullOrEmpty(context))
            {
                sb.AppendLine("Context (local Honorbuddy/Jarvis snapshot):");
                sb.AppendLine(context);
                sb.AppendLine();
            }
            sb.AppendLine("User request:");
            sb.AppendLine(string.IsNullOrWhiteSpace(question) ? "Give 3-5 practical suggestions for what I should do next." : question.Trim());
            return sb.ToString();
        }

        private static string BuildChatCompletionsJson(string model, string system, string user, float temperature, int maxTokens)
        {
            // Minimal JSON builder (no external dependency). Escape strings safely.
            return "{"
                + "\"model\":" + JsonString(model) + ","
                + "\"temperature\":" + temperature.ToString(System.Globalization.CultureInfo.InvariantCulture) + ","
                + "\"max_tokens\":" + maxTokens + ","
                + "\"messages\":["
                + "{\"role\":\"system\",\"content\":" + JsonString(system) + "},"
                + "{\"role\":\"user\",\"content\":" + JsonString(user) + "}"
                + "]"
                + "}";
        }

        private static string JsonString(string s)
        {
            if (s == null) return "null";
            var sb = new StringBuilder(s.Length + 16);
            sb.Append('"');
            foreach (char c in s)
            {
                switch (c)
                {
                    case '\\': sb.Append("\\\\"); break;
                    case '"': sb.Append("\\\""); break;
                    case '\r': break;
                    case '\n': sb.Append("\\n"); break;
                    case '\t': sb.Append("\\t"); break;
                    default:
                        if (c < 32) sb.Append("\\u").Append(((int)c).ToString("x4"));
                        else sb.Append(c);
                        break;
                }
            }
            sb.Append('"');
            return sb.ToString();
        }

        private static string PostJson(string url, string apiKey, string json, int timeoutSeconds)
        {
            var req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/json";
            req.Accept = "application/json";
            req.Timeout = timeoutSeconds * 1000;
            req.ReadWriteTimeout = timeoutSeconds * 1000;
            try { req.ServicePoint.Expect100Continue = false; } catch { }
            if (!string.IsNullOrEmpty(apiKey))
                req.Headers[HttpRequestHeader.Authorization] = "Bearer " + apiKey.Trim();

            byte[] bytes = Encoding.UTF8.GetBytes(json ?? "{}");
            req.ContentLength = bytes.Length;
            using (Stream reqStream = req.GetRequestStream())
                reqStream.Write(bytes, 0, bytes.Length);

            using (var resp = (HttpWebResponse)req.GetResponse())
            using (Stream s = resp.GetResponseStream())
            using (var reader = new StreamReader(s ?? Stream.Null, Encoding.UTF8))
                return reader.ReadToEnd();
        }

        private static string ExtractAssistantContent(string json)
        {
            if (string.IsNullOrEmpty(json)) return null;
            // OpenAI-compatible: choices[0].message.content (string). Minimal extraction without JSON lib dependency.
            // Try to find "content":"..." after "message" heuristically.
            int msg = json.IndexOf("\"message\"", StringComparison.OrdinalIgnoreCase);
            int searchFrom = msg >= 0 ? msg : 0;
            int cidx = json.IndexOf("\"content\"", searchFrom, StringComparison.OrdinalIgnoreCase);
            if (cidx < 0) cidx = json.IndexOf("\"content\"", StringComparison.OrdinalIgnoreCase);
            if (cidx < 0) return null;
            int colon = json.IndexOf(':', cidx);
            if (colon < 0) return null;
            int i = colon + 1;
            while (i < json.Length && char.IsWhiteSpace(json[i])) i++;
            if (i >= json.Length) return null;
            if (json[i] == '"') return DecodeJsonString(json, i);
            // content may be null or array in some APIs; fallback
            return null;
        }

        private static string DecodeJsonString(string json, int quoteIndex)
        {
            var sb = new StringBuilder();
            for (int i = quoteIndex + 1; i < json.Length; i++)
            {
                char c = json[i];
                if (c == '"') break;
                if (c == '\\' && i + 1 < json.Length)
                {
                    char n = json[++i];
                    switch (n)
                    {
                        case '"': sb.Append('"'); break;
                        case '\\': sb.Append('\\'); break;
                        case '/': sb.Append('/'); break;
                        case 'n': sb.Append('\n'); break;
                        case 'r': sb.Append('\r'); break;
                        case 't': sb.Append('\t'); break;
                        case 'u':
                            if (i + 4 < json.Length)
                            {
                                string hex = json.Substring(i + 1, 4);
                                ushort cp;
                                if (ushort.TryParse(hex, System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out cp))
                                    sb.Append((char)cp);
                                i += 4;
                            }
                            break;
                        default: sb.Append(n); break;
                    }
                }
                else sb.Append(c);
            }
            return sb.ToString();
        }

        private static string Truncate(string s, int max)
        {
            if (string.IsNullOrEmpty(s) || s.Length <= max) return s ?? "";
            return s.Substring(0, max) + "...";
        }
    }
}
