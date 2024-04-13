using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace exam_b
{
    public partial class FormEntry : Form
    {
        public static string EntryName { get; set; }
        public static string EntryEmail { get; set; }
        public static string EntryPassword { get; set; }
        public FormEntry()
        {
            InitializeComponent();
            labelEntry.Text = "Вас приветствует приложение - Подготовка к экзамену" + Environment.NewLine + "для получения водительских прав категории «B»!";
            textBoxPasswordEntry.UseSystemPasswordChar = true;
        }

       /// <summary>
       /// Для входа в систему
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
       private void buttonEntry_Click(object sender, EventArgs e)
        {
            if (textBoxPasswordEntry.Text != "" && textBoxNameEntry.Text != "")
            {
                if (WorkDB.GetId(textBoxNameEntry.Text, this, buttonEntry, labelEmailEntry, textBoxNameEntry, textBoxPasswordEntry, textBoxContactEntry, buttonBackEntry))
                {
                    if (WorkDB.CheckPassword(textBoxPasswordEntry.Text))
                    {
                        EntryName = textBoxNameEntry.Text;
                        EntryEmail = WorkDB.GetEmail();
                        EntryPassword = textBoxPasswordEntry.Text;
                        MainForm mainForm = new MainForm();
                        mainForm.ShowDialog();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Пароль не подходит: проверьте введенные данные", "Неправильный пароль", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    if (WorkDB.CheckName(textBoxNameEntry.Text))
                    {
                        if (MyUser.IsValidEmail(textBoxContactEntry.Text)) 
                        {
                            WorkDB.AddNewUser(textBoxNameEntry.Text, textBoxContactEntry.Text, textBoxPasswordEntry.Text);
                            EntryName = textBoxNameEntry.Text;
                            EntryEmail = textBoxContactEntry.Text;
                            EntryPassword = textBoxPasswordEntry.Text;
                            MainForm mainForm = new MainForm();
                            mainForm.ShowDialog();
                            this.Close();
                        }
                        else
                        {
                            errorProviderEntry.SetError(textBoxContactEntry, "Пустое поле/Неправильный формат");
                            if (textBoxContactEntry.Text != "") MessageBox.Show("Неправильный формат email!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Такое имя пользователя занято!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            else
            {
                MessageBox.Show("Введены не все данные", "Ошибка входа", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxPasswordEntry.UseSystemPasswordChar = true;
            }
        }

        /// <summary>
        /// Открываем пароль при нажатии кнопки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOpenPassword_MouseDown(object sender, MouseEventArgs e)
        {
            textBoxPasswordEntry.UseSystemPasswordChar = false;
        }

        /// <summary>
        /// Скрываем пароль
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOpenPassword_MouseUp(object sender, MouseEventArgs e)
        {
            textBoxPasswordEntry.UseSystemPasswordChar = true;
        }

        /// <summary>
        /// Возврат формы к начальному состоянию
        /// </summary>
        private void ResetForm() 
        {
            this.Text = "Вход";

            buttonEntry.Text = "ВХОД";

            labelEmailEntry.Visible = false;
            textBoxContactEntry.Visible = false;

            textBoxPasswordEntry.UseSystemPasswordChar = true;

            buttonBackEntry.Visible = false;

            textBoxNameEntry.Text = "";
            textBoxPasswordEntry.Text = "";
            textBoxContactEntry.Text = "";

            errorProviderEntry.Clear();

        }

        /// <summary>
        /// Из регистрации обратно к входу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonBackEntry_Click(object sender, EventArgs e)
        {
            ResetForm();
        }
        
    }
}
