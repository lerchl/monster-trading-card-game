namespace MonsterTradingCardGame.Data.Cards {

    internal class Card : Entity {

        public string Name { get; set; }
        public ElementType ElementType { get; set; }
        public double Damage { get; set; }
        public CardType CardType { get; set; }
        public Guid? PackageId { get; set; }

        // /////////////////////////////////////////////////////////////////////
        // Constructor
        // /////////////////////////////////////////////////////////////////////

        public Card(Guid id, string name, ElementType elementType, double damage, CardType cardType, Guid packageId) : base(id) {
            Name = name;
            ElementType = elementType;
            Damage = damage;
            CardType = cardType;
            PackageId = packageId;
        }
    }
}
