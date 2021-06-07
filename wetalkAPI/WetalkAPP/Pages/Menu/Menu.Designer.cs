namespace WetalkAPP.Pages.Menu
{
    partial class Menu
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
            this.MenuLabel = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.QuitButton = new System.Windows.Forms.Button();
            this.ManageButton = new System.Windows.Forms.Button();
            this.FilesButton = new System.Windows.Forms.Button();
            this.ChatButton = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // MenuLabel
            // 
            this.MenuLabel.AutoSize = true;
            this.MenuLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.MenuLabel.Location = new System.Drawing.Point(164, 232);
            this.MenuLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.MenuLabel.Name = "MenuLabel";
            this.MenuLabel.Size = new System.Drawing.Size(78, 25);
            this.MenuLabel.TabIndex = 0;
            this.MenuLabel.Text = "MENU";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.CornflowerBlue;
            this.panel1.Controls.Add(this.QuitButton);
            this.panel1.Controls.Add(this.ManageButton);
            this.panel1.Controls.Add(this.FilesButton);
            this.panel1.Controls.Add(this.ChatButton);
            this.panel1.ForeColor = System.Drawing.Color.White;
            this.panel1.Location = new System.Drawing.Point(451, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(489, 528);
            this.panel1.TabIndex = 2;
            // 
            // QuitButton
            // 
            this.QuitButton.BackColor = System.Drawing.Color.Transparent;
            this.QuitButton.ForeColor = System.Drawing.Color.Black;
            this.QuitButton.Location = new System.Drawing.Point(135, 411);
            this.QuitButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.QuitButton.Name = "QuitButton";
            this.QuitButton.Size = new System.Drawing.Size(255, 73);
            this.QuitButton.TabIndex = 14;
            this.QuitButton.Text = "QUIT";
            this.QuitButton.UseVisualStyleBackColor = false;
            this.QuitButton.Click += new System.EventHandler(this.QuitButton_Click);
            // 
            // ManageButton
            // 
            this.ManageButton.BackColor = System.Drawing.Color.Transparent;
            this.ManageButton.ForeColor = System.Drawing.Color.Black;
            this.ManageButton.Location = new System.Drawing.Point(135, 232);
            this.ManageButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ManageButton.Name = "ManageButton";
            this.ManageButton.Size = new System.Drawing.Size(255, 73);
            this.ManageButton.TabIndex = 13;
            this.ManageButton.Text = "MANAGE USERS";
            this.ManageButton.UseVisualStyleBackColor = false;
            this.ManageButton.Click += new System.EventHandler(this.ManageButton_Click);
            // 
            // FilesButton
            // 
            this.FilesButton.BackColor = System.Drawing.Color.Transparent;
            this.FilesButton.ForeColor = System.Drawing.Color.Black;
            this.FilesButton.Location = new System.Drawing.Point(135, 136);
            this.FilesButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.FilesButton.Name = "FilesButton";
            this.FilesButton.Size = new System.Drawing.Size(255, 73);
            this.FilesButton.TabIndex = 12;
            this.FilesButton.Text = "FILES";
            this.FilesButton.UseVisualStyleBackColor = false;
            this.FilesButton.Click += new System.EventHandler(this.FilesButton_Click);
            // 
            // ChatButton
            // 
            this.ChatButton.BackColor = System.Drawing.Color.Transparent;
            this.ChatButton.ForeColor = System.Drawing.Color.Black;
            this.ChatButton.Location = new System.Drawing.Point(135, 41);
            this.ChatButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ChatButton.Name = "ChatButton";
            this.ChatButton.Size = new System.Drawing.Size(255, 73);
            this.ChatButton.TabIndex = 11;
            this.ChatButton.Text = "CHAT";
            this.ChatButton.UseVisualStyleBackColor = false;
            this.ChatButton.Click += new System.EventHandler(this.ChatButton_Click);
            // 
            // Menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 519);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.MenuLabel);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "Menu";
            this.Text = "Menu";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label MenuLabel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button ChatButton;
        private System.Windows.Forms.Button FilesButton;
        private System.Windows.Forms.Button QuitButton;
        private System.Windows.Forms.Button ManageButton;
    }
}