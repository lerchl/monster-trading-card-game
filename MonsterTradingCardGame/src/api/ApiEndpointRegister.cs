using System.Reflection;
using System.Text.RegularExpressions;
using MonsterTradingCardGame.Data;
using MonsterTradingCardGame.Logic.Exceptions;
using MonsterTradingCardGame.Server;
using Newtonsoft.Json;

namespace MonsterTradingCardGame.Api {

    /// <summary>
    ///     Class for registering <see cref="ApiEndpoint"/>s.
    /// </summary>
    public class ApiEndpointRegister {

        public const string URL_EXCEPTION_MESSAGE = "Url of method '{methodName}' " +
                                                    "for {httpMethodName}-Request is not set!";
        public const string RETURN_TYPE_EXCEPTION_MESSAGE = "Return value of method '{methodName}' " +
                                                            "for {httpMethodName}-Request to {url} " +
                                                            "is not of type Response";

        private static readonly Logger<ApiEndpointRegister> _logger = new();

        private readonly Dictionary<Destination, MethodInfo> endpointTable = new();

        // /////////////////////////////////////////////////////////////////////
        // Constructor
        // /////////////////////////////////////////////////////////////////////

        public ApiEndpointRegister(params Type[] endpointClasses) {
            foreach (Type endpointClass in endpointClasses) {
                MethodInfo[] endpoints = endpointClass.GetMethods();
                foreach (MethodInfo endpoint in endpoints) {
                    ApiEndpoint? endpointAttribute = endpoint.GetCustomAttribute<ApiEndpoint>();
                    if (endpointAttribute != null) {
                        _logger.Info("Found endpoint " + endpoint.Name);

                        // check if endpoint has set an url
                        if (endpointAttribute.Url == null) {
                            string message = URL_EXCEPTION_MESSAGE
                                    .Replace("{methodName}", endpoint.Name)
                                    .Replace("{httpMethodName}", endpointAttribute.HttpMethod.ToString());
                            throw new ProgrammerFailException(message);
                        }

                        // check if endpoint's return type is Response
                        if (endpoint.ReturnType != typeof(Response)) {
                            string message = RETURN_TYPE_EXCEPTION_MESSAGE
                                    .Replace("{methodName}", endpoint.Name)
                                    .Replace("{httpMethodName}", endpointAttribute.HttpMethod.ToString())
                                    .Replace("{url}", endpointAttribute.Url);
                            throw new ProgrammerFailException(message);
                        }

                        endpointTable.TryAdd(new Destination(endpointAttribute.HttpMethod, endpointAttribute.Url), endpoint);
                    }
                }
            }
        }

        // /////////////////////////////////////////////////////////////////////
        // Methods
        // /////////////////////////////////////////////////////////////////////

        public Response Execute(HttpRequest httpRequest) {
            Destination destination = httpRequest.Destination;
            MethodInfo? methodInfo;

            if (endpointTable.ContainsKey(destination)) {
                methodInfo = endpointTable[destination];
            } else {
                // exact match for endpoint could not be found
                // might be an endpoint with path params
                Destination? genericDestination = endpointTable.Keys.ToList().FirstOrDefault(d => GenericDestinationMatches(d, destination));

                // generic endpoint could also not be found
                // endpoint does not exist
                if (genericDestination == null) {
                    _logger.Warn($"No endpoint found for {destination}");
                    throw new NoSuchDestinationException(destination);
                }

                destination = genericDestination;
                methodInfo = endpointTable[genericDestination];
            }

            Token? token = null;
            ApiEndpoint apiEndpointAttribute = methodInfo.GetCustomAttribute<ApiEndpoint>()!;

            if (apiEndpointAttribute.RequiresAuthentication) {
                token = CheckAuthorization(httpRequest.Headers);
                if (token == null) {
                    _logger.Info("Unauthorized request to authorized-only endpoint");
                    return SessionHandler.UNAUTHORIZED_RESPONSE;
                }
            }

            ParameterInfo[] parameterInfos = methodInfo.GetParameters();
            var parameters = parameterInfos.Select((parameterInfo, index) => ParseParameter(parameterInfo, httpRequest, destination, token)).ToArray();

            try {
                // both cast and converting to non-null cannot fail here
                // due to the checks done in the constructor
                return (Response) methodInfo.Invoke(null, parameters)!;
            } catch (InternalServerErrorException e) {
                _logger.Error(e.Message);
                return new Response(HttpCode.INTERNAL_SERVER_ERROR_500);
            } catch (Exception e) {
                _logger.Error($"Unexpected exception of type {e.GetType().Name}:");
                _logger.Error(e.Message);
                return new Response(HttpCode.INTERNAL_SERVER_ERROR_500);
            }
        }

        /// <summary
        ///    Checks if a <see cref="Destination"/> matches a generic <see cref="Destination"/>.
        /// </summary>
        /// <param name="genericDestination">The generic destination</param>
        /// <param name="destination">The destination</param>
        private static bool GenericDestinationMatches(Destination genericDestination, Destination destination) {
            return genericDestination.method == destination.method && new Regex(genericDestination.endpoint).IsMatch(destination.endpoint);
        }

        /// <summary>
        ///    Parses a parameter.
        ///    See: <see cref="Bearer"/>, <see cref="Header"/>, <see cref="PathParam"/>, <see cref="QueryParam"/> and <see cref="Body"/>
        /// </summary>
        /// <param name="parameterInfo">Describes how the parameter should be parsed</param>
        /// <param name="httpRequest">The http request</param>
        /// <param name="genericDestination">The generic destination</param>
        /// <param name="token">The token</param>
        /// <returns>The parsed parameter</returns>
        private static object? ParseParameter(ParameterInfo parameterInfo, HttpRequest httpRequest, Destination genericDestination, Token? token) {
            Bearer? bearerAttribute = parameterInfo.GetCustomAttribute<Bearer>();
            PathParam? pathParamAttribute = parameterInfo.GetCustomAttribute<PathParam>();
            QueryParam? queryParamAttribute = parameterInfo.GetCustomAttribute<QueryParam>();
            Body? bodyAttribute = parameterInfo.GetCustomAttribute<Body>();

            if (bearerAttribute != null) {
                return token;
            } else if (pathParamAttribute != null) {
                // cannot be null as parameterInfo.Name can only be null
                // if it is the ParameterInfo of a return value
                string name = pathParamAttribute.Name ?? parameterInfo.Name!;
                Regex regex = new(genericDestination.endpoint);
                Match match = regex.Match(httpRequest.Destination.endpoint);

                return match.Groups[name].Value;
            } else if (queryParamAttribute != null) {
                // cannot be null as parameterInfo.Name can only be null
                // if it is the ParameterInfo of a return value
                string name = queryParamAttribute.Name ?? parameterInfo.Name!;
                string pattern = name + "=(?'" + name + "'" + RegexUtils.QUERY_PARAM + ")";
                Regex regex = new(pattern);
                Match match = regex.Match(httpRequest.Destination.endpoint);

                return match.Groups[name].Value;
            } else if (bodyAttribute != null) {
                if (httpRequest.Data == null) {
                    throw new BadRequestException("Body is empty");
                }

                return new JsonSerializer().Deserialize(httpRequest.Data, parameterInfo.ParameterType);
            }

            return null;
        }

        /// <summary>
        ///    Checks if a <see cref="HttpRequest"/> is authorized to access an <see cref="ApiEndpoint"/>.
        /// </summary>
        /// <param name="headers">The http request's headers</param>
        /// <returns>The <see cref="Token"/> if the request is authorized, <see langword="null"/> otherwise.</returns>
        private static Token? CheckAuthorization(Dictionary<string, string> headers) {
            // request does not contain bearer token, is is therefore unauthorized
            if (!headers.ContainsKey("Authorization")) {
                return null;
            }

            string bearer = headers["Authorization"];
            return SessionHandler.Instance.GetSession(bearer);
        }
    }
}
