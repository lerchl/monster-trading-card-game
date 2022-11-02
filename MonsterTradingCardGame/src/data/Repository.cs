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
            if (entity.id == null || entity.id == Guid.Empty) {
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
            PropertyInfo[] properties = typeof(T).GetProperties();
            string query = $"INSERT INTO {typeof(T).Name} ({PropertiesToString(properties)}) VALUES ({ValuesOfPropertiesToString(properties, entity)}) RETURNING id;";
            var uuid = new NpgsqlCommand(query, _entityManager.connection).ExecuteScalar();
            return FindById(uuid.ToString());
        }

        private static string PropertiesToString(PropertyInfo[] properties) {
            StringBuilder sb = new();
            foreach (PropertyInfo property in properties) {
                if (property.Name != "id") {
                    sb.Append(property.Name);
                    sb.Append(',');
                }
            }
            return sb.ToString()[..^1];
        }

        private static string ValuesOfPropertiesToString(PropertyInfo[] properties, T entity) {
            StringBuilder sb = new();
            foreach (PropertyInfo property in properties) {
                if (property.Name != "id") {
                    if (property.PropertyType == typeof(string)) {
                        sb.Append('\'');
                        sb.Append(property.GetValue(entity));
                        sb.Append('\'');
                    } else {
                        sb.Append(property.GetValue(entity));
                    }
                    sb.Append(',');
                }
            }
            return sb.ToString()[..^1];
        }

        private T? Update(T entity) {
            PropertyInfo[] properties = typeof(T).GetProperties();

            string query = $"UPDATE {typeof(T).Name} SET {FieldAndValuePairsToString(properties, entity)} WHERE id = :id RETURNING id;";
            var result = new NpgsqlCommand(query, _entityManager.connection) {
                Parameters = { new(":id", entity.id) }
            }.ExecuteScalar();

            return FindById(result.ToString());
        }

        private static string FieldAndValuePairsToString(PropertyInfo[] properties, T entity) {
            StringBuilder sb = new();
            foreach (PropertyInfo property in properties) {
                if (property.Name != "id") {
                    sb.Append(property.Name);
                    sb.Append(" = ");

                    if (property.PropertyType == typeof(string)) {
                        sb.Append('\'');
                        sb.Append(property.GetValue(entity));
                        sb.Append('\'');
                    } else {
                        sb.Append(property.GetValue(entity));
                    }

                    sb.Append(", ");
                }
            }
            return sb.ToString()[..^2];
        }
    }
}
