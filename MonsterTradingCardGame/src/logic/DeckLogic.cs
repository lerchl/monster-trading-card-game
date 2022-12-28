using MonsterTradingCardGame.Data;
using MonsterTradingCardGame.Data.Cards;
using MonsterTradingCardGame.Data.Deck;
using MonsterTradingCardGame.Logic.Exceptions;
using MonsterTradingCardGame.Server;

namespace MonsterTradingCardGame.Logic {

    internal class DeckLogic : Logic<DeckRepository, Deck> {

        private readonly CardLogic _cardLogic = new();

        // /////////////////////////////////////////////////////////////////////
        // Init
        // /////////////////////////////////////////////////////////////////////

        public DeckLogic() : base(new DeckRepository()) {
            // noop
        }

        // /////////////////////////////////////////////////////////////////////
        // Methods
        // /////////////////////////////////////////////////////////////////////

        public Deck Set(Token token, Guid[] cardIds) {
            if (cardIds.Length != 4) {
                throw new BadRequestException("Deck must contain exactly 4 cards");
            }

            List<Card> cards = cardIds.Select(id => _cardLogic.FindById(id)).ToList();

            if (cards.Any(card => card.PlayerId != token.UserId)) {
                throw new ForbiddenException("You can only add your own cards to your deck");
            }

            Deck deck;

            try {
                deck = Repository.FindByPlayer(token.UserId);
            } catch (NoResultException) {
                deck = new(token.UserId, cards);
            }

            return Save(new Deck(token.UserId, cards));
        }

        public Deck FindByPlayer(Guid playerId) {
            return Repository.FindByPlayer(playerId);
        }
    }
}
