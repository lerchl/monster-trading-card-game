namespace MonsterTradingCardGame.Server {

    public class HttpCode {

        public int Code { get; private set; }
        public string Message { get; private set; }

        public static readonly HttpCode OK_200 = new(200, "OK");
        public static readonly HttpCode CREATED_201 = new(201, "Created");
        public static readonly HttpCode NO_CONTENT_204 = new(204, "No Content");

        public static readonly HttpCode BAD_REQUEST_400 = new(400, "Bad Request");
        public static readonly HttpCode UNAUTHORIZED_401 = new(401, "Unauthorized");
        public static readonly HttpCode FORBIDDEN_403 = new(403, "Forbidden");
        public static readonly HttpCode NOT_FOUND_404 = new(404, "Not Found");
        public static readonly HttpCode CONFLICT_409 = new(409, "Conflict");

        public static readonly HttpCode INTERNAL_SERVER_ERROR_500 = new(500, "Internal Server Error");

        // /////////////////////////////////////////////////////////////////////
        // Constructor
        // /////////////////////////////////////////////////////////////////////

        public HttpCode(int code, string message) {
            Code = code;
            Message = message;
        }
    }
}
