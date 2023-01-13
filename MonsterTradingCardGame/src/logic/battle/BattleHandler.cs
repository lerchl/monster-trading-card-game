using MonsterTradingCardGame.Data.BattleNS;

namespace MonsterTradingCardGame.Logic.BattleNS {

    internal class BattleHandler {

        private static BattleHandler? _instance;

        // /////////////////////////////////////////////////////////////////////
        // Properties
        // /////////////////////////////////////////////////////////////////////

        public static BattleHandler Instance {
            get {
                _instance ??= new BattleHandler();
                return _instance;
            }
        }

        private readonly BattleLogic _battleLogic = new();
        private readonly List<BattleQueueEntry> _queue = new();

        // /////////////////////////////////////////////////////////////////////
        // Init
        // /////////////////////////////////////////////////////////////////////

        private BattleHandler() {
            // noop
        }

        // /////////////////////////////////////////////////////////////////////
        // Methods
        // /////////////////////////////////////////////////////////////////////

        public BattleQueueEntry QueueBattle(Guid userId) {
            BattleQueueEntry? entry = FindBattleForUser(userId);

            if (entry == null) {
                BattleQueueEntry newEntry = new(new(), new(userId));
                _queue.Add(newEntry);
                return newEntry;
            }

            Battle battle = entry.Battle;
            battle.User2Id = userId;

            Task.Run(() => {
                _battleLogic.ExecuteBattle(battle);
                entry.ResetEvent.Set();
            });

            _queue.Remove(entry);
            return entry;
        }

        private BattleQueueEntry? FindBattleForUser(Guid userId) {
            return _queue.FirstOrDefault(entry => entry.Battle.User1Id != userId);
        }
    }
}
