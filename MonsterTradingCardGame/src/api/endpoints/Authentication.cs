using System.Net.Sockets;
using MonsterTradingCardGame;
using MonsterTradingCardGame.Api;
using MonsterTradingCardGame.Data.Player;

namespace Api.Endpoints {

    internal class Authentication {

        private static readonly Logger<Authentication> _logger = new();

        private static readonly PlayerRepository _playerRepository = new();

        // /////////////////////////////////////////////////////////////////////
        // Methods
        // /////////////////////////////////////////////////////////////////////

        [ApiEndpoint(httpMethod = EHttpMethod.POST, url = "/sessions")]
        public static void Login(Socket client, string Username, string Password) {
            Player? user = _playerRepository.FindByUsername(Username);
            if (user == null) {
                _logger.Info($"Unknown user {Username} tried to login");
                ApiEndpointUtils.SendResponse(client, 401, "Unauthorized");
            } else if (!user.password.Equals(Password)) {
                _logger.Info($"User {Username} tried to login with the wrong password");
                ApiEndpointUtils.SendResponse(client, 401, "Unauthorized");
            } else {
                _logger.Info($"User {Username} has logged in");
                ApiEndpointUtils.SendResponse(client, 200, "OK");
            }
        }
    }
}
