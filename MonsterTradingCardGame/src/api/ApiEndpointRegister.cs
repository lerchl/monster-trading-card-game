using System.Net.Sockets;
using System.Reflection;
using MonsterTradingCardGame.Server;
using Newtonsoft.Json.Linq;

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
                        Console.WriteLine("[INFO] Found endpoint {0}", methodInfo.Name);
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

        public void Execute(HttpRequest httpRequest, Socket client) {
            Destination destination = httpRequest.destination;

            if (endpointTable.ContainsKey(destination)) {
                MethodInfo methodInfo = endpointTable[destination];
                ParameterInfo[] parameterInfos = methodInfo.GetParameters();

                object[] parameters = new object[parameterInfos.Length];
                parameters[0] = client;
                for (int i = 1; i < parameterInfos.Length; i++) {
                    ParameterInfo parameterInfo = parameterInfos[i];
                    // TODO: Check if the value is present first and answer with an error if not
                    parameters[i] = httpRequest.data[parameterInfo.Name].Value<string>();
                }
                methodInfo.Invoke(null, parameters);
            } else {
                _logger.Warn($"No endpoint found for {destination}");
            }
        }
    }
}
