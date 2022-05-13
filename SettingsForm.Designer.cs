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
            System.Windows.Forms.TableLayoutPanel FormData;
            System.Windows.Forms.Panel ButtonPanel;
            this.LinesInput = new System.Windows.Forms.NumericUpDown();
            this.MsPerTickInput = new System.Windows.Forms.NumericUpDown();
            this.TickLabel = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            LinesLabel = new System.Windows.Forms.Label();
            FormData = new System.Windows.Forms.TableLayoutPanel();
            ButtonPanel = new System.Windows.Forms.Panel();
            FormData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LinesInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MsPerTickInput)).BeginInit();
            ButtonPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // LinesLabel
            // 
            LinesLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            LinesLabel.AutoSize = true;
            LinesLabel.Location = new System.Drawing.Point(3, 7);
            LinesLabel.Name = "LinesLabel";
            LinesLabel.Size = new System.Drawing.Size(180, 15);
            LinesLabel.TabIndex = 3;
            LinesLabel.Text = "Concurrent Runners per Window";
            // 
            // FormData
            // 
            FormData.ColumnCount = 2;
            FormData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            FormData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            FormData.Controls.Add(this.LinesInput, 2, 0);
            FormData.Controls.Add(this.MsPerTickInput, 2, 1);
            FormData.Controls.Add(LinesLabel, 0, 0);
            FormData.Controls.Add(this.TickLabel, 0, 1);
            FormData.Dock = System.Windows.Forms.DockStyle.Top;
            FormData.Location = new System.Drawing.Point(0, 0);
            FormData.Name = "FormData";
            FormData.RowCount = 2;
            FormData.RowStyles.Add(new System.Windows.Forms.RowStyle());
            FormData.RowStyles.Add(new System.Windows.Forms.RowStyle());
            FormData.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            FormData.Size = new System.Drawing.Size(290, 60);
            FormData.TabIndex = 0;
            // 
            // LinesInput
            // 
            this.LinesInput.Location = new System.Drawing.Point(189, 3);
            this.LinesInput.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.LinesInput.Name = "LinesInput";
            this.LinesInput.Size = new System.Drawing.Size(100, 23);
            this.LinesInput.TabIndex = 1;
            this.LinesInput.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // MsPerTickInput
            // 
            this.MsPerTickInput.Location = new System.Drawing.Point(190, 32);
            this.MsPerTickInput.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MsPerTickInput.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.MsPerTickInput.Name = "MsPerTickInput";
            this.MsPerTickInput.Size = new System.Drawing.Size(100, 23);
            this.MsPerTickInput.TabIndex = 2;
            this.MsPerTickInput.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // TickLabel
            // 
            this.TickLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.TickLabel.AutoSize = true;
            this.TickLabel.Location = new System.Drawing.Point(3, 37);
            this.TickLabel.Name = "TickLabel";
            this.TickLabel.Size = new System.Drawing.Size(117, 15);
            this.TickLabel.TabIndex = 4;
            this.TickLabel.Text = "Milliseconds per Tick";
            // 
            // ButtonPanel
            // 
            ButtonPanel.Controls.Add(this.okButton);
            ButtonPanel.Controls.Add(this.cancelButton);
            ButtonPanel.Dock = System.Windows.Forms.DockStyle.Top;
            ButtonPanel.Location = new System.Drawing.Point(0, 60);
            ButtonPanel.Name = "ButtonPanel";
            ButtonPanel.Size = new System.Drawing.Size(290, 25);
            ButtonPanel.TabIndex = 5;
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.okButton.Location = new System.Drawing.Point(140, 0);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 25);
            this.okButton.TabIndex = 3;
            this.okButton.Text = "&OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.cancelButton.Location = new System.Drawing.Point(215, 0);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 25);
            this.cancelButton.TabIndex = 4;
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
            this.ClientSize = new System.Drawing.Size(290, 90);
            this.Controls.Add(ButtonPanel);
            this.Controls.Add(FormData);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "SettingsForm";
            this.Text = "Matrix Rain Settings";
            FormData.ResumeLayout(false);
            FormData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LinesInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MsPerTickInput)).EndInit();
            ButtonPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private NumericUpDown LinesInput;
        private NumericUpDown MsPerTickInput;
        private Label TickLabel;
    }
}