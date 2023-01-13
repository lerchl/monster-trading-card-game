using System.Diagnostics.CodeAnalysis;

namespace MonsterTradingCardGame {

    internal class Logger<T> {

        // /////////////////////////////////////////////////////////////////////////
        // Methods
        // /////////////////////////////////////////////////////////////////////////

        public void Info(string message) {
            Log("INFO", message);
        }

        public void Warn(string message) {
            Log("WARN", message);
        }

        public void Error(string message) {
            Log("ERRO", message);
        }

        // want to force creation of logger with type
        [SuppressMessage("csharp", "CA1822")]
        private void Log(string level, string message) {
            Console.WriteLine($"[{level}] ({DateTime.Now:HH:mm:ss}) {message} ::: {typeof(T).Name}");
        }
    }
}
