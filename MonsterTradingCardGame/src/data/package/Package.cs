using MonsterTradingCardGame.Data.Cards;

namespace MonsterTradingCardGame.Data.Packages {

    internal class Package : Entity {

        public readonly List<Card> cards;

        // /////////////////////////////////////////////////////////////////////
        // Constructor
        // /////////////////////////////////////////////////////////////////////

        public Package(Guid id, List<Card> cards) : base(id) {
            this.cards = cards;
        }

        public Package(List<Card> cards) {
            this.cards = cards;
        }
    }
}
