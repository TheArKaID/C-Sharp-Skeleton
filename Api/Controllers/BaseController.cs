using Microsoft.AspNetCore.Mvc;

namespace RSAHyundai.Api.Controllers
{
    [Asp.Versioning.ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    { }
}
