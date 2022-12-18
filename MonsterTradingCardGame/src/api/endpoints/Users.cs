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
            dbPlayer.Password = "*redacted*";
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

            // TODO: this is kinda weird,
            // the repository should always return the updated object if the save was succesful
            // so there should be an exception rather than a possibility for null
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