namespace MyBitly.Tests.Fake
{
	using Common.Params;

	public class FakeParamsHelper : ParamsHelper, IParamsHelper
	{
		private readonly ParamsRaw _paramsRaw;

		public FakeParamsHelper(ParamsRaw paramsRaw)
		{
			_paramsRaw = paramsRaw;
		}

		public new string ShortDomen
		{
			get { return _paramsRaw.ShortDomen; }
		}
	}
}