using MonsterTradingCardGame.Data.Cards;

namespace MonsterTradingCardGame.Data.Deck {

    public class Deck : Entity {

        [Column(Name = "PLAYER_ID")]
        public Guid PlayerId { get; set; }

        [Column(Name = "CARD_1_ID")]
        public Guid Card1Id { get; set; }

        [Column(Name = "CARD_2_ID")]
        public Guid Card2Id { get; set; }

        [Column(Name = "CARD_3_ID")]
        public Guid Card3Id { get; set; }

        [Column(Name = "CARD_4_ID")]
        public Guid Card4Id { get; set; }

        // /////////////////////////////////////////////////////////////////////
        // Init
        // /////////////////////////////////////////////////////////////////////

        public Deck(Guid id, Guid playerId, Guid card1Id, Guid card2Id, Guid card3Id, Guid card4Id) : base(id) {
            PlayerId = playerId;
            Card1Id = card1Id;
            Card2Id = card2Id;
            Card3Id = card3Id;
            Card4Id = card4Id;
        }

        /// <summary>
        ///     Creates a new deck from a list of cards.
        ///     The first 4 cards in the list will be used.
        /// </summary>
        public Deck(Guid playerId, List<Card> cards) : base() {
            PlayerId = playerId;
            SetCards(cards);
        }

        // /////////////////////////////////////////////////////////////////////
        // Methods
        // /////////////////////////////////////////////////////////////////////

        public Guid[] GetCardIds() {
            return new[] { Card1Id, Card2Id, Card3Id, Card4Id };
        }

        /// <summary>
        ///     Sets the cards of this deck.
        ///     The first 4 cards in the list will be used.
        /// </summary>
        public void SetCards(List<Card> cards) {
            Card1Id = cards[0].Id;
            Card2Id = cards[1].Id;
            Card3Id = cards[2].Id;
            Card4Id = cards[3].Id;
        }
    }
}
