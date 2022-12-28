using MonsterTradingCardGame.Data.User;

namespace MonsterTradingCardGame.Server {

    public class Token {

        // /////////////////////////////////////////////////////////////////////
        // Properties
        // /////////////////////////////////////////////////////////////////////

        public Guid UserId { get; private set; }
        public string Username { get; private set; }
        public UserRole UserRole { get; private set; }
        public string Bearer { get; private set; }
        public DateTime ExpiryDate { get; private set; }

        // /////////////////////////////////////////////////////////////////////
        // Init
        // /////////////////////////////////////////////////////////////////////

        public Token(Guid userId, string username, UserRole userRole) {
            UserId = userId;
            Username = username;
            UserRole = userRole;
            Bearer = Guid.NewGuid().ToString();
            // Token expires after 30 minutes
            ExpiryDate = DateTime.Now.AddMinutes(30);
        }
    }
}
