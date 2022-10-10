namespace MonsterTradingCardGame.Data {

    internal abstract class Entity {

        public readonly string? id;

        // /////////////////////////////////////////////////////////////////////
        // Constructors
        // /////////////////////////////////////////////////////////////////////

        public Entity() {
            // default constructor
        }

        public Entity(string? id) {
            this.id = id;
        }
    }
}