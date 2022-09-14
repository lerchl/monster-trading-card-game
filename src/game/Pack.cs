namespace Game {

    internal class Pack {

        public const int COST = 5;

        public Card Card1 { get; }
        public Card Card2 { get; }
        public Card Card3 { get; }
        public Card Card4 { get; }
        public Card Card5 { get; }

        // /////////////////////////////////////////////////////////////////////
        // Constructor
        // /////////////////////////////////////////////////////////////////////

        public Pack(Card Card1, Card Card2, Card Card3, Card Card4, Card Card5) {
            this.Card1 = Card1;
            this.Card2 = Card2;
            this.Card3 = Card3;
            this.Card4 = Card4;
            this.Card5 = Card5;
        }
    }
}