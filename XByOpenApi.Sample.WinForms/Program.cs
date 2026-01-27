using System;
using System.Globalization;
using System.Windows.Forms;

namespace XByOpenApi.Sample.WinForms
{
  internal static class Program
  {
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      //Testing localization:
      //CultureInfo.CurrentCulture = new CultureInfo("en-US");
      //CultureInfo.CurrentUICulture = new CultureInfo("en-US");
      
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      bool todoManfiest;
      //SetHighDpiMode(HighDpiMode.SystemAware);

      Application.Run(new FormXByKiotaTest());
    }
  }
}