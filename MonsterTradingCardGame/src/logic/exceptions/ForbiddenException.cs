namespace MonsterTradingCardGame.Logic.Exceptions {

    internal class ForbiddenException : Exception {

        public ForbiddenException(string message) : base(message) {
            // noop
        }
    }
}
