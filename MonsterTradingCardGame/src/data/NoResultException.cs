namespace MonsterTradingCardGame.Data {

    /// <summary>
    ///    Exception for when no result is found.
    /// </summary>
    internal class NoResultException : Exception {

        // /////////////////////////////////////////////////////////////////////
        // Init
        // /////////////////////////////////////////////////////////////////////

        public NoResultException(string message) : base(message) {
            // noop
        }
    }
}
