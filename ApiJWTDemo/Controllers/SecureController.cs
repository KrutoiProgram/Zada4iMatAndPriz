using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiJwtDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecureController : ControllerBase
    {
        [HttpGet("secret")] // /api/secure/secret
        public string SecretString() => "Защищеный метод, доступный только после аутентификации";


    }
}
