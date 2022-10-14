namespace MonsterTradingCardGame.Server {

    internal class HttpCode {

        public int Code { get; private set; }
        public string Message { get; private set; }

        public static HttpCode OK_200 = new(200, "OK");
        public static HttpCode CREATED_201 = new(201, "Created");

        public static HttpCode BAD_REQUEST_400 = new(400, "Bad Request");
        public static HttpCode UNAUTHORIZED_401 = new(401, "Unauthorized");
        public static HttpCode FORBIDDEN_403 = new(403, "Forbidden");
        public static HttpCode NOT_FOUND_404 = new(404, "Not Found");

        // /////////////////////////////////////////////////////////////////////
        // Constructor
        // /////////////////////////////////////////////////////////////////////

        public HttpCode(int code, string message) {
            Code = code;
            Message = message;
        }
    }
}
