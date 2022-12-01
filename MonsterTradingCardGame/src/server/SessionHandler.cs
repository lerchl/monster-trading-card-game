namespace MonsterTradingCardGame.Server {

    /// <summary
    ///     Singleton for handling sessions.
    /// </summary>
    internal class SessionHandler {

        private static readonly Logger<SessionHandler> _logger = new();

        private static SessionHandler? _instance;

        // /////////////////////////////////////////////////////////////////////
        // Properties
        // /////////////////////////////////////////////////////////////////////

        /// <summary>
        ///     Singleton instance.
        ///     This is how you want to access the SessionHandler.
        /// </summary>
        public static SessionHandler Instance {
            get {
                _instance ??= new SessionHandler();
                return _instance;
            }
        }

        /// <summary>
        ///     A list of all active sessions
        ///     The key is the bearer token
        ///     The value is the token object
        /// </summary>
        public Dictionary<string, Token> Sessions { get; private set; }

        // /////////////////////////////////////////////////////////////////////
        // Init
        // /////////////////////////////////////////////////////////////////////

        private SessionHandler() {
            Sessions = new Dictionary<string, Token>();
        }

        // /////////////////////////////////////////////////////////////////////
        // Methods
        // /////////////////////////////////////////////////////////////////////

        /// <summary>
        ///     Creates a new session for a player
        /// </summary>
        /// <param name="playerId">The id of the player</param>
        /// <param name="username">The username of the player</param>
        /// <returns>The token linked to the session</returns>
        public Token CreateSession(Guid playerId, string username) {
            Token token = new(playerId, username);
            Sessions.Add(token.Bearer, token);
            _logger.Info($"Created session for user {username}");
            return token;
        }

        /// <summary>
        ///     Gets the token for a bearer.
        /// </summary>
        /// <param name="bearer">The bearer</param>
        /// <returns>
        ///     The token, if it exists and has not expired, null otherwise.
        /// </returns>
        public Token? GetSession(string bearer) {
            if (Sessions.ContainsKey(bearer)) {
                Token token = Sessions[bearer];
                if (token.ExpiryDate > DateTime.Now) {
                    return token;
                }

                Sessions.Remove(bearer);
                _logger.Info($"Session for user {token.Username} has expired");
            }

            return null;
        }
    }
}
