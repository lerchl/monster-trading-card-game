using System.Reflection;
using System.Text;
using Npgsql;

namespace MonsterTradingCardGame.Data {

    internal abstract class Repository<T> where T : Entity {

        protected readonly EntityManager _entityManager = EntityManager.Instance;

        // /////////////////////////////////////////////////////////////////////
        // Methods
        // /////////////////////////////////////////////////////////////////////

        public T Save(T entity) {
            if (entity.id == null) {
                return Insert(entity);
            } else {
                return Update(entity);
            }
        }

        public void FindById(string id) {
            string query = @"SELECT * FROM $1 WHERE id == $2;";
            var result = new NpgsqlCommand(query, _entityManager.connection) {
                Parameters = {
                    new("$1", typeof(T).Name.ToLower()),
                    new("$2", id)
                }
            }.ExecuteScalar();
            Console.WriteLine(result);
        }

        // Save
        // /////////////////////////////////////////////////////////////////////

        private T Insert(T entity) {
            FieldInfo[] fields = typeof(T).GetFields();

            string query = $"INSERT INTO {typeof(T).Name} ({FieldsToString(fields)}) VALUES ({ValuesOfFieldsToString(fields, entity)});";
            // var result = new NpgsqlCommand(query, _entityManager.connection) {
            //     Parameters = {
            //         new("$1", ValuesOfFieldsToString(fields, entity))
            //     }
            // }.ExecuteScalar();
            var result = new NpgsqlCommand(query, _entityManager.connection).ExecuteScalar();
            return entity;
        }

        private static string FieldsToString(FieldInfo[] fields) {
            StringBuilder sb = new();
            foreach (FieldInfo field in fields) {
                if (field.Name != "id") {
                    sb.Append(field.Name);
                    sb.Append(',');
                }
            }
            return sb.ToString()[..^1];
        }

        private static string ValuesOfFieldsToString(FieldInfo[] fields, T entity) {
            StringBuilder sb = new();
            foreach (FieldInfo field in fields) {
                if (field.Name != "id") {
                    sb.Append(field.GetValue(entity));
                    sb.Append(',');
                }
            }
            return sb.ToString()[..^1];
        }

        private T Update(T entity) {
            FieldInfo[] fields = typeof(T).GetFields();

            string query = @"UPDATE $1 SET $2 WHERE id = $3;";
            var result = new NpgsqlCommand(query, _entityManager.connection) {
                Parameters = {
                    new("$1", typeof(T).Name),
                    new("$2", FieldAndValuePairsToString(fields, entity)),
                    new("$3", entity.id)
                }
            }.ExecuteScalar();
            return entity;
        }

        private static string FieldAndValuePairsToString(FieldInfo[] fields, T entity) {
            StringBuilder sb = new();
            foreach (FieldInfo field in fields) {
                if (field.Name != "id") {
                    sb.Append(field.Name);
                    sb.Append(" = ");
                    sb.Append(field.GetValue(entity));
                    sb.Append(',');
                }
            }
            return sb.ToString()[..^1];
        }
    }
}
