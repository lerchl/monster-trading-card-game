namespace Data.User {

    internal class UserRepository {

        public User? FindByUsername(string username) {
            return users.Find(user => user.username.Equals(username));
        }

        public User Save(User user) {
            string insert = @"INSERT INTO users INSERT INTO player (id, username, password) VALUES (id:integer, 'username:text', 'password:text');";
            users.Add(user);
            user.Id = id++;
            return user;
        }
    }
}