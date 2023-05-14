using Buttler.Application.DTO;
using Buttler.Application.Repositories;
using MediatR;

namespace Buttler.Application.Common.Commanda.Customer
{
    public class CustomerTableCommand : IRequest<ResultDto<bool>>
    {
        public TablesDto Table { get; set; }
        public class Handler : IRequestHandler<CustomerTableCommand, ResultDto<bool>>
        {
            private readonly IBookTableRepo _bookTableRepo;

            public Handler(IBookTableRepo bookTableRepo)
            {
                _bookTableRepo = bookTableRepo;
            }

            public Task<ResultDto<bool>> Handle(CustomerTableCommand request, CancellationToken cancellationToken)
            {
                _bookTableRepo.BookTableForCustomer(request.Table);
                return Task.FromResult(new ResultDto<bool>(true, "Table booked."));
            }
        }
    }
}
