namespace MonsterTradingCardGame.Api {

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ApiEndpoint : Attribute {
        
        public EHttpMethod HttpMethod { get; set; }
        public string? Url { get; set; }
    }
}
