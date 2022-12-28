using MonsterTradingCardGame.Data.Cards;

namespace MonsterTradingCardGame.Logic {

    internal class CardLogic : Logic<CardRepository, Card> {

        // /////////////////////////////////////////////////////////////////////
        // Init
        // /////////////////////////////////////////////////////////////////////

        public CardLogic() : base(new CardRepository()) {
            // noop
        }

        // /////////////////////////////////////////////////////////////////////
        // Methods
        // /////////////////////////////////////////////////////////////////////

        public Card[] PullCards(Guid package, Guid player) {
            return Repository.PullCards(package, player);
        }
    }
}