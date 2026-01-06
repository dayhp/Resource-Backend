namespace Entities.RequestFeatures
{
    public class PagedResponse<T>
    {
        public IEnumerable<T> Data { get; set; }
        public PaginationMetaData Pagination { get; set; }
        public PagedResponse(IEnumerable<T> data, PaginationMetaData pagination)
        {
            Data = data;
            Pagination = pagination;
        }
    }
}
