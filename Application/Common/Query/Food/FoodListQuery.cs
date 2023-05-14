using Buttler.Application.DTO;
using Buttler.Application.Repositories;
using MediatR;

namespace Buttler.Application.Common.Query.Food
{
    public class FoodListQuery : IRequest<List<FoodsDto>>
    {
        public class Handler : IRequestHandler<FoodListQuery, List<FoodsDto>>
        {
            private readonly IFoodRepo _foodRepo;

            public Handler(IFoodRepo foodRepo)
            {
                _foodRepo = foodRepo;
            }

            public Task<List<FoodsDto>> Handle(FoodListQuery request, CancellationToken cancellationToken)
            {
                return Task.FromResult(_foodRepo.FoodItems());
            }
        }
    }
}
