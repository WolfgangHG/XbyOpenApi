namespace XbyOpenApi.OAuth2.WinForms
{
  partial class DialogOAuth2LoginWebView
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogOAuth2LoginWebView));
      this.webBrowser1 = new Microsoft.Web.WebView2.WinForms.WebView2();
      ((System.ComponentModel.ISupportInitialize)this.webBrowser1).BeginInit();
      this.SuspendLayout();
      // 
      // webBrowser1
      // 
      resources.ApplyResources(this.webBrowser1, "webBrowser1");
      this.webBrowser1.AllowExternalDrop = true;
      this.webBrowser1.CreationProperties = null;
      this.webBrowser1.DefaultBackgroundColor = System.Drawing.Color.White;
      this.webBrowser1.Name = "webBrowser1";
      this.webBrowser1.ZoomFactor = 1D;
      this.webBrowser1.CoreWebView2InitializationCompleted += this.webBrowser1_CoreWebView2InitializationCompleted;
      this.webBrowser1.NavigationCompleted += this.webBrowser1_NavigationCompleted;
      // 
      // DialogOAuth2LoginWebView
      // 
      resources.ApplyResources(this, "$this");
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.webBrowser1);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "DialogOAuth2LoginWebView";
      this.ShowInTaskbar = false;
      ((System.ComponentModel.ISupportInitialize)this.webBrowser1).EndInit();
      this.ResumeLayout(false);
    }

    #endregion

    private Microsoft.Web.WebView2.WinForms.WebView2 webBrowser1;
  }
}