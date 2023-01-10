namespace MonsterTradingCardGame.Data.Cards {

    public class Card : Entity {

        public string Name { get; set; }

        [Column(Name = "ELEMENT_TYPE")]
        public ElementType ElementType { get; set; }

        public double Damage { get; set; }

        [Column(Name = "CARD_TYPE")]
        public CardType CardType { get; set; }

        [Column(Name = "PACKAGE_ID")]
        public Guid? PackageId { get; set; }

        [Column(Name = "PLAYER_ID")]
        public Guid? UserId { get; set; }

        // /////////////////////////////////////////////////////////////////////
        // Init
        // /////////////////////////////////////////////////////////////////////

        public Card(Guid id, string name, ElementType elementType, decimal damage, CardType cardType, Guid? packageId, Guid? userId) : base(id) {
            Name = name;
            ElementType = elementType;
            Damage = (double) damage;
            CardType = cardType;
            PackageId = packageId;
            UserId = userId;
        }

        // /////////////////////////////////////////////////////////////////////
        // Methoden
        // /////////////////////////////////////////////////////////////////////

        public override string ToString() {
            return $"{Name} ({CardType}) [{ElementType}, {Damage}]";
        }
    }
}
