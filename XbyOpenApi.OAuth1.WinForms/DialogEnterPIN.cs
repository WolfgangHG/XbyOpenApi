using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XbyOpenApi.OAuth1.WinForms
{
  /// <summary>
  /// This form is shown after the the OAuth1 pin authentication flow was started: it should be shown as modal form, meanwhile
  /// the X authorization web page should be opened in an external browser window. After authentication, the web page shows a PIN.
  /// The user has to enter this pin in this form and click "OK".
  /// </summary>
  public partial class DialogEnterPIN : Form
  {
    /// <summary>
    /// Constructor, nothing special.
    /// </summary>
    public DialogEnterPIN()
    {
      InitializeComponent();
    }

    private void buttonOK_Click(object sender, EventArgs e)
    {
      if (string.IsNullOrEmpty(this.textBoxPIN.Text) == true)
      {
        MessageBox.Show(this, Resources.DialogEnterPIN.ERROR_OK_NO_PIN, Resources.DialogEnterPIN.ERROR_OK_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
        this.DialogResult = DialogResult.None;
      }
    }

    /// <summary>
    /// The pin entered by the user. Will contain a value after the user has clicked "OK".
    /// </summary>
    public string PIN
    {
      get
      {
        return this.textBoxPIN.Text;
      }
    }
  }
}
