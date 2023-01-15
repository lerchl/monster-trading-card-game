using System.Reflection;
using MonsterTradingCardGame.Data.Mapping;
using Npgsql;

namespace MonsterTradingCardGame.Data {

    public abstract class Repository<E> where E : Entity {

        private readonly string _tableName = typeof(E).GetCustomAttribute<Table>()?.Name ?? typeof(E).Name;

        // /////////////////////////////////////////////////////////////////////
        // Methods
        // /////////////////////////////////////////////////////////////////////

        /// <summary>
        ///     Saves the given entity.
        /// </summary>
        /// <param name="entity">The entity</param>
        /// <returns>The saved entity</returns>
        public virtual E Save(E entity) {
            PropertyInfo[] properties = typeof(E).GetProperties().Where(p => p.GetCustomAttribute<NotSetable>() == null).ToArray();

            if (entity.IsPersisted()) {
                return Update(entity, properties);
            } else {
                return Insert(entity, properties);
            }
        }

        /// <summary>
        ///     Find entity by its id.
        /// </summary>
        /// <param name="id">Id of the entity</param>
        /// <returns>The entity</returns>
        public virtual E FindById(Guid id) {
            string query = $"SELECT * FROM {_tableName} WHERE Id = :Id";
            var command = new NpgsqlCommand(query, EntityManager.Instance.Connection) {
                Parameters = {
                    new(":Id", id)
                }
            };
            var result = command.ExecuteReader();
            return ConstructEntity(result);
        }

        /// <summary>
        ///     Find all entities.
        /// </summary>
        /// <returns>List of entities</returns>
        public List<E> FindAll() {
            string query = $"SELECT * FROM {_tableName};";
            var result = new NpgsqlCommand(query, EntityManager.Instance.Connection).ExecuteReader();
            return ConstructEntityList(result);
        }

        /// <summary>
        ///     Delete entity.
        /// </summary>
        /// <param name="id">Id of the entity</param>
        public void Delete(Guid id) {
            string query = $"DELETE FROM {_tableName} WHERE Id = :Id";
            var command = new NpgsqlCommand(query, EntityManager.Instance.Connection) {
                Parameters = {
                    new(":Id", id)
                }
            };
            command.ExecuteNonQuery();
        }

        // Helper
        // /////////////////////////////////////////////////////////////////////

        public static T Construct<T>(NpgsqlDataReader result, bool close = true) {
            if (!result.Read()) {
                result.Close();
                throw new NoResultException($"{typeof(T).Name} not found");
            }

            object?[] values = new object[result.FieldCount];
            for (int i = 0; i < result.FieldCount; i++) {
                object value = result.GetValue(i);
                values[i] = value.GetType() == typeof(DBNull) ? null : value;
            }

            if (close) {
                result.Close();
            }

            return (T) typeof(T).GetConstructors()[0].Invoke(values)!;
        }

        public static List<T> ConstructList<T>(NpgsqlDataReader result) {
            List<T> objects = new();

            try {
                for (;;) {
                    objects.Add(Construct<T>(result, false));
                }
            } catch (NoResultException) {
                // no more objects to construct
            }

            result.Close();
            return objects;
        }

        /// <summary>
        ///    Constructs an entity from a <see cref="NpgsqlDataReader" />.
        /// </summary>
        /// <param name="result">The result of the <see cref="NpgsqlCommand" /></param>
        /// <param name="close">Determines whether the result will be closed</param>
        /// <returns>The entity</returns>
        protected static E ConstructEntity(NpgsqlDataReader result, bool close = true) {
            return Construct<E>(result, close);
        }

        /// <summary>
        ///     Constructs a list of entities from a <see cref="NpgsqlDataReader" />.
        /// </summary>
        /// <param name="result">The result of the <see cref="NpgsqlCommand"/></param>
        /// <returns>The list of entities</returns>
        /// <seealso cref="ConstructEntity(NpgsqlDataReader, bool)" />
        protected static List<E> ConstructEntityList(NpgsqlDataReader result) {
            return ConstructList<E>(result);
        }

        // Save
        // /////////////////////////////////////////////////////////////////////

        private E Insert(E entity, PropertyInfo[] properties) {
            string[] placeholders = PropertiesAsStrings(properties).Select(p => ":" + p).ToArray();

            string query = $"INSERT INTO {_tableName} " +
                           $"({string.Join(", ", PropertiesAsStrings(properties))}) " +
                           $"VALUES ({string.Join(", ", placeholders)}) RETURNING ID";

            var command = new NpgsqlCommand(query, EntityManager.Instance.Connection);
            command.Parameters.AddRange(PropertiesAsParameters(properties, entity));

            Guid id = (Guid) command.ExecuteScalar()!;
            return FindById(id);
        }

        private E Update(E entity, PropertyInfo[] properties) {
            string[] placeholders = PropertiesAsStrings(properties).Select(p => p + " = :" + p).ToArray();

            string query = $"UPDATE {_tableName} " + 
                           $"SET {string.Join(", ", placeholders)} " +
                            "WHERE Id = :Id;";

            var command = new NpgsqlCommand(query, EntityManager.Instance.Connection);
            command.Parameters.AddRange(PropertiesAsParameters(properties, entity));
            command.Parameters.Add(new NpgsqlParameter(":Id", entity.Id));
            command.ExecuteNonQuery();

            return FindById(entity.Id);
        }

        private static string[] PropertiesAsStrings(PropertyInfo[] properties) {
            return properties.Select(p => {
                Column? column = p.GetCustomAttribute<Column>();
                return column == null ? p.Name : column.Name!;
            }).ToArray();
        }

        private static NpgsqlParameter[] PropertiesAsParameters(PropertyInfo[] properties, E entity) {
            return properties.Select(p => {
                Column? column = p.GetCustomAttribute<Column>();

                object? value = null;
                if (p.PropertyType.IsEnum && p.GetValue(entity) != null) {
                    // result of getValue cannot be null,
                    // because it is checked in the if statement
                    value = (int) p.GetValue(entity)!;
                } else {
                    value = p.GetValue(entity);
                }

                return new NpgsqlParameter(":" + (column == null ? p.Name : column.Name), value ?? DBNull.Value);
            }).ToArray();
        }
    }
}
