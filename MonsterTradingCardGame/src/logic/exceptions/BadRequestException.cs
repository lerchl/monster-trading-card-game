namespace MonsterTradingCardGame.Logic.Exceptions {

    internal class BadRequestException : Exception {

        // /////////////////////////////////////////////////////////////////////
        // Init
        // /////////////////////////////////////////////////////////////////////

        public BadRequestException(string message) : base(message) {
            // noop
        }
    }
}
