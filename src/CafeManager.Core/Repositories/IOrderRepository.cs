using CafeManager.Core.Common.Interfaces;
using CafeManager.Core.Models.Orders;

namespace CafeManager.Core.Repositories;

public interface IOrderRepository : IBaseRepository<Order, long>
{

}