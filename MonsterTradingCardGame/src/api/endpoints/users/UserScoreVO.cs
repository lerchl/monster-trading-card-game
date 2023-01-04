using MonsterTradingCardGame.Data.User;
using Newtonsoft.Json;

namespace MonsterTradingCardGame.Api.Endpoints.Users {

    /// <summary>
    ///     View object for a <see cref="User" />'s stats.
    /// </summary>
    internal class UserStatsVO {

        public string? Name { get; }
        public int Elo { get; }
        public int Wins { get; }
        public int Losses { get; }

        // /////////////////////////////////////////////////////////////////////
        // Init
        // /////////////////////////////////////////////////////////////////////

        [JsonConstructor]
        public UserStatsVO(string? name, int elo, int wins, int losses) {
            Name = name;
            Elo = elo;
            Wins = wins;
            Losses = losses;
        }

        public UserStatsVO(User user, int wins, int losses) : this(user.Name, user.Elo, wins, losses) {
            // noop
        }
    }
}
