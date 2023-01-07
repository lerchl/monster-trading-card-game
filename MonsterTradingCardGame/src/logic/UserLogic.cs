using MonsterTradingCardGame.Api.Endpoints.Users;
using MonsterTradingCardGame.Data;
using MonsterTradingCardGame.Data.User;
using MonsterTradingCardGame.Logic.Exceptions;
using MonsterTradingCardGame.Server;

namespace MonsterTradingCardGame.Logic {

    public class UserLogic : Logic<UserRepository, User> {

        // /////////////////////////////////////////////////////////////////////
        // Init
        // /////////////////////////////////////////////////////////////////////

        public UserLogic() : base(new UserRepository()) {
            // noop
        }

        /// <summary>
        ///     Constructor for testing purposes.
        /// </summary>
        public UserLogic(UserRepository repository) : base(repository) {
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
                user.Elo = 100;
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

        public UserInfoVO GetInfo(Token token, string username) {
            if (token.UserRole != UserRole.Admin && !token.Username.Equals(username)) {
                throw new ForbiddenException("You can only access your own data");
            }

            return new(Repository.FindByUsername(username));
        }

        public UserInfoVO SetInfo(Token token, string username, UserInfoVO userInfoVO) {
            if (token.UserRole != UserRole.Admin && !token.Username.Equals(username)) {
                throw new ForbiddenException("You can only edit your own data");
            }

            User user = FindById(token.UserId);
            user.Name = userInfoVO.Name;
            user.Bio = userInfoVO.Bio;
            user.Image = userInfoVO.Image;

            return new(Save(user));
        }

        public UserStatsVO GetStats(Token token) {
            return Repository.FindStatsById(token.UserId);
        }

        public List<UserStatsVO> GetScoreboard() {
            return Repository.FindStats();
        }

        public void UpdateElo(Guid user1Id, Guid user2Id, Guid winnerId) {
            User user1 = FindById(user1Id);
            User user2 = FindById(user2Id);

            user1.Elo += winnerId == user1Id ? 3 : -5;
            user2.Elo += winnerId == user2Id ? 3 : -5;

            Save(user1);
            Save(user2);
        }
    }
}
