using MonsterTradingCardGame.Data.BattleNS;

namespace MonsterTradingCardGame.Logic.BattleNS {

    /// <summary>
    ///     Singleton for handling battles.
    /// </summary>
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
        private readonly Mutex _queueMutex = new();

        // /////////////////////////////////////////////////////////////////////
        // Init
        // /////////////////////////////////////////////////////////////////////

        private BattleHandler() {
            // noop
        }

        // /////////////////////////////////////////////////////////////////////
        // Methods
        // /////////////////////////////////////////////////////////////////////

        /// <summary>
        ///     Queues a User to battle
        /// </summary>
        /// <param name="userId">The User's Id</param>
        /// <returns>The BattleQueueEntry whose <see cref="ManualResetEventSlim"/>
        ////    should be waited for as it will be set when the battle is finished.
        //// </returns>
        public BattleQueueEntry QueueBattle(Guid userId) {
            _queueMutex.WaitOne();
            BattleQueueEntry? entry = FindBattleForUser(userId);

            if (entry == null) {
                BattleQueueEntry newEntry = new(new(), new(userId));
                _queue.Add(newEntry);
                _queueMutex.ReleaseMutex();
                return newEntry;
            }

            Battle battle = entry.Battle;
            battle.User2Id = userId;

            Task.Run(() => {
                _battleLogic.ExecuteBattle(battle);
                entry.ResetEvent.Set();
            });

            _queue.Remove(entry);
            _queueMutex.ReleaseMutex();
            return entry;
        }

        private BattleQueueEntry? FindBattleForUser(Guid userId) {
            return _queue.FirstOrDefault(entry => entry.Battle.User1Id != userId);
        }
    }
}
