namespace MonsterTradingCardGame.Logic.BattleNS {

    internal class BattleLog {

        public int Round { get; private set; }
        public string Message { get; private set; }

        // /////////////////////////////////////////////////////////////////////
        // Init
        // /////////////////////////////////////////////////////////////////////

        public BattleLog(int round, string message) {
            Round = round;
            Message = message;
        }
    }
}
