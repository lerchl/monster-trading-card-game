namespace MonsterTradingCardGame.Data {

    /// <summary>
    ///     Utility class for regular expressions.
    /// </summary>
    public static class RegexUtils {

        /// <summary>
        ///     Regular expression for a valid username.
        /// </summary>
        public const string USERNAME = @"[a-zA-Z0-9]{3,30}";

        /// <summary>
        ///     Regular expression for a valid Guid.
        /// </summary>
        public const string GUID = @"[a-fA-F0-9]{8}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{12}";

        /// <summary>
        ///     Regular expression for a valid query parameter.
        /// </summary>
        public const string QUERY_PARAM = @"[-\w.~%]+";

        // /////////////////////////////////////////////////////////////////////
        // Requests
        // /////////////////////////////////////////////////////////////////////

        public const string HTTP_METHOD_GROUP_NAME = "httpMethod";

        /// <summary>
        ///     Regular expression group with the name <see cref="HTTP_METHOD_GROUP_NAME"/>
        ///     for capturing the http method from a request.
        /// </summary>
        private const string HTTP_METHOD_GROUP = $"(?'{HTTP_METHOD_GROUP_NAME}'\\w+)";

        public const string ENDPOINT_GROUP_NAME = "endpoint";

        /// <summary>
        ///     Regular expression group with the name <see cref="ENDPOINT_GROUP_NAME"/>
        ///     for capturing the endpoint from a request.
        /// </summary>
        private const string ENDPOINT_GROUP = $"(?'{ENDPOINT_GROUP_NAME}'([-\\w\\?\\/.~%=]*))";

        /// <summary>
        ///     Regular expression groups for capturing the http method and endpoint from a request.
        ///     See <see cref="HTTP_METHOD_GROUP_NAME"/> and <see cref="ENDPOINT_GROUP_NAME"/>.
        ///     See also: <seealso cref="HTTP_METHOD_GROUP"/> and <seealso cref="ENDPOINT_GROUP"/>.
        /// </summary>
        public const string REQUEST_GROUPS = $"^{HTTP_METHOD_GROUP} {ENDPOINT_GROUP}";

        public const string HEADERS_KEY_GROUP_NAME = "header";

        public const string HEADERS_VALUE_GROUP_NAME = "value";

        /// <summary>
        ///     Regular expression groups for capturing the headers from a request.
        ///     See <see cref="HEADERS_KEY_GROUP_NAME"/> and <see cref="HEADERS_VALUE_GROUP_NAME"/>.
        /// </summary>
        public const string HEADERS_GROUPS = $"^(?'{HEADERS_KEY_GROUP_NAME}'\\S+): (?'{HEADERS_VALUE_GROUP_NAME}'.+)$";

        /// <summary>
        ///     Regular expression for capturing the body from a request.
        /// </summary>
        public const string REQUEST_BODY = @"(\[(.|\r|\n)*\])|(\{(.|\r|\n)*\})";
    }
}
