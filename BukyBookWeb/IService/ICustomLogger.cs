namespace BukyBookWeb.IService
{
    public interface ICustomLogger
    {
        void LogInfo(string message);
        void LogError(Exception ex, string message);
    }
}
