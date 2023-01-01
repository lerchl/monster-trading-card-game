using MonsterTradingCardGame.Data.User;
using Newtonsoft.Json;

namespace MonsterTradingCardGame.Api.Endpoints.Users {

    /// <summary>
    ///     View object for <see cref="Users" />
    /// </summary>
    internal class UserVO {

        public string? Name { get; }
        public string? Bio { get; }
        public string? Image { get; }

        // /////////////////////////////////////////////////////////////////////
        // Init
        // /////////////////////////////////////////////////////////////////////

        [JsonConstructor]
        public UserVO(string? name, string? bio, string? image) {
            Name = name;
            Bio = bio;
            Image = image;
        }

        public UserVO(User user) : this(user.Name, user.Bio, user.Image) {
            // noop
        }
    }
}
