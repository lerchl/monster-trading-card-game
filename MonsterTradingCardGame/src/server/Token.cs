namespace MonsterTradingCardGame.Server {

    public class Token {

        // /////////////////////////////////////////////////////////////////////
        // Properties
        // /////////////////////////////////////////////////////////////////////

        public Guid PlayerId { get; private set; }
        public string Username { get; private set; }
        public string Bearer { get; private set; }
        public DateTime ExpiryDate { get; private set; }

        // /////////////////////////////////////////////////////////////////////
        // Init
        // /////////////////////////////////////////////////////////////////////

        public Token(Guid playerId, string username) {
            PlayerId = playerId;
            Username = username;
            // Bearer just consists of username and "-mtcgToken" appended.
            // This is not a secure way of generating a bearer token.
            Bearer = "Bearer " + username + "-mtcgToken";
            // Token expires after 30 minutes
            ExpiryDate = DateTime.Now.AddMinutes(30);
        }
    }
}
