using Newtonsoft.Json;

namespace MonsterTradingCardGame.Data.Player {

    internal class Player : Entity {

        public string Username { get; private set; }
        public string Password { get; private set; }
        public int Money { get; set; }

        // /////////////////////////////////////////////////////////////////////
        // Constructor
        // /////////////////////////////////////////////////////////////////////

        [JsonConstructor]
        public Player(Guid id, string username, string password, int money) : base(id) {
            Username = username;
            Password = password;
            Money = money;
        }
    }
}
