namespace MyBitly.Common.Params
{
    public interface IParamsHelper
    {
        /// <summary>
        /// Укороченный домен
        /// </summary>
        string ShortDomen { get; } 

        /// <summary>
        /// Строка подключения к sql
        /// </summary>
        string ConnectionString { get; }
    }
}