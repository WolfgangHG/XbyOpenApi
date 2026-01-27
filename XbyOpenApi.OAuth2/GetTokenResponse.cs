using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace XbyOpenApi.OAuth2
{
  /// <summary>
  /// Response when fetching an OAuth2 access token
  /// </summary>
  public class GetTokenResponse
  {
    /// <summary>
    /// Token type, always "Bearer"
    /// </summary>
    [JsonPropertyName("token_type")]
    public string TokenType
    {
      get;
      set;
    } = string.Empty;

    /// <summary>
    /// Expiration of the token in seconds (e.g. "7200" - two hours)
    /// </summary>
    [JsonPropertyName("expires_in")]
    public int ExpiresIn
    {
      get;
      set;
    }

    /// <summary>
    /// The AccessToken
    /// </summary>
    [JsonPropertyName("access_token")]
    public string AccessToken
    {
      get;
      set;
    } = string.Empty;

    /// <summary>
    /// Refresh token, only filled if a RefreshToken was requested.
    /// </summary>
    [JsonPropertyName("refresh_token")]
    public string RefreshToken
    {
      get;
      set;
    } = string.Empty;

    /// <summary>
    /// Scope of the token, should be the same as the scopes requests in the Authorize step.
    /// The tokens are separated by a space char.
    /// </summary>
    [JsonPropertyName("scope")]
    public string Scope
    {
      get;
      set;
    } = string.Empty;
  }
}
