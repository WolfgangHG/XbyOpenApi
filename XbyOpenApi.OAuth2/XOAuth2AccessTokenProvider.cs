using Microsoft.Kiota.Abstractions.Authentication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XbyOpenApi.OAuth2
{
  internal class XOAuth2AccessTokenProvider : IAccessTokenProvider
  {
    private string accessToken;
    public XOAuth2AccessTokenProvider(string accessToken)
    {
      this.accessToken = accessToken;
    }
    public Task<string> GetAuthorizationTokenAsync(Uri uri, Dictionary<string, object>? additionalAuthenticationContext = null, CancellationToken cancellationToken = default)
    {
      return Task.FromResult(this.accessToken);
    }

    public AllowedHostsValidator AllowedHostsValidator => throw new NotImplementedException();
  }
}
