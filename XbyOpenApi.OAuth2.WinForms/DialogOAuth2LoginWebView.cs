using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace XbyOpenApi.OAuth2.WinForms
{
  /// <summary>
  /// Login form for X in an OAuth2 flow using a WebView2 control. 
  /// After login, the browser is redirected to the "redirect url" configured in the X application. This dialog handles this redirect
  /// and parses the authentication code.
  /// After successful (or canceled) login, the form auto closes itself.
  /// If login succeeded (DialogResult = OK), the <see cref="AuthorizationCode"/> contains the code.
  /// </summary>
  public partial class DialogOAuth2LoginWebView : Form
  {
    #region Private Vars
    /// <summary>
    /// ClientID
    /// </summary>
    private string clientID;

    /// <summary>
    /// This is the plain redirect url, as specified in the X app.
    /// </summary>
    private string realRedirectURL;

    /// <summary>
    /// True: fetch also a refresh token.
    /// </summary>
    private bool fetchRefreshToken;

    /// <summary>
    /// Requested scopes.
    /// </summary>
    private List<string> scopes;
    #endregion

    #region Public properties
    /// <summary>
    /// This is the AuthorizationCode which is returned by X after a successful authorization.
    /// NULL if authorization was canceled by clicking the "cancel" button on the login page or by closing this form.
    /// </summary>
    public string? AuthorizationCode
    {
      get;
      private set;
    }

    #endregion

    #region Constructor
    /// <summary>
    /// The constructor just copies the arguments to member variables. 
    /// </summary>
    /// <param name="clientID">ClientID</param>
    /// <param name="redirectUrl">This is the plain redirect url, as specified in the X app.</param>
    /// <param name="fetchRefreshToken">Du we also want to fetch a RefreshToken (which means adding an additional 
    /// <paramref name="scopes"/>) </param>
    /// <param name="scopes">Requested scopes, must contain at least one item.</param>
    public DialogOAuth2LoginWebView(string clientID, string redirectUrl, bool fetchRefreshToken,
       List<string> scopes)
    {
      if (scopes == null)
      {
        throw new ArgumentNullException(nameof(scopes));
      }
      if (scopes.Count  == 0)
      {
        throw new ArgumentException("Scopes must not be empty");
      }

      this.clientID = clientID;
      this.realRedirectURL = redirectUrl;
      
      this.scopes = scopes;
      this.fetchRefreshToken = fetchRefreshToken;

      InitializeComponent();
    }
    #endregion

    #region Overrides
    /// <summary>
    /// Form is loaded: browse to the authorization url
    /// </summary>
    /// <param name="e"></param>
    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);

      string strAuthorizeUrl = XClientOAuth2Util.GetAuthorizeUrl(clientID, this.realRedirectURL, this.scopes, this.fetchRefreshToken);

      this.webBrowser1.Source = new Uri(strAuthorizeUrl);
    }
    #endregion

    #region Event handler
    /// <summary>
    /// The browser control has navigated to a page: if this is the redirect url, the parameters contains either an authorization code
    /// or an error state.
    /// Intercept this request and close the dialog.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void webBrowser1_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
    {
      if (this.webBrowser1.Source.AbsoluteUri.StartsWith(realRedirectURL))
      {
        //The redirect url contains "state=xxx" (this is the state parameter that was set when building the authorization url.

        //In case of "cancel" click (when confirming the requested scopes), the url looks like this:
        //http://localhost/?error=access_denied&state=state

        //In Case of success, it contains a parameter "code".

        NameValueCollection queryArgs = HttpUtility.ParseQueryString(this.webBrowser1.Source.Query);

        if (queryArgs["error"] != null)
        {
          //"Cancel" was clicked. Cancel this form.
          this.DialogResult = DialogResult.Cancel;
          this.Close();
          return;
        }
        else
        {
          //Check state...

          string? code = queryArgs["code"];
          if (code == null)
          {
            MessageBox.Show(this, "Error - response did not contain 'code': " + this.webBrowser1.Source.OriginalString);
            return;
          }

          this.AuthorizationCode = code;

          //At this point, you can use the authorization code to create an access token and refresh token.

          this.DialogResult = DialogResult.OK;
          this.Close();

        }
      }
    }

    /// <summary>
    /// After the browser control is initialized:
    /// a) switch off navigation options.
    /// b) reset the browser cache: delete all cookies, so that every time a new login is done.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void webBrowser1_CoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
    {
      //Reset navigation options:
      this.webBrowser1.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
      this.webBrowser1.CoreWebView2.Settings.AreBrowserAcceleratorKeysEnabled = false;

      
      this.webBrowser1.CoreWebView2.CookieManager.DeleteAllCookies();

    }
    #endregion
  }
}