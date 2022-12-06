namespace MonsterTradingCardGame.Data {

    public abstract class Entity {

        public readonly Guid? id;

        // /////////////////////////////////////////////////////////////////////
        // Constructors
        // /////////////////////////////////////////////////////////////////////

        public Entity() {
            // default constructor
        }

        public Entity(Guid? id) {
            this.id = id;
        }

        // /////////////////////////////////////////////////////////////////////
        // Methods
        // /////////////////////////////////////////////////////////////////////

        public bool IsPersisted() {
            return id != null && id != Guid.Empty;
        }
    }
}
