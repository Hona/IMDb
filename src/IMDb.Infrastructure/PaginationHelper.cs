namespace IMDb.Infrastructure
{
    public static class PaginationHelper
    {
        private const int ItemsPerPage = 20;

        public static (int Skip, int Take) GetPaginationData(this int page) =>
            (Skip: page * ItemsPerPage, Take: ItemsPerPage);
    }
}