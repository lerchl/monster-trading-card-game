using System.Reflection;
using System.Text;
using Npgsql;

namespace MonsterTradingCardGame.Data {

    internal abstract class Repository<T> where T : Entity {

        protected readonly EntityManager _entityManager = EntityManager.Instance;

        // /////////////////////////////////////////////////////////////////////
        // Methods
        // /////////////////////////////////////////////////////////////////////

        public T? Save(T entity) {
            if (entity.id == null) {
                return Insert(entity);
            } else {
                return Update(entity);
            }
        }

        public T? FindById(string id) {
            string query = $"SELECT * FROM {typeof(T).Name} WHERE id = '{id}';";
            var result = new NpgsqlCommand(query, _entityManager.connection).ExecuteReader();
            return ConstructEntity(result);
        }

        protected static T? ConstructEntity(NpgsqlDataReader result) {
            if (!result.Read()) {
                result.Close();
                return null;
            }

            object[] values = new object[result.FieldCount];
            for (int i = 0; i < result.FieldCount; i++) {
                values[i] = result.GetValue(i);
            }
            result.Close();

            return typeof(T).GetConstructors()[0].Invoke(values) as T;
        }

        // Save
        // /////////////////////////////////////////////////////////////////////

        private T? Insert(T entity) {
            FieldInfo[] fields = typeof(T).GetFields();
            string query = $"INSERT INTO {typeof(T).Name} ({FieldsToString(fields)}) VALUES ({ValuesOfFieldsToString(fields, entity)}) RETURNING id;";
            var uuid = new NpgsqlCommand(query, _entityManager.connection).ExecuteScalar();
            return FindById(uuid.ToString());
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
                    if (field.FieldType == typeof(string)) {
                        sb.Append('\'');
                        sb.Append(field.GetValue(entity));
                        sb.Append('\'');
                    } else {
                        sb.Append(field.GetValue(entity));
                    }
                    sb.Append(',');
                }
            }
            return sb.ToString()[..^1];
        }

        private T? Update(T entity) {
            FieldInfo[] fields = typeof(T).GetFields();

            string query = $"UPDATE {typeof(T).Name} SET {FieldAndValuePairsToString(fields, entity)} WHERE id = :id RETURNING id;";
            var result = new NpgsqlCommand(query, _entityManager.connection) {
                Parameters = { new(":id", entity.id) }
            }.ExecuteScalar();

            return FindById(result.ToString());
        }

        private static string FieldAndValuePairsToString(FieldInfo[] fields, T entity) {
            StringBuilder sb = new();
            foreach (FieldInfo field in fields) {
                if (field.Name != "id") {
                    sb.Append(field.Name);
                    sb.Append(" = ");

                    if (field.FieldType == typeof(string)) {
                        sb.Append('\'');
                        sb.Append(field.GetValue(entity));
                        sb.Append('\'');
                    } else {
                        sb.Append(field.GetValue(entity));
                    }

                    sb.Append(',');
                }
            }
            return sb.ToString()[..^1];
        }
    }
}
