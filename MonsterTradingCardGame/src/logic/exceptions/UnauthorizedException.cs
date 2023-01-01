namespace MonsterTradingCardGame.Logic.Exceptions {

    public class UnauthorizedException : Exception {

        // /////////////////////////////////////////////////////////////////////
        // Init
        // /////////////////////////////////////////////////////////////////////

        public UnauthorizedException(string message) : base(message) {
            // noop
        }
    }
}
