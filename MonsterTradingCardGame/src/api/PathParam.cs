namespace MonsterTradingCardGame.Api {

    /// <summary>
    ///    Attribute to mark a parameter as a path parameter.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public class PathParam : Attribute {

        /// <summary>
        ///    Must match the name of the regex group
        ///    used to register the endpoint.
        /// </summary>
        public string? Name { get; set; }
    }
}
