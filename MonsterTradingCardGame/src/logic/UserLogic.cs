using MonsterTradingCardGame.Api.Endpoints.Users;
using MonsterTradingCardGame.Data;
using MonsterTradingCardGame.Data.User;
using MonsterTradingCardGame.Logic.Exceptions;
using MonsterTradingCardGame.Server;

namespace MonsterTradingCardGame.Logic {

    internal class UserLogic : Logic<UserRepository, User> {

        // /////////////////////////////////////////////////////////////////////
        // Init
        // /////////////////////////////////////////////////////////////////////

        public UserLogic() : base(new UserRepository()) {
            // noop
        }

        // /////////////////////////////////////////////////////////////////////
        // Methods
        // /////////////////////////////////////////////////////////////////////

        public void Register(User user) {
            try {
                Repository.FindByUsername(user.Username);
                throw new ConflictException("Username already taken");
            } catch (NoResultException) {
                user.Money = 20;
                Save(user);
            }
        }

        public Token Login(User user) {
            string username = user.Username;

            User dbUser;
            try {
                dbUser = Repository.FindByUsername(username);
            } catch (NoResultException) {
                throw new UnauthorizedException("Invalid username or password");
            }

            if (!dbUser.Password.Equals(user.Password)) {
                throw new UnauthorizedException("Invalid username or password");
            }

            return SessionHandler.Instance.CreateSession(dbUser.Id, dbUser.Username, dbUser.Role);
        }

        public UserVO GetInfo(Token token, string username) {
            if (token.UserRole != UserRole.Admin && !token.Username.Equals(username)) {
                throw new ForbiddenException("You can only access your own data");
            }

            return new(FindById(token.UserId));
        }

        public UserVO SetInfo(Token token, string username, UserVO userVO) {
            if (token.UserRole != UserRole.Admin && !token.Username.Equals(username)) {
                throw new ForbiddenException("You can only edit your own data");
            }

            User user = FindById(token.UserId);
            user.Name = userVO.Name;
            user.Bio = userVO.Bio;
            user.Image = userVO.Image;

            return new(Save(user));
        }
    }
}
