using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace XbyOpenApi.OAuth2
{
  /// <summary>
  /// This exception is thrown whenever an error with OAuth2 initialization occurs
  /// </summary>
  public class OAuth2Exception : Exception
  {
    /// <summary>
    /// No message constructor
    /// </summary>
    public OAuth2Exception()
    {
    }

    /// <summary>
    /// Create exception with a message text
    /// </summary>
    /// <param name="message">Error message</param>
    public OAuth2Exception(string message) : base(message)
    {
    }

    /// <summary>
    /// Create exception with a message text and an inner exception
    /// </summary>
    /// <param name="message">Error message</param>
    /// <param name="innerException">Inner exception</param>
    public OAuth2Exception(string message, Exception innerException) : base(message, innerException)
    {
    }
  }
}
