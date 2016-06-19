namespace MyBitly.Tests.Fake
{
    using Common.Params;

    public class FakeParamsHelper : ParamsHelper, IParamsHelper
    {
        private readonly ParamsRaw _paramsRaw;

        public FakeParamsHelper(ParamsRaw paramsRaw)
        {
            this._paramsRaw = paramsRaw;
        }

        public new string ShortDomen
        {
            get { return this._paramsRaw.ShortDomen; }
        }
    }

    public class ParamsRaw
    {
        public ParamsRaw()
        {
            ShortDomen = "http://mybitly.com";
        }

        public string ShortDomen { get; set; }
    }
}