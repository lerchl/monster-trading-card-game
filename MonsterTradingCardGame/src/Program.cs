using System.Diagnostics.CodeAnalysis;
using MonsterTradingCardGame.Server;

namespace MonsterTradingCardGame {

    /// <summary>
    ///     Entry point of the application.
    /// </summary>
    internal class Program {

        private static readonly Logger<Program> _logger = new();

        // /////////////////////////////////////////////////////////////////////
        // Methods
        // /////////////////////////////////////////////////////////////////////

        [SuppressMessage("csharp", "CA1806")]
        public static void Main() {
            try {
                new ServerSocket(10001);
            } catch (Exception e) {
                _logger.Error(e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}
