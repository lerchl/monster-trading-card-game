using System.Runtime.InteropServices;
using MonsterTradingCardGame.Data.Cards;

namespace MonsterTradingCardGame.Data.Deck {

    internal class Deck : Entity {

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

        public Deck(Guid? id, Guid playerId, Guid card1Id, Guid card2Id, Guid card3Id, Guid card4Id) : base(id) {
            PlayerId = playerId;
            Card1Id = card1Id;
            Card2Id = card2Id;
            Card3Id = card3Id;
            Card4Id = card4Id;
        }

        /// <summary>
        ///     Creates a new deck from a list of cards.
        ///     The first 4 cards in the list will be used.
        ///     Their ids must not be null, intended for use after validation.
        /// </summary>
        public Deck(Guid playerId, List<Card> cards) : this(null, playerId,
                (Guid) cards[0].id!, (Guid) cards[1].id!, (Guid) cards[2].id!, (Guid) cards[3].id!) {
            // noop
        }

        // /////////////////////////////////////////////////////////////////////
        // Methods
        // /////////////////////////////////////////////////////////////////////

        /// <summary>
        ///     Sets the cards of this deck.
        /// </summary>
        /// <param name="cards">
        ///    <list type="bullet">
        ///        <item>The first 4 cards in the list will be used.</item>
        ///        <item>Their ids must not be null.</item>
        ///    </list>
        /// </param>
        public void SetCards(List<Card> cards) {
            Card1Id = (Guid) cards[0].id!;
            Card2Id = (Guid) cards[1].id!;
            Card3Id = (Guid) cards[2].id!;
            Card4Id = (Guid) cards[3].id!;
        }
    }
}
