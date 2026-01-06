namespace Entities.RequestFeatures
{
    public class PageList<T> : List<T>
    {
        public PaginationMetaData Pagination { get; set; }
        public PageList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
        {
            Pagination = new PaginationMetaData
            {
                TotalCount = count,
                PageSize = pageSize,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize)
            };
            AddRange(items);
        }

        public static PageList<T> ToPageList(IEnumerable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            return new PageList<T>(items, count, pageNumber, pageSize);
        }
    }
}
