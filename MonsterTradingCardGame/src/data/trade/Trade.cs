using MonsterTradingCardGame.Data.Cards;

namespace MonsterTradingCardGame.Data.Trade {

    public class Trade : Entity {

        [Column(Name = "PLAYER_ID")]
        public Guid UserId { get; set; }

        [Column(Name = "CARD_ID")]
        public Guid CardId { get; private set; }

        [Column(Name = "CARD_TYPE")]
        public CardType CardType { get; private set; }

        [Column(Name = "MINIMUM_DAMAGE")]
        public double MinimumDamage { get; private set; }

        // /////////////////////////////////////////////////////////////////////
        // Constructor
        // /////////////////////////////////////////////////////////////////////

        public Trade(Guid id, Guid userId, Guid cardId, CardType cardType, decimal minimumDamage) : base(id) {
            UserId = userId;
            CardId = cardId;
            CardType = cardType;
            MinimumDamage = (double) minimumDamage;
        }
    }
}
