namespace MonsterTradingCardGame.Server {

    internal class Token {

        // /////////////////////////////////////////////////////////////////////
        // Properties
        // /////////////////////////////////////////////////////////////////////

        public string Username { get; private set; }
        public string Bearer { get; private set; }
        public DateTime ExpiryDate { get; private set; }

        // /////////////////////////////////////////////////////////////////////
        // Constructor
        // /////////////////////////////////////////////////////////////////////

        public Token(string username) {
            Username = username;
            Bearer = new Guid().ToString();
            // Token expires after 30 minutes
            ExpiryDate = DateTime.Now.AddMinutes(30);
        }
    }
}
