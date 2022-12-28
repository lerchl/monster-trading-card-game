using MonsterTradingCardGame.Data;
using MonsterTradingCardGame.Data.Cards;
using MonsterTradingCardGame.Data.Packages;
using MonsterTradingCardGame.Data.User;
using MonsterTradingCardGame.Logic.Exceptions;
using MonsterTradingCardGame.Server;

namespace MonsterTradingCardGame.Logic {

    internal class PackageLogic {

        public PackageRepository Repository { get; } = new();

        public CardLogic CardLogic { get; } = new();
        public UserLogic UserLogic { get; } = new();

        // /////////////////////////////////////////////////////////////////////
        // Methods
        // /////////////////////////////////////////////////////////////////////

        public Card[] CreatePackage(Token token, Card[] cards) {
            if (token.UserRole != UserRole.Admin) {
                throw new ForbiddenException("Only admins can create packages");
            }

            if (cards.Any(card => card.IsPersisted())) {
                throw new ConflictException("Card already has an id");
            }

            Guid packageId = Guid.NewGuid();
            return cards.Select(card => {
                card.PackageId = packageId;
                card.PlayerId = null;
                return CardLogic.Save(card);
            }).ToArray();
        }

        public Card[] AcquirePackage(Token token) {
            User user = UserLogic.FindById(token.UserId);

            if (user.Money < 5) {
                throw new ForbiddenException("Not enough money");
            }

            List<Guid> packages = Repository.FindAll();

            if (packages.Count == 0) {
                throw new NoResultException("No packages available");
            }

            Guid package = packages[new Random().Next(packages.Count)];
            Card[] cards = CardLogic.PullCards(package, user.Id);

            user.Money -= 5;
            UserLogic.Save(user);

            return cards;
        }
    }
}