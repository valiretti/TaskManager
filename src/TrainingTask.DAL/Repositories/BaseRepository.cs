using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using TrainingTask.Common.Models;

namespace TrainingTask.DAL.Repositories
{
    public abstract class BaseRepository
    {
        protected string ConnectionString { get; }

        protected BaseRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public virtual int Create(FormattableString sqlExpression)
        {
            var (sql, parameters) = GetSqlWithParameters(sqlExpression);
            return Create(sql, parameters);
        }

        public virtual int Create(string sqlExpression, IDictionary<string, object> parameters)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                foreach (var parameter in parameters)
                {
                    command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                }
                SqlParameter idParam = new SqlParameter("@id", SqlDbType.Int) { Direction = ParameterDirection.Output };
                command.Parameters.Add(idParam);
                command.ExecuteNonQuery();

                return (int)idParam.Value;
            }
        }

        public virtual void Update(FormattableString sqlExpression)
        {
            var (sql, parameters) = GetSqlWithParameters(sqlExpression);
            Update(sql, parameters);
        }

        public virtual void Update(string sqlExpression, IDictionary<string, object> parameters)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                foreach (var parameter in parameters)
                {
                    command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                }
                command.ExecuteNonQuery();
            }
        }

        public virtual void Delete(string sqlExpression, IDictionary<string, object> parameters)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                if (parameters != null)
                {
                    foreach (var parameter in parameters)
                    {
                        command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                    }
                }

                command.ExecuteNonQuery();
            }
        }

        public virtual void Delete(FormattableString sqlExpression)
        {
            var (sql, parameters) = GetSqlWithParameters(sqlExpression);
            Delete(sql, parameters);
        }

        public virtual IEnumerable<T> GetAll<T>(FormattableString sqlExpression, Func<IDataRecord, T> map)
        {
            var (sql, parameters) = GetSqlWithParameters(sqlExpression);
            return GetAll(sql, parameters, map);
        }

        public virtual IEnumerable<T> GetAll<T>(string sqlExpression,
              IDictionary<string, object> parameters, Func<IDataRecord, T> map)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                if (parameters != null)
                {
                    foreach (var parameter in parameters)
                    {
                        command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                    }
                }

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            yield return map(reader);
                        }
                    }
                }
            }
        }

        public virtual int Count(string tableName)
        {
            string sqlExpression = $"SELECT COUNT(*) FROM {tableName}";
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                object count = command.ExecuteScalar();

                return Convert.ToInt32(count);
            }
        }

        private static (string sql, Dictionary<string, object> parameters) GetSqlWithParameters(FormattableString sqlExpression)
        {
            var parameters = sqlExpression.GetArguments()
                .Select((p, i) => new { Key = $"@p{i}", Value = p })
                .ToDictionary(x => x.Key, x => x.Value);
            var sql = string.Format(sqlExpression.Format, sqlExpression.GetArguments().Select((_, i) => $"@p{i}").ToArray());
            return (sql, parameters);
        }
    }
}
