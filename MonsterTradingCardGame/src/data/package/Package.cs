using MonsterTradingCardGame.Data.Cards;

namespace MonsterTradingCardGame.Data.Packages {

    internal class Package : Entity {

        public List<Card> Cards { get; set; }

        // /////////////////////////////////////////////////////////////////////
        // Constructor
        // /////////////////////////////////////////////////////////////////////

        public Package(Guid id, List<Card> cards) : base(id) {
            Cards = cards;
        }

        public Package(List<Card> cards) {
            Cards = cards;
        }
    }
}
