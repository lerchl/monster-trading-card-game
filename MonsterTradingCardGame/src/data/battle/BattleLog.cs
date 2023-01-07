using MonsterTradingCardGame.Logic.BattleNS;

namespace MonsterTradingCardGame.Data.BattleNS {

    internal class BattleLog {

        private readonly List<BattleLogEntry> _logs = new();

        // /////////////////////////////////////////////////////////////////////
        // Methods
        // /////////////////////////////////////////////////////////////////////

        public override string ToString() {
            return _logs.Select(log => log.ToString()).Aggregate((a, b) => a + "\n" + b) ?? "";
        }

        public void Add(int round, string message) {
            _logs.Add(new BattleLogEntry(round, message));
        }
    }
}
