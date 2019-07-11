using AspNetTemplate.DataAccess.Repository.IRepository;
using AspNetTemplate.DataAccess.Repository.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace AspNetTemplate.DataAccess.UnitOfWork
{
    public class DapperUnitOfWork : IUnitOfWork
    {
        #region Fields
        private IDbConnection _connection;
        private IDbTransaction _transaction;
        private IUserRepository _userRepository;

        private string _connectionString;
        private bool _disposed;
        #endregion

        public DapperUnitOfWork(string connectionString)
        {
            _connectionString = connectionString;
            _connection = new SqlConnection(connectionString);
            _connection.Open();
            _transaction = _connection.BeginTransaction();
        }

        public IDbTransaction Transaction
        {
            get
            {
                if (_transaction.Connection.State != ConnectionState.Closed)
                    return _transaction;
                else
                {
                    _connection = new SqlConnection(_connectionString);
                    _connection.Open();
                    _transaction = _connection.BeginTransaction();
                    return _transaction;
                }
            }
        }


        public DapperUnitOfWork(SqlConnection connection)
        {
            _connection = connection;
            if (_connection.State == ConnectionState.Closed)
                _connection.Open();
            _transaction = _connection.BeginTransaction();
        }

        #region IUnitOfWork Members

        public IUserRepository UserRepository
        {
            get
            {
                return _userRepository
                    ?? (_userRepository = new UserRepository(_transaction));
            }
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

                _connection = new SqlConnection(_connectionString);
                _connection.Open();
                _transaction = _connection.BeginTransaction();
                resetRepositories();
            }
        }

        public void Dispose()
        {
            dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Private Methods
        private void resetRepositories()
        {
            _userRepository = new UserRepository(_transaction);

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

        ~DapperUnitOfWork()
        {
            dispose(false);
        }
        #endregion
    }
}
