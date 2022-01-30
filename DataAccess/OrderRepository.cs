using System.Data;
using System.Threading.Tasks;
using Dapper;
using DataAccess.Domain;

namespace DataAccess
{
    public class OrderRepository:IOrderRepository
    {
        private readonly IDbConnection _databaseConnection;

        public OrderRepository(IDbConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }

        public Task<Order> Get(int id)
        {
            var query = @"SELECT * FROM [dbo].Order WHERE Id=@id";
            var queryParameters = new { Id = id };

            return _databaseConnection.QueryFirstAsync<Order>(query, queryParameters);
        }

        public Task<int> Upsert(Order order)
        {
            var query = @"
                           UPDATE [dbo].[Order]
                           SET [TotalPrice] = @TotalPrice,
                               [Status] = @Status,
                           WHERE [Id] = @Id
                           IF @@ROWCOUNT = 0
                           BEGIN
                              INSERT INTO [dbo].[Order] ([TotalPrice], [Status]) VALUES (@TotalPrice, @Status)
                           END";

            var queryParameters = new
            {
                order.Id,
                order.TotalPrice,
                order.Status
            };

            return _databaseConnection.ExecuteScalarAsync<int>(query, queryParameters);
        }
    }
}
