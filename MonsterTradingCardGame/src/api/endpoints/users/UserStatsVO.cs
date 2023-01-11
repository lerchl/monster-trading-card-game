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
        public long Draws { get; }

        // /////////////////////////////////////////////////////////////////////
        // Init
        // /////////////////////////////////////////////////////////////////////

        [JsonConstructor]
        public UserStatsVO(string? name, int elo, long wins, long losses, long draws) {
            Name = name;
            Elo = elo;
            Wins = wins;
            Losses = losses;
            Draws = draws;
        }
    }
}
