using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Buttler_3._0.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        private ISender _sender;
        protected ISender Mediator => _sender ??= HttpContext.RequestServices.GetRequiredService<IMediator>();
    }
}
