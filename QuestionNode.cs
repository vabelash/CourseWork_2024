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
using System.Configuration;

namespace exam_b
{
    public class QuestionNode : TreeNode
    {
        public long Id { get; set; }
        public int Ticket { get; set; }
        public int Category { get; set; }
        public string Explanation { get; set; }
        public string Picrure { get; set; }

        public QuestionNode(string txt, long id, int ticket, int ctg, string exp, string pict) : base(txt)
        {
            Id = id;
            Ticket = ticket;
            Category = ctg;
            Explanation = exp;
            Picrure = pict;
        }

    }
}
