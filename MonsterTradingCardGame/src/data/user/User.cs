namespace MonsterTradingCardGame.Data.User {

    [Table(Name = "player")]
    public class User : Entity {

        public string Username { get; private set; }
        public string Password { get; private set; }
        public UserRole Role { get; set; }
        public int Money { get; set; }
        public string? Name { get; set; }
        public string? Bio { get; set; }
        public string? Image { get; set; }
        public int Elo { get; set; }

        // /////////////////////////////////////////////////////////////////////
        // Init
        // /////////////////////////////////////////////////////////////////////

        public User(Guid id, string username, string password, UserRole role, int money,
                string name, string bio, string image, int elo) : base(id) {
            Username = username;
            Password = password;
            Role = role;
            Money = money;
            Name = name;
            Bio = bio;
            Image = image;
            Elo = elo;
        }
    }
}
