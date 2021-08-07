using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using static Dapper.SqlMapper;

namespace Repository
{
	public class RepositoryBase
	{
		#region Fields
		private readonly string connectionString;
		#endregion

		#region Properties
		private string ConnectionString = "Data Source=Liquid;Initial Catalog=NavtechPrelims;Integrated Security=True";
		#endregion


		#region Query execution methods
		/// <summary>
		/// Used to execute query with parameter pass to it
		/// </summary>
		/// <param name="query">query can be simple query or stored procedure</param>
		/// <param name="parameters">parameters to be pass with stored procedure</param>
		/// <param name="isStoredProcedure">flag for query whether it is stored procedur or not</param>
		/// <returns>returns integer value based on execution of query</returns>
		public int Execute(string query, DynamicParameters parameters, bool isStoredProcedure = false)
		{
			using (var connection = new SqlConnection(ConnectionString))
			{
				connection.Open();
				return connection.Execute(query, parameters, commandType: isStoredProcedure ? CommandType.StoredProcedure : CommandType.Text);
			}
		}

		/// <summary>
		/// Used to execute query with parameter pass to it for an individual item to fetch
		/// </summary>
		/// <typeparam name="T">Type of item that needs to be returned</typeparam>
		/// <param name="query">query can be simple query or stored procedure</param>
		/// <param name="parameters">parameters to be pass with stored procedure</param>
		/// <param name="isStoredProcedure">flag for query whether it is stored procedur or not</param>
		/// <returns>returns typed value based on execution of query</returns>
		public T ExecuteFirstOrDefault<T>(string query, DynamicParameters parameters, bool isStoredProcedure = false)
		{
			using (var connection = new SqlConnection(ConnectionString))
			{
				connection.Open();
				return connection.QueryFirstOrDefault<T>(query, parameters, commandType: isStoredProcedure ? CommandType.StoredProcedure : CommandType.Text);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T">Type of items to be returned</typeparam>
		/// <param name="query">query can be simple query or stored procedure</param>
		/// <param name="parameters">parameters to be pass with stored procedure</param>
		/// <param name="isStoredProcedure">flag for query whether it is stored procedur or not</param>
		/// <returns>returns typed list based on execution of query</returns>
		public List<T> ExecuteList<T>(string query, DynamicParameters parameters, bool isStoredProcedure = false)
		{
			using (var connection = new SqlConnection(ConnectionString))
			{
				connection.Open();
				return connection.Query<T>(query, parameters, commandType: isStoredProcedure ? CommandType.StoredProcedure : CommandType.Text).ToList();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T1">Type of items to be returned</typeparam>
		/// <typeparam name="T2">Type of items to be returned</typeparam>
		/// <param name="query">query can be simple query or stored procedure</param>
		/// <param name="parameters">parameters to be pass with stored procedure</param>
		/// <param name="isStoredProcedure">flag for query whether it is stored procedur or not</param>
		/// <returns>returns typed two lists based on execution of query</returns>
		public Tuple<List<T1>, List<T2>> ExecuteMultipleQuery<T1, T2>(string query, DynamicParameters parameters, bool isStoredProcedure = false)
		{
			using (var connection = new SqlConnection(ConnectionString))
			{
				connection.Open();
				var response = connection.QueryMultiple(query, parameters, commandType: isStoredProcedure ? CommandType.StoredProcedure : CommandType.Text);
				var result1 = response.Read<T1>().ToList();
				var result2 = response.Read<T2>().ToList();
				return new Tuple<List<T1>, List<T2>>(result1, result2);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T1">Type of items to be returned</typeparam>
		/// <typeparam name="T2">Type of items to be returned</typeparam>
		/// <param name="query">query can be simple query or stored procedure</param>
		/// <param name="parameters">parameters to be pass with stored procedure</param>
		/// <param name="isStoredProcedure">flag for query whether it is stored procedur or not</param>
		/// <returns>returns typed two lists based on execution of query</returns>
		public GridReader ExecuteMultipleQuery(string query, DynamicParameters parameters, bool isStoredProcedure = false)
		{
			using (var connection = new SqlConnection(ConnectionString))
			{
				connection.Open();
				var response = connection.QueryMultiple(query, parameters, commandType: isStoredProcedure ? CommandType.StoredProcedure : CommandType.Text);
				return response;
			}
		}
		#endregion
	}
}
