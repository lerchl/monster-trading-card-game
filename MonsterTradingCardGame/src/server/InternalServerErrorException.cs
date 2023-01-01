namespace MonsterTradingCardGame.Server {

    internal class InternalServerErrorException : Exception {

        public InternalServerErrorException(string message) : base(message) {
            // noop
        }
    }
}
