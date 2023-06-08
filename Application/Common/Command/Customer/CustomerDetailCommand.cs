using Buttler.Application.DTO;
using Buttler.Application.Repositories;
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

            public async Task<int> Handle(CustomerDetailCommand request, CancellationToken cancellationToken)
            {
                return await _bookTable.TakeCustomerDetails(request.Customer);
            }
        }
    }
}
