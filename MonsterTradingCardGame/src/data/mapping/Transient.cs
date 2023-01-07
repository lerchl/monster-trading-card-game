using MonsterTradingCardGame.Data.Mapping;

namespace MonsterTradingCardGame.Data {

    /// <summary>
    ///     Marks a property as transient.
    ///     Transient properties are not saved/read to/from the database.
    /// </summary>
    internal class Transient : NotSetable {
        // noop
    }
}
