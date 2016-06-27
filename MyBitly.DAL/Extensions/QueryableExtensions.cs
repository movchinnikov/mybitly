namespace MyBitly.DAL.Extensions
{
	using System;
	using System.Linq;
	using System.Linq.Expressions;

	public static class QueryableExtensions
	{
		public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string ordering)
		{
			var type = typeof(T);
			var property = type.GetProperty(ordering);
			var parameter = Expression.Parameter(type, "p");
			var propertyAccess = Expression.MakeMemberAccess(parameter, property);
			var orderByExp = Expression.Lambda(propertyAccess, parameter);
			var resultExp = Expression.Call(typeof(Queryable), "OrderBy", new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExp));
			return source.Provider.CreateQuery<T>(resultExp);
		}

		public static IQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string ordering)
		{
			var type = typeof(T);
			var property = type.GetProperty(ordering);
			var parameter = Expression.Parameter(type, "p");
			var propertyAccess = Expression.MakeMemberAccess(parameter, property);
			var orderByExp = Expression.Lambda(propertyAccess, parameter);
			var resultExp = Expression.Call(typeof(Queryable), "OrderByDescending", new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExp));
			return source.Provider.CreateQuery<T>(resultExp);
		}
	}
}