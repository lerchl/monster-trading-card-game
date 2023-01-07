using MonsterTradingCardGame.Data.User;
using Newtonsoft.Json;

namespace MonsterTradingCardGame.Api.Endpoints.Users {

    /// <summary>
    ///     View object for a <see cref="User" />'s stats.
    /// </summary>
    public class UserStatsVO {

        public string? Name { get; }
        public int Elo { get; }
        public long Wins { get; }
        public long Losses { get; }

        // /////////////////////////////////////////////////////////////////////
        // Init
        // /////////////////////////////////////////////////////////////////////

        [JsonConstructor]
        public UserStatsVO(string? name, int elo, long wins, long losses) {
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
