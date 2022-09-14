namespace Game {

    internal class Deck {

        public List<Card> Cards { get; set; }

        // /////////////////////////////////////////////////////////////////////
        // Constructor
        // /////////////////////////////////////////////////////////////////////

        public Deck(List<Card> Cards) {
            this.Cards = Cards;
        }
    }
}
