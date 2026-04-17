namespace ClinicaMedica.Web.ViewModels.Shared
{
    public class PaginationViewModel
    {
        public int CurrentPage { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public int TotalItems { get; set; }

        public int TotalPages => PageSize == 0
    ? 1
    : (int)Math.Ceiling((double)TotalItems / PageSize);

        public int StartItem => TotalItems == 0 ? 0 : (CurrentPage - 1) * PageSize + 1;

        public int EndItem => Math.Min(CurrentPage * PageSize, TotalItems);

        public bool HasPreviousPage => CurrentPage > 1;

        public bool HasNextPage => CurrentPage < TotalPages;
    }
}