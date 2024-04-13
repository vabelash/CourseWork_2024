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
using TreeView = System.Windows.Forms.TreeView;

namespace exam_b
{
    public class WorkDB
    {
        public static string cnct = "server=localhost;user=root;database=exam_db;port=3306;password=";
        public static long ID = 0;

        /// <summary>
        /// Генерирует ID
        /// </summary>
        /// <returns></returns>
        private static long GenerateNeId() 
        {
            return new Random().Next(10, 100000000); ;
        }


        #region Для входа и регистрации пользователей

        /// <summary>
        /// Проверка id для входа пользователя
        /// </summary>
        /// <param name="name"></param>
        /// <param name="frm"></param>
        /// <param name="buttonEntry"></param>
        /// <param name="labelEmailEntry"></param>
        /// <param name="textBoxNameEntry"></param>
        /// <param name="textBoxPasswordEntry"></param>
        /// <param name="textBoxContactEntry"></param>
        /// <returns></returns>
        public static bool GetId(string name, Form frm, System.Windows.Forms.Button buttonEntry, Label labelEmailEntry, System.Windows.Forms.TextBox textBoxNameEntry, System.Windows.Forms.TextBox textBoxPasswordEntry, MaskedTextBox textBoxContactEntry, System.Windows.Forms.Button buttonBackEntry)
        {

            using (var con = new MySqlConnection(cnct))
            {
                con.Open();
                var cmd = new MySqlCommand(@"SELECT user_id FROM users WHERE user_name = @name;", con);
                cmd.Parameters.AddWithValue("@name", name);
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        ID = (long)dr["user_id"];
                    }
                }
            }
            if (ID == 0)
            {
                if (MessageBox.Show("Вы хотите зарегестрировать нового пользователя с таким именем?", "Введено несуществующее имя пользователя", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    frm.Text = "Регистрация нового пользователя";
                    buttonEntry.Text = "Зарегистрироваться";
                    labelEmailEntry.Visible = true;
                    textBoxContactEntry.Visible = true;
                    buttonBackEntry.Visible = true;
                }
                else
                {
                    textBoxNameEntry.Text = "";
                    textBoxPasswordEntry.Text = "";
                }
                return false;
            }
            return true;
        }

        /// <summary>
        /// Получение значение email из БД
        /// </summary>
        /// <returns></returns>
        public static string GetEmail() 
        {
            string email = "email@mail.ru";
            using (var con = new MySqlConnection(cnct))
            {
                con.Open();
                var cmd = new MySqlCommand(@"SELECT email FROM users WHERE user_id = @id;", con);
                cmd.Parameters.AddWithValue("@id", ID);
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        email = dr["email"].ToString();
                    }
                }
            }
            return email;
        }

        /// <summary>
        /// Получение баллов за теорию БД
        /// </summary>
        /// <returns></returns>
        public static int GetTS()
        {
            int ts = 0;
            using (var con = new MySqlConnection(cnct))
            {
                con.Open();
                var cmd = new MySqlCommand(@"SELECT theory_score FROM users WHERE user_id = @id;", con);
                cmd.Parameters.AddWithValue("@id", ID);
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        ts = (int)dr["theory_score"];
                    }
                }
            }
            return ts;
        }

        /// <summary>
        /// Получение баллов за практику БД
        /// </summary>
        /// <returns></returns>
        public static int GetPS()
        {
            int ps = 0;
            using (var con = new MySqlConnection(cnct))
            {
                con.Open();
                var cmd = new MySqlCommand(@"SELECT pracice_score FROM users WHERE user_id = @id;", con);
                cmd.Parameters.AddWithValue("@id", ID);
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        ps = (int)dr["pracice_score"];
                    }
                }
            }
            return ps;
        }

        /// <summary>
        /// Подходит ли пароль для выбранного пользователя
        /// </summary>
        /// <param name="id"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool CheckPassword(string password)
        {
            using (var con = new MySqlConnection(cnct))
            {
                con.Open();
                var cmd = new MySqlCommand(@"SELECT password FROM users WHERE user_id = @id;", con);
                cmd.Parameters.AddWithValue("@id", ID);
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        return dr["password"].ToString() == password;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Свободно ли имя для пользователя
        /// </summary>
        /// <param name="id"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool CheckName(string name)
        {
            bool isExit = false;
            using (var con = new MySqlConnection(cnct))
            {
                con.Open();
                var cmd = new MySqlCommand(@"SELECT user_name FROM users;", con);
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        isExit = dr["user_name"].ToString() == name;
                        if (isExit) return false;
                    }
                }
            }
            return true;
        }

        public static void AddNewUser(string name, string email, string password)
        {
            using (var con = new MySqlConnection(cnct))
            {
                con.Open();
                long id = GenerateNeId();
                var cmd = new MySqlCommand(@"INSERT INTO users(user_id, user_name, email, password, pracice_score, theory_score) 
                                                        VALUES (@id, @name, @email, @password, 0, 0)", con);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@password", password);
                cmd.ExecuteNonQuery();

                ID = id;
            }
        }
        #endregion

        #region Для проверки теории

        /// <summary>
        /// Загрузка билетов в боковую панель
        /// </summary>
        /// <param name="parent">Вопросы ПДД по билетам</param>
        public static void LoadTickets(TreeNode parent)
        {
            using (var con = new MySqlConnection(cnct))
            {
                con.Open();
                var cmd = new MySqlCommand(@"SELECT theory_tickets_id, theory_tickets_name FROM theory_tickets ORDER BY theory_tickets_name", con);
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var node = new TreeNode(dr["theory_tickets_name"].ToString());
                        parent.Nodes.Add(node);
                        LoadQuestions(node);
                    }
                }
            }
        }

        /// <summary>
        /// Загрузка тем в боковую панель
        /// </summary>
        /// <param name="parent">Вопросы ПДД по темам</param>
        public static void LoadCategories(TreeNode parent)
        {
            using (var con = new MySqlConnection(cnct))
            {
                con.Open();
                var cmd = new MySqlCommand(@"SELECT theory_categories_id, theory_category_name FROM theory_categories ORDER BY theory_category_name", con);
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var node = new TreeNode(dr["theory_category_name"].ToString());
                        parent.Nodes.Add(node);
                        LoadQuestions(node);
                    }
                }
            }
        }
        /// <summary>
        /// Загрузка вопросов (3 уровень тривью)
        /// </summary>
        /// <param name="parent"></param>
        public static void LoadQuestions(TreeNode parent)
        {
            using (var con = new MySqlConnection(cnct))
            {
                con.Open();
                var cmd = new MySqlCommand(@"SELECT question_text, theory_questions_id, theory_ticket, theory_category, 
                                             explanation, picture FROM theory_questions, theory_tickets 
                                             WHERE theory_ticket = theory_tickets_id AND theory_tickets_name = @id
                                             ORDER BY question_text", con);
                if (parent.Parent.Text == "Вопросы ПДД по темам")
                {
                    cmd = new MySqlCommand(@"SELECT question_text, theory_questions_id, theory_ticket, theory_category, 
                                             explanation, picture FROM theory_questions, theory_categories 
                                             WHERE theory_category = theory_categories_id AND theory_category_name = @id
                                             ORDER BY question_text", con);
                }
                cmd.Parameters.AddWithValue("@id", parent.Text);
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var node = new QuestionNode(dr["question_text"].ToString(), (long)dr["theory_questions_id"], (int)dr["theory_ticket"], (int)dr["theory_category"], dr["explanation"].ToString(), dr["picture"].ToString());
                        parent.Nodes.Add(node);
                    }
                }
            }
        }

        /// <summary>
        /// Загрузка избранных вопросов
        /// </summary>
        /// <param name="parent">Мое избранное</param>
        public static void LoadFavorites(TreeNode parent)
        {
            using (var con = new MySqlConnection(cnct))
            {
                con.Open();
                var cmd = new MySqlCommand(@"SELECT theory_questions.question_text FROM favourites, theory_questions 
                                             WHERE favourites.user = @UserId 
                                             AND theory_questions.theory_questions_id = favourites.question 
                                             ORDER BY question_text", con);
                cmd.Parameters.AddWithValue("@UserId", ID);
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        //var node = new TreeNode(dr["question_text"].ToString());
                        parent.Nodes.Add(dr["question_text"].ToString(), dr["question_text"].ToString());

                    }
                }
            }
        }

       
        /// <summary>
        /// Подключение к БД для получения вариантов ответа
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static string[] GetAnswersName(QuestionNode node)
        {
            string[] answers = new string[4];

            using (var con = new MySqlConnection(cnct))
            {
                con.Open();
                var cmd = new MySqlCommand(@"SELECT * FROM answers WHERE question = @quest", con);
                cmd.Parameters.AddWithValue("@quest", node.Id);
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if ((int)dr["letter"] == 1)
                        {
                            answers[0] = dr["answer"].ToString();
                        }
                        if ((int)dr["letter"] == 2)
                        {
                            answers[1] = dr["answer"].ToString();
                        }
                        if ((int)dr["letter"] == 3)
                        {
                            answers[2] = dr["answer"].ToString();
                        }
                        if ((int)dr["letter"] == 4)
                        {
                            answers[3] = dr["answer"].ToString();
                        }
                    }
                }
            }
            return answers;
        }
        /// <summary>
        /// Подключение к БД для получения правильного ответа
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static bool[] GetAnswersRight(QuestionNode node)
        {
            bool[] answers = new bool[4];

            using (var con = new MySqlConnection(cnct))
            {
                con.Open();
                var cmd = new MySqlCommand(@"SELECT * FROM answers WHERE question = @quest", con);
                cmd.Parameters.AddWithValue("@quest", node.Id);
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if ((int)dr["letter"] == 1)
                        {
                            answers[0] = (bool)dr["isRight"];
                        }
                        if ((int)dr["letter"] == 2)
                        {
                            answers[1] = (bool)dr["isRight"];
                        }
                        if ((int)dr["letter"] == 3)
                        {
                            answers[2] = (bool)dr["isRight"];
                        }
                        if ((int)dr["letter"] == 4)
                        {
                            answers[3] = (bool)dr["isRight"];
                        }
                    }
                }
            }
            return answers;
        }

        /// <summary>
        /// Удалить вопрос из избранного
        /// </summary>
        /// <param name="node"></param>
        /// <param name="id_quest"></param>
        /// <param name="treeView"></param>
        /// <param name="buttonLike"></param>
        public static void DeleteQuestion(TreeNode node, long id_quest, TreeView treeView, System.Windows.Forms.Button buttonLike)
        {
            if (MessageBox.Show("Вы хотите убрать вопрос из избранного?", "Удаление вопроса", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (var con = new MySqlConnection(cnct))
                {
                    con.Open();
                    var cmd = new MySqlCommand(@"DELETE FROM favourites
                                                WHERE question = @quest", con);
                    cmd.Parameters.AddWithValue("@quest", id_quest);
                    cmd.ExecuteNonQuery();
                    treeView.Nodes[2].Nodes.RemoveByKey(node.Text);
                    buttonLike.BackColor = Color.Silver;
                }
            }
        }
        /// <summary>
        /// Добавить вопрос в избанное
        /// </summary>
        /// <param name="node"></param>
        /// <param name="id_quest"></param>
        /// <param name="treeView"></param>
        public static void AddQuestion(TreeNode node, long id_quest, TreeView treeView)
        {
            using (var con = new MySqlConnection(cnct))
            {
                con.Open();
                long new_id = GenerateNeId();
                var cmd = new MySqlCommand(@"INSERT INTO favourites(favourite_id, question, user) 
                                                        VALUES (@id, @question, @user)", con);
                cmd.Parameters.AddWithValue("@id", new_id);
                cmd.Parameters.AddWithValue("@question", id_quest);
                cmd.Parameters.AddWithValue("@user", ID);
                cmd.ExecuteNonQuery();
                //var new_node = new TreeNode(node.Text);
                treeView.Nodes[2].Nodes.Add(node.Text, node.Text);
            }
        }
        #endregion

        #region Для личного кабинета

        //public static object[] GetMyUserData(long id)
        //{
        //    object[] myUser = new object[5];
        //    using (var con = new MySqlConnection("server=localhost;user=root;database=exam_db;port=3306;password="))
        //    {
        //        con.Open();
        //        var cmd = new MySqlCommand(@"SELECT * FROM users WHERE user_id=@user", con);
        //        cmd.Parameters.AddWithValue("@user", id);
        //        using (var dr = cmd.ExecuteReader())
        //        {
        //            while (dr.Read())
        //            {
        //                myUser[0] = (string)dr["user_name"];
        //                myUser[1] = (string)dr["email"];
        //                myUser[2] = (string)dr["password"];
        //                myUser[3] = (int)dr["pracice_score"];
        //                myUser[4] = (int)dr["theory_score"];
        //            }
        //        }
        //    }
        //    return myUser;
        //}

        /// <summary>
        /// Изменить имя пользователя
        /// </summary>
        /// <param name="textBoxName"></param>
        /// <param name="User"></param>
        public static void UpdateName(string name, MyUser User)
        {
            using (var con = new MySqlConnection(cnct))
            {
                con.Open();
                if (MessageBox.Show("Вы хотите изменить имя?", "Изменение имя пользователя", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (CheckName(name)) 
                    {
                        if (name != "")
                        {
                            var cmdUpdate = new MySqlCommand(@"UPDATE users
                                            SET user_name = @UserName
                                            WHERE user_id=@id", con);
                            cmdUpdate.Parameters.AddWithValue("@id", ID);
                            cmdUpdate.Parameters.AddWithValue("@UserName", name);
                            cmdUpdate.ExecuteNonQuery();
                            User.Name = name;
                        }
                        else
                        {
                            MessageBox.Show("Введите имя пользователя", "Пустой ввод", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Такое имя пользователя занято!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

       /// <summary>
       /// Изменить email
       /// </summary>
       /// <param name="textBoxContactUser"></param>
       /// <param name="User"></param>
       public static void UpdateEmail(MaskedTextBox textBoxContactUser, MyUser User)
        {
            using (var con = new MySqlConnection(cnct))
            {
                try
                {
                    User.Email = textBoxContactUser.Text;
                    con.Open();
                    if (MessageBox.Show("Вы хотите изменить e-mail?", "Изменение e-mail", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        var cmdUpdate = new MySqlCommand(@"UPDATE users
                                            SET email = @UserEmail
                                            WHERE user_id=@id", con);
                        cmdUpdate.Parameters.AddWithValue("@id", ID);
                        cmdUpdate.Parameters.AddWithValue("@UserEmail", User.Email);
                        cmdUpdate.ExecuteNonQuery();
                    }
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        /// <summary>
        /// Изменить пароль
        /// </summary>
        /// <param name="textBoxPassword"></param>
        /// <param name="User"></param>
        public static void UpdatePassword(System.Windows.Forms.TextBox textBoxPassword, MyUser User)
        {
            using (var con = new MySqlConnection(cnct))
            {
                try
                {
                    User.Password = textBoxPassword.Text;
                    con.Open();
                    if (MessageBox.Show("Вы хотите изменить пароль?", "Изменение пароля", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        textBoxPassword.UseSystemPasswordChar = false;
                        var cmdUpdate = new MySqlCommand(@"UPDATE users
                                            SET password = @UserPassword
                                            WHERE user_id=@id", con);
                        cmdUpdate.Parameters.AddWithValue("@id", ID);
                        cmdUpdate.Parameters.AddWithValue("@UserPassword", User.Password);
                        cmdUpdate.ExecuteNonQuery();
                    }
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        #endregion

        #region Для сбора статистики

        public static void AddAttempt(bool isAccept, DateTime time, TreeView treeView)
        {
            QuestionNode node = treeView.SelectedNode as QuestionNode;
            using (var con = new MySqlConnection(cnct))
            {
                con.Open();
                long new_id = GenerateNeId();
                var cmd = new MySqlCommand(@"INSERT INTO theory_attempts(theory_attempt_id, user, question, isAccept, data) 
                                                        VALUES (@id, @user, @quest_id, @isA, @data)", con);
                cmd.Parameters.AddWithValue("@id", new_id);
                cmd.Parameters.AddWithValue("@user", ID);
                cmd.Parameters.AddWithValue("@quest_id", node.Id);
                cmd.Parameters.AddWithValue("@isA", isAccept);
                cmd.Parameters.AddWithValue("@data", time);
                cmd.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// Загрузка данных о прохождении пользователем теории
        /// </summary>
        /// <returns></returns>
        public static List<string> LoadTheoryProgress()
        {
            List<string> data = new List<string>();

            using (var con = new MySqlConnection(cnct))
            {
                con.Open();
                var cmd = new MySqlCommand(@"SELECT question, isAccept, data FROM theory_attempts WHERE user=@user", con);
                cmd.Parameters.AddWithValue("@user", ID);
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        data.Add($"{(DateTime)dr["data"]};{(long)dr["question"]};{(bool)dr["isAccept"]}");
                    }
                }
            }
            return data;
        }

        /// <summary>
        /// Данные для построения графикак прогресса прохождения теории
        /// </summary>
        /// <returns></returns>
        public static object[] GetIfrormationGraph()
        {
            object[] results = new object[4];
            List<DateTime> theoryDates = new List<DateTime>();
            List<long> theoryRightAttempts = new List<long>();

            List<DateTime> practiceDates = new List<DateTime>();
            List<object> practiceScores = new List<object>();

            using (var con = new MySqlConnection(cnct))
            {
                con.Open();
                var cmdTheory = new MySqlCommand(@"SELECT data, COUNT(theory_attempt_id) as count_rights 
                                                    FROM theory_attempts WHERE isAccept=True AND user=@user 
                                                    GROUP BY data", con);
                cmdTheory.Parameters.AddWithValue("@user", ID);
                using (var dr = cmdTheory.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        theoryDates.Add((DateTime)dr["data"]);
                        theoryRightAttempts.Add((long)dr["count_rights"]);
                    }
                }
                var cmdPractice = new MySqlCommand(@"SELECT data, SUM(score) as count_scores 
                                                    FROM practice_attempts WHERE user=@user 
                                                    GROUP BY data", con);
                cmdPractice.Parameters.AddWithValue("@user", ID);
                using (var dr = cmdPractice.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        practiceDates.Add((DateTime)dr["data"]);
                        practiceScores.Add((object)dr["count_scores"]);
                    }
                }
            }
            results[0] = theoryDates;
            results[1] = theoryRightAttempts;
            results[2] = practiceDates;
            results[3] = practiceScores;
            return results;
        }

        #endregion

        #region 

        public static void AddPracticeAttempt(DateTime time, int score)
        {
            using (var con = new MySqlConnection(cnct))
            {
                con.Open();
                long new_id = GenerateNeId();
                var cmd = new MySqlCommand(@"INSERT INTO practice_attempts(practice_attempt_id, user, data, score) 
                                                        VALUES (@id, @user, @data, @score)", con);
                cmd.Parameters.AddWithValue("@id", new_id);
                cmd.Parameters.AddWithValue("@user", ID);
                cmd.Parameters.AddWithValue("@data", time);
                cmd.Parameters.AddWithValue("@score", score);
                cmd.ExecuteNonQuery();
            }
        }

        #endregion
    }
}
