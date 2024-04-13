namespace exam_b
{
    partial class FormEntry
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEntry));
            this.labelEntry = new System.Windows.Forms.Label();
            this.textBoxNameEntry = new System.Windows.Forms.TextBox();
            this.textBoxPasswordEntry = new System.Windows.Forms.TextBox();
            this.labelNameEntry = new System.Windows.Forms.Label();
            this.labelPasswordEntry = new System.Windows.Forms.Label();
            this.buttonEntry = new System.Windows.Forms.Button();
            this.textBoxContactEntry = new System.Windows.Forms.MaskedTextBox();
            this.labelEmailEntry = new System.Windows.Forms.Label();
            this.buttonBackEntry = new System.Windows.Forms.Button();
            this.buttonOpenPassword = new System.Windows.Forms.Button();
            this.errorProviderEntry = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderEntry)).BeginInit();
            this.SuspendLayout();
            // 
            // labelEntry
            // 
            this.labelEntry.AutoEllipsis = true;
            this.labelEntry.AutoSize = true;
            this.labelEntry.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelEntry.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelEntry.Location = new System.Drawing.Point(0, 0);
            this.labelEntry.Name = "labelEntry";
            this.labelEntry.Size = new System.Drawing.Size(92, 37);
            this.labelEntry.TabIndex = 0;
            this.labelEntry.Text = "текст";
            // 
            // textBoxNameEntry
            // 
            this.textBoxNameEntry.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxNameEntry.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxNameEntry.CausesValidation = false;
            this.textBoxNameEntry.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.textBoxNameEntry.Location = new System.Drawing.Point(29, 148);
            this.textBoxNameEntry.Name = "textBoxNameEntry";
            this.textBoxNameEntry.Size = new System.Drawing.Size(741, 29);
            this.textBoxNameEntry.TabIndex = 6;
            // 
            // textBoxPasswordEntry
            // 
            this.textBoxPasswordEntry.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxPasswordEntry.CausesValidation = false;
            this.textBoxPasswordEntry.Location = new System.Drawing.Point(30, 235);
            this.textBoxPasswordEntry.Name = "textBoxPasswordEntry";
            this.textBoxPasswordEntry.Size = new System.Drawing.Size(741, 29);
            this.textBoxPasswordEntry.TabIndex = 8;
            // 
            // labelNameEntry
            // 
            this.labelNameEntry.AutoSize = true;
            this.labelNameEntry.Location = new System.Drawing.Point(28, 120);
            this.labelNameEntry.Name = "labelNameEntry";
            this.labelNameEntry.Size = new System.Drawing.Size(278, 25);
            this.labelNameEntry.TabIndex = 10;
            this.labelNameEntry.Text = "Введите имя пользователя:";
            // 
            // labelPasswordEntry
            // 
            this.labelPasswordEntry.AutoSize = true;
            this.labelPasswordEntry.Location = new System.Drawing.Point(28, 207);
            this.labelPasswordEntry.Name = "labelPasswordEntry";
            this.labelPasswordEntry.Size = new System.Drawing.Size(167, 25);
            this.labelPasswordEntry.TabIndex = 11;
            this.labelPasswordEntry.Text = "Введите пароль:";
            // 
            // buttonEntry
            // 
            this.buttonEntry.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonEntry.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonEntry.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.14286F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonEntry.Location = new System.Drawing.Point(647, 354);
            this.buttonEntry.Name = "buttonEntry";
            this.buttonEntry.Size = new System.Drawing.Size(385, 84);
            this.buttonEntry.TabIndex = 12;
            this.buttonEntry.Text = "ВХОД";
            this.buttonEntry.UseVisualStyleBackColor = false;
            this.buttonEntry.Click += new System.EventHandler(this.buttonEntry_Click);
            // 
            // textBoxContactEntry
            // 
            this.textBoxContactEntry.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxContactEntry.Location = new System.Drawing.Point(30, 310);
            this.textBoxContactEntry.Name = "textBoxContactEntry";
            this.textBoxContactEntry.Size = new System.Drawing.Size(741, 29);
            this.textBoxContactEntry.TabIndex = 13;
            this.textBoxContactEntry.Visible = false;
            // 
            // labelEmailEntry
            // 
            this.labelEmailEntry.AutoSize = true;
            this.labelEmailEntry.Location = new System.Drawing.Point(28, 282);
            this.labelEmailEntry.Name = "labelEmailEntry";
            this.labelEmailEntry.Size = new System.Drawing.Size(148, 25);
            this.labelEmailEntry.TabIndex = 14;
            this.labelEmailEntry.Text = "Введите email:";
            this.labelEmailEntry.Visible = false;
            // 
            // buttonBackEntry
            // 
            this.buttonBackEntry.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonBackEntry.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonBackEntry.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.14286F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonBackEntry.Image = global::exam_b.Properties.Resources._2203523_arrow_back_botton_left_icon1;
            this.buttonBackEntry.Location = new System.Drawing.Point(553, 354);
            this.buttonBackEntry.Name = "buttonBackEntry";
            this.buttonBackEntry.Size = new System.Drawing.Size(88, 84);
            this.buttonBackEntry.TabIndex = 15;
            this.buttonBackEntry.UseVisualStyleBackColor = false;
            this.buttonBackEntry.Visible = false;
            this.buttonBackEntry.Click += new System.EventHandler(this.buttonBackEntry_Click);
            // 
            // buttonOpenPassword
            // 
            this.buttonOpenPassword.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonOpenPassword.Image = global::exam_b.Properties.Resources._211738_eye_icon;
            this.buttonOpenPassword.Location = new System.Drawing.Point(782, 235);
            this.buttonOpenPassword.Name = "buttonOpenPassword";
            this.buttonOpenPassword.Size = new System.Drawing.Size(32, 32);
            this.buttonOpenPassword.TabIndex = 9;
            this.buttonOpenPassword.UseVisualStyleBackColor = true;
            this.buttonOpenPassword.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonOpenPassword_MouseDown);
            this.buttonOpenPassword.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonOpenPassword_MouseUp);
            // 
            // errorProviderEntry
            // 
            this.errorProviderEntry.ContainerControl = this;
            // 
            // FormEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1044, 450);
            this.Controls.Add(this.buttonBackEntry);
            this.Controls.Add(this.labelEmailEntry);
            this.Controls.Add(this.textBoxContactEntry);
            this.Controls.Add(this.buttonEntry);
            this.Controls.Add(this.labelPasswordEntry);
            this.Controls.Add(this.labelNameEntry);
            this.Controls.Add(this.buttonOpenPassword);
            this.Controls.Add(this.textBoxNameEntry);
            this.Controls.Add(this.textBoxPasswordEntry);
            this.Controls.Add(this.labelEntry);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1068, 514);
            this.MinimizeBox = false;
            this.Name = "FormEntry";
            this.Text = "Вход";
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderEntry)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelEntry;
        private System.Windows.Forms.TextBox textBoxNameEntry;
        private System.Windows.Forms.TextBox textBoxPasswordEntry;
        private System.Windows.Forms.Button buttonOpenPassword;
        private System.Windows.Forms.Label labelNameEntry;
        private System.Windows.Forms.Label labelPasswordEntry;
        private System.Windows.Forms.Button buttonEntry;
        private System.Windows.Forms.MaskedTextBox textBoxContactEntry;
        private System.Windows.Forms.Label labelEmailEntry;
        private System.Windows.Forms.Button buttonBackEntry;
        private System.Windows.Forms.ErrorProvider errorProviderEntry;
    }
}