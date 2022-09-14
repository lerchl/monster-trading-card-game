namespace Game {

    internal abstract class Card {

        public string Name { get; set; }
        public ElementType elementType { get; set; }
        // TODO: the damage of a card is constant and does not change
        public int Damage { get; set; }

        // /////////////////////////////////////////////////////////////////////
        // Constructor
        // /////////////////////////////////////////////////////////////////////

        public Card(string Name, int Damage) {
            this.Name = Name;
            this.Damage = Damage;
        }
    }
}