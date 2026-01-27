using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Authentication;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TinyOAuth1;

namespace XbyOpenApi.OAuth1
{
  /// <summary>
  /// AuthenticationProvider für OAuth1-Zugriffe auf die Twitter-API.
  /// </summary>
  internal class XOAuth1AuthenticationProvider : IAuthenticationProvider
  {
    private TinyOAuth tinyOAuth;
    private string strAccessToken;
    private string strAccessTokenSecret;

    public XOAuth1AuthenticationProvider(TinyOAuth _tinyOAuth, string _strAccessToken, string _strAccessTokenSecret)
    {
      this.tinyOAuth = _tinyOAuth;
      this.strAccessToken = _strAccessToken;
      this.strAccessTokenSecret = _strAccessTokenSecret;
    }
    public Task AuthenticateRequestAsync(RequestInformation request, Dictionary<string, object>? additionalAuthenticationContext = null, CancellationToken cancellationToken = default)
    {
      //Convert URI and HttpMethod 
      string strUrl = request.URI.AbsoluteUri;
      HttpMethod httpMethod = request.HttpMethod switch
      {
        Method.GET => HttpMethod.Get,
        Method.POST => HttpMethod.Post,
        Method.DELETE => HttpMethod.Delete,
        _ => throw new NotImplementedException("Nicht unterstützt für " + request.HttpMethod)
      };
      AuthenticationHeaderValue authHeader = this.tinyOAuth.GetAuthorizationHeader(this.strAccessToken, this.strAccessTokenSecret, strUrl, httpMethod);

      //"ToString" hängt Scheme und Parameter aneinander, d.h. "
      request.Headers.Add("Authorization", authHeader.ToString());

      return Task.CompletedTask;
    }
  }
}
