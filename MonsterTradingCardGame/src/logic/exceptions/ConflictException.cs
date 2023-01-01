namespace MonsterTradingCardGame.Logic.Exceptions {

    internal class ConflictException : Exception {

        // /////////////////////////////////////////////////////////////////////
        // Init
        // /////////////////////////////////////////////////////////////////////

        public ConflictException(string message) : base(message) {
            // noop
        }
    }
}
