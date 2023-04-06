namespace LinkListPuzzleGame
{
    partial class Form1
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
            usernameLbl = new System.Windows.Forms.Label();
            usernameTxt = new System.Windows.Forms.TextBox();
            loginBtn = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // usernameLbl
            // 
            usernameLbl.AutoSize = true;
            usernameLbl.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            usernameLbl.Location = new System.Drawing.Point(211, 36);
            usernameLbl.Name = "usernameLbl";
            usernameLbl.Size = new System.Drawing.Size(167, 30);
            usernameLbl.TabIndex = 0;
            usernameLbl.Text = "Kullanıcı Adınız";
            // 
            // usernameTxt
            // 
            usernameTxt.Location = new System.Drawing.Point(176, 95);
            usernameTxt.Name = "usernameTxt";
            usernameTxt.Size = new System.Drawing.Size(239, 23);
            usernameTxt.TabIndex = 1;
            usernameTxt.TextChanged += usernameTxt_TextChanged;
            // 
            // loginBtn
            // 
            loginBtn.BackColor = System.Drawing.Color.LightGreen;
            loginBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            loginBtn.Location = new System.Drawing.Point(247, 140);
            loginBtn.Name = "loginBtn";
            loginBtn.Size = new System.Drawing.Size(88, 20);
            loginBtn.TabIndex = 2;
            loginBtn.Text = "BAŞLA";
            loginBtn.UseVisualStyleBackColor = false;
            loginBtn.Click += loginBtn_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.SystemColors.ActiveCaption;
            ClientSize = new System.Drawing.Size(604, 209);
            Controls.Add(loginBtn);
            Controls.Add(usernameTxt);
            Controls.Add(usernameLbl);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            Name = "Form1";
            Text = "Giriş Sayfası";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label usernameLbl;
        private System.Windows.Forms.TextBox usernameTxt;
        private System.Windows.Forms.Button loginBtn;
    }
}
