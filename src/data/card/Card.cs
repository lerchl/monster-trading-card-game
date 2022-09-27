namespace Data.Cards {

    internal abstract class Card {

        public readonly string id;
        public readonly string name;
        public readonly ElementType elementType;
        public readonly int damage;

        // /////////////////////////////////////////////////////////////////////
        // Constructor
        // /////////////////////////////////////////////////////////////////////

        public Card(string id, string name, ElementType elementType, int damage) {
            this.id = id;
            this.name = name;
            this.elementType = elementType;
            this.damage = damage;
        }
    }
}