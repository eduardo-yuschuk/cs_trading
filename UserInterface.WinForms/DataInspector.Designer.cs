/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

namespace UserInterface.WinForms {
  partial class DataInspector {
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
      this.groupBoxDataManagement = new System.Windows.Forms.GroupBox();
      this.buttonDraw = new System.Windows.Forms.Button();
      this.listBoxDays = new System.Windows.Forms.ListBox();
      this.buttonLoad = new System.Windows.Forms.Button();
      this.comboBoxMonth = new System.Windows.Forms.ComboBox();
      this.comboBoxYear = new System.Windows.Forms.ComboBox();
      this.comboBoxProvider = new System.Windows.Forms.ComboBox();
      this.comboBoxAsset = new System.Windows.Forms.ComboBox();
      this.textBoxRootPath = new System.Windows.Forms.TextBox();
      this.label5 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.chartCustomControl1 = new UserInterface.WinForms.ChartCustomControl();
      this.groupBoxDataManagement.SuspendLayout();
      this.SuspendLayout();
      // 
      // groupBoxDataManagement
      // 
      this.groupBoxDataManagement.Controls.Add(this.chartCustomControl1);
      this.groupBoxDataManagement.Controls.Add(this.buttonDraw);
      this.groupBoxDataManagement.Controls.Add(this.listBoxDays);
      this.groupBoxDataManagement.Controls.Add(this.buttonLoad);
      this.groupBoxDataManagement.Controls.Add(this.comboBoxMonth);
      this.groupBoxDataManagement.Controls.Add(this.comboBoxYear);
      this.groupBoxDataManagement.Controls.Add(this.comboBoxProvider);
      this.groupBoxDataManagement.Controls.Add(this.comboBoxAsset);
      this.groupBoxDataManagement.Controls.Add(this.textBoxRootPath);
      this.groupBoxDataManagement.Controls.Add(this.label5);
      this.groupBoxDataManagement.Controls.Add(this.label4);
      this.groupBoxDataManagement.Controls.Add(this.label3);
      this.groupBoxDataManagement.Controls.Add(this.label2);
      this.groupBoxDataManagement.Controls.Add(this.label1);
      this.groupBoxDataManagement.Location = new System.Drawing.Point(12, 12);
      this.groupBoxDataManagement.Name = "groupBoxDataManagement";
      this.groupBoxDataManagement.Size = new System.Drawing.Size(531, 375);
      this.groupBoxDataManagement.TabIndex = 0;
      this.groupBoxDataManagement.TabStop = false;
      this.groupBoxDataManagement.Text = "Data management";
      // 
      // buttonDraw
      // 
      this.buttonDraw.Location = new System.Drawing.Point(119, 332);
      this.buttonDraw.Name = "buttonDraw";
      this.buttonDraw.Size = new System.Drawing.Size(75, 23);
      this.buttonDraw.TabIndex = 12;
      this.buttonDraw.Text = "Draw";
      this.buttonDraw.UseVisualStyleBackColor = true;
      this.buttonDraw.Click += new System.EventHandler(this.ButtonDraw_Click);
      // 
      // listBoxDays
      // 
      this.listBoxDays.FormattingEnabled = true;
      this.listBoxDays.Location = new System.Drawing.Point(73, 179);
      this.listBoxDays.Name = "listBoxDays";
      this.listBoxDays.Size = new System.Drawing.Size(121, 147);
      this.listBoxDays.TabIndex = 11;
      // 
      // buttonLoad
      // 
      this.buttonLoad.Location = new System.Drawing.Point(119, 45);
      this.buttonLoad.Name = "buttonLoad";
      this.buttonLoad.Size = new System.Drawing.Size(75, 23);
      this.buttonLoad.TabIndex = 10;
      this.buttonLoad.Text = "Load";
      this.buttonLoad.UseVisualStyleBackColor = true;
      // 
      // comboBoxMonth
      // 
      this.comboBoxMonth.FormattingEnabled = true;
      this.comboBoxMonth.Location = new System.Drawing.Point(73, 152);
      this.comboBoxMonth.Name = "comboBoxMonth";
      this.comboBoxMonth.Size = new System.Drawing.Size(121, 21);
      this.comboBoxMonth.TabIndex = 9;
      this.comboBoxMonth.SelectedIndexChanged += new System.EventHandler(this.ComboBoxMonth_SelectedIndexChanged);
      // 
      // comboBoxYear
      // 
      this.comboBoxYear.FormattingEnabled = true;
      this.comboBoxYear.Location = new System.Drawing.Point(73, 126);
      this.comboBoxYear.Name = "comboBoxYear";
      this.comboBoxYear.Size = new System.Drawing.Size(121, 21);
      this.comboBoxYear.TabIndex = 8;
      this.comboBoxYear.SelectedIndexChanged += new System.EventHandler(this.ComboBoxYear_SelectedIndexChanged);
      // 
      // comboBoxProvider
      // 
      this.comboBoxProvider.FormattingEnabled = true;
      this.comboBoxProvider.Location = new System.Drawing.Point(73, 100);
      this.comboBoxProvider.Name = "comboBoxProvider";
      this.comboBoxProvider.Size = new System.Drawing.Size(121, 21);
      this.comboBoxProvider.TabIndex = 7;
      this.comboBoxProvider.SelectedIndexChanged += new System.EventHandler(this.ComboBoxProvider_SelectedIndexChanged);
      // 
      // comboBoxAsset
      // 
      this.comboBoxAsset.FormattingEnabled = true;
      this.comboBoxAsset.Location = new System.Drawing.Point(73, 74);
      this.comboBoxAsset.Name = "comboBoxAsset";
      this.comboBoxAsset.Size = new System.Drawing.Size(121, 21);
      this.comboBoxAsset.TabIndex = 6;
      this.comboBoxAsset.SelectedIndexChanged += new System.EventHandler(this.ComboBoxAsset_SelectedIndexChanged);
      // 
      // textBoxRootPath
      // 
      this.textBoxRootPath.Location = new System.Drawing.Point(73, 19);
      this.textBoxRootPath.Name = "textBoxRootPath";
      this.textBoxRootPath.Size = new System.Drawing.Size(121, 20);
      this.textBoxRootPath.TabIndex = 5;
      this.textBoxRootPath.Text = "C:\\quotes";
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(30, 155);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(37, 13);
      this.label5.TabIndex = 4;
      this.label5.Text = "Month";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(38, 129);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(29, 13);
      this.label4.TabIndex = 3;
      this.label4.Text = "Year";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(21, 103);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(46, 13);
      this.label3.TabIndex = 2;
      this.label3.Text = "Provider";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(11, 77);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(56, 13);
      this.label2.TabIndex = 1;
      this.label2.Text = "Instrument";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(13, 22);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(54, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Root path";
      // 
      // chartCustomControl1
      // 
      this.chartCustomControl1.Location = new System.Drawing.Point(200, 19);
      this.chartCustomControl1.Name = "chartCustomControl1";
      this.chartCustomControl1.Size = new System.Drawing.Size(315, 336);
      this.chartCustomControl1.TabIndex = 13;
      this.chartCustomControl1.Text = "chartCustomControl1";
      // 
      // DataInspector
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(555, 399);
      this.Controls.Add(this.groupBoxDataManagement);
      this.Name = "DataInspector";
      this.Text = "Data inspector";
      this.Load += new System.EventHandler(this.DataInspector_Load);
      this.groupBoxDataManagement.ResumeLayout(false);
      this.groupBoxDataManagement.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.GroupBox groupBoxDataManagement;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox textBoxRootPath;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.ComboBox comboBoxMonth;
    private System.Windows.Forms.ComboBox comboBoxYear;
    private System.Windows.Forms.ComboBox comboBoxProvider;
    private System.Windows.Forms.ComboBox comboBoxAsset;
    private System.Windows.Forms.Button buttonDraw;
    private System.Windows.Forms.ListBox listBoxDays;
    private System.Windows.Forms.Button buttonLoad;
    private ChartCustomControl chartCustomControl1;
  }
}