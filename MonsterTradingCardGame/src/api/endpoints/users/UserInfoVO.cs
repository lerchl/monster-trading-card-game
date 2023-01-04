using MonsterTradingCardGame.Data.User;
using Newtonsoft.Json;

namespace MonsterTradingCardGame.Api.Endpoints.Users {

    /// <summary>
    ///     View object for a <see cref="User" />'s info.
    /// </summary>
    internal class UserInfoVO {

        public string? Name { get; }
        public string? Bio { get; }
        public string? Image { get; }

        // /////////////////////////////////////////////////////////////////////
        // Init
        // /////////////////////////////////////////////////////////////////////

        [JsonConstructor]
        public UserInfoVO(string? name, string? bio, string? image) {
            Name = name;
            Bio = bio;
            Image = image;
        }

        public UserInfoVO(User user) : this(user.Name, user.Bio, user.Image) {
            // noop
        }
    }
}
