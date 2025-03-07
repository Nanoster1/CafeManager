
using CafeManager.Core.Models.Orders;
using CafeManager.Core.Repositories;
using CafeManager.Data.EFCore.Common.Classes;

namespace CafeManager.Data.EFCore.Repositories;

public class OrderRepository(CafeManagerContext context) : BaseRepository<Order, long>(context), IOrderRepository
{
}