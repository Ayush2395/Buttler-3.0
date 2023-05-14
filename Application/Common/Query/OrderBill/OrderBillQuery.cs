using Buttler.Application.DTO;
using Buttler.Application.Repositories;
using MediatR;

namespace Buttler.Application.Common.Query.OrderBill
{
    public class OrderBillQuery : IRequest<List<BillingDto>>
    {
        public class Handler : IRequestHandler<OrderBillQuery, List<BillingDto>>
        {
            private IBillingRepo _billingRepo;

            public Handler(IBillingRepo billingRepo)
            {
                _billingRepo = billingRepo;
            }

            public Task<List<BillingDto>> Handle(OrderBillQuery request, CancellationToken cancellationToken)
            {
                return Task.FromResult(_billingRepo.GetAllCustomerBills());
            }
        }
    }
}
