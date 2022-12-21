namespace MonsterTradingCardGame.Api {

    /// <summary>
    ///    Attribute to mark a parameter as a query parameter.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public class QueryParam : Attribute {

        public string? Name { get; set; }
    }
}
