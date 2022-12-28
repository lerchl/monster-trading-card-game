using MonsterTradingCardGame;
using MonsterTradingCardGame.Api;
using MonsterTradingCardGame.Data;
using MonsterTradingCardGame.Data.User;
using MonsterTradingCardGame.Server;

namespace Api.Endpoints {

    internal class Authentication {

        private static readonly Logger<Authentication> _logger = new();

        private static readonly UserRepository _userRepository = new();

        // /////////////////////////////////////////////////////////////////////
        // Methods
        // /////////////////////////////////////////////////////////////////////

        [ApiEndpoint(HttpMethod = EHttpMethod.POST, Url = "^/sessions$", RequiresAuthentication = false)]
        public static Response Login([Body] User user) {
            string username = user.Username;

            User dbPlayer;
            try {
                dbPlayer = _userRepository.FindByUsername(username);
            } catch (NoResultException) {
                _logger.Info($"Unknown user {username} tried to login");
                return new Response(HttpCode.UNAUTHORIZED_401, "{message: \"username or password wrong\"}");
            }

            if (!dbPlayer.Password.Equals(user.Password)) {
                _logger.Info($"User {username} tried to login with the wrong password");
                return new Response(HttpCode.UNAUTHORIZED_401, "{message: \"username or password wrong\"}");
            } else {
                _logger.Info($"User {username} has logged in");
                Token token = SessionHandler.Instance.CreateSession(dbPlayer.Id, dbPlayer.Username, dbPlayer.Role);
                return new Response(HttpCode.OK_200, $"{{\"token\": \"{token.Bearer}\"}}");
            }
        }

        [ApiEndpoint(HttpMethod = EHttpMethod.POST, Url = "^/users$", RequiresAuthentication = false)]
        public static Response Register([Body] User player) {
            string username = player.Username;

            try {
                _userRepository.FindByUsername(username);
            } catch (NoResultException) {
                player.Money = 20;
                _userRepository.Save(player);
                _logger.Info($"User {username} has registered");
                return new Response(HttpCode.CREATED_201);
            }

            _logger.Info($"User {username} tried to register but the username is already taken");
            return new Response(HttpCode.CONFLICT_409, "{message: \"username already taken\"}");
        }
    }
}
