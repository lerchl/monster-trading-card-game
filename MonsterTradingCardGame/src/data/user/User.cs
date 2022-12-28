namespace MonsterTradingCardGame.Data.User {

    [Table(Name = "player")]
    public class User : Entity {

        public string Username { get; private set; }
        public string Password { get; set; }
        public int Money { get; set; }
        public UserRole Role { get; set; }
        public string? Name { get; set; }
        public string? Bio { get; set; }
        public string? Image { get; set; }

        // /////////////////////////////////////////////////////////////////////
        // Constructor
        // /////////////////////////////////////////////////////////////////////

        public User(Guid id, string username, string password, int money, UserRole role,
                string name, string bio, string image) : base(id) {
            Username = username;
            Password = password;
            Money = money;
            Role = role;
            Name = name;
            Bio = bio;
            Image = image;
        }
    }
}
