namespace MonsterTradingCardGame.Logic.Exceptions {

    public class ConflictException : Exception {

        // /////////////////////////////////////////////////////////////////////
        // Init
        // /////////////////////////////////////////////////////////////////////

        public ConflictException(string message) : base(message) {
            // noop
        }
    }
}
