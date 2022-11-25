namespace MonsterTradingCardGame.Server {

    internal class SessionHandler {

        private static readonly Logger<SessionHandler> _logger = new();

        private static SessionHandler? _instance;

        // /////////////////////////////////////////////////////////////////////
        // Properties
        // /////////////////////////////////////////////////////////////////////

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
        // Constructor
        // /////////////////////////////////////////////////////////////////////

        private SessionHandler() {
            Sessions = new Dictionary<string, Token>();
        }

        // /////////////////////////////////////////////////////////////////////
        // Methods
        // /////////////////////////////////////////////////////////////////////

        public Token CreateSession(string username) {
            Token token = new(username);
            Sessions.Add(token.Bearer, token);
            _logger.Info($"Created session for user {username}");
            return token;
        }

        public Token? GetSession(string bearer) {
            if (Sessions.ContainsKey(bearer)) {
                Token token = Sessions[bearer];
                if (token.ExpiryDate > DateTime.Now) {
                    return token;
                } else {
                    Sessions.Remove(bearer);
                    _logger.Info($"Session for user {token.Username} has expired");
                }
            }

            return null;
        }
    }
}