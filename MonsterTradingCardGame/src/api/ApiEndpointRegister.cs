using System.Reflection;
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
            Destination destination = httpRequest.destination;

            if (endpointTable.ContainsKey(destination)) {
                MethodInfo methodInfo = endpointTable[destination];
                ParameterInfo[] parameterInfos = methodInfo.GetParameters();
                var parameters = parameterInfos.Select((parameterInfo, index) => ParseParameter(parameterInfo, httpRequest)).ToArray();
                var returnValue = methodInfo.Invoke(null, parameters);

                if (returnValue is Response response) {
                    return response;
                }

                throw new ProgrammerFailException($"Return value of method '{methodInfo.Name}' " +
                                                  $"for {destination.method}-Request " +
                                                  $"to {destination.endpoint} is not of type Response");
            } else {
                _logger.Warn($"No endpoint found for {destination}");
                throw new NoSuchDestinationException(destination);
            }
        }

        private static object? ParseParameter(ParameterInfo parameterInfo, HttpRequest httpRequest) {
            var attributes = parameterInfo.GetCustomAttributesData();
            if (attributes.Any(cad => cad.AttributeType == typeof(Body))) {
                return new JsonSerializer().Deserialize(httpRequest.data, parameterInfo.ParameterType);
            } else if (attributes.Any(cad => cad.AttributeType == typeof(Header))) {
                // TODO: return value of header key from request
                return null;
            }
            // TODO: implement query parameters
            return null;
        }
    }
}
