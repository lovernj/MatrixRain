namespace MatrixRain
{
    partial class SettingsForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.Label LinesLabel;
            System.Windows.Forms.Label Heading1;
            System.Windows.Forms.Label AuthorSubheading;
            System.Windows.Forms.GroupBox groupBox;
            this.LinesInput = new System.Windows.Forms.NumericUpDown();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            LinesLabel = new System.Windows.Forms.Label();
            Heading1 = new System.Windows.Forms.Label();
            AuthorSubheading = new System.Windows.Forms.Label();
            groupBox = new System.Windows.Forms.GroupBox();
            groupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LinesInput)).BeginInit();
            this.SuspendLayout();
            // 
            // LinesLabel
            // 
            LinesLabel.AutoSize = true;
            LinesLabel.Location = new System.Drawing.Point(7, 76);
            LinesLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            LinesLabel.Name = "LinesLabel";
            LinesLabel.Size = new System.Drawing.Size(164, 15);
            LinesLabel.TabIndex = 3;
            LinesLabel.Text = "Concurrent Lines per Window";
            // 
            // Heading1
            // 
            Heading1.AutoSize = true;
            Heading1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            Heading1.Location = new System.Drawing.Point(7, 18);
            Heading1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Heading1.Name = "Heading1";
            Heading1.Size = new System.Drawing.Size(121, 25);
            Heading1.TabIndex = 0;
            Heading1.Text = "Matrix Rain";
            // 
            // AuthorSubheading
            // 
            AuthorSubheading.AutoSize = true;
            AuthorSubheading.Location = new System.Drawing.Point(7, 47);
            AuthorSubheading.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            AuthorSubheading.Name = "AuthorSubheading";
            AuthorSubheading.Size = new System.Drawing.Size(249, 15);
            AuthorSubheading.TabIndex = 0;
            AuthorSubheading.Text = "By Jakob Lovern (Southern Oregon University)";
            // 
            // groupBox
            // 
            groupBox.Controls.Add(Heading1);
            groupBox.Controls.Add(this.LinesInput);
            groupBox.Controls.Add(AuthorSubheading);
            groupBox.Controls.Add(LinesLabel);
            groupBox.Location = new System.Drawing.Point(2, 1);
            groupBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox.Name = "groupBox";
            groupBox.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox.Size = new System.Drawing.Size(306, 119);
            groupBox.TabIndex = 0;
            groupBox.TabStop = false;
            // 
            // LinesInput
            // 
            this.LinesInput.Location = new System.Drawing.Point(186, 74);
            this.LinesInput.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.LinesInput.Name = "LinesInput";
            this.LinesInput.Size = new System.Drawing.Size(102, 23);
            this.LinesInput.TabIndex = 1;
            this.LinesInput.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(124, 126);
            this.okButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(88, 27);
            this.okButton.TabIndex = 2;
            this.okButton.Text = "&OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.CausesValidation = false;
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(220, 127);
            this.cancelButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(88, 27);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // SettingsForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(314, 163);
            this.Controls.Add(groupBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.Name = "SettingsForm";
            this.Text = "Settings";
            groupBox.ResumeLayout(false);
            groupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LinesInput)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.NumericUpDown LinesInput;
    }
}