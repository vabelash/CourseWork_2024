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
using System.Text.RegularExpressions;
using System.Windows.Forms.DataVisualization;

namespace exam_b
{
    public class MyUser
    {
        private string name;
        private string email;
        private string password;
        
        public long UserID { get; set; }
        public string Name
        {
            get => name;
            set
            {
                if (value.Length>200)
                    name = value.Substring(0, 200);
                else
                    name = value;
            }
        }
        public string Email
        {
            get => email;
            set
            {
                if (IsValidEmail(value))
                    email = value;
                else
                    throw new ArgumentException("Неверный формат Email");
            }
        }

        public string Password
        {
            get => password;
            set
            {
                if (value.Length > 200)
                    password = value.Substring(0, 200);
                else
                    password = value;
            }
        }
        public int PraciceScore { get; set; }
        public int TheoryScore { get; set; }

        public static bool IsValidEmail(string email)
        {
            string pattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
            return Regex.IsMatch(email, pattern);
        }

        public MyUser(long id, string name, string email, string password, int ps, int ts) 
        {
            UserID = id;
            Name = name;
            Email = email;
            Password = password;
            PraciceScore = ps;
            TheoryScore = ts;
        }
    }                 
}                     