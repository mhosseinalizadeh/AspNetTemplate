using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Security;
using System.Threading.Tasks;
using System.Transactions;
using Dapper;

namespace PropSearch.DataAccess.Repositories
{
    public abstract class RepositoryBase
    {
        private IDbTransaction _transaction;
        private IDbConnection Connection { get { return _transaction.Connection; } }

        public RepositoryBase(IDbTransaction transaction)
        {
            _transaction = transaction;
        }

        protected T ExecuteScalar<T>(string sql, object param)
        {
            return Connection.ExecuteScalar<T>(sql, param, _transaction);
        }

        protected async Task<T> ExecuteScalarAsync<T>(string sql, object param)
        {
            return await Connection.ExecuteScalarAsync<T>(sql, param, _transaction);
        }

        protected T QuerySingleOrDefault<T>(string sql, object param)
        {
            return Connection.QuerySingleOrDefault<T>(sql, param, _transaction);
        }

        protected async Task<T> QuerySingleOrDefaultAsync<T>(string sql, object param)
        {
            return await Connection.QuerySingleOrDefaultAsync<T>(sql, param, _transaction);
        }

        protected IEnumerable<T> Query<T>(string sql, object param = null)
        {
            return Connection.Query<T>(sql, param, _transaction);
        }

        protected async Task<IEnumerable<TFirst>> QueryAsync<TFirst, TSecond>(string sql, Func<TFirst, TSecond, TFirst> map ,object param = null, string splitOn = "Id")
        {
            return await Connection.QueryAsync<TFirst,TSecond,TFirst>(sql,  map, param,_transaction,true, splitOn);
        }
        protected async Task<IEnumerable<TFirst>> QueryAsync<TFirst, TSecond, TThird>(string sql, Func<TFirst, TSecond,TThird, TFirst> map, object param = null, string splitOn = "Id")
        {
            return await Connection.QueryAsync<TFirst, TSecond,TThird, TFirst>(sql, map, param, _transaction, true, splitOn);
        }

        protected async Task<IEnumerable<TFirst>> QueryAsync<TFirst, TSecond, TThird, TFourth>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFirst> map, object param = null, string splitOn = "Id")
        {
            return await Connection.QueryAsync<TFirst, TSecond, TThird, TFourth, TFirst>(sql, map, param, _transaction, true, splitOn);
        }

        protected async Task<IEnumerable<TFirst>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TFirst> map, object param = null, string splitOn = "Id")
        {
            return await Connection.QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth ,TFirst>(sql, map, param, _transaction, true, splitOn);
        }

        protected async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null)
        {
            return await Connection.QueryAsync<T>(sql, param, _transaction);
        }

        protected void Execute(string sql, object param)
        {
            Connection.Execute(sql, param, _transaction);
        }

        protected async Task ExecuteAsync(string sql, object param)
        {
            await Connection.ExecuteAsync(sql, param, _transaction);
        }

        protected async Task SqlBulkInsert(DataTable table, string destinationTable, string connectionString) {               
            using (SqlBulkCopy copy = new SqlBulkCopy((SqlConnection)_transaction.Connection, SqlBulkCopyOptions.KeepIdentity, (SqlTransaction)_transaction))
            {
                try
                {
                copy.DestinationTableName = destinationTable;
                copy.ColumnMappings.Remove(new SqlBulkCopyColumnMapping("RecNo", "RecNo"));
                copy.ColumnMappings.Add("ImageName", "ImageName");
                copy.ColumnMappings.Add("CompanyCode", "CompanyCode");
                copy.ColumnMappings.Add("Barcode", "Barcode");
                            
                    await copy.WriteToServerAsync(table);
                    //newTrans.Commit();
                }
                catch (Exception ex)
                {
                    //newTrans.Rollback();
                    throw ex;
                }

            }
        }
    }
}
