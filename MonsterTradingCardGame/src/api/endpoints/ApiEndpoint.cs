namespace MonsterTradingCardGame.Api {

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    internal class ApiEndpoint : Attribute {
        
        public EHttpMethod httpMethod { get; set; }
        public string? url { get; set; }
    }
}
