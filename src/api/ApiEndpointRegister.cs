using System.Reflection;

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

        public void execute(Destination destination) {
            endpointTable[destination].Invoke(null, null);
        }
    }
}
