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
        public static void Login(string Username, string Password) {
            Player? user = _playerRepository.FindByUsername(Username);
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
