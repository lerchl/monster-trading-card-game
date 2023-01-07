namespace MonsterTradingCardGame.Data.Mapping {

    /// <summary>
    ///     Marks a property as not setable.
    ///     Not setable properties are not saved to the database.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    internal abstract class NotSetable : Attribute {
        // noop
    }
}
