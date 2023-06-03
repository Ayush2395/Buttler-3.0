using Buttler.Application.DTO;
using Buttler.Application.Repositories;
using Buttler.Domain.Models;
using MediatR;

namespace Buttler.Application.Common.Commanda.Customer
{
    public class CustomerDetailCommand : IRequest<CustomerDto>
    {
        public CustomerDto Customer { get; set; }
        public class Handler : IRequestHandler<CustomerDetailCommand, CustomerDto>
        {
            private readonly IBookTableRepo _bookTable;

            public Handler(IBookTableRepo bookTable)
            {
                _bookTable = bookTable;
            }

            public Task<CustomerDto> Handle(CustomerDetailCommand request, CancellationToken cancellationToken)
            {
                return Task.FromResult(_bookTable.TakeCustomerDetails(request.Customer));
            }
        }
    }
}
