class Logger {

    public static Logger Instance {
        get {
            if (Instance == null) {
                Instance = new Logger();
            }
            return Instance;
        } private set {
            Instance = value;
        }
    }

    // /////////////////////////////////////////////////////////////////////////
    // Methods
    // /////////////////////////////////////////////////////////////////////////

    public void Info(string message) {
        Console.WriteLine($"[INFO] ({DateTime.Now.ToString("HH:mm:ss")}) {message}");
    }

    internal void Warn(string v) {
        throw new NotImplementedException();
    }
}
