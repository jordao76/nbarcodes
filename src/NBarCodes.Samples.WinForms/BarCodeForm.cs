using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using NBarCodes;
using NBarCodes.Forms;

namespace NBarCodes.Samples.WinForms
{
  /// <summary>
  /// Summary description for Form1.
  /// </summary>
  public class BarCodeForm : System.Windows.Forms.Form
  {
    private System.Windows.Forms.TextBox tbxData;
    private System.Windows.Forms.ComboBox cboBarCodeType;
    private System.Windows.Forms.Button btnGenerate;
    private System.Drawing.Printing.PrintDocument printDocument;
    private System.Windows.Forms.Button btnPrint;
    private System.Windows.Forms.PrintDialog printDialog;
    private BarCodeControl barCodeControl1;

    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.Container components = null;

    public BarCodeForm()
    {
      //
      // Required for Windows Form Designer support
      //
      InitializeComponent();

      //
      // TODO: Add any constructor code after InitializeComponent call
      //
    }

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    protected override void Dispose( bool disposing )
    {
      if( disposing )
      {
        if (components != null) 
        {
          components.Dispose();
        }
      }
      base.Dispose( disposing );
    }

    #region Windows Form Designer generated code
    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.tbxData = new System.Windows.Forms.TextBox();
      this.cboBarCodeType = new System.Windows.Forms.ComboBox();
      this.btnGenerate = new System.Windows.Forms.Button();
      this.printDocument = new System.Drawing.Printing.PrintDocument();
      this.btnPrint = new System.Windows.Forms.Button();
      this.printDialog = new System.Windows.Forms.PrintDialog();
      this.barCodeControl1 = new NBarCodes.Forms.BarCodeControl();
      this.SuspendLayout();
      // 
      // tbxData
      // 
      this.tbxData.Location = new System.Drawing.Point(16, 8);
      this.tbxData.Name = "tbxData";
      this.tbxData.Size = new System.Drawing.Size(256, 20);
      this.tbxData.TabIndex = 0;
      this.tbxData.Text = "NBarCodes";
      // 
      // cboBarCodeType
      // 
      this.cboBarCodeType.Location = new System.Drawing.Point(16, 40);
      this.cboBarCodeType.Name = "cboBarCodeType";
      this.cboBarCodeType.Size = new System.Drawing.Size(168, 21);
      this.cboBarCodeType.TabIndex = 1;
      // 
      // btnGenerate
      // 
      this.btnGenerate.Location = new System.Drawing.Point(16, 219);
      this.btnGenerate.Name = "btnGenerate";
      this.btnGenerate.Size = new System.Drawing.Size(75, 23);
      this.btnGenerate.TabIndex = 2;
      this.btnGenerate.Text = "&OK";
      this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
      // 
      // printDocument
      // 
      this.printDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument_PrintPage);
      // 
      // btnPrint
      // 
      this.btnPrint.Location = new System.Drawing.Point(104, 219);
      this.btnPrint.Name = "btnPrint";
      this.btnPrint.Size = new System.Drawing.Size(75, 23);
      this.btnPrint.TabIndex = 3;
      this.btnPrint.Text = "&Print";
      this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
      // 
      // printDialog
      // 
      this.printDialog.Document = this.printDocument;
      // 
      // barCodeControl1
      // 
      this.barCodeControl1.BackColor = System.Drawing.Color.WhiteSmoke;
      this.barCodeControl1.BarColor = System.Drawing.Color.DimGray;
      this.barCodeControl1.Data = "NBarCodes";
      this.barCodeControl1.Font = new System.Drawing.Font("Verdana", 15F, System.Drawing.FontStyle.Bold);
      this.barCodeControl1.FontColor = System.Drawing.Color.DarkOliveGreen;
      this.barCodeControl1.Location = new System.Drawing.Point(16, 91);
      this.barCodeControl1.Name = "barCodeControl1";
      this.barCodeControl1.Size = new System.Drawing.Size(144, 97);
      this.barCodeControl1.TabIndex = 0;
      // 
      // BarCodeForm
      // 
      this.AcceptButton = this.btnGenerate;
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.ClientSize = new System.Drawing.Size(353, 261);
      this.Controls.Add(this.btnPrint);
      this.Controls.Add(this.btnGenerate);
      this.Controls.Add(this.cboBarCodeType);
      this.Controls.Add(this.tbxData);
      this.Controls.Add(this.barCodeControl1);
      this.Name = "BarCodeForm";
      this.Text = "BarCode Form";
      this.Load += new System.EventHandler(this.BarCodeForm_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }
    #endregion

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main() 
    {
      Application.Run(new BarCodeForm());
    }

    private void BarCodeForm_Load(object sender, System.EventArgs e) {
      BindBarCodeType();
      RenderBarCode();
    }

    private void BindBarCodeType() {
      cboBarCodeType.DataSource = Enum.GetNames(typeof(BarCodeType));
      cboBarCodeType.SelectedItem = cboBarCodeType.Items[(int)BarCodeType.Code128];
    }

    private void RenderBarCode() {
      barCodeControl1.Data = tbxData.Text;
      barCodeControl1.Type = (BarCodeType)Enum.Parse(typeof(BarCodeType), cboBarCodeType.SelectedItem.ToString());
      barCodeControl1.Refresh();
    }

    private void btnGenerate_Click(object sender, System.EventArgs e) {
      RenderBarCode();
    }

    private void printDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e) {
      barCodeControl1.DrawBarCode(e.Graphics);
    }

    private void btnPrint_Click(object sender, System.EventArgs e) {
      DialogResult userAction = printDialog.ShowDialog();
      if (userAction == DialogResult.OK) {
        printDocument.Print();
      }
    }
  }
}
