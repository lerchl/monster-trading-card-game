namespace MonsterTradingCardGame.Data.Cards {

    internal class SpellCard : Card {

        public SpellCard(string id, string name, ElementType elementType, int damage)
                  : base(id, name, elementType, damage) {
            // noop
        }
    }
}
