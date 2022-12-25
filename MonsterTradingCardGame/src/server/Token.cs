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
            Bearer = "Bearer " + Guid.NewGuid().ToString();
            // Token expires after 30 minutes
            ExpiryDate = DateTime.Now.AddMinutes(30);
        }
    }
}
