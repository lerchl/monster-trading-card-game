using data.user;

namespace Api.Endpoints {

    internal class Users {

        private static readonly Logger<Users> _logger = new Logger<Users>();

        private static readonly UserRepository userRepository = new UserRepository();

        private const string URL = "/users";

        // /////////////////////////////////////////////////////////////////////
        // Methods
        // /////////////////////////////////////////////////////////////////////

        [ApiEndpoint(httpMethod = EHttpMethod.GET, url = URL)]
        public static void get() {
            // TODO
            Console.WriteLine("GET");
        }

        [ApiEndpoint(httpMethod = EHttpMethod.POST, url = URL)]
        public static void post(string Username, String Password) {
            User? user = userRepository.findByUsername(Username);
            if (user != null) {
                _logger.Info($"User {Username} tried to register but already exists");
            } else {
                userRepository.save(new User(Username, Password));
                _logger.Info($"User {Username} registered");
                // TODO: Answer that registration has been sucessful
            }
        }

        [ApiEndpoint(httpMethod = EHttpMethod.PUT, url = URL)]
        public static void put() {
            // TODO
            Console.WriteLine("PUT");
        }

        [ApiEndpoint(httpMethod = EHttpMethod.DELETE, url = URL)]
        public static void delete() {
            // TODO
            Console.WriteLine("DELETE");
        }
    }
}
