using System.Reflection;
using Npgsql;

namespace MonsterTradingCardGame.Data {

    internal abstract class Repository<E> where E : Entity {

        protected readonly EntityManager _entityManager = EntityManager.Instance;

        // /////////////////////////////////////////////////////////////////////
        // Methods
        // /////////////////////////////////////////////////////////////////////

        /// <summary>
        ///     Saves the given entity.
        /// </summary>
        /// <param name="entity">The entity</param>
        /// <returns>The saved entity</returns>
        public E Save(E entity) {
            PropertyInfo[] properties = typeof(E).GetProperties().Where(p => p.Name != "id").ToArray();

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
        public E FindById(Guid id) {
            string query = $"SELECT * FROM {typeof(E).Name} WHERE id = :id";
            var command = new NpgsqlCommand(query, _entityManager.connection) {
                Parameters = {
                    new(":id", id)
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
            string query = $"SELECT * FROM {typeof(E).Name};";
            var result = new NpgsqlCommand(query, _entityManager.connection).ExecuteReader();
            return ConstructEntityList(result);
        }

        /// <summary>
        ///     Delete entity.
        /// </summary>
        /// <param name="id">Id of the entity</param>
        public void Delete(Guid id) {
            string query = $"DELETE FROM {typeof(E).Name} WHERE id = :id";
            var command = new NpgsqlCommand(query, _entityManager.connection) {
                Parameters = {
                    new(":id", id)
                }
            };
            command.ExecuteNonQuery();
        }

        // Helper
        // /////////////////////////////////////////////////////////////////////

        /// <summary>
        ///    Constructs an entity from a <see cref="NpgsqlDataReader" />.
        /// </summary>
        /// <param name="result">The result of the <see cref="NpgsqlCommand" /></param>
        /// <param name="close">Determines whether the result will be closed</param>
        /// <returns>The entity</returns>
        protected static E ConstructEntity(NpgsqlDataReader result, bool close = true) {
            if (!result.Read()) {
                result.Close();
                throw new NoResultException();
            }

            object?[] values = new object[result.FieldCount];
            for (int i = 0; i < result.FieldCount; i++) {
                object value = result.GetValue(i);
                values[i] = value.GetType() == typeof(DBNull) ? null : value;
            }

            if (close) {
                result.Close();
            }

            return (typeof(E).GetConstructors()[0].Invoke(values) as E)!;
        }

        /// <summary>
        ///     Constructs a list of entities from a <see cref="NpgsqlDataReader" />.
        /// </summary>
        /// <param name="result">The result of the <see cref="NpgsqlCommand"/></param>
        /// <returns>The list of entities</returns>
        /// <seealso cref="ConstructEntity(NpgsqlDataReader, bool)" />
        protected static List<E> ConstructEntityList(NpgsqlDataReader result) {
            List<E> entities = new();

            try {
               for (;;) {
                    entities.Add(ConstructEntity(result, false));
                }
            } catch (NoResultException) {
                // no more entities to construct
            }

            result.Close();
            return entities;
        }

        // Save
        // /////////////////////////////////////////////////////////////////////

        private E Insert(E entity, PropertyInfo[] properties) {
            string[] placeholders = PropertiesAsStrings(properties).Select(p => ":" + p).ToArray();

            string query = $"INSERT INTO {typeof(E).Name} " +
                           $"({string.Join(", ", PropertiesAsStrings(properties))}) " +
                           $"VALUES ({string.Join(", ", placeholders)}) RETURNING ID";

            var command = new NpgsqlCommand(query, _entityManager.connection);
            command.Parameters.AddRange(PropertiesAsParameters(properties, entity));

            Guid id = (Guid) command.ExecuteScalar()!;
            return FindById(id);
        }

        private E Update(E entity, PropertyInfo[] properties) {
            string[] placeholders = PropertiesAsStrings(properties).Select(p => p + " = :" + p).ToArray();

            string query = $"UPDATE {typeof(E).Name} " + 
                           $"SET {string.Join(", ", placeholders)} " +
                            "WHERE id = :id;";

            var command = new NpgsqlCommand(query, _entityManager.connection);
            command.Parameters.AddRange(PropertiesAsParameters(properties, entity));
            command.Parameters.Add(new NpgsqlParameter(":id", entity.id));
            command.ExecuteNonQuery();

            // id cannot be null, see Save method
            return FindById((Guid) entity.id!);
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
