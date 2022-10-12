namespace MonsterTradingCardGame.Data.Player {

    internal class Player : Entity {

        // TODO: make readonly again
        public string username;
        public readonly string password;

        // /////////////////////////////////////////////////////////////////////
        // Constructor
        // /////////////////////////////////////////////////////////////////////

        public Player(Guid id, string username, string password) : base(id) {
            this.username = username;
            this.password = password;
        }

        public Player(string username, string password) {
            this.username = username;
            this.password = password;
        }
    }
}
