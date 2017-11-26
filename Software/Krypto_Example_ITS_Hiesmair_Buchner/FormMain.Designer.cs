namespace Krypto_Example_ITS_Hiesmair_Buchner
{
    partial class FormMain
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnCreateKeys = new System.Windows.Forms.Button();
            this.btnExportKeys = new System.Windows.Forms.Button();
            this.btnEncrypt = new System.Windows.Forms.Button();
            this.btnDecrypt = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbxStatus = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnCreateKeys
            // 
            this.btnCreateKeys.Location = new System.Drawing.Point(35, 59);
            this.btnCreateKeys.Name = "btnCreateKeys";
            this.btnCreateKeys.Size = new System.Drawing.Size(272, 45);
            this.btnCreateKeys.TabIndex = 0;
            this.btnCreateKeys.Text = "Create Asymmetric RSA-keys";
            this.btnCreateKeys.UseVisualStyleBackColor = true;
            this.btnCreateKeys.Click += new System.EventHandler(this.btnCreateKeys_Click);
            // 
            // btnExportKeys
            // 
            this.btnExportKeys.Location = new System.Drawing.Point(313, 59);
            this.btnExportKeys.Name = "btnExportKeys";
            this.btnExportKeys.Size = new System.Drawing.Size(227, 45);
            this.btnExportKeys.TabIndex = 1;
            this.btnExportKeys.Text = "Export public RSA-key";
            this.btnExportKeys.UseVisualStyleBackColor = true;
            this.btnExportKeys.Click += new System.EventHandler(this.btnExportKeys_Click);
            // 
            // btnEncrypt
            // 
            this.btnEncrypt.Location = new System.Drawing.Point(30, 243);
            this.btnEncrypt.Name = "btnEncrypt";
            this.btnEncrypt.Size = new System.Drawing.Size(251, 45);
            this.btnEncrypt.TabIndex = 2;
            this.btnEncrypt.Text = "Encrypt File";
            this.btnEncrypt.UseVisualStyleBackColor = true;
            this.btnEncrypt.Click += new System.EventHandler(this.btnEncrypt_Click);
            // 
            // btnDecrypt
            // 
            this.btnDecrypt.Location = new System.Drawing.Point(287, 243);
            this.btnDecrypt.Name = "btnDecrypt";
            this.btnDecrypt.Size = new System.Drawing.Size(253, 45);
            this.btnDecrypt.TabIndex = 3;
            this.btnDecrypt.Text = "Decrypt File";
            this.btnDecrypt.UseVisualStyleBackColor = true;
            this.btnDecrypt.Click += new System.EventHandler(this.btnDecrypt_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(30, 321);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(160, 45);
            this.btnClear.TabIndex = 4;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(35, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(189, 25);
            this.label1.TabIndex = 5;
            this.label1.Text = "RSA-key Handling";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(30, 215);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(138, 25);
            this.label2.TabIndex = 6;
            this.label2.Text = "File Handling";
            // 
            // tbxStatus
            // 
            this.tbxStatus.Location = new System.Drawing.Point(35, 152);
            this.tbxStatus.Name = "tbxStatus";
            this.tbxStatus.ReadOnly = true;
            this.tbxStatus.Size = new System.Drawing.Size(505, 26);
            this.tbxStatus.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(31, 123);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 22);
            this.label3.TabIndex = 8;
            this.label3.Text = "Status";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(576, 392);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbxStatus);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnDecrypt);
            this.Controls.Add(this.btnEncrypt);
            this.Controls.Add(this.btnExportKeys);
            this.Controls.Add(this.btnCreateKeys);
            this.Name = "FormMain";
            this.Text = "RSA Encryption & Decryption";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCreateKeys;
        private System.Windows.Forms.Button btnExportKeys;
        private System.Windows.Forms.Button btnEncrypt;
        private System.Windows.Forms.Button btnDecrypt;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbxStatus;
        private System.Windows.Forms.Label label3;
    }
}

