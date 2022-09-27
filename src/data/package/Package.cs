using Data.Cards;

namespace Data.Packages {

    internal class Package {

        public readonly string id;
        public readonly List<Card> cards;

        // /////////////////////////////////////////////////////////////////////
        // Constructor
        // /////////////////////////////////////////////////////////////////////

        public Package(string id, List<Card> cards) {
            this.id = id;
            this.cards = cards;
        }
    }
}
