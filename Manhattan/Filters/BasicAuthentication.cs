using Manhattan.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Http.Results;

namespace Manhattan.Filters
{
    public class BasicAuthentication : Attribute, IAuthenticationFilter
    {
        private readonly string realm;
        public bool AllowMultiple { get { return false; } }

        public BasicAuthentication()
        {
            this.realm = "api";
        }

        public Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            var req = context.Request;
            // Check if HTTP Basic Auth is provided
            if (req.Headers.Authorization != null && req.Headers.Authorization.Scheme.Equals("basic", StringComparison.OrdinalIgnoreCase))
            {
                Encoding encoding = Encoding.GetEncoding("utf-8");
                string credentials = encoding.GetString(Convert.FromBase64String(req.Headers.Authorization.Parameter));

                string[] parts = credentials.Split(':');
                string username = parts[0].Trim();
                string password = parts[1].Trim();

                if (Users.verifyUser(username, password))
                {
                    var claims = new List<Claim>()
                                    {
                                        new Claim(ClaimTypes.Name, "User")
                                    };
                    var id = new ClaimsIdentity(claims, "Basic");
                    var principal = new ClaimsPrincipal(new[] { id });
                    context.Principal = principal;
                }
                else
                {
                    // Invalid user account
                    context.ErrorResult = new UnauthorizedResult(new AuthenticationHeaderValue[0], context.Request);
                }

            }
            else
            {
                // Invalid Base64
                context.ErrorResult = new UnauthorizedResult(new AuthenticationHeaderValue[0], context.Request);
            }

            return Task.FromResult(0);
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            context.Result = new ResultWithChallenge(context.Result, realm);
            return Task.FromResult(0);
        }
    }

    public class ResultWithChallenge : IHttpActionResult
    {
        private readonly IHttpActionResult next;
        private readonly string realm;

        public ResultWithChallenge(IHttpActionResult next, string realm)
        {
            this.next = next;
            this.realm = realm;
        }

        public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var res = await next.ExecuteAsync(cancellationToken);
            if (res.StatusCode == HttpStatusCode.Unauthorized)
            {
                res.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue("Basic", this.realm));
            }

            return res;
        }
    }
}