namespace MonsterTradingCardGame.Data.Cards {

    internal class Card : Entity {

        public string Name { get; set; }
        public ElementType ElementType { get; set; }
        public double Damage { get; set; }
        public CardType CardType { get; set; }

        // /////////////////////////////////////////////////////////////////////
        // Constructor
        // /////////////////////////////////////////////////////////////////////

        public Card(string name, ElementType elementType, double damage, CardType cardType) {
            Name = name;
            ElementType = elementType;
            Damage = damage;
            CardType = cardType;
        }
    }
}
