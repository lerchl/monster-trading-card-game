using MonsterTradingCardGame.Data.BattleNS;

namespace MonsterTradingCardGame.Logic.BattleNS {

    internal class BattleQueueEntry {

        // /////////////////////////////////////////////////////////////////////
        // Properties
        // /////////////////////////////////////////////////////////////////////

        public ManualResetEventSlim ResetEvent { get; }
        public Battle Battle { get; }

        // /////////////////////////////////////////////////////////////////////
        // Init
        // /////////////////////////////////////////////////////////////////////

        public BattleQueueEntry(ManualResetEventSlim resetEvent, Battle battle) {
            ResetEvent = resetEvent;
            Battle = battle;
        }
    }
}
