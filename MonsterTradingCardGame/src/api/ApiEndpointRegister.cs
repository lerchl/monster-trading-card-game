using System.Reflection;
using System.Text.RegularExpressions;
using MonsterTradingCardGame.Server;
using Newtonsoft.Json;

namespace MonsterTradingCardGame.Api {

    public class ApiEndpointRegister {

        private static readonly Logger<ApiEndpointRegister> _logger = new();

        private readonly Dictionary<Destination, MethodInfo> endpointTable = new();

        // /////////////////////////////////////////////////////////////////////
        // Constructor
        // /////////////////////////////////////////////////////////////////////

        public ApiEndpointRegister(params Type[] endpoints) {
            foreach (Type endpoint in endpoints) {
                MethodInfo[] methodInfos = endpoint.GetMethods();
                foreach (MethodInfo methodInfo in methodInfos) {
                    // TODO: durch GetCustomAttribute ersetzen
                    var attribute = methodInfo.GetCustomAttributesData()
                            .FirstOrDefault(cad => cad.AttributeType == typeof(ApiEndpoint));
                    if (attribute != null) {
                        _logger.Info("Found endpoint " + methodInfo.Name);
                        EHttpMethod httpMethod = (EHttpMethod) attribute.NamedArguments[0].TypedValue.Value;
                        string url = (string) attribute.NamedArguments[1].TypedValue.Value;
                        endpointTable.TryAdd(new Destination(httpMethod, url), methodInfo);
                    }
                }
            }
        }

        // /////////////////////////////////////////////////////////////////////
        // Methods
        // /////////////////////////////////////////////////////////////////////

        public Response Execute(HttpRequest httpRequest) {
            Destination destination = httpRequest.Destination;
            MethodInfo? methodInfo = endpointTable[destination];

            // exact match for endpoint could not be found
            // might be an endpoint with path params
            if (methodInfo == null) {
                Destination? genericDestination = endpointTable.Keys.ToList().FirstOrDefault(genericDestination => new Regex(genericDestination.endpoint).IsMatch(destination.endpoint));

                // generic endpoint could also not be found
                // endpoint does not exist
                if (genericDestination == null) {
                    _logger.Warn($"No endpoint found for {destination}");
                    throw new NoSuchDestinationException(destination);
                }

                methodInfo = endpointTable[genericDestination];
            }

            ParameterInfo[] parameterInfos = methodInfo.GetParameters();
            var parameters = parameterInfos.Select((parameterInfo, index) => ParseParameter(parameterInfo, httpRequest)).ToArray();
            var returnValue = methodInfo.Invoke(null, parameters);

            if (returnValue is Response response) {
                return response;
            }

            throw new ProgrammerFailException($"Return value of method '{methodInfo.Name}' " +
                                              $"for {destination.method}-Request " +
                                              $"to {destination.endpoint} is not of type Response");
        }

        private static object? ParseParameter(ParameterInfo parameterInfo, HttpRequest httpRequest) {
            Header? headerAttribute = parameterInfo.GetCustomAttribute<Header>();
            PathParam? pathParamAttribute = parameterInfo.GetCustomAttribute<PathParam>();
            QueryParam? queryParamAttribute = parameterInfo.GetCustomAttribute<QueryParam>();
            Body? bodyAttribute = parameterInfo.GetCustomAttribute<Body>();

            if (headerAttribute != null) {
                var headers = httpRequest.Headers;
                if (headers.ContainsKey(headerAttribute.Name)) {
                    return headers[headerAttribute.Name];
                }

                // TODO: maybe throw a different error, also needs to be caught
                //       maybe not throw exception but return null and have invoked
                //       method handle it
                throw new MissingFieldException($"Header '{headerAttribute.Name}' is missing");
            } else if (pathParamAttribute != null) {
                // TODO: path params
            } else if (queryParamAttribute != null) {
                // TODO: query params
            } else if (bodyAttribute != null) {
                return new JsonSerializer().Deserialize(httpRequest.Data, parameterInfo.ParameterType);
            }

            return null;
        }
    }
}
