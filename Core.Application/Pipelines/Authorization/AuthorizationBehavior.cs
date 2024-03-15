using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Security.Constants;
using Core.Security.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Core.CrossCuttingConcerns.Exceptions.Types;

namespace Core.Application.Pipelines.Authorization
{
    public class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
            where TRequest : IRequest<TResponse>, ISecuredRequest
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public AuthorizationBehavior(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            List<string>? userRoleClaims = _contextAccessor.HttpContext.User.ClaimRoles();

            if (userRoleClaims == null)
                throw new AuthorizationException("Not Authenticated!");

            bool isNotMatchedAUserRoleClaimWithRequestRoles = userRoleClaims.FirstOrDefault(userRoleClaim =>
                    userRoleClaim == GeneralOperationClaims.Admin || request.Roles.Any(role => role == userRoleClaim))
                .IsNullOrEmpty();

            if(isNotMatchedAUserRoleClaimWithRequestRoles)
                throw new AuthorizationException("Not Authorized!");

            TResponse response = await next();

            return response;
        }
    }
}
