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
        public static Response FindByUsername([Header(Name = "Authorization")]          string bearer,
                                              [PathParam(Name = USERNAME_PATH_PARAM)]   string username) {
            // TODO: abstract authorization because of code duplication
            Token? token = SessionHandler.Instance.GetSession(bearer.Split(" ")[1]);
            if (token == null) {
                _logger.Info("Unauthorized request to authorized-only endpoint");
                return new(HttpCode.UNAUTHORIZED_401, "{message: \"not logged in\"}");
            }

            if (!token.Username.Equals(username)) {
                _logger.Info("Player tried to get another player's data");
                return new(HttpCode.FORBIDDEN_403, "{message: \"not allowed\"}");
            }

            Player? dbPlayer = _playerRepository.FindById(token.PlayerId);

            if (dbPlayer == null) {
                // this can theoretically never happen because
                // the token is only valid if the player exists
                // and accessing another player's data is forbidden
                _logger.Error($"Player {username} not found");
                return new(HttpCode.NOT_FOUND_404, "{message: \"player not found\"}");
            }

            dbPlayer.Password = "*redacted*";
            return new(HttpCode.OK_200, dbPlayer);
        }

        [ApiEndpoint(HttpMethod = EHttpMethod.PUT, Url = URL)]
        public static Response EditByUsername([Header(Name = "Authorization")]          string bearer,
                                              [PathParam(Name = USERNAME_PATH_PARAM)]   string username,
                                              [Body]                                    Player player) {
            // TODO: abstract authorization because of code duplication
            Token? token = SessionHandler.Instance.GetSession(bearer.Split(" ")[1]);
            if (token == null) {
                _logger.Info("Unauthorized request to authorized-only endpoint");
                return new(HttpCode.UNAUTHORIZED_401, "{message: \"not logged in\"}");
            }

            if (!token.Username.Equals(username)) {
                _logger.Info("Player tried to edit another player's data");
                return new(HttpCode.FORBIDDEN_403, "{message: \"not allowed\"}");
            }

            Player? dbPlayer = _playerRepository.FindById(token.PlayerId);

            if (dbPlayer == null) {
                // this can theoretically never happen because
                // the token is only valid if the player exists
                // and accessing another player's data is forbidden
                _logger.Error($"Player {username} not found");
                return new(HttpCode.NOT_FOUND_404, "{message: \"player not found\"}");
            }

            dbPlayer.Name = player.Name;
            dbPlayer.Bio = player.Bio;
            dbPlayer.Image = player.Image;

            dbPlayer = _playerRepository.Save(dbPlayer);

            if (dbPlayer == null) {
                _logger.Error($"Player {username} could not be saved");
                return new(HttpCode.INTERNAL_SERVER_ERROR_500, "{message: \"could not save player\"}");
            }

            dbPlayer.Password = "*redacted*";
            return new(HttpCode.OK_200, dbPlayer);
        }
    }
}