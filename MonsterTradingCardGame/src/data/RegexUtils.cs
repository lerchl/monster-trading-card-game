namespace MonsterTradingCardGame.Data {

    /// <summary>
    ///     Utility class for regular expressions.
    /// </summary>
    public static class RegexUtils {

        /// <summary>
        ///     Regular expression for a valid username.
        /// </summary>
        public const string Username = @"[a-zA-Z0-9]{3,30}";

        /// <summary>
        ///     Regular expression for a valid Guid.
        /// </summary>
        public const string Guid = @"[a-fA-F0-9]{8}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{12}";
    }
}
