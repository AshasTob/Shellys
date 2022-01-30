using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DataAccess.Domain;

namespace DataAccess
{
    public class MenuRepository: IMenuRepository
    {
        private readonly IDbConnection _databaseConnection;

        public MenuRepository(IDbConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }

        public async Task Add(MenuItem item)
        {
            var query = @"INSERT INTO [dbo].[Menu] ([Name], [Price]) VALUES (@Name, @Price)";
            var queryParameters = new
            {
                item.Name, item.Price
            };

            await _databaseConnection.ExecuteScalarAsync(query, queryParameters);
        }

        public Task<MenuItem> GetItem(string name)
        {
            var query = @"SELECT * FROM [dbo].Menu WHERE Name=@Name";
            var queryParameters = new { Name = name };

            return _databaseConnection.QueryFirstAsync<MenuItem>(query, queryParameters);
        }

        public async Task<MenuItem[]> Get()
        {
            var query = @"SELECT * FROM [dbo].Menu";

            return (await _databaseConnection.QueryAsync<MenuItem>(query)).ToArray();
        }

        public Task Remove(string name)
        {
            var query = @"DELETE FROM [dbo].[Menu] WHERE Name=@Name";
            var queryParameters = new
            {
                Name=name
            };

            return _databaseConnection.ExecuteScalarAsync(query, queryParameters);
        }
    }
}
