namespace data.user {

    internal class User {

        public int Id { get; set; }
        public readonly string username;
        public readonly string password;

        // /////////////////////////////////////////////////////////////////////
        // Constructor
        // /////////////////////////////////////////////////////////////////////
    
        public User(string username, string password) {
            this.username = username;
            this.password = password;
        }

        public User(int id, string username, string password) : this(username, password) {
            this.Id = id;
        }
    }
}
