using data;
using Npgsql;

namespace Data {

    internal abstract class Repository<T> where T : Entity {

        public void FindById(string id) {
            string query = @"SELECT * FROM $1 WHERE id == $2";
            var result = new NpgsqlCommand(query, EntityManager.Instance.connection) {
                Parameters = {
                    new NpgsqlParameter("$1", typeof(T).Name.ToLower()),
                    new NpgsqlParameter("$2", id)
                }
            }.ExecuteScalar();
            Console.WriteLine(result);
        }
    }
}