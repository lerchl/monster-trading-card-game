namespace MonsterTradingCardGame.Logic.Exceptions {

    internal class UnauthorizedException : Exception {

        // /////////////////////////////////////////////////////////////////////
        // Init
        // /////////////////////////////////////////////////////////////////////

        public UnauthorizedException(string message) : base(message) {
            // noop
        }
    }
}
