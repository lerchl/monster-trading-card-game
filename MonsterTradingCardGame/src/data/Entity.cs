namespace MonsterTradingCardGame.Data {

    internal abstract class Entity {

        public readonly Guid? id;

        // /////////////////////////////////////////////////////////////////////
        // Constructors
        // /////////////////////////////////////////////////////////////////////

        public Entity() {
            // default constructor
        }

        public Entity(Guid id) {
            this.id = id;
        }
    }
}
