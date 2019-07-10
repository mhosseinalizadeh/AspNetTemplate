using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;
using System.Text;


namespace AspNetTemplate.DataAccess.UnitOfWork
{
    public class ExcelUnitOfWork : IUnitOfWork
    {
        private string _connectionString;
        private IDbConnection _connection;
        private IDbTransaction  _transaction;
        private bool _disposed;

        public ExcelUnitOfWork(string connectionString)
        {
            _connectionString = connectionString;
            createNewConnection();
        }
        public void Commit()
        {
            try
            {
                _transaction.Commit();
            }
            catch
            {
                _transaction.Rollback();
            }
            finally
            {
                _transaction.Dispose();

                createNewConnection();
            }
        }

        public IDbTransaction Transaction
        {
            get
            {
                if (_transaction.Connection.State != ConnectionState.Closed)
                    return _transaction;
                else
                {
                    createNewConnection();
                    return _transaction;
                }
            }
        }

        private void createNewConnection() {
            _connection = new OleDbConnection(_connectionString);
            _connection.Open();
            _transaction = _connection.BeginTransaction();
        }

        private void dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_transaction != null)
                    {
                        _transaction.Dispose();
                        _transaction = null;
                    }
                    if (_connection != null)
                    {
                        _connection.Dispose();
                        _connection = null;
                    }
                }
                _disposed = true;
            }
        }

        ~ExcelUnitOfWork()
        {
            dispose(false);
        }
    }
}
