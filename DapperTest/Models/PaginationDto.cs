namespace DapperTest.Models
{
    public class PaginationDTO
    {
        private int _totalRecords;
        public PaginationDTO(int Page, int PageSize, int totalRecords)
        {
            this.currentPage = Page;
            _totalRecords = totalRecords;
            totalPage = (int)Math.Ceiling((double)totalRecords / PageSize);
        }

        public int currentPage { get; set; }
        public int totalPage { get; set; }

        public int totalRecords
        {
            get { return _totalRecords; }
        }

    }
}
