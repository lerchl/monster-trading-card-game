namespace Data.Cards {

    internal class MonsterCard : Card {

        public MonsterCard(string id, string name, ElementType elementType, int damage)
                    : base(id, name, elementType, damage) {
            // noop
        }
    }
}
