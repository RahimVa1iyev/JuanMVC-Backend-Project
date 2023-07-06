namespace JuanMVC.Areas.Manage.ViewModels
{
    public class PaginatedList<T>
    {
        public PaginatedList(List<T> ıtems, int pageIndex, int totalPage)
        {
            Items = ıtems;
            PageIndex = pageIndex;
            TotalPage = totalPage;
        }

        public List<T> Items { get; set; }

        public int PageIndex { get; set; }

        public int TotalPage { get; set; }

        public bool HasPrev  => PageIndex > 1; 

        public bool HasNext => PageIndex < TotalPage;


        public static PaginatedList<T> Create(IQueryable<T> query , int pageIndex , int size)
        {
            var items = query.Skip((pageIndex-1) * size).Take(size).ToList();
            var totalPage = (int)Math.Ceiling(query.Count() / (double)size);    

            return new PaginatedList<T>(items, pageIndex, totalPage);
        }

    }
}
