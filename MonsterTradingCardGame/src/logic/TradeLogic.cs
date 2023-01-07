using MonsterTradingCardGame.Data;
using MonsterTradingCardGame.Data.Cards;
using MonsterTradingCardGame.Data.Deck;
using MonsterTradingCardGame.Data.Trade;
using MonsterTradingCardGame.Logic.Exceptions;
using MonsterTradingCardGame.Server;

namespace MonsterTradingCardGame.Logic {

    public class TradeLogic : Logic<TradeRepository, Trade> {

        private readonly CardRepository _cardRepository;
        private readonly DeckRepository _deckRepository;

        // /////////////////////////////////////////////////////////////////////
        // Init
        // /////////////////////////////////////////////////////////////////////

        public TradeLogic() : base(new TradeRepository()) {
            _cardRepository = new();
            _deckRepository = new();
        }

        /// <summary>
        ///     Constructor for testing purposes.
        /// </summary>
        public TradeLogic(TradeRepository repository, CardRepository cardRepository, DeckRepository deckRepository) : base(repository) {
            _cardRepository = cardRepository;
            _deckRepository = deckRepository;
        }

        // /////////////////////////////////////////////////////////////////////
        // Methods
        // /////////////////////////////////////////////////////////////////////

        /// <summary>
        ///    Creates a new trade offer.
        /// </summary>
        /// <param name="token">The player's token</param>
        /// <param name="trade">The trade offer</param>
        /// <returns>The created trade offer</returns>
        /// <exception cref="NoResultException" />
        /// <exception cref="ForbiddenException" />
        public Trade Create(Token token, Trade trade) {
            Card card = _cardRepository.FindById(trade.CardId);

            if (token.UserId != card.PlayerId) {
                throw new ForbiddenException("You can only create trade offers for your own cards");
            }

            trade.UserId = token.UserId;
            return Save(trade);
        }

        /// <summary>
        ///     Trades a card for another card.
        /// </summary>
        /// <param name="token">The player's token</param>
        /// <param name="tradeId">The trade's id</param>
        /// <param name="cardId">The card's id</param>
        /// <exception cref="NoResultException" />
        /// <exception cref="ForbiddenException" />
        public void Trade(Token token, Guid tradeId, Guid cardId) {
            Trade trade = FindById(tradeId);
            Card card = _cardRepository.FindById(cardId);

            if (token.UserId == trade.UserId) {
                throw new ForbiddenException("You can't trade with yourself");
            } else if (token.UserId != card.PlayerId) {
                throw new ForbiddenException("You can only trade your own cards");
            } else if (_deckRepository.IsCardInDeck(token.UserId, cardId)) {
                throw new ForbiddenException("You can't trade cards that are in your deck");
            } else if (trade.CardType != card.CardType) {
                throw new ForbiddenException("The trade expects a card of type" + trade.CardType);
            } else if (trade.MinimumDamage > card.Damage) {
                throw new ForbiddenException("The trade expects a card with a minimum damage of " + trade.MinimumDamage);
            }

            Card tradeCard = _cardRepository.FindById(trade.CardId);
            (card.PlayerId, tradeCard.PlayerId) = (tradeCard.PlayerId, card.PlayerId);
            _cardRepository.Save(card);
            _cardRepository.Save(tradeCard);

            // TODO: delete this trade
            Delete(tradeId);
            // TODO: delete other trades with the same card
        }

        /// <summary>
        ///    Deletes a trade offer.
        /// </summary>
        /// <param name="token">The player's token</param>
        /// <param name="tradeId">The trade's id</param>
        /// <exception cref="NoResultException" />
        /// <exception cref="ForbiddenException" />
        public void Delete(Token token, Guid tradeId) {
            Trade trade = FindById(tradeId);

            if (token.UserId != trade.UserId) {
                throw new ForbiddenException("You can only delete your own trades");
            }

            Delete(tradeId);
        }
    }
}
