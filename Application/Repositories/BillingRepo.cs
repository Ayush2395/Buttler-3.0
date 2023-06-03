using Buttler.Application.DTO;
using Buttler.Domain.Data;

namespace Buttler.Application.Repositories
{
    public class BillingRepo : IBillingRepo
    {
        private readonly ButtlerContext _context;

        public BillingRepo(ButtlerContext context)
        {
            _context = context;
        }

        public List<BillingDto> GetAllCustomerBills()
        {
            var result = (from customer in _context.Customers
                          join ordMst in _context.OrderMasters
                          on customer.CustomerId equals ordMst.CustomerId
                          select new BillingDto()
                          {
                              CustomerName = customer.CustomerName,
                              CustomerPhoneNumber = customer.PhoneNumber,
                              OrderStatus = ordMst.OrderStatus,
                              Bill = ordMst.TotalBill,
                              DateOfOrder = ordMst.DateOfOrder,
                          }).ToList();
            return result;
        }
    }

    public interface IBillingRepo
    {
        List<BillingDto> GetAllCustomerBills();
    }
}
