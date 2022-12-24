using MonsterTradingCardGame.Data.Cards;

namespace MonsterTradingCardGame.Data.Trade {

    internal class Trade : Entity {

        [Column(Name = "PLAYER_ID")]
        public Guid PlayerId { get; set; }

        [Column(Name = "CARD_ID")]
        public Guid CardId { get; private set; }

        [Column(Name = "CARD_TYPE")]
        public CardType Type { get; private set; }

        [Column(Name = "MINIMUM_DAMAGE")]
        public double MinimumDamage { get; private set; }

        // /////////////////////////////////////////////////////////////////////
        // Constructor
        // /////////////////////////////////////////////////////////////////////

        public Trade(Guid id, Guid playerId, Guid cardId, CardType type, decimal minimumDamage) : base(id) {
            PlayerId = playerId;
            CardId = cardId;
            Type = type;
            MinimumDamage = (double) minimumDamage;
        }
    }
}
