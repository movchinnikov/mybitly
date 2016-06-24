namespace MyBitly.DAL.Utils.Helpers
{
    using System;
    using System.Configuration;
    using Common.Resources;

    public static class ConnectionStringHelper
    {
        private static string _connectionString;

        /// <summary>
        /// Строка подключения к sql
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_connectionString))
                    return _connectionString;

                _connectionString = ConfigurationManager.ConnectionStrings["MyBitly"].ConnectionString;

                if (string.IsNullOrWhiteSpace(_connectionString))
                    throw new ArgumentException(MyBitlyResources.ConnectionStringNotConfigure);

                return _connectionString;
            }
        } 
    }
}