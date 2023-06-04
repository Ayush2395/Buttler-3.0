using Buttler.Application.DTO;
using Buttler.Domain.Data;
using Buttler.Domain.Models;

namespace Buttler.Application.Repositories
{
    public class BookTableRepo : IBookTableRepo
    {
        private readonly ButtlerContext _context;

        public BookTableRepo(ButtlerContext context)
        {
            _context = context;
        }

        public int BookTableForCustomer(TablesDto table)
        {
            if (table != null && _context.Customers.Any(r => r.CustomerId == table.CustomerId))
            {
                _context.Tables.Add(new()
                {
                    TableNumber = table.TableNumber,
                    CustomerId = table.CustomerId,
                });
                _context.SaveChanges();
                return _context.Tables.OrderBy(r => r.TablesId).LastOrDefault()!.TableNumber;
            }
            return 0;
        }

        public int TakeCustomerDetails(CustomerDto customer)
        {
            if (customer != null)
            {
                _context.Customers.Add(new()
                {
                    CustomerName = customer.CustomerName,
                    Gender = customer.CustomerGender,
                    PhoneNumber = customer.PhoneNumber,
                });
                _context.SaveChanges();
                var id = _context.Customers.OrderBy(r => r.CustomerId).LastOrDefault()!.CustomerId;

                return id;
            }
            return 0;
        }
    }

    public interface IBookTableRepo
    {
        int TakeCustomerDetails(CustomerDto customer);
        int BookTableForCustomer(TablesDto table);
    }
}
