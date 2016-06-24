namespace MyBitly.Common.Params
{
    using System;
    using System.Configuration;
    using Resources;

    public class ParamsHelper : IParamsHelper
    {
        private string _shortDomen;

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
    }
}