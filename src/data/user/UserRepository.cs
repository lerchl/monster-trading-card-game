namespace data.user {

    internal class UserRepository {

        private static int id = 100;

        private List<User> users = new List<User>();

        public UserRepository() {
            users.Add(new User(0, "admin", "admin"));
            users.Add(new User(1, "test", "test"));
        }

        public User? findByUsername(string username) {
            return users.Find(user => user.username.Equals(username));
        }

        public User save(User user) {
            users.Add(user);
            user.Id = id++;
            return user;
        }
    }
}