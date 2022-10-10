namespace MonsterTradingCardGame.Data.Player {

    internal class Player : Entity {

        public readonly string username;
        public readonly string password;

        // /////////////////////////////////////////////////////////////////////
        // Constructor
        // /////////////////////////////////////////////////////////////////////
    
        public Player(string username, string password) {
            this.username = username;
            this.password = password;
        }
    }
}
