/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

namespace UserInterface.WinForms {
  partial class MainForm {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this.buttonRunDataInspector = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // buttonRunDataInspector
      // 
      this.buttonRunDataInspector.Location = new System.Drawing.Point(33, 32);
      this.buttonRunDataInspector.Name = "buttonRunDataInspector";
      this.buttonRunDataInspector.Size = new System.Drawing.Size(168, 23);
      this.buttonRunDataInspector.TabIndex = 0;
      this.buttonRunDataInspector.Text = "Inspección de datos";
      this.buttonRunDataInspector.UseVisualStyleBackColor = true;
      this.buttonRunDataInspector.Click += new System.EventHandler(this.ButtonRunDataInspector_Click);
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(480, 244);
      this.Controls.Add(this.buttonRunDataInspector);
      this.Name = "MainForm";
      this.Text = "Trading System UI";
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button buttonRunDataInspector;
  }
}

