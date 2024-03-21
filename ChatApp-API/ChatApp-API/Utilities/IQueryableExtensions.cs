using ChatApp_API.DTOs;

namespace ChatApp_API.Utilities
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, Pagination pagination)
        {
            return queryable
                .Skip((pagination.Page - 1) * pagination.Quantity)
                .Take(pagination.Quantity);
        }
    }
}
