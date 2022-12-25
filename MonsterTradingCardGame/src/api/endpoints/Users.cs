using MonsterTradingCardGame.Data;
using MonsterTradingCardGame.Data.Player;
using MonsterTradingCardGame.Server;

namespace MonsterTradingCardGame.Api.Endpoints {

    internal class Users {

        private const string USERNAME_PATH_PARAM = "username";
        private const string URL = $"^/users/(?'{USERNAME_PATH_PARAM}'{RegexUtils.Username})$";

        private static readonly Logger<Users> _logger = new();
        private static readonly PlayerRepository _playerRepository = new();

        // /////////////////////////////////////////////////////////////////////
        // Methods
        // /////////////////////////////////////////////////////////////////////
    
        [ApiEndpoint(HttpMethod = EHttpMethod.GET, Url = URL)]
        public static Response FindByUsername([Bearer]                                Token bearer,
                                              [PathParam(Name = USERNAME_PATH_PARAM)] string username) {
            if (!bearer.Username.Equals(username)) {
                _logger.Info("Player tried to get another player's data");
                return new(HttpCode.FORBIDDEN_403, "{message: \"not allowed\"}");
            }

            Player dbPlayer = _playerRepository.FindById(bearer.PlayerId)!;
            dbPlayer.Password = "";
            return new(HttpCode.OK_200, dbPlayer);
        }

        [ApiEndpoint(HttpMethod = EHttpMethod.PUT, Url = URL)]
        public static Response EditByUsername([Bearer]                                Token bearer,
                                              [PathParam(Name = USERNAME_PATH_PARAM)] string username,
                                              [Body]                                  Player player) {
            if (!bearer.Username.Equals(username)) {
                _logger.Info("Player tried to edit another player's data");
                return new(HttpCode.FORBIDDEN_403, "{message: \"not allowed\"}");
            }

            Player dbPlayer = _playerRepository.FindById(bearer.PlayerId)!;
            dbPlayer.Name = player.Name;
            dbPlayer.Bio = player.Bio;
            dbPlayer.Image = player.Image;

            dbPlayer = _playerRepository.Save(dbPlayer);

            dbPlayer.Password = "";
            return new(HttpCode.OK_200, dbPlayer);
        }
    }
}