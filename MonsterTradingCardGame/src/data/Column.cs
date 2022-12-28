namespace MonsterTradingCardGame.Data {

    /// <summary
    ///     Maps a property to a column. To use when their names do not match.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    internal class Column : Attribute {

        public string? Name { get; set; }
    }
}
