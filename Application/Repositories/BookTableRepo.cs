using Buttler.Application.DTO;
using Buttler.Domain.Data;
using Microsoft.Extensions.Logging;

namespace Buttler.Application.Repositories
{
    public class BookTableRepo : IBookTableRepo
    {
        private readonly ButtlerContext _context;
        private readonly ILogger<string> _logger;

        public BookTableRepo(ButtlerContext context, ILogger<string> logger)
        {
            _context = context;
            _logger = logger;
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

        public async Task<int> TakeCustomerDetails(CustomerDto customer)
        {
            try
            {
                if (customer != null && customer.PhoneNumber.Length == 10)
                {
                    _context.Customers.Add(new()
                    {
                        CustomerName = customer.CustomerName,
                        Gender = customer.CustomerGender,
                        PhoneNumber = customer.PhoneNumber,
                    });
                    await _context.SaveChangesAsync();
                    var id = _context.Customers.OrderBy(r => r.CustomerId).LastOrDefault()!.CustomerId;

                    return id;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while adding customer");
                throw;
            }
            return 0;
        }
    }

    public interface IBookTableRepo
    {
        Task<int> TakeCustomerDetails(CustomerDto customer);
        int BookTableForCustomer(TablesDto table);
    }
}
