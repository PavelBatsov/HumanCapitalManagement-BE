using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace HCM.Domain.Helpers
{
    public class UserHelper
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public UserHelper(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public Guid CurrentUserId()
            => Guid.Parse(httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        public string CurrentUserRole()
            => httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value;
    }
}
