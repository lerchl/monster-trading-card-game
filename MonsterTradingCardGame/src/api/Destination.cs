namespace MonsterTradingCardGame.Api {

    public class Destination {

        public readonly EHttpMethod method;
        public readonly string endpoint;

        // /////////////////////////////////////////////////////////////////////
        // Constructor
        // /////////////////////////////////////////////////////////////////////

        public Destination(EHttpMethod method, string endpoint) {
            this.method = method;
            this.endpoint = endpoint;
        }

        // /////////////////////////////////////////////////////////////////////
        // Methods
        // /////////////////////////////////////////////////////////////////////

        public override string ToString() {
            return $"{method}-Request to {endpoint}";
        }

        public override bool Equals(object? obj) {
            if (obj == null || GetType() != obj.GetType()) {
                return false;
            }

            Destination other = (Destination) obj;

            return method == other.method && endpoint.Equals(other.endpoint);
        }

        public override int GetHashCode() {
            return method.GetHashCode() * endpoint.GetHashCode() % int.MaxValue;
        }
    }
}