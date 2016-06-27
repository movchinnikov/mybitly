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
                if (!string.IsNullOrWhiteSpace(_shortDomen))
                    return _shortDomen;

                _shortDomen = ConfigurationManager.AppSettings["shortDomen"];

                if (string.IsNullOrWhiteSpace(_shortDomen))
                    throw new ArgumentException(MyBitlyResources.ShortDomenNotConfigure);

                return _shortDomen;
            }
        }
    }
}