using data.user;

namespace Api.Endpoints {

    internal class Authentication {

        private static readonly Logger<Authentication> _logger = new Logger<Authentication>();

        private static readonly UserRepository userRepository = new UserRepository();

        // /////////////////////////////////////////////////////////////////////
        // Methods
        // /////////////////////////////////////////////////////////////////////

        [ApiEndpoint(httpMethod = EHttpMethod.POST, url = "/sessions")]
        public static void login(string Username, string Password) {
            User? user = userRepository.findByUsername(Username);
            if (user == null) {
                _logger.Info($"Unknown user {Username} tried to login");
            } else if (!user.password.Equals(Password)) {
                _logger.Info($"User {Username} tried to login with the wrong password");
            } else {
                _logger.Info($"User {Username} logged in");
                // TODO: Answer with token for further authentication
            }
        }
    }
}
