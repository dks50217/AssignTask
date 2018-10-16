using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AssignTask
{
    /// <summary>
    /// EmployeeDetail.xaml 的互動邏輯
    /// </summary>
    public partial class EmployeeDetail : Window
    {
        public EmployeeDetail()
        {
            InitializeComponent();
            
        }

        private string m_usEmployeeName;

        public string usEmployeeName
        {
            get
            {
                return m_usEmployeeName;
            }
            set
            {
                m_usEmployeeName = value; //Other sender Name (usEmployeeName)
                this.FillDetailData(m_usEmployeeName);               
            }
        }

        ProjectManagementEntities1 DbContext = new ProjectManagementEntities1();
        private void FillDetailData(string m_usEmployeeName)
        {
            var q = from p in this.DbContext.Tasks
                    where p.Employee.EmployeeName == m_usEmployeeName
                    select p;
            foreach (var item in q.ToList())
            {
                this.grid1.Items.Add(new MyItem { Task = item.TaskName, Work = item.Works.WorkName, TaskStatus = item.Works.Status.StatusName, EstWorkTime = item.WorkTime });
            }
            FillBarData(m_usEmployeeName, q);
        }
        private void FillBarData(string m_usEmployeeName, IQueryable<Tasks> q)
        {
            //1.假設某人接了A案子TaskA預計WorkTime為16小時，B案子TaskB預計WorkTime為8小時
            //2.假設 Task A & B都要一周內完成
            //3.以一周工時來計算40小時
            this.workloadBar1.Maximum = 40;
            try
            {            
                foreach(var item in q.ToList())
                {                 
                    this.workloadBar1.Value = int.Parse(item.WorkTime);
                }
                double p = this.workloadBar1.Value / 100;

                if (p >= this.workloadBar1.Maximum-10)
                {
                    this.workloadBar1.Foreground = new SolidColorBrush(Colors.Red);
                }
                decimal myValue = (decimal)p;
                NumberFormatInfo percentageFormat = new NumberFormatInfo { PercentPositivePattern = 1, PercentNegativePattern = 1 };
                this.barValueLabel.Content = myValue.ToString("P0", percentageFormat);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
