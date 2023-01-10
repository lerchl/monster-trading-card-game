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

        public Deck FindByUser(Guid playerId) {
            return Repository.FindByPlayer(playerId);
        }

        public Card[] Get(Token token) {
            Deck deck;
            try {
                deck = Repository.FindByPlayer(token.UserId);
            } catch (NoResultException) {
                throw new NoContentException("Deck not found");
            }

            return GetCards(token.Username, deck.GetCardIds());
        }

        public Card[] Set(Token token, Guid[] cardIds) {
            if (cardIds.Length != 4) {
                throw new BadRequestException("Deck must contain exactly 4 cards");
            }

            List<Card> cards = cardIds.Select(id => _cardLogic.FindById(id)).ToList();

            if (cards.Any(card => card.UserId != token.UserId)) {
                throw new ForbiddenException("You can only add your own cards to your deck");
            }

            Deck deck;

            try {
                deck = Repository.FindByPlayer(token.UserId);
            } catch (NoResultException) {
                deck = new(token.UserId, cards);
            }

            deck = Save(new Deck(token.UserId, cards));
            return GetCards(token.Username, deck.GetCardIds());
        }

        private Card[] GetCards(string username, Guid[] cardIds) {
            try {
                return cardIds.Select(_cardLogic.FindById).ToArray();
            } catch (NoResultException) {
                string message = $"Card in deck of user {username} not found";
                throw new InternalServerErrorException(message);
            }
        }
    }
}
