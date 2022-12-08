using DapperTest.Models;

namespace DapperTest.Services
{
    public class StudentService
    {
        private readonly IDapper _dapper;

        public StudentService(IDapper dapper)
        {
            _dapper = dapper;
        }

    }
}
