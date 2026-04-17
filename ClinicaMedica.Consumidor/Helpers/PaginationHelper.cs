using ClinicaMedica.Consumidor.ViewModels;

namespace ClinicaMedica.Consumidor.Helpers;

public static class PaginationHelper
{
    public static IReadOnlyList<T> Slice<T>(IEnumerable<T> source, int page, int pageSize)
    {
        return source
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }

    public static PaginationViewModel Create(int page, int totalItems, int pageSize, string? database = null)
    {
        var totalPages = Math.Max(1, (int)Math.Ceiling(totalItems / (double)pageSize));
        var currentPage = Math.Min(Math.Max(page, 1), totalPages);

        return new PaginationViewModel
        {
            CurrentPage = currentPage,
            TotalPages = totalPages,
            TotalItems = totalItems,
            PageSize = pageSize,
            Database = database
        };
    }
}
