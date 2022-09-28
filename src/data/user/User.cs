namespace Data.User {

    internal class User : Entity {

        public readonly string username;
        public readonly string password;

        // /////////////////////////////////////////////////////////////////////
        // Constructor
        // /////////////////////////////////////////////////////////////////////
    
        public User(string username, string password) {
            this.username = username;
            this.password = password;
        }
    }
}
