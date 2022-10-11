namespace MonsterTradingCardGame.Api {

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ApiEndpoint : Attribute {
        
        public EHttpMethod httpMethod { get; set; }
        public string? url { get; set; }
    }
}
