using System.Net.Sockets;
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

        [ApiEndpoint(httpMethod = EHttpMethod.POST, url = "/sessions")]
        public static void Login(Socket client, string Username, string Password) {
            Player? user = _playerRepository.FindByUsername(Username);
            if (user == null) {
                _logger.Info($"Unknown user {Username} tried to login");
                ApiEndpointUtils.SendResponse(client, HttpCode.UNAUTHORIZED_401, "{message: \"username or password wrong\"}");
            } else if (!user.password.Equals(Password)) {
                _logger.Info($"User {Username} tried to login with the wrong password");
                ApiEndpointUtils.SendResponse(client, HttpCode.UNAUTHORIZED_401, "{message: \"username or password wrong\"}");
            } else {
                _logger.Info($"User {Username} has logged in");
                // TODO: send bearer token
                ApiEndpointUtils.SendResponse(client, HttpCode.OK_200);
            }
        }

        [ApiEndpoint(httpMethod = EHttpMethod.POST, url = "/users")]
        public static void Register(Socket client, string Username, string Password) {
            Player? user = _playerRepository.FindByUsername(Username);
            if (user == null) {
                _playerRepository.Save(new Player(Username, Password));
                _logger.Info($"User {Username} has registered");
                ApiEndpointUtils.SendResponse(client, HttpCode.CREATED_201);
            } else {
                _logger.Info($"User {Username} tried to register but the username is already taken");
                ApiEndpointUtils.SendResponse(client, HttpCode.CONFLICT_409, "{message: \"username already taken\"}");
            }
        }
    }
}
