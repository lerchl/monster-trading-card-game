using System.Reflection;
using Newtonsoft.Json.Linq;

namespace Api {

    internal class ApiEndpointRegister {

        public readonly Dictionary<Destination, MethodInfo> endpointTable = new Dictionary<Destination, MethodInfo>();

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
                        HttpMethod httpMethod = (HttpMethod) attribute.NamedArguments[0].TypedValue.Value;
                        string url = (string) attribute.NamedArguments[1].TypedValue.Value;
                        endpointTable.TryAdd(new Destination(httpMethod, url), methodInfo);
                    }
                }
            }
        }

        // /////////////////////////////////////////////////////////////////////
        // Methods
        // /////////////////////////////////////////////////////////////////////

        public void execute(Destination destination, JObject data) {
            if (endpointTable.ContainsKey(destination)) {
                MethodInfo methodInfo = endpointTable[destination];
                ParameterInfo[] parameterInfos = methodInfo.GetParameters();
                object[] parameters = new object[parameterInfos.Length];
                for (int i = 0; i < parameterInfos.Length; i++) {
                    ParameterInfo parameterInfo = parameterInfos[i];
                    // TODO: Check if the value is present first and answert with an error if not
                    parameters[i] = data[parameterInfo.Name].Value<string>();
                }
                methodInfo.Invoke(null, parameters);
            } else {
                Logger.Instance.Warn($"No endpoint found for {destination}");
            }
        }
    }
}
