using System.Runtime.Intrinsics.X86;
using System.Reflection;
using System.Text;
using Npgsql;
using Npgsql.Internal.TypeHandlers;

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

        public T? FindById(string id) {
            string query = $"SELECT * FROM {typeof(T).Name} WHERE id = '{id}';";
            var result = new NpgsqlCommand(query, _entityManager.connection).ExecuteReader();

            if (!result.Read()) {
                return null;
            }

            
            result.GetFieldValue<object>(0);

            Console.WriteLine(result.GetGuid(0));
            Console.WriteLine(result.GetString(1));
            Console.WriteLine(result.GetString(2));
            return null;
        }

        // Save
        // /////////////////////////////////////////////////////////////////////

        private T Insert(T entity) {
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
