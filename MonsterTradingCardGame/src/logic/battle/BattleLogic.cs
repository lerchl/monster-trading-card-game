using MonsterTradingCardGame.Data;
using MonsterTradingCardGame.Data.BattleNS;
using MonsterTradingCardGame.Data.Deck;
using MonsterTradingCardGame.Logic.Exceptions;
using MonsterTradingCardGame.Server;

namespace MonsterTradingCardGame.Logic.BattleNS {

    internal class BattleLogic : Logic<BattleRepository, Battle> {

        private readonly DeckLogic _deckLogic = new();

        // /////////////////////////////////////////////////////////////////////
        // Init
        // /////////////////////////////////////////////////////////////////////

        public BattleLogic() : base(new BattleRepository()) {
            // noop
        }

        // /////////////////////////////////////////////////////////////////////
        // Methods
        // /////////////////////////////////////////////////////////////////////

        public List<BattleLog> Battle(Token token) {
            Deck deck = _deckLogic.FindByPlayer(token.UserId);

            Battle battle;

            try {
                battle = Repository.FindOpenBattle();
            } catch (NoResultException) {
                battle = Save(new Battle(token.UserId));
            }

            // TODO: execute battle / wait

            return new List<BattleLog>();
        }
    }
}
