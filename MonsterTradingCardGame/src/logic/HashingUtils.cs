using System.Security.Cryptography;
using System.Text;

namespace MonsterTradingCardGame.Logic {

    /// <summary>
    ///     Utility class for hashing
    /// </summary>
    public class HashingUtils {

        private static readonly SHA256 _sha256 = SHA256.Create();

        // /////////////////////////////////////////////////////////////////////
        // Methods
        // /////////////////////////////////////////////////////////////////////

        /// <summary>
        ///     Hashes a string with SHA256
        /// </summary>
        /// <param name="text">The string to hash</param>
        /// <returns>The hashed string</returns>
        public static string Sha256(string text) {
            return _sha256.ComputeHash(Encoding.ASCII.GetBytes(text))
                    .Select(b => b.ToString("X2"))
                    .Aggregate((a, b) => a + b);
        }
    }
}
