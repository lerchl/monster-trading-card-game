using MonsterTradingCardGame.Data.User;

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

        public UserVO(User user) {
            Name = user.Name;
            Bio = user.Bio;
            Image = user.Image;
        }
    }
}
