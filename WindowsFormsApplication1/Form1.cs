﻿using JsmCalendar;
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
        JsmCalendar.CalendarView calendarView1 = null;
        public Form1()
        {
            InitializeComponent();
            this.calendarView1 = new CalendarView();
            this.calendarView1.CalendarViewMode = CalendarViewModel.TimeSpan;
            this.calendarView1.TaskMouseClick += calendarView1_TaskMouseClick;
            panel1.Controls.Add(this.calendarView1);
            this.calendarView1.Dock = DockStyle.Fill;
        }

        void calendarView1_TaskMouseClick(object sender, TaskEventNode e)
        {
            TaskEventNode node = e;

        }

        int month = 7;
        private void Form1_Load(object sender, EventArgs e)
        { 
            month = DateTime.Now.Month;

            TaskEventNode taskEvent2 = new TaskEventNode("创建日程管理1", "完成日历控件", "公司","tl", new DateTime(2015, 8, 30, 0, 30, 0), new DateTime(2015, 8, 30, 12, 30, 0));
            this.calendarView1.AddTask(taskEvent2);


            TaskEventNode taskEvent01 = new TaskEventNode("日历控件跨区域显示测试1", "完成日历控件", "公司", "tl", new DateTime(2015, 7, 30, 8, 30, 0), new DateTime(2015, 7, 30, 18, 0, 0));
            this.calendarView1.AddTask(taskEvent01);


            TaskEventNode taskEvent02 = new TaskEventNode("日历控件跨区域显示测试2", "完成日历控件", "公司", "tl", new DateTime(2015, 7, 30, 8, 30, 0), new DateTime(2015, 7, 30, 18, 0, 0));
            this.calendarView1.AddTask(taskEvent02);

            TaskEventNode taskEvent0 = new TaskEventNode("日历控件跨区域显示测试3", "完成日历控件", "公司", "tl", new DateTime(2015, 7, 29, 8, 30, 0), new DateTime(2015, 8, 2, 18, 0, 0));
            this.calendarView1.AddTask(taskEvent0);


            TaskEventNode taskEvent1 = new TaskEventNode("完成日历控件", "完成日历控件", "公司", "tl", new DateTime(2015, 8, 27, 8, 30, 0), new DateTime(2015, 8, 27, 10, 0, 0));
            this.calendarView1.AddTask(taskEvent1);


            TaskEventNode taskEvent3 = new TaskEventNode("创建日程管理2", "完成日历控件", "公司", "tl", new DateTime(2015, 7, 30, 10, 30, 0), new DateTime(2015, 8, 1, 14, 0, 0));
            this.calendarView1.AddTask(taskEvent3);


            TaskEventNode taskEvent6 = new TaskEventNode("创建日程管理3", "完成日历控件", "公司", "tl", new DateTime(2015, 8, 19, 14, 30, 0), new DateTime(2015, 8, 23, 15, 0, 0));
            this.calendarView1.AddTask(taskEvent6);

            TaskEventNode taskEvent7 = new TaskEventNode("创建日程管理4", "完成日历控件", "公司", "tl", new DateTime(2015, 8, 26, 00, 30, 0), new DateTime(2015, 8, 27, 23, 30, 0));
            this.calendarView1.AddTask(taskEvent7);

            TaskEventNode taskEvent8 = new TaskEventNode("创建日程管理4", "完成日历控件", "公司", "tl", new DateTime(2015, 8, 26, 10, 30, 0), new DateTime(2015, 8, 30, 13, 30, 0));
            this.calendarView1.AddTask(taskEvent8);


            TaskEventNode taskEvent10 = new TaskEventNode("创建日程管理4", "完成日历控件", "公司", "tl", new DateTime(2015, 9, 1, 9, 30, 0), new DateTime(2015, 9, 1, 13, 30, 0));
            this.calendarView1.AddTask(taskEvent10);

            TaskEventNode taskEvent11 = new TaskEventNode("创建日程管理4", "完成日历控件", "公司", "tl", new DateTime(2015, 9, 1, 10, 30, 0), new DateTime(2015, 9, 1, 13, 30, 0));
            this.calendarView1.AddTask(taskEvent11);

            TaskEventNode taskEvent12 = new TaskEventNode("创建日程管理4", "完成日历控件", "公司", "tl", new DateTime(2015, 9, 1, 8, 30, 0), new DateTime(2015, 9, 1, 13, 30, 0));
            this.calendarView1.AddTask(taskEvent12);


            TaskEventNode taskEvent13 = new TaskEventNode("创建日程管理4", "完成日历控件", "公司", "tl", new DateTime(2015, 9, 2, 9, 30, 0), new DateTime(2015, 9, 2, 13, 30, 0));
            this.calendarView1.AddTask(taskEvent13);

            TaskEventNode taskEvent14 = new TaskEventNode("创建日程管理4", "完成日历控件", "公司", "tl", new DateTime(2015, 9, 3, 10, 30, 0), new DateTime(2015, 9, 3, 13, 30, 0));
            this.calendarView1.AddTask(taskEvent14);

            TaskEventNode taskEvent15 = new TaskEventNode("创建日程管理4", "完成日历控件", "公司", "tl", new DateTime(2015, 9, 2, 8, 30, 0), new DateTime(2015, 9, 2, 13, 30, 0));
            this.calendarView1.AddTask(taskEvent15);
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

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            month--;
            this.calendarView1.PreView();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            month++;
            this.calendarView1.NextView();

        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            this.calendarView1.CalendarViewMode = CalendarViewModel.Day;
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            this.calendarView1.CalendarViewMode = CalendarViewModel.Week;
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            this.calendarView1.CalendarViewMode = CalendarViewModel.WorkWeek;
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            this.calendarView1.CalendarViewMode = CalendarViewModel.Month;
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            this.calendarView1.CalendarViewMode = CalendarViewModel.Year;
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            this.calendarView1.CalendarViewMode = CalendarViewModel.TimeSpan;
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            this.calendarView1.CalendarViewMode = CalendarViewModel.MonthWeek;

        }

        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            this.calendarView1.GotoCalendarDate(DateTime.Now);
            //this.calendarView1.TaskEventNodes[0].Visible = false;
            this.calendarView1.Invalidate();
        }
    }
}