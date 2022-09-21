class Logger<T> {

    // /////////////////////////////////////////////////////////////////////////
    // Methods
    // /////////////////////////////////////////////////////////////////////////

    public void Info(string message) {
        Log("INFO", message);
    }

    public void Warn(string message) {
        Log("WARN", message);
    }

    private void Log(string level, string message) {
        Console.WriteLine($"[{level}] ({DateTime.Now.ToString("HH:mm:ss")}) {message} ::: {typeof(T).Name}");
    }
}
