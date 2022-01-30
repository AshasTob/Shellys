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

        public async Task<Order> Get(int id)
        {
            var query = @"SELECT * FROM [dbo].[Order] WHERE Id=@Id";
            var queryParameters = new { Id = id };
            return await _databaseConnection.QueryFirstAsync<Order>(query, queryParameters);
        }

        public async Task<int> Upsert(Order order)
        {
            var query = @"
                           UPDATE [dbo].[Order]
                           SET [TotalPrice] = @TotalPrice,
                               [Status] = @Status
                           WHERE [Id] = @Id
                           IF @@ROWCOUNT = 0
                           BEGIN
                              INSERT INTO [dbo].[Order] ([TotalPrice], [Status]) VALUES (@TotalPrice, @Status)
                              SELECT SCOPE_IDENTITY()
                           END";

            var queryParameters = new
            {
                order.Id,
                TotalPrice = order.TotalPrice,
                Status = (int)order.Status
            };

            int? id = await _databaseConnection.ExecuteScalarAsync<int?>(query, queryParameters);
            if (id == null)
            {
                return order.Id;
            }

            return id.Value;
        }
    }
}
