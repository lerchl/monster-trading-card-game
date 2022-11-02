namespace MonsterTradingCardGame.Api {

    internal class NoSuchDestinationException : Exception {

        public NoSuchDestinationException(Destination destination)
                : base($"no destination for {destination.method} to ${destination.endpoint}") {
            // noop
        }
    }
}
