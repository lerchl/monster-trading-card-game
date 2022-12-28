namespace MonsterTradingCardGame.Data {

    /// <summary>
    ///     Maps a class to a table. To use when their names do not match.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    internal class Table : Attribute {

        public string? Name { get; set; }
    }
}
