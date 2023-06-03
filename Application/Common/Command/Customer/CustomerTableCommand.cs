using Buttler.Application.DTO;
using Buttler.Application.Repositories;
using MediatR;

namespace Buttler.Application.Common.Commanda.Customer
{
    public class CustomerTableCommand : IRequest<ResultDto<object>>
    {
        public TablesDto Table { get; set; }
        public class Handler : IRequestHandler<CustomerTableCommand, ResultDto<object>>
        {
            private readonly IBookTableRepo _bookTableRepo;

            public Handler(IBookTableRepo bookTableRepo)
            {
                _bookTableRepo = bookTableRepo;
            }

            public Task<ResultDto<object>> Handle(CustomerTableCommand request, CancellationToken cancellationToken)
            {
                var tablenumber = _bookTableRepo.BookTableForCustomer(request.Table);
                return Task.FromResult(new ResultDto<object>(true, new { tableNumber = tablenumber }, "Table booked."));
            }
        }
    }
}
