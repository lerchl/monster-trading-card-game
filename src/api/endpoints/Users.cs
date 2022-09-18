using System.Net.Http;
using Api;

namespace Api.Endpoints {

    internal class Users {

        private const string URL = "/users";

        [ApiEndpoint(httpMethod = HttpMethod.GET, url = URL)]
        public static void get() {
            // TODO
            Console.WriteLine("GET");
        }

        [ApiEndpoint(httpMethod = HttpMethod.POST, url = URL)]
        public static void post() {
            // TODO
            Console.WriteLine("POST");
        }

        [ApiEndpoint(httpMethod = HttpMethod.PUT, url = URL)]
        public static void put() {
            // TODO
            Console.WriteLine("PUT");
        }

        [ApiEndpoint(httpMethod = HttpMethod.DELETE, url = URL)]
        public static void delete() {
            // TODO
            Console.WriteLine("DELETE");
        }
    }
}
