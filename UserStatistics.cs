using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace exam_b
{
    public class UserStatistics
    {
        object[] dataChart { get; set; }
        System.Windows.Forms.DataVisualization.Charting.Chart GraphChart { get; set; }
        public UserStatistics(object[] data, System.Windows.Forms.DataVisualization.Charting.Chart chart)
        {
            dataChart = data;
            GraphChart = chart;
        }
        public void BuildBoth()
        {
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series("Успехи в теории");
            BuildTheory(series1);

            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series("Успехи в практике");
            BuildPractice(series2);

            GraphChart.Series.Clear();
            GraphChart.Series.Add(series1);
            GraphChart.Series.Add(series2);


            GraphChart.ResetAutoValues();
            GraphChart.Titles.Clear();

            GraphChart.Titles.Add("Ваш прогресс");
            GraphChart.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.LightGray;
            GraphChart.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.LightGray;
        }

        public void BuildTheory(System.Windows.Forms.DataVisualization.Charting.Series series1) 
        {
            List<DateTime> theoryX = (List<DateTime>)dataChart[0];
            List<long> theoryY = (List<long>)dataChart[1];

            series1.ChartType = SeriesChartType.FastLine;
            series1.BorderWidth = 7;
            for (int i = 0; i < theoryX.Count(); i++)
            {
                series1.Points.AddXY(theoryX[i].ToString("d"), theoryY[i]);
            }

            GraphChart.Series.Clear();
            GraphChart.Series.Add(series1);

            GraphChart.ResetAutoValues();
            GraphChart.Titles.Clear();

            GraphChart.Titles.Add("Ваш прогресс");
            GraphChart.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.LightGray;
            GraphChart.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.LightGray;
        }

        public void BuildPractice(System.Windows.Forms.DataVisualization.Charting.Series series2)
        {
            List<DateTime> practiceX = (List<DateTime>)dataChart[2];
            List<object> practiceY = (List<object>)dataChart[3];

            series2.ChartType = SeriesChartType.FastLine;
            series2.BorderWidth = 7;
            for (int i = 0; i < practiceX.Count(); i++)
            {
                series2.Points.AddXY(practiceX[i].ToString("d"), practiceY[i]);
                series2.ChartType = SeriesChartType.FastLine;
            }

            GraphChart.Series.Clear();
            GraphChart.Series.Add(series2);


            GraphChart.ResetAutoValues();
            GraphChart.Titles.Clear();

            GraphChart.Titles.Add("Ваш прогресс");
            GraphChart.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.LightGray;
            GraphChart.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.LightGray;
        }

        public void ClearGraph() 
        {
            GraphChart.Series.Clear();

            GraphChart.ResetAutoValues();
            GraphChart.Titles.Clear();

            GraphChart.Titles.Add("Ваш прогресс");
            GraphChart.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.LightGray;
            GraphChart.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.LightGray;
        }

        public void SaveImage() 
        {
            try
            {
                string name = $"d:\\ProgressGraph_{DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff")}.jpg";
                this.GraphChart.SaveImage(@name, ChartImageFormat.Jpeg);
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
        }
    }

}
