using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using _excel = Microsoft.Office.Interop.Excel;

namespace exam_b
{
    public class ExcelTable
    {
        _Application excel = new _excel.Application();
        Workbook workbook;
        Worksheet worksheet;

        public void NewFile()
        {
            this.workbook = excel.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
            this.worksheet = this.workbook.Worksheets[1];
        }

        public void NewSheet()
        {
            Worksheet newSheet = excel.Worksheets.Add(After: this.worksheet);
        }

        public void SaveAs(string path)
        {
            workbook.SaveAs(path);
        }

        public void Close()
        {
            workbook.Close();
        }
        public void Load() 
        {
            List<string> data = WorkDB.LoadTheoryProgress();
            for (int i = 0; i < data.Count; i++)
            {
                worksheet.Cells[i+1, 1].Value = data[i];
            }
        }

        public static void SaveExcel()
        {

            ExcelTable file = new ExcelTable();

            file.NewFile();
            file.NewSheet();
            file.Load();
            try
            {
                string name = $"d:\\ProgressStatistics_{DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff")}.csv";
                file.SaveAs(@name);
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show("Ошибка доступа к файлу", ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (IOException ex)
            {
                MessageBox.Show("Ошибка записи файла", ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Попробуйте сохранить файл позже", ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            file.Close();
        }
    }
}

