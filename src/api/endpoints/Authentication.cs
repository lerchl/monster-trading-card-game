using data.user;

namespace Api.Endpoints {

    internal class Authentication {

        private static readonly UserRepository userRepository = new UserRepository();

        [ApiEndpoint(httpMethod = HttpMethod.POST, url = "/login")]
        public static void login(string username, string password) {
            User? user = userRepository.findByUsername(username);
            if (user == null) {
                Logger.Instance.Info($"Unknown user {username} tried to login");
            } else if (!user.password.Equals(password)) {
                Logger.Instance.Info($"User {username} tried to login with the wrong password");
            } else {
                Logger.Instance.Info($"User {username} logged in");
                // TODO: Answer with token for further authentication
            }
        }

        [ApiEndpoint(httpMethod = HttpMethod.POST, url = "/register")]
        public static void register(string username, string password) {
            User? user = userRepository.findByUsername(username);
            if (user != null) {
                Logger.Instance.Info($"User {username} tried to register but already exists");
            } else {
                userRepository.save(new User(username, password));
                Logger.Instance.Info($"User {username} registered");
                // TODO: Answer that registration has been sucessful
            }
        }
    }
}
