using Newtonsoft.Json;

namespace MonsterTradingCardGame.Data.Player {

    internal class Player : Entity {

        public string Username { get; private set; }
        public string Password { get; set; }
        public int Money { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }
        public string Image { get; set; }

        // /////////////////////////////////////////////////////////////////////
        // Constructor
        // /////////////////////////////////////////////////////////////////////

        [JsonConstructor]
        public Player(Guid id, string username, string password, int money, string name, string bio, string image) : base(id) {
            Username = username;
            Password = password;
            Money = money;
            Name = name;
            Bio = bio;
            Image = image;
        }
    }
}
