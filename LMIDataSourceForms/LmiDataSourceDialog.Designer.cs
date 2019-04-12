using System;

namespace LMIDataSourceForms
{
    partial class LmiDataSourceDialog
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
            this.label1 = new System.Windows.Forms.Label();
            this.hostBox = new System.Windows.Forms.TextBox();
            this.queryLabel = new System.Windows.Forms.Label();
            this.queryBox = new System.Windows.Forms.TextBox();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.userNameBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.userPassBox = new System.Windows.Forms.TextBox();
            this.resultPicker = new System.Windows.Forms.ListBox();
            this.newQueryChoice = new System.Windows.Forms.RadioButton();
            this.existingResultsChoice = new System.Windows.Forms.RadioButton();
            this.checkingLabel = new System.Windows.Forms.Label();
            this.runCorrChoice = new System.Windows.Forms.RadioButton();
            this.fromPicker = new System.Windows.Forms.DateTimePicker();
            this.toPicker = new System.Windows.Forms.DateTimePicker();
            this.fromLabel = new System.Windows.Forms.Label();
            this.toLabel = new System.Windows.Forms.Label();
            this.corrResultsChoice = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Host";
            // 
            // hostBox
            // 
            this.hostBox.Location = new System.Drawing.Point(69, 24);
            this.hostBox.Name = "hostBox";
            this.hostBox.Size = new System.Drawing.Size(364, 20);
            this.hostBox.TabIndex = 1;
            // 
            // queryLabel
            // 
            this.queryLabel.AutoSize = true;
            this.queryLabel.Location = new System.Drawing.Point(19, 203);
            this.queryLabel.Name = "queryLabel";
            this.queryLabel.Size = new System.Drawing.Size(35, 13);
            this.queryLabel.TabIndex = 2;
            this.queryLabel.Text = "Query";
            // 
            // queryBox
            // 
            this.queryBox.AcceptsReturn = true;
            this.queryBox.AcceptsTab = true;
            this.queryBox.Location = new System.Drawing.Point(69, 200);
            this.queryBox.Multiline = true;
            this.queryBox.Name = "queryBox";
            this.queryBox.Size = new System.Drawing.Size(363, 116);
            this.queryBox.TabIndex = 3;
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(12, 384);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(111, 36);
            this.okButton.TabIndex = 11;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.OnOkButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(395, 384);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(111, 36);
            this.cancelButton.TabIndex = 12;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "User";
            // 
            // userNameBox
            // 
            this.userNameBox.Location = new System.Drawing.Point(70, 58);
            this.userNameBox.Name = "userNameBox";
            this.userNameBox.Size = new System.Drawing.Size(144, 20);
            this.userNameBox.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(255, 184);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(0, 13);
            this.label4.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(230, 61);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Password";
            // 
            // userPassBox
            // 
            this.userPassBox.Location = new System.Drawing.Point(289, 58);
            this.userPassBox.Name = "userPassBox";
            this.userPassBox.Size = new System.Drawing.Size(144, 20);
            this.userPassBox.TabIndex = 3;
            this.userPassBox.UseSystemPasswordChar = true;
            // 
            // resultPicker
            // 
            this.resultPicker.FormattingEnabled = true;
            this.resultPicker.Location = new System.Drawing.Point(69, 200);
            this.resultPicker.Name = "resultPicker";
            this.resultPicker.Size = new System.Drawing.Size(340, 121);
            this.resultPicker.TabIndex = 8;
            this.resultPicker.Visible = false;
            // 
            // newQueryChoice
            // 
            this.newQueryChoice.AutoSize = true;
            this.newQueryChoice.Checked = true;
            this.newQueryChoice.Location = new System.Drawing.Point(69, 93);
            this.newQueryChoice.Name = "newQueryChoice";
            this.newQueryChoice.Size = new System.Drawing.Size(167, 26);
            this.newQueryChoice.TabIndex = 4;
            this.newQueryChoice.TabStop = true;
            this.newQueryChoice.Text = "Run new advanced search";
            this.newQueryChoice.UseVisualStyleBackColor = true;
            this.newQueryChoice.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // existingResultsChoice
            // 
            this.existingResultsChoice.AutoSize = true;
            this.existingResultsChoice.Location = new System.Drawing.Point(69, 116);
            this.existingResultsChoice.Name = "existingResultsChoice";
            this.existingResultsChoice.Size = new System.Drawing.Size(214, 26);
            this.existingResultsChoice.TabIndex = 5;
            this.existingResultsChoice.Text = "Use existing advanced search results";
            this.existingResultsChoice.UseVisualStyleBackColor = true;
            this.existingResultsChoice.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // checkingLabel
            // 
            this.checkingLabel.AutoSize = true;
            this.checkingLabel.Location = new System.Drawing.Point(142, 396);
            this.checkingLabel.Name = "checkingLabel";
            this.checkingLabel.Size = new System.Drawing.Size(150, 13);
            this.checkingLabel.TabIndex = 17;
            this.checkingLabel.Text = "Checking connection to LMI...";
            this.checkingLabel.Visible = false;
            // 
            // runCorrChoice
            // 
            this.runCorrChoice.AutoSize = true;
            this.runCorrChoice.Location = new System.Drawing.Point(69, 139);
            this.runCorrChoice.Name = "runCorrChoice";
            this.runCorrChoice.Size = new System.Drawing.Size(168, 26);
            this.runCorrChoice.TabIndex = 6;
            this.runCorrChoice.TabStop = true;
            this.runCorrChoice.Text = "Run new correlation search";
            this.runCorrChoice.UseVisualStyleBackColor = true;
            this.runCorrChoice.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // fromPicker
            // 
            this.fromPicker.CustomFormat = "yyyy\'-\'MM\'-\'dd HH\':\'mm\':\'ss";
            this.fromPicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.fromPicker.Location = new System.Drawing.Point(103, 327);
            this.fromPicker.Name = "fromPicker";
            this.fromPicker.Size = new System.Drawing.Size(137, 20);
            this.fromPicker.TabIndex = 9;
            this.fromPicker.Value = new System.DateTime(2018, 6, 11, 0, 0, 0, 0);
            this.fromPicker.Visible = false;
            // 
            // toPicker
            // 
            this.toPicker.CustomFormat = "yyyy\'-\'MM\'-\'dd HH\':\'mm\':\'ss";
            this.toPicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.toPicker.Location = new System.Drawing.Point(289, 327);
            this.toPicker.Name = "toPicker";
            this.toPicker.Size = new System.Drawing.Size(144, 20);
            this.toPicker.TabIndex = 10;
            this.toPicker.Value = new System.DateTime(2018, 6, 11, 0, 0, 0, 0);
            this.toPicker.Visible = false;
            // 
            // fromLabel
            // 
            this.fromLabel.AutoSize = true;
            this.fromLabel.Location = new System.Drawing.Point(67, 328);
            this.fromLabel.Name = "fromLabel";
            this.fromLabel.Size = new System.Drawing.Size(30, 13);
            this.fromLabel.TabIndex = 22;
            this.fromLabel.Text = "From";
            this.fromLabel.Visible = false;
            // 
            // toLabel
            // 
            this.toLabel.AutoSize = true;
            this.toLabel.Location = new System.Drawing.Point(263, 328);
            this.toLabel.Name = "toLabel";
            this.toLabel.Size = new System.Drawing.Size(20, 13);
            this.toLabel.TabIndex = 23;
            this.toLabel.Text = "To";
            this.toLabel.Visible = false;
            // 
            // corrResultsChoice
            // 
            this.corrResultsChoice.AutoSize = true;
            this.corrResultsChoice.Location = new System.Drawing.Point(68, 162);
            this.corrResultsChoice.Name = "corrResultsChoice";
            this.corrResultsChoice.Size = new System.Drawing.Size(215, 26);
            this.corrResultsChoice.TabIndex = 7;
            this.corrResultsChoice.Text = "Use existing correlation search results";
            this.corrResultsChoice.UseVisualStyleBackColor = true;
            this.corrResultsChoice.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // LmiDataSourceDialog
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(518, 432);
            this.Controls.Add(this.corrResultsChoice);
            this.Controls.Add(this.toLabel);
            this.Controls.Add(this.fromLabel);
            this.Controls.Add(this.toPicker);
            this.Controls.Add(this.fromPicker);
            this.Controls.Add(this.runCorrChoice);
            this.Controls.Add(this.checkingLabel);
            this.Controls.Add(this.existingResultsChoice);
            this.Controls.Add(this.newQueryChoice);
            this.Controls.Add(this.resultPicker);
            this.Controls.Add(this.userPassBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.userNameBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.queryBox);
            this.Controls.Add(this.queryLabel);
            this.Controls.Add(this.hostBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LmiDataSourceDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Get data from LogLogic LMI advanced search";
            this.Shown += new System.EventHandler(this.HttpDataSourceDialog_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox hostBox;
        private System.Windows.Forms.Label queryLabel;
        private System.Windows.Forms.TextBox queryBox;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox userNameBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox userPassBox;
        private System.Windows.Forms.ListBox resultPicker;
        private System.Windows.Forms.RadioButton newQueryChoice;
        private System.Windows.Forms.RadioButton existingResultsChoice;
        private System.Windows.Forms.Label checkingLabel;
        private System.Windows.Forms.RadioButton runCorrChoice;
        private System.Windows.Forms.DateTimePicker fromPicker;
        private System.Windows.Forms.DateTimePicker toPicker;
        private System.Windows.Forms.Label fromLabel;
        private System.Windows.Forms.Label toLabel;
        private System.Windows.Forms.RadioButton corrResultsChoice;
    }
}