namespace MonsterTradingCardGame.Logic.BattleNS {

    internal class BattleLogEntry {

        public int Round { get; private set; }
        public string Message { get; private set; }

        // /////////////////////////////////////////////////////////////////////
        // Init
        // /////////////////////////////////////////////////////////////////////

        public override string ToString() {
            return string.Format("{0:000}: {1}", Round, Message);
        }

        public BattleLogEntry(int round, string message) {
            Round = round;
            Message = message;
        }
    }
}
