namespace MonsterTradingCardGame.Api {

    /// <summary>
    ///     Wrapper for single <see cref="Guid"/> JSON request body
    /// </summary>
    internal class GuidWrapper {

        public string Id { get; }

        // /////////////////////////////////////////////////////////////////////
        // Init
        // /////////////////////////////////////////////////////////////////////

        public GuidWrapper(string id) {
            Id = id;
        }
    }
}
