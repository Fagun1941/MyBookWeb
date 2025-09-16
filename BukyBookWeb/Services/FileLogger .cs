using BukyBookWeb.IService;

namespace BukyBookWeb.Services
{
    public class CustomLogger : ICustomLogger
    {
        private readonly string _filePath = "Logs/custom-log.txt";

        public void LogInfo(string message)
        {
            WriteLog("INFO", message);
        }

        public void LogError(Exception ex, string message)
        {
            WriteLog("ERROR", $"{message} | Exception: {ex.Message}");
        }

        private void WriteLog(string level, string message)
        {
            Directory.CreateDirectory("Logs"); // Ensure folder exists

            string logLine = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level}] {message}";
            File.AppendAllText(_filePath, logLine + Environment.NewLine);
        }
    }
}
