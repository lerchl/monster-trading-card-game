namespace Game {

    internal abstract class Card {

        private string Name { get; set; }
        // TODO: the damage of a card is constant and does not change
        private int Damage { get; set; }

        // /////////////////////////////////////////////////////////////////////
        // Constructor
        // /////////////////////////////////////////////////////////////////////

        public Card(string Name, int Damage) {
            this.Name = Name;
            this.Damage = Damage;
        }
    }
}