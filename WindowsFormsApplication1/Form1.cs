using SynapticEffect.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.calendarView1.CalendarViewMode = CalendarViewModel.WorkWeek;
            this.calendarView1.TaskMouseClick += calendarView1_TaskMouseClick;
        }

        void calendarView1_TaskMouseClick(object sender, TaskEventNode e)
        {
            TaskEventNode node = e;

        }

        int month = 7;
        private void Form1_Load(object sender, EventArgs e)
        {
            month = DateTime.Now.Month;
            this.calendarView1.LoadCalendar();



            TaskEventNode taskEvent01 = new TaskEventNode("日历控件跨区域显示测试1", "完成日历控件", "公司", new DateTime(2015, 7, 30, 8, 30, 0), new DateTime(2015, 7, 30, 18, 0, 0));
            this.calendarView1.AddTask(taskEvent01);


            TaskEventNode taskEvent02 = new TaskEventNode("日历控件跨区域显示测试2", "完成日历控件", "公司", new DateTime(2015, 7, 30, 8, 30, 0), new DateTime(2015, 7, 30, 18, 0, 0));
            this.calendarView1.AddTask(taskEvent02);

            TaskEventNode taskEvent0 = new TaskEventNode("日历控件跨区域显示测试3", "完成日历控件", "公司", new DateTime(2015, 7, 29, 8, 30, 0), new DateTime(2015, 8, 2, 18, 0, 0));
            this.calendarView1.AddTask(taskEvent0);


            //TaskEventNode taskEvent1=new TaskEventNode("完成日历控件","完成日历控件","公司",new DateTime(2015,7,27,8,30,0),new DateTime(2015,7,27,10,0,0));
            //this.calendarView1.AddTask(taskEvent1);

            TaskEventNode taskEvent2 = new TaskEventNode("创建日程管理1", "完成日历控件", "公司", new DateTime(2015, 7, 29, 11, 30, 0), new DateTime(2015, 7, 30, 12, 30, 0));
            this.calendarView1.AddTask(taskEvent2);


            TaskEventNode taskEvent3 = new TaskEventNode("创建日程管理2", "完成日历控件", "公司", new DateTime(2015, 7, 30, 10, 30, 0), new DateTime(2015, 8, 1, 14, 0, 0));
            this.calendarView1.AddTask(taskEvent3);


            TaskEventNode taskEvent6 = new TaskEventNode("创建日程管理3", "完成日历控件", "公司", new DateTime(2015, 7, 29, 14, 30, 0), new DateTime(2015, 7, 29, 15, 0, 0));
            this.calendarView1.AddTask(taskEvent6);

            TaskEventNode taskEvent7 = new TaskEventNode("创建日程管理4", "完成日历控件", "公司", new DateTime(2015, 7, 28, 16, 30, 0), new DateTime(2015, 7, 28, 17, 0, 0));
            this.calendarView1.AddTask(taskEvent7);

            //TaskEventNode taskEvent8 = new TaskEventNode("创建日程管理4", "完成日历控件", "公司", new DateTime(2015, 7, 28, 16, 30, 0), new DateTime(2015, 7, 28, 17, 0, 0));
            //this.calendarView1.AddTask(taskEvent8);

            //TaskEventNode taskEvent9 = new TaskEventNode("创建日程管理4", "完成日历控件", "公司", new DateTime(2015, 7, 28, 16, 30, 0), new DateTime(2015, 7, 28, 17, 0, 0));
            //this.calendarView1.AddTask(taskEvent9);

            ////TaskEventNode taskEvent7 = new TaskEventNode("创建日程管理4", "完成日历控件", "公司", new DateTime(2015, 7, 28, 16, 30, 0), new DateTime(2015, 7, 28, 17, 0, 0));
            ////this.calendarView1.AddTask(taskEvent7);



            //TaskEventNode taskEvent4 = new TaskEventNode("日历控件跨区域显示测试", "完成日历控件", "公司", new DateTime(2015, 8, 2, 8, 30, 0), new DateTime(2015, 8, 4, 18, 0, 0));
            //this.calendarView1.AddTask(taskEvent4);


            //TaskEventNode taskEvent5 = new TaskEventNode("日历控件跨区域显示测试", "完成日历控件", "公司", new DateTime(2015, 7, 25, 8, 30, 0), new DateTime(2015, 7, 30, 18, 0, 0));
            //this.calendarView1.AddTask(taskEvent5);

            this.toolStripLabel1.Text = month.ToString();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            month--;
            this.calendarView1.PreView();
            this.toolStripLabel1.Text = month.ToString();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            month++;
            this.calendarView1.NextView();
            this.toolStripLabel1.Text = month.ToString();

        }
    }
}
