using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using System.Security.Claims;
public class LoginReq : IAuthorizationRequirement
{
    protected internal string Login { get; set; }

    public LoginReq(string login)
    {
        Login = login;
    }
}

 
public class AgeHandler : AuthorizationHandler<LoginReq>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, LoginReq requirement)
    {
        if (context.User.HasClaim(c => c.Type == ClaimTypes.Name))
        {
                if (context.User.FindFirst(c => c.Type == ClaimTypes.Name).Value == requirement.Login)
                {
                    context.Succeed(requirement);
                }
            }
        return Task.CompletedTask;
    }
}