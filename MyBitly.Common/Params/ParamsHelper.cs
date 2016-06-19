namespace MyBitly.Common.Params
{
    using System;
    using System.Configuration;
    using Resources;

    public class ParamsHelper : IParamsHelper
    {
        private string _shortDomen;
        private string _connectionString;

        public string ShortDomen
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this._shortDomen))
                    return this._shortDomen;

                this._shortDomen = ConfigurationManager.AppSettings["shortDomen"];

                if (string.IsNullOrWhiteSpace(this._shortDomen))
                    throw new ArgumentException(MyBitlyResources.ShortDomenNotConfigure);

                return this._shortDomen;
            }
        }

        public string ConnectionString
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this._connectionString))
                    return this._connectionString;

                this._connectionString = ConfigurationManager.ConnectionStrings["MyBitly"].ConnectionString;

                if (string.IsNullOrWhiteSpace(this._connectionString))
                    throw new ArgumentException(MyBitlyResources.ConnectionStringNotConfigure);

                return this._connectionString;
            }
        }
    }
}