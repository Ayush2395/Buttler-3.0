using Buttler.Application.DTO;
using Buttler.Application.Repositories;
using Buttler.Domain.Models;
using MediatR;

namespace Buttler.Application.Common.Commanda.Customer
{
    public class CustomerDetailCommand : IRequest<int>
    {
        public CustomerDto Customer { get; set; }
        public class Handler : IRequestHandler<CustomerDetailCommand, int>
        {
            private readonly IBookTableRepo _bookTable;

            public Handler(IBookTableRepo bookTable)
            {
                _bookTable = bookTable;
            }

            public Task<int> Handle(CustomerDetailCommand request, CancellationToken cancellationToken)
            {
                return Task.FromResult(_bookTable.TakeCustomerDetails(request.Customer));
            }
        }
    }
}
