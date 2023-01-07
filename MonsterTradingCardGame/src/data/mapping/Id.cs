using MonsterTradingCardGame.Data.Mapping;

namespace MonsterTradingCardGame.Data {

    /// <summary>
    ///     Marks a property as the Id (usually the primary key) of an <see cref="Entity"/>.
    ///     The Id will not be taken into consideration when saving the entity.
    ///     See: <seealso cref="Repository{E}.Save(E)"/>
    /// </summary>
    internal class Id : NotSetable {
        // noop
    }
}
