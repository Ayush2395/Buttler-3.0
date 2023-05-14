using Buttler.Application.DTO;
using Buttler.Application.Repositories;
using Buttler.Domain.Models;
using MediatR;

namespace Buttler.Application.Common.Commanda.Customer
{
    public class CustomerDetailCommand : IRequest<Customers>
    {
        public CustomerDto Customer { get; set; }
        public class Handler : IRequestHandler<CustomerDetailCommand, Customers>
        {
            private readonly IBookTableRepo _bookTable;

            public Handler(IBookTableRepo bookTable)
            {
                _bookTable = bookTable;
            }

            public Task<Customers> Handle(CustomerDetailCommand request, CancellationToken cancellationToken)
            {
                return Task.FromResult(_bookTable.TakeCustomerDetails(request.Customer));
            }
        }
    }
}
