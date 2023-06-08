using Buttler.Application.DTO;
using Buttler.Domain.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Buttler.Application.Common.Command.Admin
{
    public class UpdateOrderStatusCommand : IRequest<ResultDto<bool>>
    {
        public int OrderStatus { get; set; }
        public int OrderMasterId { get; set; }
        public class Handler : IRequestHandler<UpdateOrderStatusCommand, ResultDto<bool>>
        {
            private readonly ButtlerContext _context;
            private readonly ILogger<string> _logger;

            public Handler(ButtlerContext context, ILogger<string> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<ResultDto<bool>> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var orderStatus = await _context.OrderMasters.Where(r => r.OrderMasterId == request.OrderMasterId).FirstOrDefaultAsync(cancellationToken: cancellationToken);
                    if (orderStatus != null)
                    {
                        orderStatus.OrderStatus = request.OrderStatus;
                        await _context.SaveChangesAsync(cancellationToken);
                        return new ResultDto<bool>(true, true, "Order status has been updated.");
                    }
                    return new ResultDto<bool>(false, false, "Order status is not updated, because its not found.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occured, while updating status.");
                    throw;
                }
            }
        }
    }
}
