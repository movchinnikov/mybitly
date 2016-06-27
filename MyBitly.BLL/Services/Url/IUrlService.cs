namespace MyBitly.BLL.Services
{
	using Models;

	public interface IUrlService
	{
		/// <summary>
		/// Сокращение ссылок
		/// </summary>
		/// <param name="longUrl">Ссылка, которую надо сократить</param>
		/// <returns>Информация о сокращенной ссылке</returns>
		ShortenResponse Shorten(string longUrl);

		/// <summary>
		/// Получение информации о соркщенной ссылке
		/// </summary>
		/// <param name="hash">Хэш для сокращенной ссылки</param>
		/// <returns>Информация о сокращенной ссылке</returns>
		ShortenResponse Get(string hash);

		/// <summary>
		/// Получение информцацию о ссылках, сокращенных пользователем
		/// </summary>
		/// <param name="request">Параметры запроса</param>
		/// <returns>Список данных о сокращенных ссылках</returns>
		ListResponse LinkHistory(UrlHistoryRequest request);

		/// <summary>
		/// Увелиние счетчика перехода по сокращенной ссылке
		/// </summary>
		/// <param name="hash">Хэш для сокращенной ссылки</param>
		/// <returns>Информация о сокращенной ссылке</returns>
		ShortenResponse Increment(string hash);
	}
}