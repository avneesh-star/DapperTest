namespace DapperTest.Models
{
    public class UserListResponseDTO
    {
        public List<Dictionary<string,object>> values { get; set; }
        public PaginationDTO pagination { get; set; }
    }
}
