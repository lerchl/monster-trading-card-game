using MonsterTradingCardGame;
using MonsterTradingCardGame.Api;
using MonsterTradingCardGame.Data.Player;
using MonsterTradingCardGame.Server;

namespace Api.Endpoints {

    internal class Authentication {

        private static readonly Logger<Authentication> _logger = new();

        private static readonly PlayerRepository _playerRepository = new();

        // /////////////////////////////////////////////////////////////////////
        // Methods
        // /////////////////////////////////////////////////////////////////////

        [ApiEndpoint(HttpMethod = EHttpMethod.POST, Url = "/sessions")]
        public static Response Login([Body] Player player) {
            string username = player.Username;
            Player? dbPlayer = _playerRepository.FindByUsername(username);

            if (dbPlayer == null) {
                _logger.Info($"Unknown user {username} tried to login");
                return new Response(HttpCode.UNAUTHORIZED_401, "{message: \"username or password wrong\"}");
            } else if (!dbPlayer.Password.Equals(player.Password)) {
                _logger.Info($"User {username} tried to login with the wrong password");
                return new Response(HttpCode.UNAUTHORIZED_401, "{message: \"username or password wrong\"}");
            } else {
                _logger.Info($"User {username} has logged in");
                // TODO: send bearer token
                return new Response(HttpCode.OK_200);
            }
        }

        [ApiEndpoint(HttpMethod = EHttpMethod.POST, Url = "/users")]
        public static Response Register([Body] Player player) {
            string username = player.Username;
            Player? dbPlayer = _playerRepository.FindByUsername(username);

            if (dbPlayer == null) {
                _playerRepository.Save(player);
                _logger.Info($"User {username} has registered");
                return new Response(HttpCode.CREATED_201);
            } else {
                _logger.Info($"User {username} tried to register but the username is already taken");
                return new Response(HttpCode.CONFLICT_409, "{message: \"username already taken\"}");
            }
        }
    }
}
