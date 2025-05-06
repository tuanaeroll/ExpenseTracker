namespace ExpenseTracker.Business.Logging.Interfaces
{
    public interface ILoggingService<T>
    {
        void LogInfo(string message, params object[] args);
        void LogWarning(string message, params object[] args);
        void LogError(Exception ex, string message, params object[] args);
    }
}
