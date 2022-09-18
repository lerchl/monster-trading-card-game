namespace Api {

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    internal class ApiEndpoint : Attribute {
        
        public HttpMethod httpMethod { get; set; }
        public string? url { get; set; }
    }
}
