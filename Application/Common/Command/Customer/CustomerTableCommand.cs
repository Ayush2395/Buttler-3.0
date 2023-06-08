using Buttler.Application.DTO;
using Buttler.Application.Repositories;
using MediatR;

namespace Buttler.Application.Common.Commanda.Customer
{
    public class CustomerTableCommand : IRequest<int>
    {
        public TablesDto Table { get; set; }
        public class Handler : IRequestHandler<CustomerTableCommand, int>
        {
            private readonly IBookTableRepo _bookTableRepo;

            public Handler(IBookTableRepo bookTableRepo)
            {
                _bookTableRepo = bookTableRepo;
            }

            public Task<int> Handle(CustomerTableCommand request, CancellationToken cancellationToken)
            {
                var tablenumber = _bookTableRepo.BookTableForCustomer(request.Table);
                return Task.FromResult(tablenumber);
            }
        }
    }
}
