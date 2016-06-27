namespace MyBitly.DAL.Models
{
	using System.Collections.Generic;
	using Entities;

	public class ListPage<TEntity>
		where TEntity : EntityBase
	{
		/// <summary>
		/// Возвращемые данные
		/// </summary>
		public IEnumerable<TEntity> Data { get; set; }

		/// <summary>
		/// Общее количество данных в таблице, соответствующих указанному запросу
		/// </summary>
		public int TotalCount { get; set; }
	}
}