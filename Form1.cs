using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Runtime.InteropServices;
using MySqlX.XDevAPI.Common;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using System.Windows.Forms.VisualStyles;
using Org.BouncyCastle.Asn1.X509.SigI;
using System.Windows.Forms.DataVisualization;
using System.Windows.Forms.DataVisualization.Charting;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;
using System.Numerics;
using static Mysqlx.Expect.Open.Types.Condition.Types;
using Microsoft.Office.Interop.Excel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Status;
using Series = System.Windows.Forms.DataVisualization.Charting.Series;
using System.Security.Policy;
using exam_b.Properties;

namespace exam_b
{
    public partial class MainForm : Form
    {

        public static MyUser YoungDriver = new MyUser(WorkDB.ID, FormEntry.EntryName, FormEntry.EntryEmail, FormEntry.EntryPassword, 0, 0);

        public MainForm()
        {
            InitializeComponent();

            // Для личного кабинета

            Instruction.Text = "";

            SetPersonalData();

            SetStatistics();

            // Для тестирования 

            CreateTreeQuestions();

            // Для практики 
            labelPractice.Text = "Вы управляете красным автомобилем, который движется вперед. Чтобы остановить движение автомобиля, зажмите левую кнопку мыши. Результаты прохождения упражнений выводятся ниже.";
        }


        #region Кнопки ЛК
        /// <summary>
        /// Открыть руководство пользователя
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonUserHelp_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Выход из аккаунта на форму входа
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonExit_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("Вы действительно хотите выйти из аккаунта?\n- Да, хочу выйти\n- Нет", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.Close();
                FormEntry entryForm = new FormEntry();
                entryForm.ShowDialog();
            }
        }

        /// <summary>
        /// Скачать данные о прохождении теории и практики
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы хотите сохранить статистику вашего прогресса (в папке D:)?", "Сохранение статистики", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                object[] dataChartTheory = WorkDB.GetIfrormationGraph();
                UserStatistics userStatistics = new UserStatistics(dataChartTheory, chartTheory);
                SetStatistics();

                userStatistics.SaveImage();
                ExcelTable.SaveExcel();
            }
        }
        #endregion

        #region Формирование ЛК
        /// <summary>
        /// Загрузка в форму ЛК персональной информации
        /// </summary>
        private void SetPersonalData() 
        {
            textBoxName.Text = YoungDriver.Name;
            textBoxContactUser.Text = YoungDriver.Email;
            textBoxPassword.Text = YoungDriver.Password;

            textBoxPassword.UseSystemPasswordChar = true;
        }

        private void SetStatistics()
        {
            object[] dataChartTheory = WorkDB.GetIfrormationGraph();
            UserStatistics userStatistics = new UserStatistics(dataChartTheory, chartTheory);

            if (checkBoxT.Checked && checkBoxP.Checked)
            {
                userStatistics.BuildBoth();
            }
            else if (checkBoxT.Checked)
            {
                userStatistics.BuildTheory(new System.Windows.Forms.DataVisualization.Charting.Series("Успехи в теории"));
            }
            else if (checkBoxP.Checked)
            {
                userStatistics.BuildPractice(new System.Windows.Forms.DataVisualization.Charting.Series("Успехи в практике"));
            }
            else 
            {
                userStatistics.ClearGraph();
            }

        }

        private void checkBoxT_CheckedChanged(object sender, EventArgs e)
        {
            SetStatistics();
        }

        private void checkBoxP_CheckedChanged(object sender, EventArgs e)
        {
            SetStatistics();
        }
        #endregion

        #region Дерево вопросов

        /// <summary>
        /// Создание дерева вопросов
        /// </summary>
        private void CreateTreeQuestions()
        {
            var nodeTickets = new TreeNode("Вопросы ПДД по билетам");
            treeView.Nodes.Add(nodeTickets);
            WorkDB.LoadTickets(nodeTickets);

            var nodeCategories = new TreeNode("Вопросы ПДД по темам");
            treeView.Nodes.Add(nodeCategories);
            WorkDB.LoadCategories(nodeCategories);

            var nodeFavorites = new TreeNode("Избранные вопросы");
            treeView.Nodes.Add(nodeFavorites);
            WorkDB.LoadFavorites(nodeFavorites);
        }


        /// <summary>
        /// События после выбора узла  дерева вопросов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // При выборе верхнего узла открывается первый вопрос из списка
            if (treeView.SelectedNode.Text != "Избранные вопросы" && treeView.SelectedNode.Level == 0)
            {
                treeView.SelectedNode = treeView.SelectedNode.Nodes[0].Nodes[0];
            }
            else if (treeView.SelectedNode.Nodes.Count > 0 && ((!(treeView.SelectedNode.Parent is null) && treeView.SelectedNode.Parent.Text != "Избранные вопросы" && treeView.SelectedNode.Level == 1) || treeView.SelectedNode.Text == "Избранные вопросы"))
            {
                treeView.SelectedNode = treeView.SelectedNode.Nodes[0];
            }

            // Для вопросов из избранного находится источник вопроса
            if (!(treeView.SelectedNode.Parent is null) && treeView.SelectedNode.Parent.Text == "Избранные вопросы") 
            {
                foreach (TreeNode ticket in treeView.Nodes[0].Nodes)
                    foreach (QuestionNode question in ticket.Nodes)
                        if (treeView.SelectedNode.Text == question.Text)
                            treeView.SelectedNode = question;
            }

            // При выборе конкретного вопроса загружаются... 
            if (treeView.SelectedNode.Level == 2) 
            {
                QuestionNode node = treeView.SelectedNode as QuestionNode;

                // ...Отображение типа выбранного вопроса
                textType.Text = node.Parent.Parent.Text + ": " + node.Parent.Text;

                // ...Отображение текста вопроса
                textBoxQuestion.Text = node.Text;

                // ...Отображение картинки для вопроса
                pictureBoxQuestion.Load(node.Picrure);

                // ...Открытие кнопки для отправки ответа
                buttonGiveAnswer.Enabled = true;
                buttonAgainGiveAnswer.Visible = false;

                // ...Отображение вариантов ответы
                string[] answersName = WorkDB.GetAnswersName(node);
                SetButtonLetter(ButtonA, answersName[0]);
                SetButtonLetter(ButtonB, answersName[1]);
                SetButtonLetter(ButtonC, answersName[2]);
                SetButtonLetter(ButtonD, answersName[3]);

                // ...Объяснение правильного ответа
                buttonShowExplaination.Enabled = true;
                textBoxExplaination.Text = "";

                // ...Навигация по вопросам
                if (node.PrevNode is null) buttonPrevQ.Enabled = false; else buttonPrevQ.Enabled = true;
                if (node.NextNode is null) buttonNextQ.Enabled = false; else buttonNextQ.Enabled = true;

                // ...Работа  с избранным
                buttonLike.Enabled = true;
                if (treeView.Nodes[2].Nodes.ContainsKey(node.Text)) buttonLike.BackColor = Color.LightGreen; else buttonLike.BackColor = Color.Silver;

            }
        }
        #endregion

        #region Кнопки для навигации по вопросам

        /// <summary>
        /// К предыдущему вопросу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonPrevQ_Click(object sender, EventArgs e)
        {
            var node = treeView.SelectedNode;
            if (node.PrevNode is null)
            {
                buttonPrevQ.Enabled = false;
            }
            else
            {
                treeView.SelectedNode = treeView.SelectedNode.PrevNode;
            }
        }

        /// <summary>
        /// К следующему вопросу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonNextQ_Click(object sender, EventArgs e)
        {
            var node = treeView.SelectedNode;
            if (node.NextNode is null)
            {
                buttonNextQ.Enabled = false;
            }
            else
            {
                treeView.SelectedNode = treeView.SelectedNode.NextNode;
            }
        }
        #endregion

        #region Кнопки для окна тестирования
        private void buttonGiveAnswer_Click(object sender, EventArgs e)
        {
            ButtonA.Enabled = false;
            ButtonB.Enabled = false;
            ButtonC.Enabled = false;
            ButtonD.Enabled = false;

            QuestionNode node = treeView.SelectedNode as QuestionNode;
            bool[] answersRight = WorkDB.GetAnswersRight(node);

            CheckAnswer(ButtonA, answersRight[0]);
            CheckAnswer(ButtonB, answersRight[1]);
            CheckAnswer(ButtonC, answersRight[2]);
            CheckAnswer(ButtonD, answersRight[3]);

            buttonGiveAnswer.Enabled = false;
            buttonAgainGiveAnswer.Visible = true;

        }

        private void buttonAgainGiveAnswer_Click(object sender, EventArgs e)
        {
            ButtonA.Enabled = true;
            ButtonB.Enabled = true;
            ButtonC.Enabled = true;
            ButtonD.Enabled = true;

            ButtonA.BackColor = Color.Transparent;
            ButtonB.BackColor = Color.Transparent;
            ButtonC.BackColor = Color.Transparent;
            ButtonD.BackColor = Color.Transparent;

            textBoxExplaination.Text = "";

            buttonGiveAnswer.Enabled = true;
            buttonAgainGiveAnswer.Visible = false;
        }

        private void buttonShowExplaination_Click(object sender, EventArgs e)
        {
            QuestionNode node = treeView.SelectedNode as QuestionNode;
            if (!(node is null) && (node.Level == 2 || node.Parent.Text == "Избранные вопросы"))
            {
                buttonShowExplaination.Enabled = true;
                textBoxExplaination.Text = node.Explanation;
            }
            else
            {
                textBoxExplaination.Text = "";
                buttonShowExplaination.Enabled = false;
            }
        }

        /// <summary>
        /// Работа с избранными вопросами
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonLike_Click(object sender, EventArgs e)
        {
            if (!(treeView.SelectedNode is null) && treeView.SelectedNode.Level == 2)
            {
                QuestionNode node = treeView.SelectedNode as QuestionNode;
                // При нажатии на кнопку проверяется наличие вопроса в избранном
                if (buttonLike.BackColor == Color.LightGreen)
                {
                    WorkDB.DeleteQuestion(node, node.Id, treeView, buttonLike);
                }
                else
                {
                    WorkDB.AddQuestion(node, node.Id, treeView);
                    buttonLike.BackColor = Color.LightGreen;
                }
            }
        }
        #endregion

        #region Выбор ответа

        /// <summary>
        /// Загрузка кнопки ответов 
        /// </summary>
        /// <param name="btn"> Кнопка </param>
        /// <param name="name"> Название </param>
        private void SetButtonLetter(RadioButton btn, string name)
        {
            btn.Text = name;
            btn.Visible = true;
            btn.Enabled = true;
            btn.BackColor = Color.Transparent;

        }
        /// <summary>
        /// Проверка правильности выбранной кнопки 
        /// </summary>
        /// <param name="selectedRadioButton">Выбранная кнопка</param>
        /// <param name="isRight">Является ли ответом</param>
        private void CheckAnswer(RadioButton selectedRadioButton, bool isRight)
        {

            if (selectedRadioButton != null && selectedRadioButton.Checked)
            {
                if (isRight)
                {
                    selectedRadioButton.BackColor = Color.LightGreen;
                    WorkDB.AddAttempt(true, DateTime.Now, treeView);
                }
                else
                {
                    selectedRadioButton.BackColor = Color.Red;
                    WorkDB.AddAttempt(false, DateTime.Now, treeView);
                }
                    WorkDB.AddAttempt(false, DateTime.Now, treeView);
                //object[] dataCharTheory = WorkDB.GetIfrormationTheoryGraph();
                //UserStatistics userStatistics = new UserStatistics(dataCharTheory, chartTheory);
                //userStatistics.BuildGaph();
                SetStatistics();
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            if (radioButton.Checked)
            {
                radioButton.BackColor = Color.LightGray;
                ButtonB.BackColor = Color.Transparent;
                ButtonC.BackColor = Color.Transparent;
                ButtonD.BackColor = Color.Transparent;
            }
        }

        private void ButtonB_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            if (radioButton.Checked)
            {
                radioButton.BackColor = Color.LightGray;
                ButtonA.BackColor = Color.Transparent;
                ButtonC.BackColor = Color.Transparent;
                ButtonD.BackColor = Color.Transparent;
            }
        }
        private void ButtonC_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            if (radioButton.Checked)
            {
                radioButton.BackColor = Color.LightGray;
                ButtonA.BackColor = Color.Transparent;
                ButtonB.BackColor = Color.Transparent;
                ButtonD.BackColor = Color.Transparent;
            }
        }

        private void ButtonD_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            if (radioButton.Checked)
            {
                radioButton.BackColor = Color.LightGray;
                ButtonA.BackColor = Color.Transparent;
                ButtonB.BackColor = Color.Transparent;
                ButtonC.BackColor = Color.Transparent;
            }
        }
        #endregion

        #region Кнопки для изменения персональных данных
        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            WorkDB.UpdateName(textBoxName.Text, YoungDriver);
            textBoxName.Text = YoungDriver.Name;
        }

        private void buttonUpdateEmail_Click(object sender, EventArgs e)
        {
            WorkDB.UpdateEmail(textBoxContactUser, YoungDriver);
            textBoxContactUser.Text = YoungDriver.Email;
        }

        private void buttonUpdatePassword_Click(object sender, EventArgs e)
        {
            WorkDB.UpdatePassword(textBoxPassword, YoungDriver);
            textBoxPassword.Text = YoungDriver.Password;
            textBoxPassword.UseSystemPasswordChar = true;
        }

        private void textBoxPassword_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы хотите, чтобы пароль был открытым?", "Отображение пароля", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                textBoxPassword.UseSystemPasswordChar = false;
            }
            else
            {
                textBoxPassword.UseSystemPasswordChar = true;
            }
        }
        #endregion

        #region Кнопки симулятора

        /// <summary>
        /// Начать отработку упражнения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            Game.gameTime = 0;

            Reset();
            if (radioButtonRedSignal.Checked) player.Top = pictureBoxRoadDown.Bottom - 5;
            
            buttonPausePractice.Enabled = true;
        }

        /// <summary>
        /// Красный светофор
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButtonRedSignal_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            if (radioButton.Checked)
            {
                
                MessageBox.Show("Проезд на красный свет запрещен и может привести к штрафу или лишению водительских прав: вам следует остановиться на перекрестке перед пересекаемой проезжей частью до включения зеленого сигнала.", "Отработка остановки на красный сигнал светофора", MessageBoxButtons.OK);

                carAI.Visible = false;
                pictureBoxSignal.Visible = true;

                Game.exerciseIndex = 0;
            }
        }

        /// <summary>
        /// Помеха справа
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButtonRightCar_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            if (radioButton.Checked)
            {
                MessageBox.Show("На равнозначном перекрестке траектории движения пересекаются (серый автомобиль тоже едет прямо): вам следует определить, к которому транспортное средство приближается справа.", "Отработка правила «помеха справа»", MessageBoxButtons.OK);

                carAI.Visible = true;
                pictureBoxSignal.Visible = false;

                Game.exerciseIndex = 1;
            }
        }

       /// <summary>
       /// Остановить упражнение
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
       private void buttonPausePractice_Click(object sender, EventArgs e)
        {
            gameOver("незаконченная попытка");
        }
        #endregion


        #region Автоматически созданные методы
        private void textBoxPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxQuestion_TextChanged(object sender, EventArgs e)
        {

        }

        

        private void textBoxExplaination_TextChanged(object sender, EventArgs e)
        {

        }

        private void splitContainer2_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void labelPassword_Click(object sender, EventArgs e)
        {

        }

        private void labelEmail_Click(object sender, EventArgs e)
        {

        }


        private void panelAnswers_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panelProgress_Paint(object sender, PaintEventArgs e)
        {

        }
        private void Road_Paint(object sender, PaintEventArgs e)
        {

        }

        private void moveCar(object sender, MouseEventArgs e)
        {

        }

        private void stopCar(object sender, MouseEventArgs e)
        {

        }

        #endregion

        #region Механика игры
        private void timerPractice_Tick(object sender, EventArgs e)
        {
            Game.gameTime++;
            if (Game.exerciseIndex == 1)
            {
                Game.GoRightCar(carAI, pictureBoxRoadLeft);
                Game.GoPlayer(player, pictureBoxRoadUp);
                if (!Game.CheckFirst(player, carAI, DangerousZone))
                {
                    Game.currentScore = 0;
                    gameOver("вы нарушили правило о помехи справа");
                }
                else if (player.Bottom <= pictureBoxRoadUp.Top)
                {
                    Game.currentScore = 2;
                    gameOver("вы успешно выполненили упражнения на помеху справа");
                }
            }
            else 
            {
                Game.GoPlayer(player, pictureBoxRoadUp);
                if (Game.gameTime <= 15) 
                {
                    if (Game.CheckSignal(player, pictureBoxRoadCenter))
                    {
                        Game.currentScore = 0;
                        gameOver("вы нарушили ПДД, проехав на запрещающий сигнал");
                    }
                }
                else if (Game.gameTime > 15 && Game.gameTime <= 140)
                {
                    pictureBoxSignal.Image = Resources.redSignalLight;
                    if (Game.CheckSignal(player, pictureBoxRoadCenter))
                    {
                        Game.currentScore = 0;
                        gameOver("вы нарушили ПДД, проехав на запрещающий сигнал");

                    }
                }
                else if (Game.gameTime > 140) 
                {
                    pictureBoxSignal.Image = Resources.greenSignalLight;
                    if (Game.CheckSignal(player, pictureBoxRoadCenter) && player.Bottom <= pictureBoxRoadUp.Top)
                    {
                        Game.currentScore = 2;
                        gameOver("вы успешно выполненили упражнения на сигналы светофора");
                    }
                }

            }
        }

        private void Reset()
        {
            textBoxPracticeResults.Visible = true;
            buttonRestartPractice.Enabled = false;

            carAI.Left = pictureBoxRoadRight.Right;
            player.Top = pictureBoxRoadDown.Bottom;

            pictureBoxSignal.Image = Resources.yellowSignalLight;

            player.Enabled = true;

            timerPractice.Start();
        }

        private void gameOver(string txt) 
        {
            timerPractice.Stop();

            WorkDB.AddPracticeAttempt(DateTime.Now, Game.currentScore);
            SetStatistics();

            textBoxPracticeResults.Text = textBoxPracticeResults.Text + $"{DateTime.Now}: {txt}: {Game.currentScore}" + Environment.NewLine;
            
            buttonRestartPractice.Enabled = true;
            buttonPausePractice.Enabled = false;

            player.Enabled = false;

        }

        #endregion

        #region Управление красной машиной с помощью мышки 

        private System.Drawing.Point MouseDownLocation;
        private void playerMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                MouseDownLocation = e.Location;
            }
        }
        
        private void playerMouseMove(object sender, MouseEventArgs e)
        {
            int speed = Game.carSpeed;
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                player.Top = e.Y + player.Top - MouseDownLocation.Y;
                Game.carSpeed = 0;
            }
            Game.carSpeed = speed;
        }
        #endregion
    }
}