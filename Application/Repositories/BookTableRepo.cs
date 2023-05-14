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

        public void BookTableForCustomer(TablesDto table)
        {
            if (table != null)
            {
                _context.Tables.Add(new()
                {
                    TableNumber = table.TableNumber,
                    CustomerId = table.CustomerId,
                });
                _context.SaveChanges();
            }
        }

        public Customers TakeCustomerDetails(CustomerDto customer)
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
            }
            var customers = _context.Customers.OrderBy(rec => rec.CustomerId).LastOrDefault();
            return customers != null ? customers : null!;
        }
    }

    public interface IBookTableRepo
    {
        Customers TakeCustomerDetails(CustomerDto customer);
        void BookTableForCustomer(TablesDto table);
    }
}
