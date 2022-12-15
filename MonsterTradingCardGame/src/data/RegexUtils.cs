using System.Text.RegularExpressions;

namespace MonsterTradingCardGame.Data {

    /// <summary>
    ///     Utility class for regular expressions.
    /// </summary>
    public static class RegexUtils {

        /// <summary>
        ///     Regular expression for a valid username.
        /// </summary>
        public const string Username = @"^[a-zA-Z0-9]{3,30}$";
    }
}
