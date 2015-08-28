// CalendarView class
// Author: tang lei
// Email: tanglei331@hotmail.com
//  
////////////////////////////////////////////////////////

using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace JsmCalendar
{
	#region Enumerations
	#endregion

	#region Event Stuff
	#endregion

    #region ChineseDate
    public class ChineseDate
    {

        private string cyear = "";

        public string Cyear
        {
            get { return cyear; }
            set { cyear = value; }
        }
        private string cmonth = "";

        public string Cmonth
        {
            get { return cmonth; }
            set { cmonth = value; }
        }
        private string cday = "";

        public string Cday
        {
            get { return cday; }
            set { cday = value; }
        }
        private string cdate = "";

        public string Cdate
        {
            get { return cdate; }
            set { cdate = value; }
        }

        public ChineseDate(DateTime datetime)
        {
            int year = ChineseCalendar.GetYear(datetime);
            int month = ChineseCalendar.GetMonth(datetime);
            int day = ChineseCalendar.GetDayOfMonth(datetime);
            //获取闰月， 0 则表示没有闰月
            int leapMonth = ChineseCalendar.GetLeapMonth(year);

            bool isleap = false;

            if (leapMonth > 0)
            {
                if (leapMonth == month)
                {
                    //闰月
                    isleap = true;
                    month--;
                }
                else if (month > leapMonth)
                {
                    month--;
                }
            }
            cyear = GetLunisolarYear(year);
            cmonth = GetLunisolarMonth(month);
            cday = GetLunisolarDay(day);

            cdate= string.Concat("农历" + GetLunisolarYear(year), "年", isleap ? "闰" : string.Empty, GetLunisolarMonth(month), "月", GetLunisolarDay(day));
       
        }


        private static ChineseLunisolarCalendar ChineseCalendar = new ChineseLunisolarCalendar();

        ///<summary>
        /// 十天干
        ///</summary>
        private static string[] tg = { "甲", "乙", "丙", "丁", "戊", "己", "庚", "辛", "壬", "癸" };

        ///<summary>
        /// 十二地支
        ///</summary>
        private static string[] dz = { "子", "丑", "寅", "卯", "辰", "巳", "午", "未", "申", "酉", "戌", "亥" };

        ///<summary>
        /// 十二生肖
        ///</summary>
        private static string[] sx = { "鼠", "牛", "虎", "免", "龙", "蛇", "马", "羊", "猴", "鸡", "狗", "猪" };

        ///<summary>
        /// 返回农历天干地支年
        ///</summary>
        ///<param name="year">农历年</param>
        ///<return s></return s>
        public static string GetLunisolarYear(int year)
        {
            if (year > 3)
            {
                int tgIndex = (year - 4) % 10;
                int dzIndex = (year - 4) % 12;

                return string.Concat(tg[tgIndex], dz[dzIndex], "(", sx[dzIndex], ")");
            }

            throw new ArgumentOutOfRangeException("无效的年份!");
        }

        ///<summary>
        /// 农历月
        ///</summary>

        ///<return s></return s>
        private static string[] months = { "正", "二", "三", "四", "五", "六", "七", "八", "九", "十", "十一", "十二(腊)" };

        ///<summary>
        /// 农历日
        ///</summary>
        private static string[] days1 = { "初", "十", "廿", "三" };
        ///<summary>
        /// 农历日
        ///</summary>
        private static string[] days = { "一", "二", "三", "四", "五", "六", "七", "八", "九", "十" };


        ///<summary>
        /// 返回农历月
        ///</summary>
        ///<param name="month">月份</param>
        ///<return s></return s>
        public static string GetLunisolarMonth(int month)
        {
            if (month < 13 && month > 0)
            {
                return months[month - 1];
            }

            throw new ArgumentOutOfRangeException("无效的月份!");
        }

        ///<summary>
        /// 返回农历日
        ///</summary>
        ///<param name="day">天</param>
        ///<return s></return s>
        public static string GetLunisolarDay(int day)
        {
            if (day > 0 && day < 32)
            {
                if (day != 20 && day != 30)
                {
                    return string.Concat(days1[(day - 1) / 10], days[(day - 1) % 10]);
                }
                else
                {
                    return string.Concat(days[(day - 1) / 10], days1[1]);
                }
            }

            throw new ArgumentOutOfRangeException("无效的日!");
        }



        ///<summary>
        /// 根据公历获取农历日期
        ///</summary>
        ///<param name="datetime">公历日期</param>
        ///<return s></return s>
        public static string GetChineseDateTime(DateTime datetime)
        {
            int year = ChineseCalendar.GetYear(datetime);
            int month = ChineseCalendar.GetMonth(datetime);
            int day = ChineseCalendar.GetDayOfMonth(datetime);
            //获取闰月， 0 则表示没有闰月
            int leapMonth = ChineseCalendar.GetLeapMonth(year);

            bool isleap = false;

            if (leapMonth > 0)
            {
                if (leapMonth == month)
                {
                    //闰月
                    isleap = true;
                    month--;
                }
                else if (month > leapMonth)
                {
                    month--;
                }
            }

            return string.Concat("农历" + GetLunisolarYear(year), "年", isleap ? "闰" : string.Empty, GetLunisolarMonth(month), "月", GetLunisolarDay(day));
        }
    }
    #endregion
     
    #region TaskEvent
     
    [Serializable]
    public class TaskEventNode :IComparable
    {
        public object tag=null;

        private string title = "";

        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        private string content = "";

        public string Content
        {
            get { return content; }
            set { content = value; }
        }
        private string location = "";

        public string Location
        {
            get { return location; }
            set { location = value; }
        }
        private DateTime startTime = DateTime.Now;

        public DateTime StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }
        private DateTime endTime = DateTime.Now;

        public DateTime EndTime
        {
            get { return endTime; }
            set { endTime = value; }
        }

        private bool selected = false;

        public bool Selected
        {
            get { return selected; }
            set { selected = value; }
        }

        private List<TreeListNode> relateNodes;

        public List<TreeListNode> RelateNodes
        {
            get { return relateNodes; }
            set { relateNodes = value; }
        }
        private Color forecolor = SystemColors.WindowText;

        public Color ForeColor
        {
            get { return forecolor; }
            set { forecolor = value; }
        }
        private Color backColor = SystemColors.Control;

        public Color BackColor
        {
            get { return backColor; }
            set { backColor = value; }
        }
        private List<Rectangle> rectAreas;

        public List<Rectangle> RectAreas
        {
            get { return rectAreas; }
            set { rectAreas = value; }
        }

        private List<Rectangle> titleAreas;

        public List<Rectangle> TitleAreas
        {
            get { return titleAreas; }
            set { titleAreas = value; }
        }
        private Rectangle selectedTitleArea;

        public Rectangle SelectedTitleArea
        {
            get { return selectedTitleArea; }
            set { selectedTitleArea = value; }
        }

        private int areaIndex = 0;

        public int AreaIndex
        {
            get { return areaIndex; }
            set { areaIndex = value; }
        }
        private int colSubIndex = 0;

        public int ColSubIndex
        {
            get { return colSubIndex; }
            set { colSubIndex = value; }
        }
        private int colSubCount = 0;

        public int ColSubCount
        {
            get { return colSubCount; }
            set { colSubCount = value; }
        }
         
        public TaskEventNode(string title,string content,string location,DateTime dtStart,DateTime dtEnd)
        {
            this.title = title;
            this.content = content;
            this.location = location;
            this.startTime = dtStart;
            this.endTime = dtEnd;
            relateNodes = new List<TreeListNode>();
            rectAreas = new List<Rectangle>();
            titleAreas = new List<Rectangle>();
        }

        public int CompareTo(object obj)
        {
            int r = 0;
            TaskEventNode taskEvent = obj as TaskEventNode;
            if (this.startTime > taskEvent.startTime)
            {
                r = 1;
            }
            else if (this.startTime < taskEvent.startTime)
            {
                r = -1;
            }
            return r;
        }

    }
    #endregion
    #region CalenderViewModel
    public enum CalendarViewModel { Day,WorkWeek,Week,MonthWeek,Month,Year,TimeSpan};
    #endregion
    #region TreeListNode
    [DesignTimeVisible(false), TypeConverter("JsmCalendar.TreeListNodeConverter")]
	public class TreeListNode: IParentChildList
	{
		#region Event Handlers
		public event MouseEventHandler MouseDown;

		private void OnSubItemsChanged(object sender, ItemsChangedEventArgs e)
		{
			subitems[e.IndexChanged].MouseDown += new MouseEventHandler(OnSubItemMouseDown);
		}

		private void OnSubItemMouseDown(object sender, MouseEventArgs e)
		{
			if (MouseDown != null)
				MouseDown(this, e);
		}

		private void OnSubNodeMouseDown(object sender, MouseEventArgs e)
		{
			if (MouseDown != null)
				MouseDown(sender, e);
		}
		#endregion

		#region Variables
		private Color backcolor = SystemColors.Control;
		private Rectangle bounds;
		private bool ischecked = false;
		private bool focused = false;
		private Font font, nodeFont;
		private Color forecolor = SystemColors.WindowText;
		private int imageindex = 0;
		private int stateimageindex = 0;
		private int index = 0;
        private CalendarView calendarview;
		private bool selected = false;
		private ContainerSubListViewItemCollection subitems;
		private object tag;
		private string text;
		private bool styleall = false;
		private bool hovered = false; 

		private TreeListNode curChild = null;
		private TreeListNodeCollection nodes;
		private string fullPath = "";
		private bool expanded = false;
		private bool visible = true;

		private TreeListNode parent;
        private  int selectedImageIndex;
       
        //设置选中后的图标索引;
        private int selectedImageInde;
        //private TreeListNode selectedNode = null;

        private bool loadedChildNode = false;

        private object info;
        private string dataName = "";

        private DateTime date = DateTime.Now;

        private int week = 0; 
        private int year = 0; 
        private int month = 0; 
        private int day = 0;
        private bool isToday = false;
        private bool isHoliady = false;

        private ChineseDate chinesDate;
        private List<TaskEventNode> relateTasks;
        private int row = 0; 
        private int col = 0;

        private bool isBlankNode = false;


       

        #endregion

		#region Constructor

        public TreeListNode(DateTime dt)
        {
            date = dt;
            week = Convert.ToInt32(dt.DayOfWeek.ToString("d"));
            year = dt.Year;
            month = dt.Month;
            day = dt.Day;
            if (year == DateTime.Now.Year && month == DateTime.Now.Month && day == DateTime.Now.Day)
            {
                isToday = true;
            }
            chinesDate = new ChineseDate(dt);
            backcolor = Color.Honeydew;
            nodes = new TreeListNodeCollection();
            relateTasks = new List<TaskEventNode>();
            nodes.Owner = this;
            nodes.MouseDown += new MouseEventHandler(OnSubNodeMouseDown);


            subitems = new ContainerSubListViewItemCollection();
            subitems.ItemsChanged += new ItemsChangedEventHandler(OnSubItemsChanged);
        }

        public TreeListNode()
        {
            subitems = new ContainerSubListViewItemCollection();
            subitems.ItemsChanged += new ItemsChangedEventHandler(OnSubItemsChanged);

            nodes = new TreeListNodeCollection();
            nodes.Owner = this;
            nodes.MouseDown += new MouseEventHandler(OnSubNodeMouseDown);
        }

        //public TreeListNode(string Text)
        //{
        //    subitems = new ContainerSubListViewItemCollection();
        //    subitems.ItemsChanged += new ItemsChangedEventHandler(OnSubItemsChanged);

        //    nodes = new TreeListNodeCollection();
        //    nodes.Owner = this;
        //    nodes.MouseDown += new MouseEventHandler(OnSubNodeMouseDown);
        //    this.Text = Text;
        //}

		#endregion

		#region Properties
		[
		Category("Behavior"),
		Description("Sets the parent node of this node.")
		]
		public TreeListNode Parent
		{
            get { return parent; }
			set { parent = value; }
		}

		[
		Category("Data"), 
		Description("The collection of root nodes in the treelist."),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		Editor(typeof(CollectionEditor), typeof(UITypeEditor))
		]
		public TreeListNodeCollection Nodes
		{
			get { return nodes; }
		}

		[Category("Behavior"), DefaultValue(false)]
		public bool IsExpanded
		{
			get { return expanded; }
			set { expanded = value; }
		}

		[Category("Behavior"), DefaultValue(true)]
		public bool IsVisible
		{
			get { return visible; }
			set { visible = value; }
		}

		[Category("Behavior")]
		public string FullPath
		{
            set { fullPath = value; }
			get { return fullPath; }
		}

		[Category("Appearance")]
		public Color BackColor
		{
			get { return backcolor;	}
			set { backcolor = value; }
		}

		[Browsable(false)]
		public Rectangle Bounds
		{
			get { return bounds; }
		}

		[Category("Behavior")]
		public bool Checked
		{
			get { return ischecked; }
			set { ischecked = value; }
		}

		[Browsable(false)]
		public bool Focused
		{
			get { return focused; }
			set { focused = value; }
		}

		[Category("Appearance")]
		public Font Font
		{
			get { return font; }
			set { font = value; }
		}

		[Category("Appearance")]
		public Color ForeColor
		{
			get { return forecolor; }
			set { forecolor = value; }
		}

		[Category("Behavior")]
		public int ImageIndex
		{
			get { return imageindex; }
			set { imageindex = value; }
		}

        [Category("Behavior")]
        public int SelectedImageIndex
        {
            get { return selectedImageIndex; }
            set { selectedImageIndex = value; }
        }

		[Browsable(false)]
		public int Index
		{
			get { return index; }
			set { index = value; }
		}

		public CalendarView CalendarView
		{
			get { return calendarview; }
		}

		[Browsable(false)]
		public bool Selected
		{
			get { return selected; }
			set 
            {
                selected = value;
                
            }
		}

		[
		Category("Behavior"),
		Description("The items collection of sub controls."),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		Editor(typeof(CollectionEditor), typeof(UITypeEditor))		 
		]
		public ContainerSubListViewItemCollection SubItems
		{
			get { return subitems; }
		}

		[Category("Behavior")]
		public int StateImageIndex
		{
			get { return stateimageindex; }
			set { stateimageindex = value; }
		}

		[Browsable(false)]
		public object Tag
		{
			get { return tag; }
			set { tag = value; }
		}

		[Category("Appearance")]
		public string Text
		{
			get { return text; }
			set { text = value; }
		}

		[Category("Behavior")]
		public bool UseItemStyleForSubItems
		{
			get { return styleall; }
			set { styleall = value; }
		}

		[Browsable(false)]
		public bool Hovered
		{
			get { return hovered; }
			set { hovered = value; }
		}

        [Category("Appearance")]
        public bool LoadedChildNode
        {
            get { return loadedChildNode; }
            set { loadedChildNode = value; }
        }

        [Browsable(false)]
        public object Info
        {
            get { return info; }
            set { info = value; }
        }

        [Browsable(false)]
        public string DataName
        {
            get { return dataName; }
            set { dataName = value; }
        }


        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        public int Week
        {
            get { return week; }
            set { week = value; }
        }
        public int Year
        {
            get { return year; }
            set { year = value; }
        }
        public int Month
        {
            get { return month; }
            set { month = value; }
        }
        public int Day
        {
            get { return day; }
            set { day = value; }
        }
        public bool IsToday
        {
            get { return isToday; }
            set { isToday = value; }
        }
         
        public bool IsHoliady
        {
            get { return isHoliady; }
            set { isHoliady = value; }
        }
        
        public List<TaskEventNode> RelateTasks
        {
            get { return relateTasks; }
            set { relateTasks = value; }
        }
        public ChineseDate ChinesDate
        {
            get { return chinesDate; }
            set { chinesDate = value; }
        } 
      
        public int Row
        {
            get { return row; }
            set { row = value; }
        }
        public int Col
        {
            get { return col; }
            set { col = value; }
        }
        public bool IsBlankNode
        {
            get { return isBlankNode; }
            set { isBlankNode = value; }
        }
        #endregion

		#region Methods
		public void Collapse()
		{
			expanded = false;
		}

		public void CollapseAll()
		{
			for(int i=0; i<nodes.Count; i++)
			{
				nodes[i].CollapseAll();
			}
			Collapse();
		}
		public void Expand()
		{
			expanded = true;
        }

		public void ExpandAll()
		{
			for (int i=0; i<nodes.Count; i++) 
				((TreeListNode)nodes[i]).ExpandAll();

			expanded = true;
		}

		public int GetNodeCount(bool includeSubTrees)
		{
			int c=0;

			if (includeSubTrees)
			{
				for (int n=0; n<nodes.Count; n++)
				{
					c += nodes[n].GetNodeCount(true);
				}
			}
			c += nodes.Count;
			return c;
		}

		public int GetVisibleNodeCount(bool includeSubTrees)
		{
			int c=0;

			if (expanded)
			{
				if (includeSubTrees)
				{
					for (int n=0; n<nodes.Count; n++)
					{
						if ((nodes[n].IsExpanded) && nodes[n].IsVisible)
							c += nodes[n].GetVisibleNodeCount(true);
					}
				}

				for (int n=0; n<nodes.Count; n++)
				{
					if (nodes[n].IsVisible)
						c++;
				}
			}		

			return c;
		}

		public void Remove()
		{
			int c = nodes.IndexOf(curChild);
			nodes.Remove(curChild);
			if (nodes.Count > 0 && nodes.Count > c)
				curChild = nodes[c];
			else
				curChild = nodes[nodes.Count];
		}

		public void Toggle()
		{
			if (expanded)
				expanded = false;
			else
				expanded = true;
		}
		#endregion

		#region IParentChildList
		public object ParentNode()
		{
			return parent;
		}

		public object PreviousSibling()
		{
			if (parent != null)
			{
				int thisIndex = parent.Nodes[this];
				if (thisIndex > 0)
					return parent.Nodes[thisIndex-1];
			}

			return null;
		}

		public object NextSibling()
		{
			if (parent != null)
			{
				int thisIndex = parent.Nodes[this];
				if (thisIndex < parent.Nodes.Count-1)
					return parent.Nodes[thisIndex+1];
			}

			return null;
		}

		public object FirstChild()
		{
			curChild = Nodes[0];
			return curChild;
		}

		public object LastChild()
		{
			curChild = Nodes[Nodes.Count-1];
			return curChild;
		}

		public object NextChild()
		{
			curChild = (TreeListNode)curChild.NextSibling();
			return curChild;
		}

		public object PreviousChild()
		{
			curChild = (TreeListNode)curChild.PreviousSibling();
			return curChild;
		}
		#endregion
	}

	public class TreeListNodeCollection: CollectionBase
	{
		#region Events
		public event MouseEventHandler MouseDown;
		public event EventHandler NodesChanged;

		private void OnMouseDown(object sender, MouseEventArgs e)
		{
			if (MouseDown != null)
				MouseDown(sender, e);
		}

		private void OnNodesChanged()
		{
			OnNodesChanged(this, new EventArgs());
		}

		private void OnNodesChanged(object sender, EventArgs e)
		{
			if (NodesChanged != null)
				NodesChanged(sender, e);
		}
		#endregion

		#region Variables
		private TreeListNode owner;
		#endregion

		public TreeListNodeCollection()
		{
		}

		public TreeListNodeCollection(TreeListNode owner)
		{
			this.owner = owner;
		}

		public TreeListNode Owner
		{
			get { return owner; }
			set { owner = value; }
		}

		public int TotalCount
		{
			get 
			{
				int tcnt = 0;
				tcnt += List.Count;
				foreach (TreeListNode n in List)
				{
					tcnt += n.Nodes.TotalCount;
				}

				return tcnt;
			}
		}

		#region Implementation
		public TreeListNode this[int index]
		{
			get 
			{ 
				if (List.Count > 0 && index>-1)
				{
					return List[index] as TreeListNode;
				}
				else
					return null;
			}
			set 
			{
				List[index] = value;
				TreeListNode tln = ((TreeListNode)List[index]);
				tln.MouseDown += new MouseEventHandler(OnMouseDown);
				tln.Nodes.NodesChanged += new EventHandler(OnNodesChanged);
				tln.Parent = owner;
				OnNodesChanged();
			}
		}

		public int this[TreeListNode item]
		{
			get { return List.IndexOf(item); }
		}

		public int Add(TreeListNode item)
		{
			item.MouseDown += new MouseEventHandler(OnMouseDown);
			item.Nodes.NodesChanged += new EventHandler(OnNodesChanged);
			item.Parent = owner;
            item.FullPath = GetFullPath(item);
		//	OnNodesChanged();
			return item.Index = List.Add(item);
		}

        public void Insert(int pos, TreeListNode item)
        {
            item.MouseDown += new MouseEventHandler(OnMouseDown);
            item.Nodes.NodesChanged += new EventHandler(OnNodesChanged);
            item.Parent = owner;
            item.FullPath = GetFullPath(item);
        //    OnNodesChanged();
            List.Insert(pos, item);
            //return item.Index = List.Add(item);

            //Add By T.L
            for (int i = pos; i < List.Count; i++)
            {
                TreeListNode node = (TreeListNode)List[i];
                node.Index = i;
            }
          

        }


        //向TreeListNodeCollection中加已有的Node
        public int Add(TreeListNode item, Boolean bz)
        {
            if (bz)
            {
                item.MouseDown += new MouseEventHandler(OnMouseDown);
                item.Nodes.NodesChanged += new EventHandler(OnNodesChanged);
                item.Parent = owner;
                item.FullPath = GetFullPath(item);
                OnNodesChanged();
                return item.Index = List.Add(item);
            }
            else
            {
                return item.Index = List.Add(item);
            }
        }

        private string GetFullPath(TreeListNode item)
        {
            if (item.Parent == null)
            {
                return item.Text;
            }
            else if (item.Parent.Tag == null)
            {
                return item.Text;
            }
            else
            {
                return item.Parent.FullPath + "\\" + item.Text;
            }
        }

		public void AddRange(TreeListNode[] items)
		{
			lock(List.SyncRoot)
			{
				for (int i=0; i<items.Length; i++)
				{
					items[i].MouseDown += new MouseEventHandler(OnMouseDown);
					items[i].Nodes.NodesChanged+= new EventHandler(OnNodesChanged);
					items[i].Parent = owner;
					items[i].Index = List.Add(items[i]);
				}
				OnNodesChanged();
			}
		}

		public void Remove(TreeListNode item)
		{
			List.Remove(item);

            //Add By T.L
            int pos = item.Index;

            for (int i = pos; i < List.Count; i++)
            {
                TreeListNode node = (TreeListNode)List[i];
                node.Index = i;
            }


		}

        public new void Clear()
		{
			for (int i=0; i<List.Count; i++)
			{
				ContainerSubListViewItemCollection col = ((TreeListNode)List[i]).SubItems;
                if(col!=null)
                {
                    for (int j = 0; j < col.Count; j++)
                    {
                        if (col[j].ItemControl != null)
                        {
                            col[j].ItemControl.Parent = null;
                            col[j].ItemControl.Visible = false;
                            col[j].ItemControl = null;
                        }
                    } 
                }
				
				((TreeListNode)List[i]).Nodes.Clear();
			}
			List.Clear();
			OnNodesChanged();
		}

		public int IndexOf(TreeListNode item)
		{
			return List.IndexOf(item);
		}
		#endregion
	}
	#endregion

	#region CalendarView
	/// <summary>
	/// CalendarView provides a hybrid listview whos first
	/// column can behave as a treeview. This control extends
	/// ContainerListView, allowing subitems to contain 
	/// controls.
	/// </summary>
	public class CalendarView : JsmCalendar.ContainerListView
	{
		#region Events
		protected override void OnSubControlMouseDown(object sender, MouseEventArgs e)
		{
			TreeListNode node = (TreeListNode)sender;
			
			UnselectNodes(nodes);
			
			node.Focused = true;
			node.Selected = true;				
			//focusedIndex = firstSelected = i;

			if (e.Clicks >= 2)
				node.Toggle();

			// set selected items and indices collections							
			//selectedIndices.Add(i);						
			//selectedItems.Add(items[i]);
			Invalidate(ClientRectangle);
		}

		protected virtual void OnNodesChanged(object sender, EventArgs e)
		{ 
		}

        public delegate void TreeListViewEventHandler(object sender, TreeListNode e);
        public delegate void CalendarTaskEventHandler(object sender, TaskEventNode e);

        /// <summary>
        /// Occurs after the tree node is expanded
        /// </summary>
        [Description("Occurs after the tree node is expanded")]
        public event TreeListViewEventHandler AfterExpand;
        /// <summary>
        /// Occurs after the tree node is collapsed
        /// </summary>
        [Description("Occurs after the tree node is collapsed")]
        public event TreeListViewEventHandler AfterCollapse;

        /// <summary>
        /// Occurs after the tree node is all expanded
        /// </summary>
        [Description("Occurs after the tree node is all expanded")]
        public event TreeListViewEventHandler AfterExpandAll;
        /// <summary>
        /// Occurs after the tree node is all collapsed
        /// </summary>
        [Description("Occurs after the tree node is all collapsed")]
        public event TreeListViewEventHandler AfterCollapseAll;



        [Description("Occurs after the tree node is click")]
        public event TreeListViewEventHandler NodeMouseClick;

        [Description("Occurs after the taskEvent is click")]
        public event CalendarTaskEventHandler TaskMouseClick;
         
        [Description("Occurs after the taskEvent is  double click")]
        public event CalendarTaskEventHandler TaskDoubleClick;
		#endregion

		#region Variables
		protected TreeListNodeCollection nodes;
		protected int indent = 19;
		protected int itemheight = 20;
        protected int itemwidth = 20;
        protected int taskHeight = 20;
        protected int lfWidth = 60;
        protected int ltHeight = 0;
        protected int colCount = 7;
		protected bool showlines = false, showrootlines = false, showplusminus = true;

		protected ListDictionary pmRects;
        protected ListDictionary nodeRowRects;
          
		protected bool alwaysShowPM = false;

		protected Bitmap bmpMinus, bmpPlus;

		private TreeListNode curNode;
		private TreeListNode virtualParent;

		private TreeListNodeCollection selectedNodes;
        private List<TaskEventNode> taskEventNodes;
         

		private bool mouseActivate = false;

		private bool allCollapsed = false;

        private bool gridStyle = false;

        //设置选中的节点;
        private TreeListNode selectedNode = null;
        private TaskEventNode selectedTask = null;

        private Color tempBackColor;
        private Color tempForeColor;


        private int selectionStartLine = 0;
        private int selectionEndLine = 0;

        private DateTime currentTime=DateTime.Now;
        private CalendarViewModel calendarViewMode= CalendarViewModel.Month;

        private DateTime minTime = DateTime.Now; 
        private DateTime maxTime = DateTime.Now; 

        TextBox txtNode = null;
        System.Windows.Forms.Timer timerTxt;

        private bool progressTimeMode = false;
        private int progressClickX = 0;
		#endregion

		#region Constructor
        public CalendarView() : base()
        {
            virtualParent = new TreeListNode();

            nodes = virtualParent.Nodes;
            nodes.Owner = virtualParent;
            nodes.MouseDown += new MouseEventHandler(OnSubControlMouseDown);
            nodes.NodesChanged += new EventHandler(OnNodesChanged);

            taskEventNodes = new List<TaskEventNode>();
            selectedNodes = new TreeListNodeCollection();

            nodeRowRects = new ListDictionary();
            pmRects = new ListDictionary();
             
            txtNode = new TextBox();
            txtNode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtNode.MouseDown += txtNode_MouseDown;
            txtNode.MouseUp += txtNode_MouseUp;
            txtNode.MouseMove += txtNode_MouseMove;
            txtNode.KeyDown += txtNode_KeyDown;
            txtNode.Leave += txtNode_Leave;

            timerTxt = new Timer();
            timerTxt.Interval = 1000;
            timerTxt.Enabled = false;
            timerTxt.Tick += timerTxt_Tick;

            currentTime = DateTime.Now;
            // Use reflection to load the
            // embedded bitmaps for the
            // styles plus and minus icons
            Assembly myAssembly = Assembly.GetAssembly(Type.GetType("JsmCalendar.CalendarView"));
            ////string filename = Application.StartupPath + @"\Image\tv_minus.bmp";
            ////bmpMinus = new Bitmap(filename);  yixun
            ////bmpMinus = new Bitmap(Application.StartupPath + @"\Image\tv_plus.bmp");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CalendarView));
           //// bmpMinus = ((System.Drawing.Bitmap)(resources.GetObject("tv_minus.bmp")));
        }


        #endregion

		#region Properties
		[
		Browsable(false)
		]
		public TreeListNodeCollection SelectedNodes
		{
			get { return GetSelectedNodes(virtualParent); }
		}

		[
		Category("Behavior"),
		Description("Determins wether an item is activated or expanded by a double click."),
		DefaultValue(false)
		]
		public bool MouseActivte
		{
			get { return mouseActivate; }
			set { mouseActivate = value; }
		}

		[
		Category("Behavior"),
		Description("Specifies wether to always show plus/minus signs next to each node."),
		DefaultValue(false)
		]
		public bool AlwaysShowPlusMinus
		{
			get { return alwaysShowPM; }
			set { alwaysShowPM = value; }
		}

		[
		Category("Data"), 
		Description("The collection of root nodes in the treelist."),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		Editor(typeof(CollectionEditor), typeof(UITypeEditor))
		]
		public TreeListNodeCollection Nodes
		{
			get { return nodes; }
		}

		[Browsable(false)]
		public override ContainerListViewItemCollection Items
		{
			get { return items; }
		}

		[
		Category("Behavior"),
		Description("The indentation of child nodes in pixels."),
		DefaultValue(19)
		]
		public int Indent
		{
			get { return indent; }
			set { indent = value; }
		}

        //[
        //Category("Appearance"),
        //Description("The height of every item in the treelistview."),
        //DefaultValue(18)
        //]
        //public int ItemHeight
        //{
        //    get { return itemheight; }
        //    set { itemheight = value; }
        //}

		[
		Category("Behavior"),
		Description("Indicates wether lines are shown between sibling nodes and between parent and child nodes."),
		DefaultValue(false)
		]
		public bool ShowLines
		{
			get { return showlines; }
			set { showlines = value; }
		}

		[
		Category("Behavior"),
		Description("Indicates wether lines are shown between root nodes."),
		DefaultValue(false)
		]
		public bool ShowRootLines
		{
			get { return showrootlines; }
			set { showrootlines = value; }
		}

		[
		Category("Behavior"),
		Description("Indicates wether plus/minus signs are shown next to parent nodes."),
		DefaultValue(true)
		]
		public bool ShowPlusMinus
		{
			get { return showplusminus; }
			set { showplusminus = value; }
		}

        [Browsable(false)]
        public TreeListNode SelectedNode
        {
            get { return selectedNode; }
            set 
            {
                if (selectedNode == value) return;

                if (selectedNode != null)
                {
                    selectedNode.Focused = false;
                    selectedNode.Selected = false;
                }
                selectedNode = value; 
                OnSelectedIndexChanged(new EventArgs());
                Invalidate();
            }
        }


        [Browsable(false)]
        public TaskEventNode SelectedTask
        {
            get { return selectedTask; }
            set 
            {
                if (selectedTask == value) return;
                if (selectedTask!=null)
                {
                    selectedTask.Selected = false; 
                }
                selectedTask = value;
                selectedTask.Selected = true;
                Invalidate();

            }
        }

        public bool GridStyle
        {
            get
            {
                return gridStyle;
            }
            set
            {
                gridStyle = value;
            }
        }

        public int SelectionStartLine
        {
            get { return selectionStartLine; }
            set { selectionStartLine = value; }
        }

        public int SelectionEndLine
        {
            get { return selectionEndLine; }
            set { selectionEndLine = value; }
        }

        public Color TempBackColor
        {
            get { return tempBackColor; }
            set { tempBackColor = value; }
        }

        public Color TempForeColor
        {
            get { return tempForeColor; }
            set { tempForeColor = value; }
        }
        [
        Category("Behavior"),
        Description("define the calendar view .")
        ]
        public CalendarViewModel CalendarViewMode
        {
            get { return calendarViewMode; }
            set
            {
                calendarViewMode = value;
                LoadCalendarView(); 
            }
        }

        public DateTime MinTime
        {
            get { return minTime; }
            set { minTime = value; }
        }
        public DateTime MaxTime
        {
            get { return maxTime; }
            set { maxTime = value; }
        }
        public List<TaskEventNode> TaskEventNodes
        {
            get { return taskEventNodes; } 
        }

		#endregion

		#region Overrides
		public override bool PreProcessMessage(ref Message msg)
		{
			if (msg.Msg == WM_KEYDOWN)
			{
				if (nodes.Count > 0)
				{
					if (curNode != null)
					{
						Keys keyData = ((Keys) (int) msg.WParam) | ModifierKeys;
						Keys keyCode = ((Keys) (int) msg.WParam);

						if (keyCode == Keys.Left)	// collapse current node or move up to parent
						{
							if (curNode.IsExpanded)
							{
								curNode.Collapse();
							}
							else if (curNode.ParentNode() != null)
							{
								TreeListNode t = (TreeListNode)curNode.ParentNode();
								if (t.ParentNode() != null) // never select virtualParent node
								{
									if (!multiSelect)
										curNode.Selected = false;
									//else
									curNode.Focused = false;
									curNode = (TreeListNode)curNode.ParentNode();
                                    if (!multiSelect)
                                    {
                                        curNode.Selected = true;
                                        this.SelectedNode = curNode; 
                                    }
									//else
									curNode.Focused = true;
                                    this.SelectedNode = curNode;
								}
							}

							Invalidate();
							return true;
						}
						else if (keyCode == Keys.Right) // expand current node or move down to first child
						{
							if (!curNode.IsExpanded)
							{
								curNode.Expand();
							}
							else if (curNode.IsExpanded && curNode.GetNodeCount(false) > 0)
							{
								if (!multiSelect)
									curNode.Selected = false;
								//else
								curNode.Focused = false;
								curNode = (TreeListNode)curNode.FirstChild();
								if (!multiSelect)
									curNode.Selected = true;
								//else
								curNode.Focused = true;
                                this.SelectedNode = curNode;
							}

							Invalidate();
							return true;
						}

						else if (keyCode == Keys.Up)
						{
							if (curNode.PreviousSibling() == null && curNode.ParentNode() != null)
							{
								TreeListNode t = (TreeListNode)curNode.ParentNode();
								if (t.ParentNode() != null) // never select virtualParent node
								{
									if (!multiSelect)
										curNode.Selected = false;
									//else
									curNode.Focused = false;
									curNode = (TreeListNode)curNode.ParentNode();
									if (!multiSelect)
										curNode.Selected = true;
									//else
									curNode.Focused = true;
                                    this.SelectedNode = curNode;
								}

								Invalidate();
								return true;
							}
							else if (curNode.PreviousSibling() != null)
							{
								TreeListNode t = (TreeListNode)curNode.PreviousSibling();
								if (t.GetNodeCount(false) > 0 && t.IsExpanded)
								{
									do
									{
										t = (TreeListNode)t.LastChild();
										if (!t.IsExpanded)
										{
											if (!multiSelect)
												curNode.Selected = false;
											//else
											curNode.Focused = false;
											curNode = t;
											if (!multiSelect)
												curNode.Selected = true;
											//else
											curNode.Focused = true;
                                            this.SelectedNode = curNode;
										}
									} while (t.GetNodeCount(false) > 0 && t.IsExpanded);
								}
								else
								{
									if (!multiSelect)
										curNode.Selected = false;
									//else
									curNode.Focused = false;
									curNode = (TreeListNode)curNode.PreviousSibling();
									if (!multiSelect)
										curNode.Selected = true;
									//else
									curNode.Focused = true;
                                    this.SelectedNode = curNode;
								}

								Invalidate();
								return true;
							}
						}
						else if (keyCode == Keys.Down)
						{
							if (curNode.IsExpanded && curNode.GetNodeCount(false) > 0)
							{
								if (!multiSelect)
									curNode.Selected = false;
								//else
								curNode.Focused = false;
								curNode = (TreeListNode)curNode.FirstChild();
                                if (!multiSelect)
                                {
                                    curNode.Selected = true;
                                    this.SelectedNode = curNode;
                                }
								//else
								curNode.Focused = true;
							}
							else if (curNode.NextSibling() == null && curNode.ParentNode() != null)
							{
								TreeListNode t = curNode;
								do
								{
									t = (TreeListNode)t.ParentNode();
									if (t.NextSibling() != null)
									{
										if (!multiSelect)
											curNode.Selected = false;
										//else
										curNode.Focused = false;
										curNode = (TreeListNode)t.NextSibling();
										if (!multiSelect)
											curNode.Selected = true;
										//else
										curNode.Focused = true;
                                        this.SelectedNode = curNode;
										break;
									}	
								} while (t.NextSibling() == null && t.ParentNode() != null);
							}						
							else if (curNode.NextSibling() != null)
							{
								if (!multiSelect)
									curNode.Selected = false;
								//else
								curNode.Focused = false;
								curNode = (TreeListNode)curNode.NextSibling();							
								if (!multiSelect)
									curNode.Selected = true;
								//else
								curNode.Focused = true;
                                this.SelectedNode = curNode;
							}

							Invalidate();
							return true;
						}						
					}
				}
			}

			return base.PreProcessMessage(ref msg);
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e); 
            RefreshCalendarView();
		}


		private void AutoSetColWidth(TreeListNodeCollection nodes, ref int mwid, ref int twid, int i)
		{
			for (int j=0; j<nodes.Count; j++)
			{
				if (i > 0)
					twid = GetStringWidth(nodes[j].SubItems[i-1].Text);
				else
					twid = GetStringWidth(nodes[j].Text);
				twid += 5;
				if (twid > mwid)
					mwid = twid;

				if (nodes[j].Nodes.Count > 0)
				{
					AutoSetColWidth(nodes[j].Nodes, ref mwid, ref twid, i);
				}
			}
		}

        System.Text.RegularExpressions.Regex regNumber = new System.Text.RegularExpressions.Regex(@"^[-]?\d+[.]?\d*$");
        private void SortNodes(TreeListNodeCollection lstNodes, int i)
        {
            foreach (TreeListNode treelistNode in lstNodes)
            {
                if (treelistNode.Nodes.Count > 0) SortNodes(treelistNode.Nodes, i);
            }

            //将树节点重新排序 
            TreeListNode[] arrayNode = new TreeListNode[lstNodes.Count];
            for (int row = 0; row < lstNodes.Count; row++)
            {
                arrayNode[row] = lstNodes[row];
            }

            if (i == 0)
            {
                Array.Sort(arrayNode, delegate(TreeListNode node1, TreeListNode node2)
                {

                    if (sortAsc)
                    {
                        //数字 
                        if (regNumber.IsMatch(node1.Text) && regNumber.IsMatch(node2.Text))
                        {
                            return decimal.Parse(node1.Text).CompareTo(decimal.Parse(node2.Text));
                        }
                        else
                        {
                            return node1.Text.CompareTo(node2.Text);
                        }
                    }
                    else
                    {
                        //数字
                        if (regNumber.IsMatch(node1.Text) && regNumber.IsMatch(node2.Text))
                        {
                            return decimal.Parse(node2.Text).CompareTo(decimal.Parse(node1.Text));
                        }
                        else
                        {
                            return node2.Text.CompareTo(node1.Text);
                        }
                    }
                });
            }
            else  //比较的subitems
            {
                Array.Sort(arrayNode, delegate(TreeListNode node1, TreeListNode node2)
                {
                    string text1 = "";
                    string text2 = "";
                    if (node1.SubItems.Count > i - 1)
                    {
                        text1 = node1.SubItems[i - 1].Text;
                    }
                    if (node2.SubItems.Count > i - 1)
                    {
                        text2 = node2.SubItems[i - 1].Text;
                    }

                    if (sortAsc)
                    {
                        //数字 
                        if (regNumber.IsMatch(text1) && regNumber.IsMatch(text2))
                        {
                            return decimal.Parse(text1).CompareTo(decimal.Parse(text2));
                        }
                        else
                        {
                            return text1.CompareTo(text2);
                        }
                    }
                    else
                    {
                        //数字 
                        if (regNumber.IsMatch(text1) && regNumber.IsMatch(text2))
                        {
                            return decimal.Parse(text2).CompareTo(decimal.Parse(text1));
                        }
                        else
                        {
                            return text2.CompareTo(text1);
                        }
                    }

                });  //自定义排序方式
            }

            //将排序后的节点重新添加到集合中
            for (int c = lstNodes.Count - 1; c >= 0; c--)
            {
                lstNodes.RemoveAt(c);
            }

            for (int c = 0; c < arrayNode.Length; c++)
            {
                lstNodes.Add(arrayNode[c]);
            }

        }

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

            HideTxtBox();
             
            if (e.Button == MouseButtons.Left)
            { 
                if (e.Y > headerBuffer + ltHeight) //点击日历区域 ,选择日期
                {
                    UnselectNodes(nodes);
                    //selectedNodes.Clear();
                    selectedNode = null;
                    TreeListNode cnode = NodeInNodeRow(e);
                    if (cnode != null)
                    {
                        cnode.Focused = true;
                        cnode.Selected = true;
                        curNode = cnode;

                        this.SelectedNode = cnode;
                        //添加被选中节点事件;
                        if (NodeMouseClick != null)
                        {
                            NodeMouseClick(this, cnode);
                        }
                    }
                }
                else//点击标题区域
                {
                    //拖动时间区域
                    if (calendarViewMode == CalendarViewModel.TimeSpan)
                    {
                        if (e.Y > headerBuffer && e.Y < headerBuffer + ltHeight)
                        {
                            Cursor.Current = Cursors.Hand;
                            progressTimeMode = true;
                            progressClickX = e.X;
                        }
                    }
                    if (calendarViewMode == CalendarViewModel.Week || calendarViewMode == CalendarViewModel.WorkWeek || calendarViewMode == CalendarViewModel.Day || calendarViewMode== CalendarViewModel.MonthWeek)
                    {
                        if (e.Y > 40 && e.Y < headerBuffer)
                        {
                            Cursor.Current = Cursors.Hand;
                            progressTimeMode = true;
                            progressClickX = e.X;
                        }
                    }

                }

                if (e.Y > headerBuffer) //点击标题以下区域，选择任务事件
                {
                    UnselectTask();
                    selectedTask = null;
                    TaskEventNode taskNode = taskInTaskRow(e);
                    if (taskNode != null)
                    {
                        taskNode.Selected = true;
                        this.selectedTask = taskNode;
                        if (e.Clicks == 2)
                        {
                            if (TaskDoubleClick != null)
                            {
                                TaskDoubleClick(this, taskNode);
                            }
                        }
                        else
                        {
                            //添加任务事件被选中事件
                            if (TaskMouseClick != null)
                            {
                                TaskMouseClick(this, taskNode);
                            }
                            //显示textBox 
                            TaskEventNode taskInTitle = taskTitleInTaskRow(e);
                            if (taskInTitle != null)
                            {
                                Rectangle rc = titleRectInTaskRow(e);
                                if (rc.Width != 0)
                                {
                                    taskInTitle.SelectedTitleArea = rc;
                                }
                                txtNode.Tag = taskNode;
                                timerTxt.Enabled = true;
                            }
                        }
                    }
                }


                
                
              
                Invalidate();

            } 
		}
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            //拖动时间区域
            if (progressTimeMode)
            {
                int tHeader = 0;
                int bHeader = 0;
                if (calendarViewMode == CalendarViewModel.TimeSpan)
                {
                    tHeader = headerBuffer;
                    bHeader = headerBuffer + ltHeight;
                }
                else if(calendarViewMode == CalendarViewModel.Week || calendarViewMode == CalendarViewModel.WorkWeek || calendarViewMode == CalendarViewModel.Day || calendarViewMode== CalendarViewModel.MonthWeek)
                {
                    tHeader = 40;
                    bHeader = headerBuffer;
                }

                if (e.Y > tHeader && e.Y < bHeader)
                {
                    int scaleEx = e.X - progressClickX;
                    if (scaleEx > itemwidth)  //向右拖动
                    {
                        Cursor.Current = Cursors.Hand;

                        NextView();
                        progressClickX = e.X;
                    }
                    else if (scaleEx < -itemwidth) //向左拖动
                    {
                        Cursor.Current = Cursors.Hand; 
                        PreView();
                        progressClickX = e.X;
                    }
                }
            }
           
        
            

        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if(progressTimeMode)
            {
                progressTimeMode = false;
                progressClickX = 0;
            }
        }
        protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp(e);

			if (e.KeyCode == Keys.F5)
			{
                if (allCollapsed)
                {
                    ExpandAll();

                    if (AfterExpandAll != null)
                    {
                        AfterExpandAll(this, null);
                    }

                }
                else
                {
                    CollapseAll();

                    if (AfterCollapseAll != null)
                    {
                        AfterCollapseAll(this, null);
                    }
                }
			}
		}

        //同步两个树的节点;
        private void AsynTwoTreeList(TreeListNode cnode)
        {
            if (cnode.IsExpanded)
            {
                if (AfterExpand != null)
                {
                    AfterExpand(this, cnode);
                }
            }
            else
            {
                if (AfterCollapse != null)
                {
                    AfterCollapse(this, cnode);
                }
            }
        }

        int rendcnt = 0;
		protected override void DrawRows(Graphics g, Rectangle r)
		{
			// render item rows
			int i; 
			int maxrend = ClientRectangle.Height/itemheight+1;

            rendcnt = 0;


            nodeRowRects.Clear(); 

            if(calendarViewMode== CalendarViewModel.Month)
            {
                colCount = 7;
                itemwidth = r.Width / colCount;
                itemheight = (r.Height - headerBuffer) / 5;
                ltHeight = 0;
                AdjustScrollbars();
                for (i = 0; i < nodes.Count; i++)
                {
                    RenderMonthDateNode(nodes[i], g, r, 0);
                }
                for (i = 0; i < taskEventNodes.Count; i++)
                {
                    RenderMonthTaskEventNode(taskEventNodes[i], g, r, 0);
                }
            }
            else if (calendarViewMode == CalendarViewModel.Week || calendarViewMode == CalendarViewModel.WorkWeek || calendarViewMode == CalendarViewModel.Day || calendarViewMode== CalendarViewModel.MonthWeek)
            {
                if (calendarViewMode == CalendarViewModel.Week)
                {
                    colCount = 7; 
                }
                else if(calendarViewMode== CalendarViewModel.WorkWeek)
                {
                    colCount = 5;
                }
                else if ( calendarViewMode== CalendarViewModel.Day)
                {
                    colCount = 1;
                }
                else if(calendarViewMode== CalendarViewModel.MonthWeek)
                {
                    colCount = System.Threading.Thread.CurrentThread.CurrentUICulture.Calendar.GetDaysInMonth(currentTime.Year, currentTime.Month); 
                } 
                itemheight = 20;
                ltHeight = 0;

                //跨区域任务 
                foreach (TaskEventNode taskEvent in taskEventNodes)
                {
                    int dayCount = 1;
                    for (i = 1; i < taskEvent.RelateNodes.Count; i++)
                    {
                        //是否绘制开始时间
                        bool isInNode = true;
                        if (taskEvent.RelateNodes[0].Date > taskEvent.StartTime)
                        {
                            isInNode = false;
                        }

                        DateTime dtMinTaskEnd = new DateTime(taskEvent.EndTime.Year, taskEvent.EndTime.Month, taskEvent.EndTime.Day, 0, 0, 0);
                        if (taskEvent.RelateNodes[taskEvent.RelateNodes.Count - 1].Date < dtMinTaskEnd)
                        {
                            isInNode = false;
                        }
                        if (isInNode == false)
                        {
                            dayCount++;
                            break;
                        }

                        if (taskEvent.RelateNodes[i].Date.Year == taskEvent.RelateNodes[i - 1].Date.Year
                           && taskEvent.RelateNodes[i].Date.Month == taskEvent.RelateNodes[i - 1].Date.Month
                           && taskEvent.RelateNodes[i].Date.Day == taskEvent.RelateNodes[i - 1].Date.Day)
                        {
                            continue;
                        }
                        else
                        {
                            dayCount++;
                        }
                    }
                    if (dayCount > 1)
                    {
                        ltHeight += 30;

                        taskEvent.AreaIndex = ltHeight / 30;
                    }
                }


                AdjustScrollbars();

                itemwidth = (r.Width - lfWidth - vsize) / colCount;
                //绘制左侧小时
                RenderWeekLeftInfo(g, r, colCount);
                //绘制每个小时节点
                for (i = 0; i < nodes.Count; i++)
                {
                    RenderWeekDateNode(nodes[i], g, r, 0);
                }
                for (i = 0; i < taskEventNodes.Count; i++)
                {
                    RenderWeekTaskEventNode(taskEventNodes[i], g, r, 0);
                }
            }
            else if(calendarViewMode== CalendarViewModel.Year)
            { 
                ltHeight = 0;
                colCount = 37;
                itemwidth = (r.Width - lfWidth) / colCount; 
                itemheight = (r.Height - headerBuffer) / 12;
                AdjustScrollbars();

                //绘制左侧月
                RenderYearLeftInfo(g, r, 0);
                for (i = 0; i < nodes.Count; i++)
                {
                    RenderMonthDateNode(nodes[i], g, r, 0);
                }
                for (i = 0; i < taskEventNodes.Count; i++)
                {
                    RenderMonthTaskEventNode(taskEventNodes[i], g, r, 0);
                }
            }
            else if(calendarViewMode == CalendarViewModel.TimeSpan)
            {
                colCount = nodes.Count;

                ltHeight = 35;
                itemwidth = 25;
                itemheight = r.Height - headerBuffer - ltHeight;
                AdjustScrollbars();

                //绘制时间条标题
                RenderTimeSpanTopInfo(g, r, 0);
                for (i = 0; i < nodes.Count; i++)
                {
                    RenderTimeSpanDateNode(nodes[i], g, r, 0);
                }
                taskEventNodes.Sort();
                for (i = 0; i < taskEventNodes.Count; i++)
                {
                    RenderTimeSpanTaskEventNode(taskEventNodes[i], g, r, 0);
                }
            } 
		}

        protected override void DrawHeaders(Graphics g, Rectangle r)
        {
            // render column headers and trailing column header

            g.Clip = new Region(new Rectangle(r.Left, r.Top, r.Width, r.Top + headerBuffer + 4));

            g.FillRectangle(new SolidBrush(SystemColors.Control), r.Left, r.Top, r.Width - 2, headerBuffer);

            int last = 0;
            int i;
            int lh = 22;

            //绘制顶部信息区域
            int lp_scr = r.Left - hscrollBar.Value; 
            Rectangle rectHeaderInfo = new Rectangle(lp_scr + last, r.Top + 1, r.Width, r.Top + headerBuffer - lh);
            LinearGradientBrush headerInfoebrush = new LinearGradientBrush(rectHeaderInfo, Color.FromArgb(237, 246, 245), Color.FromArgb(112, 178, 235), 90f);
            Blend blendheaderInfo = new Blend();
            float[] fsheaderInfo = new float[5] { 0f, 0.5f, 0.6f, 0.8f, 1f };
            float[] fheaderInfo = new float[5] { 0f, 0.3f, 0.5f, 0.8f, 0.4f };
            blendheaderInfo.Positions = fsheaderInfo;
            blendheaderInfo.Factors = fheaderInfo;
            headerInfoebrush.Blend = blendheaderInfo;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.FillRectangle(headerInfoebrush, rectHeaderInfo);
            g.DrawRectangle(Pens.LightSteelBlue, rectHeaderInfo);

            Font textHeader = new System.Drawing.Font("微软雅黑", 11.0f);
            Font textDate = new System.Drawing.Font("微软雅黑", 9.0f);

            g.DrawString("日历", textHeader, new SolidBrush(Color.SteelBlue), 10, 10);
            if(SelectedNode!=null)
            {
                string[] weekArray = new string[] { "星期天", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
                string dateInfo = SelectedNode.Date.ToString("yyyy年MM月dd日") + " " + weekArray[SelectedNode.Week] + " " + SelectedNode.ChinesDate.Cdate;
                string dayInfo = "第" + SelectedNode.Date.DayOfYear + "天";
                g.DrawString(dateInfo, textDate, new SolidBrush(Color.SteelBlue), 50, 6);
                g.DrawString(dayInfo, textDate, new SolidBrush(Color.SteelBlue), 50, 22);

            }

            if( calendarViewMode== CalendarViewModel.Month)
            {
                string[] weekArray = new string[] { "星期天", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };

                int width = r.Width / 7;
                for (i = 0; i < 7; i++)
                {
                    //绘制标题栏按钮 
                    //System.Windows.Forms.ControlPaint.DrawButton(g, lp_scr + last, r.Top + 2, width, r.Top + headerBuffer, ButtonState.Flat);
                    if(i==6)
                    {
                        width = r.Width - (lp_scr + last) - 1;
                    }
                    Rectangle rect = new Rectangle(lp_scr + last, r.Top + headerBuffer-lh, width,lh);
                    LinearGradientBrush linebrush = new LinearGradientBrush(rect, Color.FromArgb(237, 246, 245), Color.FromArgb(112, 178, 235), 90f);
                    Blend blend = new Blend();
                    float[] fs = new float[5] { 0f, 0.5f, 0.6f, 0.8f, 1f };
                    float[] f = new float[5] { 0f, 0.3f, 0.5f, 0.8f, 0.1f };
                    blend.Positions = fs;
                    blend.Factors = f;
                    linebrush.Blend = blend;
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.FillRectangle(linebrush, rect);
                    g.DrawRectangle(Pens.LightSteelBlue, rect);
                    //绘制标题文本

                    string sp = TruncatedString(weekArray[i], width, 25, g);
                    Font textFont = new System.Drawing.Font("微软雅黑", 10.0f);
                    g.DrawString(sp, textFont, SystemBrushes.ControlText, (float)(lp_scr + last + (width / 2) - (Helpers.StringTools.MeasureDisplayStringWidth(g, sp, this.Font) / 2)), (float)(r.Top + 4 + headerBuffer - lh));

                    last += width;
                }  
            }
            else if (calendarViewMode == CalendarViewModel.Week || calendarViewMode == CalendarViewModel.WorkWeek || calendarViewMode == CalendarViewModel.Day || calendarViewMode== CalendarViewModel.MonthWeek)
            {
                int days = 1;
                if(calendarViewMode== CalendarViewModel.Day)
                {
                    days = 1;
                }
                if(calendarViewMode== CalendarViewModel.WorkWeek)
                {
                    days = 5;
                }
                else if(calendarViewMode== CalendarViewModel.Week)
                {
                    days = 7;
                } 
                else if(calendarViewMode == CalendarViewModel.MonthWeek)
                {
                    days = System.Threading.Thread.CurrentThread.CurrentUICulture.Calendar.GetDaysInMonth(currentTime.Year,currentTime.Month);
                }
                //当前一周 
                int lf = lfWidth;
                if (nodes.Count < 1) return; 
                Rectangle rect = new Rectangle(lp_scr +last, r.Top + headerBuffer - lh, lf, lh);
                LinearGradientBrush linebrush = new LinearGradientBrush(rect, Color.FromArgb(237, 246, 245), Color.FromArgb(112, 178, 235), 90f);
                Blend blend = new Blend();
                float[] fs = new float[5] { 0f, 0.5f, 0.6f, 0.8f, 1f };
                float[] f = new float[5] { 0f, 0.3f, 0.5f, 0.8f, 0.1f };
                blend.Positions = fs;
                blend.Factors = f;
                linebrush.Blend = blend;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.FillRectangle(linebrush, rect);
                g.DrawRectangle(Pens.LightSteelBlue, rect);

                int width = (r.Width - lf - vsize) / days;
                for (i = 0; i < days; i++)
                {
                    //绘制标题栏按钮  
                    int nodeIndex = 48 * i;
                    TreeListNode node = nodes[nodeIndex];
                    string dayInfo =node.Day.ToString();
                    if(days<=7)
                    {
                        dayInfo = node.Month + "月" + node.Day + "日"  + " " + GetWeekString(node.Week) + " " + node.ChinesDate.Cday; 
                    }

                    if (i == days - 1)
                    {
                        width = r.Width - (lp_scr + lf + last)-1;
                    }
                    rect = new Rectangle(lp_scr + lf + last, r.Top + headerBuffer - lh, width, lh);
                    linebrush = new LinearGradientBrush(rect, Color.FromArgb(237, 246, 245), Color.FromArgb(112, 178, 235), 90f);
                  
                    linebrush.Blend = blend;
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.FillRectangle(linebrush, rect);
                    g.DrawRectangle(Pens.LightSteelBlue, rect);
                    //绘制标题文本

                    string sp = TruncatedString(dayInfo, width, 25, g);
                    Font textFont = new System.Drawing.Font("微软雅黑", 10.0f);
                    g.DrawString(sp, textFont, SystemBrushes.ControlText, (float)(lp_scr + lf + last + (width / 2) - (Helpers.StringTools.MeasureDisplayStringWidth(g, sp, this.Font) / 2)), (float)(r.Top + 4 + headerBuffer - lh));

                    last += width;
                }

            }  
            else if(calendarViewMode== CalendarViewModel.Year)
            {
                int lf = lfWidth;
                if (nodes.Count < 1) return;
                string[] weekArray = new string[] { "日", "一", "二", "三", "四", "五", "六" };

                int width= (r.Width-lf) / 37;

                Font textFont = new System.Drawing.Font("微软雅黑", 10.0f);

                Blend blend = new Blend();
                float[] fs = new float[5] { 0f, 0.5f, 0.6f, 0.8f, 1f };
                float[] f = new float[5] { 0f, 0.3f, 0.5f, 0.8f, 0.1f };
                blend.Positions = fs;
                blend.Factors = f;

                g.SmoothingMode = SmoothingMode.AntiAlias;

                Rectangle rectYearArea = new Rectangle(lp_scr , r.Top + headerBuffer - lh, lf, lh);
                LinearGradientBrush lineYearbrush = new LinearGradientBrush(rectYearArea, Color.FromArgb(237, 246, 245), Color.FromArgb(112, 178, 235), 90f);
                lineYearbrush.Blend = blend;
                g.FillRectangle(lineYearbrush, rectYearArea);
                g.DrawRectangle(Pens.LightSteelBlue, rectYearArea);

                for (i = 0; i < 37; i++)
                {
                    int weekNum = i % 7;

                    //绘制标题栏按钮 
                    //System.Windows.Forms.ControlPaint.DrawButton(g, lp_scr + last, r.Top + 2, width, r.Top + headerBuffer, ButtonState.Flat);

                    Rectangle rect = new Rectangle(lp_scr + lf + last, r.Top + headerBuffer - lh, width, lh);
                    if(i==36 && rect.Right!=r.Right)
                    {
                        rect = new Rectangle(rect.Left, rect.Top, rect.Width + (r.Right - rect.Right), rect.Height);
                    }
                    LinearGradientBrush linebrush = new LinearGradientBrush(rect, Color.FromArgb(237, 246, 245), Color.FromArgb(112, 178, 235), 90f); 
                    linebrush.Blend = blend;
                    g.FillRectangle(linebrush, rect);
                    g.DrawRectangle(Pens.LightSteelBlue, rect);
                    //绘制标题文本

                    string sp = weekArray[weekNum];
                    g.DrawString(sp, textFont, SystemBrushes.ControlText, (float)(rect.Left+(rect.Width/2)-6), (float)(rect.Top + 3));

                    last += width;
                }  
            }
            else if(calendarViewMode== CalendarViewModel.TimeSpan)
            {
                string[] weekArray = new string[] { "星期天", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };

                DateTime dtFirstNode = nodes[0].Date;
                int startColIndex = 0;
                for (i = 0; i < nodes.Count; i++)
                {
                    if(!(nodes[i].Date.Year==dtFirstNode.Year && nodes[i].Date.Month==dtFirstNode.Month && nodes[i].Date.Day== dtFirstNode.Day) || i==nodes.Count-1)
                    {
                        int width = (int)(itemwidth * (i - startColIndex ));

                        //绘制新的日期标题
                        Rectangle rect = new Rectangle(lp_scr + last, r.Top + headerBuffer - lh, width, lh);
                        LinearGradientBrush linebrush = new LinearGradientBrush(rect, Color.FromArgb(237, 246, 245), Color.FromArgb(112, 178, 235), 90f);
                        Blend blend = new Blend();
                        float[] fs = new float[5] { 0f, 0.5f, 0.6f, 0.8f, 1f };
                        float[] f = new float[5] { 0f, 0.3f, 0.5f, 0.8f, 0.1f };
                        blend.Positions = fs;
                        blend.Factors = f;
                        linebrush.Blend = blend;
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        g.FillRectangle(linebrush, rect);
                        g.DrawRectangle(Pens.LightSteelBlue, rect);

                        int week = nodes[startColIndex].Week;
                        string daytitle = nodes[startColIndex].Date.ToString("yyyy年MM月dd日") +" "+ weekArray[week] +" "+ nodes[startColIndex].ChinesDate.Cmonth + "月" + nodes[startColIndex].ChinesDate.Cday;
                        string sp = TruncatedString(daytitle, width, 25, g);
                        Font textFont = new System.Drawing.Font("微软雅黑", 10.0f);
                        g.DrawString(sp, textFont, SystemBrushes.ControlText, (float)(lp_scr + last + (width / 2) - (Helpers.StringTools.MeasureDisplayStringWidth(g, sp, this.Font) / 2)), (float)(r.Top + 4 + headerBuffer - lh));

                        last += width;



                        dtFirstNode = nodes[i].Date;
                        startColIndex = i;
                    }
                    else
                    {
                       


                    } 
                }  
            }
        }
        protected override void DrawExtra(Graphics g, Rectangle r)
        { 
        }

        protected override void DrawBackground(Graphics g, Rectangle r)
        {
            base.DrawBackground(g, r);

            //int AllShowNodeCount = 0;
            //foreach (TreeListNode node in nodes)
            //{
            //    AllShowNodeCount++;
            //    if (node.IsExpanded)
            //    {
            //        AllShowNodeCount += GetChildNodeCount(node);
            //    }
            //}

            //if (editStyle)
            //{
            //    //g.DrawLine(p, r.Left + lwidth + columns[i].Width - hscrollBar.Value, r.Top + 2 + headerBuffer, r.Left + lwidth + columns[i].Width - hscrollBar.Value, r.Top + headerBuffer + (itemheight * AllShowNodeCount));

            //    g.FillRectangle(SystemBrushes.Control, r.Left - hscrollBar.Value, r.Top + headerBuffer, columns[0].Width, this.Height - r.Top - headerBuffer);
            //    g.DrawLine(Pens.DarkGray, r.Left + columns[0].Width - hscrollBar.Value, r.Top + headerBuffer, r.Left + columns[0].Width - hscrollBar.Value, this.Height);
            //}


            ////绘制分隔条背景
            //if (separateLine)
            //{
            //    int lheight = 2;
            //    int i = 0;
            //    int startIndex = 0;
            //    if (nodes.Count % 2 == 0)
            //    {
            //        startIndex = 1;
            //    }
            //    else
            //    {
            //        startIndex = 0;
            //    }

            //    for (i = startIndex; i < nodes.Count; i += 2)
            //    {
            //        lheight = itemheight * i+2;
            //        g.FillRectangle(SystemBrushes.GradientInactiveCaption, r.Left + 2, r.Top + headerBuffer  + lheight - vscrollBar.Value, Width, itemheight); 
            //    }
            //}


            //if (gridLines && AllShowNodeCount > 0)
            //{ 
            //    Pen p = new Pen(new SolidBrush(gridLineColor), 1.0f);
            //    int i;
            //    int lwidth = 1, lheight = 1;


            //    // vertical
            //    for (i = 0; i < columns.Count; i++)
            //    {
            //        if (r.Left + lwidth + columns[i].Width - hscrollBar.Value >= r.Left + r.Width - 2)
            //            break;

            //        g.DrawLine(p, r.Left + lwidth + columns[i].Width - hscrollBar.Value, r.Top + 2 + headerBuffer, r.Left + lwidth + columns[i].Width - hscrollBar.Value, r.Top + headerBuffer + (itemheight * AllShowNodeCount));
            //        lwidth += columns[i].Width;
            //    }


            //    //horizal 
            //    for (i = 0; i < AllShowNodeCount; i++)
            //    {
            //        g.DrawLine(p, r.Left + 2, r.Top + headerBuffer + itemheight + lheight - vscrollBar.Value, r.Left + r.Width, r.Top + headerBuffer + itemheight + lheight - vscrollBar.Value);
            //        lheight += itemheight;
            //    }
            //}


        }

        private int GetChildNodeCount(TreeListNode node)
        {
            int rs = 0;
            foreach (TreeListNode n in node.Nodes)
            {
                rs++;
                if (n.IsExpanded)
                {
                    rs = rs + GetChildNodeCount(n);
                }
            }
            return rs;
        }
		#endregion

        #region TextBoxEdit 
        void txtNode_Leave(object sender, EventArgs e)
        { 
        }
        void txtNode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode== Keys.Escape)
            {
                HideTxtBox();
            }
        }

        void txtNode_MouseMove(object sender, MouseEventArgs e)
        {
            //if (muiltTxtDown)
            //{
            //    if (e.Y > txtNode.Height || e.Y < 0)
            //    {
            //        if (txtNode.Visible)
            //        {
            //            txtNode.Visible = false; 
            //        }
            //    }
            //}
        }

        void txtNode_MouseUp(object sender, MouseEventArgs e)
        {
            //muiltTxtDown = false;
        }

        void txtNode_MouseDown(object sender, MouseEventArgs e)
        {
            //muiltTxtDown = true;
        }

        void timerTxt_Tick(object sender, EventArgs e)
        { 
            timerTxt.Enabled = false;
            ShowTxtBox();
        }

        private void ShowTxtBox()
        {
            if (txtNode.Tag == null) return;
            TaskEventNode task = txtNode.Tag as TaskEventNode;
            Rectangle r = task.SelectedTitleArea;

            txtNode.AccessibleDescription = "Save";
            txtNode.Text = task.Title;
            int left = r.Left;
            int top = r.Top < headerBuffer ? headerBuffer : r.Top;
            int height = r.Height - (r.Top < headerBuffer ? headerBuffer - r.Top : 0);
            txtNode.Location=new Point(left,top);
            if (calendarViewMode == CalendarViewModel.Month || calendarViewMode== CalendarViewModel.Year)
            {
                txtNode.Multiline = false;
                txtNode.ClientSize = new Size(r.Width - 6, height);
            }
            else if(calendarViewMode== CalendarViewModel.TimeSpan)
            {
                txtNode.Multiline = false;
                txtNode.ClientSize = new Size(r.Width - 2, height);
            }
            else if (calendarViewMode == CalendarViewModel.Week || calendarViewMode== CalendarViewModel.WorkWeek || calendarViewMode == CalendarViewModel.Day || calendarViewMode== CalendarViewModel.MonthWeek)
            {
                txtNode.Multiline = true;
                txtNode.ClientSize = new Size(r.Width - 2, height - 2);

            }
            txtNode.Parent = this;
            txtNode.Select(txtNode.Text.Length, 0);
            txtNode.Visible = true;
            txtNode.Focus(); 
        }
        private void HideTxtBox()
        {
            if(this.txtNode.Tag!=null && this.txtNode.AccessibleDescription!=null && this.txtNode.AccessibleDescription == "Save")
            { 
                TaskEventNode task = txtNode.Tag as TaskEventNode;
                task.Title = this.txtNode.Text;
            }

            this.txtNode.Tag = null;
            this.txtNode.AccessibleDescription = "";
            if(this.txtNode.Visible)
            {  
                if (this.Controls.Contains(txtNode))
                {
                    this.Controls.Remove(txtNode);
                }
                this.txtNode.Visible = false;
            }

        }
        #endregion
        private void ClearNodeRows(TreeListNode node, Boolean show)
        {
            Boolean bz = show && node.IsExpanded;
            foreach (TreeListNode n in node.Nodes)
            {
                if (!bz)
                {
                    //将不显示的节点图标显示关闭
                    for (int j = 0; j < n.SubItems.Count && j < columns.Count; j++)
                    {
                        if (n.SubItems[j].ItemControl != null)
                        {
                            Control c = n.SubItems[j].ItemControl;
                            c.Visible = false;
                        }
                    }
                }
                ClearNodeRows(n, bz);
            }
        }

        public void GotoCalendarDate(DateTime dt)
        {
            currentTime = dt;
            LoadCalendarView(); 
        }

        private void LoadCalendarView()
        {
            HideTxtBox();
            switch (calendarViewMode)
            {
                case CalendarViewModel.Month:
                    LoadCalendarMonthView();
                    break;
                case CalendarViewModel.Week:
                    LoadCalendarWeekView(7);
                    break;
                case CalendarViewModel.WorkWeek:
                    LoadCalendarWeekView(5);
                    break;
                case CalendarViewModel.Day:
                    LoadCalendarWeekView(1);
                    break;
                case CalendarViewModel.MonthWeek:
                    LoadCalendarWeekView(31);
                    break;
                case CalendarViewModel.Year:
                    LoadCalendarYearView();
                    break;
                case CalendarViewModel.TimeSpan:
                    currentTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, 0, 0, 0);
                    LoadCalendarTimeSpanView();
                    break;
            }
            AdjustScrollbars();
            Invalidate();
        }

        private void RefreshCalendarView()
        {
            HideTxtBox();
            switch (calendarViewMode)
            { 
                case CalendarViewModel.TimeSpan:
                    LoadCalendarTimeSpanView();
                    break;
            } 
            Invalidate();
        }

        public void PreView()
        {
            HideTxtBox();
            switch (calendarViewMode)
            {
                case CalendarViewModel.Month:
                    currentTime = currentTime.AddMonths(-1);
                    LoadCalendarMonthView();
                    break;
                case CalendarViewModel.Week:
                    currentTime = currentTime.AddDays(-7);
                    LoadCalendarWeekView(7);
                    break;
                case CalendarViewModel.WorkWeek:
                    currentTime = currentTime.AddDays(-7);
                    LoadCalendarWeekView(5);
                    break;
                case CalendarViewModel.Day:
                    currentTime = currentTime.AddDays(-1);
                    LoadCalendarWeekView(1);
                    break;
                case CalendarViewModel.MonthWeek:
                    currentTime = currentTime.AddMonths(-1);
                    LoadCalendarWeekView(31);
                    break;
                case CalendarViewModel.Year:
                    currentTime = currentTime.AddYears(-1);
                    LoadCalendarYearView();
                    break;
                case CalendarViewModel.TimeSpan:
                    currentTime = currentTime.AddMinutes(-30);
                    LoadCalendarTimeSpanView();
                    break;
            }


        }
        public void NextView()
        {
            HideTxtBox();
            switch (calendarViewMode)
            {
                case CalendarViewModel.Month:
                    currentTime = currentTime.AddMonths(1);
                    LoadCalendarMonthView();
                    break;
                case CalendarViewModel.Week:
                    currentTime = currentTime.AddDays(7);
                    LoadCalendarWeekView(7);
                    break;
                case CalendarViewModel.WorkWeek:
                    currentTime = currentTime.AddDays(7);
                    LoadCalendarWeekView(5);
                    break;
                case CalendarViewModel.Day:
                    currentTime = currentTime.AddDays(1);
                    LoadCalendarWeekView(1);
                    break; 
                case CalendarViewModel.MonthWeek:
                    currentTime = currentTime.AddMonths(1);
                    LoadCalendarWeekView(31);
                    break;
                case CalendarViewModel.Year:
                    currentTime = currentTime.AddYears(1);
                    LoadCalendarYearView();
                    break;
                case CalendarViewModel.TimeSpan:
                    currentTime = currentTime.AddMinutes(30);
                    LoadCalendarTimeSpanView();
                    break;
            }
        }

        private void LoadCalendarMonthView()
        {
            int year = currentTime.Year;
            int month = currentTime.Month;

            lfWidth = 0; 

            nodes.Clear(); 
            int nowMonthDays = System.Threading.Thread.CurrentThread.CurrentUICulture.Calendar.GetDaysInMonth(year, month);
             
            for (int i = 1; i <= nowMonthDays; i++)
            {
                TreeListNode dtNode = new TreeListNode(new DateTime(year, month, i,0,0,0));
                nodes.Add(dtNode);
            }
            int firstDayWeek = nodes[0].Week;
            int lastDayWeek = nodes[nowMonthDays - 1].Week;
            //第一周的天数
            if (firstDayWeek != 0)
            {
                for (int i = 0; i < firstDayWeek; i++)
                {
                    DateTime dtLastMonthFirst = nodes[0].Date.AddDays(-1);
                    TreeListNode dtNode = new TreeListNode(dtLastMonthFirst);
                    dtNode.BackColor = Color.LightYellow;
                    nodes.Insert(0, dtNode);
                }
            }
            //最后一周的天数
            if (lastDayWeek != 6)
            {
                for (int i = lastDayWeek + 1; i < 7; i++)
                {
                    DateTime dtLastMonthDay = nodes[nodes.Count - 1].Date.AddDays(1);
                    TreeListNode dtNode = new TreeListNode(dtLastMonthDay); 
                    dtNode.BackColor = Color.LightYellow;
                    nodes.Add(dtNode);
                }
            }
            for (int i = 0; i < nodes.Count; i++)
            {
                int row = i / 7;
                int col = i % 7;
                nodes[i].Row = row;
                nodes[i].Col = col; 
            }
            BindTaskToNode();   
            Invalidate();
             
            //日历当前视图时间
            minTime = nodes[0].Date;
            maxTime = nodes[nodes.Count - 1].Date;

        }
        private void LoadCalendarWeekView(int dayCount)
        {
            DateTime startDayTime = DateTime.Now;
            if(dayCount==1)  //一天
            {
                startDayTime = currentTime;
            }
            else if(dayCount==31)  //一个月
            {
                int year = currentTime.Year;
                int month = currentTime.Month;
                int nowMonthDays = System.Threading.Thread.CurrentThread.CurrentUICulture.Calendar.GetDaysInMonth(year, month);
                startDayTime = new DateTime(year, month, 1);
                dayCount = nowMonthDays; 
            }
            else if(dayCount==5) //工作周
            {
                startDayTime = currentTime.AddDays(1 - Convert.ToInt32(currentTime.DayOfWeek.ToString("d")));  
            }
            else if(dayCount==7)  //周
            {
                startDayTime = currentTime.AddDays(1 - Convert.ToInt32(currentTime.DayOfWeek.ToString("d")));   
            }
            int week = GetWeekOfYear(currentTime);

            lfWidth = 60;
            itemheight = 20;
            nodes.Clear();
            int i = 0;
            for (i = 0; i < dayCount; i++)
            {
                DateTime dt = startDayTime.AddDays(i);
                for(int j=0;j<24;j++)
                { 
                    TreeListNode dtNode1 = new TreeListNode(new DateTime(dt.Year, dt.Month, dt.Day,j,0,0));
                    dtNode1.Row = j * 2;
                    dtNode1.Col = i;
                    nodes.Add(dtNode1);
                    TreeListNode dtNode2 = new TreeListNode(new DateTime(dt.Year, dt.Month, dt.Day, j, 30, 0));
                    dtNode2.Row = j * 2+1;
                    dtNode2.Col = i;
                    nodes.Add(dtNode2);
                    if (dtNode1.Week == 6 || dtNode1.Week == 0)
                    {
                        dtNode1.BackColor = Color.LightYellow;
                        dtNode2.BackColor = Color.LightYellow;
                    }
                }
            } 
            BindTaskToNode();
            RefreshTaskEvent();
            Invalidate();

            //日历当前视图时间
            minTime = nodes[0].Date;
            maxTime = nodes[nodes.Count - 1].Date;

        } 

        private void LoadCalendarYearView()
        {
            int year = currentTime.Year;

            int nowYearsDay = System.Threading.Thread.CurrentThread.CurrentUICulture.Calendar.GetDaysInYear(year);

            lfWidth = 52;
            nodes.Clear();

            int startIndex = 0;
            for (int m = 1; m <= 12;m++ )
            {
                int month = m;
                int nowMonthDays = System.Threading.Thread.CurrentThread.CurrentUICulture.Calendar.GetDaysInMonth(year, month);

                for (int i = 1; i <= nowMonthDays; i++)
                {
                    TreeListNode dtNode = new TreeListNode(new DateTime(year, month, i, 0, 0, 0));
                    nodes.Add(dtNode);
                }

                int firstDayWeek = nodes[startIndex].Week;
                int lastDayWeek = nodes[nowMonthDays - 1].Week;

                if(firstDayWeek!=0)
                {
                    for (int i = 0; i < firstDayWeek; i++)
                    {
                        DateTime dtLastMonthFirst = nodes[startIndex].Date.AddDays(-1);
                        TreeListNode dtNode = new TreeListNode(dtLastMonthFirst);
                        dtNode.IsBlankNode = true;
                        dtNode.BackColor = Color.LightGray;
                        nodes.Insert(startIndex, dtNode);
                    }
                }
                int monthNodeCount = nodes.Count - startIndex;
                if(monthNodeCount<37)
                { 
                    for(int i=0;i<37-monthNodeCount;i++)
                    { 
                        DateTime dtLastMonthDay = nodes[nodes.Count - 1].Date.AddDays(1);
                        TreeListNode dtNode = new TreeListNode(dtLastMonthDay);
                        dtNode.IsBlankNode = true;
                        dtNode.BackColor = Color.LightGray;
                        nodes.Add(dtNode);
                    }
                }
                startIndex = nodes.Count;
            }
                
            for (int i = 0; i < nodes.Count; i++)
            {
                int row = i / 37;
                int col = i % 37;
                nodes[i].Row = row;
                nodes[i].Col = col;
            }
            BindTaskToNode();
            Invalidate();

            //日历当前视图时间
            minTime = nodes[0].Date;
            maxTime = nodes[nodes.Count - 1].Date;
        }

        private void LoadCalendarTimeSpanView()
        {
            itemwidth = 25;
            int nodeCount = this.Width / (int)itemwidth + 2;
             
            DateTime dtStart = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, currentTime.Hour, currentTime.Minute, currentTime.Second);

            lfWidth = 0;
            nodes.Clear();
            for (int i = 0; i < nodeCount; i++)
            {
                DateTime dt = dtStart.AddMinutes(30 * i);
                TreeListNode dtNode = new TreeListNode(dt);
                if((dt.Hour==23 && dt.Minute==30) || (dt.Hour==0 && dt.Minute==0))
                {
                    dtNode.BackColor = Color.LightYellow;
                }
                nodes.Add(dtNode);
            }
           
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].Row = 0;
                nodes[i].Col = i;
            }
            BindTaskToNode();
            Invalidate();

            //日历当前视图时间
            minTime = nodes[0].Date;
            maxTime = nodes[nodes.Count - 1].Date;
        }
          

        private void RefreshTaskEvent()
        {
            if (calendarViewMode == CalendarViewModel.Week || calendarViewMode== CalendarViewModel.WorkWeek || calendarViewMode == CalendarViewModel.Day ||calendarViewMode== CalendarViewModel.MonthWeek)
            {
                int i = 0;
                //对任务事件位置进行排序
                if (taskEventNodes.Count > 1)
                {
                    //对所有的任务进行排序
                    taskEventNodes.Sort();

                    for (i = 0; i < taskEventNodes.Count; i++)
                    {
                        taskEventNodes[i].ColSubIndex = 0;
                        taskEventNodes[i].ColSubCount = 0; 
                    }
                    DateTime firstDay = taskEventNodes[0].StartTime;
                    int colSubIndex = 0;
                    for (i = 0; i < taskEventNodes.Count; i++)
                    {
                        if (taskEventNodes[i].StartTime.Year == firstDay.Year && taskEventNodes[i].StartTime.Month == firstDay.Month && taskEventNodes[i].StartTime.Day == firstDay.Day)
                        {
                            for (int j = 0; j < i; j++)
                            {
                                if (TaskTimeInArea(taskEventNodes[i], taskEventNodes[j]) || TaskTimeInArea(taskEventNodes[j], taskEventNodes[i]))
                                {
                                    taskEventNodes[i].ColSubIndex = taskEventNodes[j].ColSubIndex + 1; 
                                }
                            }

                            for (int ix = colSubIndex; ix <= i; ix++)
                            {
                                if (taskEventNodes[i].ColSubIndex + 1 > taskEventNodes[ix].ColSubCount)
                                { 
                                    taskEventNodes[ix].ColSubCount = taskEventNodes[i].ColSubIndex + 1;
                                }
                            }
                        }
                        else
                        {
                            colSubIndex = i;  //日期开始下标
                            firstDay = taskEventNodes[i].StartTime;
                        }
                    }
                } 
            }
           

        }
        private int GetWeekOfYear(DateTime date)
        {
            int firstWeekend = 7 - Convert.ToInt32(DateTime.Parse(date.Year + "-1-1").DayOfWeek);

            int currentDay = date.DayOfYear;

            return Convert.ToInt32(Math.Ceiling((currentDay - firstWeekend) / 7.0)) + 1;
        }

        private string GetWeekString(int index)
        {
            if (index > 6) index = 0;

            string[] weekArray = new string[] { "星期天", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };

            return weekArray[index];
        }

        public void AddTask(TaskEventNode task)
        {
            if (!taskEventNodes.Contains(task))
            {
                taskEventNodes.Add(task);
                BindTaskToNode(task);
                RefreshTaskEvent();
                Invalidate();
            }
        }

        public void AddTask( List<TaskEventNode> lstTask)
        { 
            foreach(TaskEventNode task in lstTask)
            { 
                taskEventNodes.Add(task);
            }
            BindTaskToNode();
            RefreshTaskEvent();
            Invalidate();
        }

        public void RemoveTask(TaskEventNode task)
        {
            if (taskEventNodes.Contains(task))
            {
                taskEventNodes.Remove(task);

                if (selectedTask == task)
                {
                    selectedTask = null;
                }
                RefreshTaskEvent();
                Invalidate();
            }

        }

        public void ClearTask()
        {
            taskEventNodes.Clear();
            Invalidate();
        }

        private void BindTaskToNode()
        {
            DateTime firstTime = nodes[0].Date;
            DateTime lastTime = nodes[nodes.Count - 1].Date.AddDays(1);

            foreach (TaskEventNode taskEvent in taskEventNodes)
            {
                taskEvent.RelateNodes.Clear();
                if ((taskEvent.StartTime > firstTime && taskEvent.StartTime < lastTime) || (taskEvent.EndTime > firstTime && taskEvent.EndTime < lastTime)
                || (firstTime > taskEvent.StartTime && firstTime < taskEvent.EndTime) || (lastTime > taskEvent.StartTime && lastTime < taskEvent.EndTime))
                {
                    foreach (TreeListNode node in nodes)
                    {
                        if (node.IsBlankNode) continue;
                        DateTime dtNodeMax = node.Date;
                        if (calendarViewMode == CalendarViewModel.Month || calendarViewMode== CalendarViewModel.Year)
                        {
                            dtNodeMax = new DateTime(node.Date.Year, node.Date.Month, node.Date.Day, 23, 59, 59);
                        }
                        else if (calendarViewMode == CalendarViewModel.Week || calendarViewMode== CalendarViewModel.WorkWeek || calendarViewMode == CalendarViewModel.Day || calendarViewMode== CalendarViewModel.MonthWeek)
                        {
                            dtNodeMax = new DateTime(node.Date.Year, node.Date.Month, node.Date.Day, node.Date.Hour, node.Date.Minute, 59);
                        } 
                        else if (calendarViewMode == CalendarViewModel.TimeSpan)
                        {
                            dtNodeMax = new DateTime(node.Date.Year, node.Date.Month, node.Date.Day, node.Date.Hour, node.Date.Minute, 59);
                        }
                        if (taskEvent.StartTime < dtNodeMax && node.Date < taskEvent.EndTime)
                        {
                            taskEvent.RelateNodes.Add(node);
                            node.RelateTasks.Add(taskEvent);
                        } 
                    }
                }
            }
        }
        private void BindTaskToNode(TaskEventNode taskEvent)
        {
            DateTime firstTime = nodes[0].Date;
            DateTime lastTime = nodes[nodes.Count - 1].Date.AddDays(1); 
            taskEvent.RelateNodes.Clear();
            if ((taskEvent.StartTime > firstTime && taskEvent.StartTime < lastTime) || (taskEvent.EndTime > firstTime && taskEvent.EndTime < lastTime)
            || (firstTime > taskEvent.StartTime && firstTime < taskEvent.EndTime) || (lastTime > taskEvent.StartTime && lastTime < taskEvent.EndTime))
            {
                foreach (TreeListNode node in nodes)
                {
                    if (node.IsBlankNode) continue;
                    DateTime dtNodeMax = node.Date;
                    if(calendarViewMode == CalendarViewModel.Month || calendarViewMode== CalendarViewModel.Year)
                    {
                        dtNodeMax = new DateTime(node.Date.Year, node.Date.Month, node.Date.Day, 23, 59, 59);
                    }
                    else if (calendarViewMode == CalendarViewModel.Week || calendarViewMode == CalendarViewModel.WorkWeek || calendarViewMode == CalendarViewModel.Day || calendarViewMode== CalendarViewModel.MonthWeek)
                    { 
                        dtNodeMax = new DateTime(node.Date.Year, node.Date.Month, node.Date.Day, node.Date.Hour, node.Date.Minute, 59);
                    } 
                    else if(calendarViewMode== CalendarViewModel.TimeSpan)
                    {
                        dtNodeMax = new DateTime(node.Date.Year, node.Date.Month, node.Date.Day, node.Date.Hour, node.Date.Minute, 59);  
                    }
                    if ((taskEvent.StartTime < dtNodeMax && node.Date < taskEvent.EndTime))
                    {
                        taskEvent.RelateNodes.Add(node);
                        node.RelateTasks.Add(taskEvent);
                    } 
                }
            }

        }

		#region Helper Functions

        private int vsize, hsize;
        public override void AdjustScrollbars()
        {
            if((calendarViewMode== CalendarViewModel.Week || calendarViewMode== CalendarViewModel.WorkWeek || calendarViewMode== CalendarViewModel.Day || calendarViewMode== CalendarViewModel.MonthWeek) && nodes.Count>0)
            { 
                allRowsHeight = 0;
                int rowCount = 24*2;
                for (int i = 0; i < rowCount; i++)
                {
                    allRowsHeight += itemheight; // itemheight * ((TreeListNode)nodes[i]).GetVisibleNodeCount(true);  //itemheight * 2 +
                }
                if (allRowsHeight > 0) allRowsHeight += ltHeight;

                vsize = vscrollBar.Width;
              
                vscrollBar.Left = this.ClientRectangle.Left + this.ClientRectangle.Width - vscrollBar.Width - 2;
                vscrollBar.Top = this.ClientRectangle.Top + headerBuffer + 2;
                vscrollBar.Height = this.ClientRectangle.Height - hsize - headerBuffer - 2;
                vscrollBar.Maximum = allRowsHeight;
                vscrollBar.LargeChange = (this.ClientRectangle.Height - headerBuffer - hsize - 4 > 0 ? this.ClientRectangle.Height - headerBuffer - hsize - 4 : 0);
                if (allRowsHeight > this.ClientRectangle.Height - headerBuffer - 4 - hsize)
                {
                    vscrollBar.Show();
                    vsize = vscrollBar.Width;
                }
                else
                {
                    vscrollBar.Hide();
                    vscrollBar.Value = 0;
                    vsize = 0;
                }


                //allColsWidth = 0;
                //for (int i = 0; i < columns.Count; i++)
                //{
                //    allColsWidth += columns[i].Width;
                //}
                //hsize = hscrollBar.Height; 
                //hscrollBar.Left = this.ClientRectangle.Left + 2;
                //hscrollBar.Width = this.ClientRectangle.Width - vsize - 4;
                //hscrollBar.Top = this.ClientRectangle.Top + this.ClientRectangle.Height - hscrollBar.Height - 2;
                //hscrollBar.Maximum = allColsWidth;
                //if (allColsWidth > this.ClientRectangle.Width - 4 - vsize)
                //{
                //    hscrollBar.Show();
                //    hsize = hscrollBar.Height;
                //}
                //else
                //{
                //    hscrollBar.Hide();
                //    hscrollBar.Value = 0;
                //    hsize = 0;
                //} 
                //hscrollBar.Width = this.ClientRectangle.Width - vsize - 4;
                //hscrollBar.LargeChange = (this.ClientRectangle.Width - vsize - 4 > 0 ? this.ClientRectangle.Width - vsize - 4 : 0);
                //hscrollBar.Top = this.ClientRectangle.Top + this.ClientRectangle.Height - hscrollBar.Height - 2;
                //if (allColsWidth > this.ClientRectangle.Width - 4 - vsize)
                //{
                //    hscrollBar.Show();
                //}
                //else
                //{
                //    hscrollBar.Hide();
                //    hscrollBar.Value = 0;
                //    hsize = 0;
                //}
            }
            else
            {

                vscrollBar.Hide();
                vscrollBar.Value = 0;
                vsize = 0;
            }
        }

		private void UnfocusNodes(TreeListNodeCollection nodecol)
		{
			for (int i=0; i<nodecol.Count; i++)
			{
				UnfocusNodes(nodecol[i].Nodes);
				nodecol[i].Focused = false;
			}
		}

		private void UnselectNodes(TreeListNodeCollection nodecol)
		{
			for (int i=0; i<nodecol.Count; i++)
			{
				UnselectNodes(nodecol[i].Nodes);
				nodecol[i].Focused = false;
				nodecol[i].Selected = false;
			}
		}
        private void UnselectTask()
        {
            for (int i = 0; i < taskEventNodes.Count; i++)
            {
                taskEventNodes[i].Selected = false;
            }
        }

        private bool TaskTimeInArea(TaskEventNode taskEvent1,TaskEventNode taskEvent2)
        {
            bool rs = false;
            if((taskEvent1.StartTime<= taskEvent2.StartTime && taskEvent2.StartTime<=taskEvent1.EndTime) ||(taskEvent1.StartTime<= taskEvent2.EndTime && taskEvent2.EndTime<=taskEvent1.EndTime))
            {
                rs = true;
            }
            return rs;
        }
		private TreeListNode NodeInNodeRow(MouseEventArgs e)
		{
			IEnumerator ek = nodeRowRects.Keys.GetEnumerator();
			IEnumerator ev = nodeRowRects.Values.GetEnumerator();

			while (ek.MoveNext() && ev.MoveNext())
			{
				Rectangle r = (Rectangle)ek.Current;

				if (r.Left <= e.X && r.Left+r.Width >= e.X
					&& r.Top <= e.Y && r.Top+r.Height >= e.Y)
				{
					return (TreeListNode)ev.Current;
				}
			}

			return null;
		}

        private TaskEventNode taskInTaskRow(MouseEventArgs e)
        { 
            foreach (TaskEventNode taskEvent in taskEventNodes)
            {
                foreach (Rectangle r in taskEvent.RectAreas)
                {
                    if (r.Left <= e.X && r.Left + r.Width >= e.X
                        && r.Top <= e.Y && r.Top + r.Height >= e.Y)
                    {
                        return taskEvent;
                    }
                }
            }

            return null;
        } 

        private TaskEventNode taskTitleInTaskRow(MouseEventArgs e)
        { 
            foreach (TaskEventNode taskEvent in taskEventNodes)
            {
                foreach (Rectangle r in taskEvent.TitleAreas)
                {
                    if (r.Left <= e.X && r.Left + r.Width >= e.X
                        && r.Top <= e.Y && r.Top + r.Height >= e.Y)
                    {
                        return taskEvent;
                    }
                }
            }
             
            return null; 
        }
        private Rectangle titleRectInTaskRow(MouseEventArgs e)
        {
            foreach (TaskEventNode taskEvent in taskEventNodes)
            {
                foreach (Rectangle r in taskEvent.TitleAreas)
                {
                    if (r.Left <= e.X && r.Left + r.Width >= e.X
                        && r.Top <= e.Y && r.Top + r.Height >= e.Y)
                    {
                        return r;
                    }
                }
            }

            return new Rectangle(0,0,0,0);
        }

        //判断区域内是否有其他任务区域
        private TaskEventNode taskInTaskRow(Rectangle sr)
        {
            foreach (TaskEventNode taskEvent in taskEventNodes)
            {
                foreach (Rectangle r in taskEvent.RectAreas)
                {

                    if (r.Left <= sr.Left+4 && r.Left + r.Width >= sr.Left+4
                        && r.Top <= sr.Top+4 && r.Top + r.Height >= sr.Top+4)
                    {
                        return taskEvent;
                    }
                    if (r.Left <= sr.Right-4 && r.Left + r.Width >= sr.Right-4
                        && r.Top <= sr.Top+4 && r.Top + r.Height >= sr.Top+4)
                    {
                        return taskEvent;
                    }
                    //if (r.Left <= e.X && r.Left + r.Width >= e.X
                    //    && r.Top <= e.Y && r.Top + r.Height >= e.Y)
                    //{
                    //    return taskEvent;
                    //}
                }
            }

            return null;
        } 

		private TreeListNode NodePlusClicked(MouseEventArgs e)
		{
			IEnumerator ek = pmRects.Keys.GetEnumerator();
			IEnumerator ev = pmRects.Values.GetEnumerator();

			while (ek.MoveNext() && ev.MoveNext())
			{
				Rectangle r = (Rectangle)ek.Current;

				if (r.Left <= e.X && r.Left+r.Width >= e.X
					&& r.Top <= e.Y && r.Top+r.Height >= e.Y)
				{
					return (TreeListNode)ev.Current;
				}
			}

			return null;
		}

        private void RenderMonthDateNode(TreeListNode node, Graphics g, Rectangle r, int level)
        { 
            //定位日期所在的位置
            int row =node.Row;  //日期所在行  
            int col = node.Col;  //日期所在列
             
            int lb = lfWidth;
            int hb = headerBuffer;
            int tb = 1;
            Rectangle sr = new Rectangle(r.Left + (int)(itemwidth * col) + lb - hscrollBar.Value, r.Top + itemheight * row + tb + hb - vscrollBar.Value, (int)itemwidth, itemheight);
            if(col == colCount-1 && sr.Right<r.Right)
            {
                sr = new Rectangle(sr.Left, sr.Top, sr.Width + (r.Right - sr.Right), sr.Height);
            }

            nodeRowRects.Add(sr, node);
                     
            g.Clip = new Region(sr); 
            //填充日期区域背景颜色
            g.FillRectangle(new SolidBrush(node.BackColor), sr);
            //g.DrawRectangle(Pens.Black, sr); 
            //绘制日期区域边框
            if(row!=0)
                g.DrawLine(Pens.Black, sr.Left, sr.Top, sr.Right, sr.Top);  
            if(col!=0)
                g.DrawLine(Pens.Black, sr.Left, sr.Top, sr.Left, sr.Bottom); 
            g.DrawLine(Pens.Black, sr.Right, sr.Top, sr.Right, sr.Bottom);
            g.DrawLine(Pens.Black, sr.Left, sr.Bottom, sr.Right, sr.Bottom); 
           

            //绘制日期文本信息
            if (calendarViewMode == CalendarViewModel.Month)
            {
                //绘制选中节点
                if (node.Selected)
                {
                    g.FillRectangle(new SolidBrush(rowSelectColor), sr.Left + 2, sr.Top + 2, sr.Width - 2, 22);
                } 
                Font textFont = new Font("微软雅黑", 10.0f);
                //1. 日期
                string daytext = node.Day.ToString();
                if (node.Day == 1)
                {
                    daytext = node.Month + "月, 1";
                }
                if (node.Month == 1 && node.Day == 1)
                {
                    daytext = node.Year + "年, " + node.Month + "月, 1";
                }
                if (node.IsToday)
                {
                    textFont = new Font("微软雅黑", 10.0f, FontStyle.Bold);
                    daytext = daytext + ", 今日";
                }
                if (node.Selected)
                    g.DrawString(daytext, textFont, rowSelectBrush, (float)(sr.Left + 4 - hscrollBar.Value), (float)(sr.Top + 4 - vscrollBar.Value));
                else
                    g.DrawString(daytext, textFont, new SolidBrush(node.ForeColor), (float)(sr.Left + 4 - hscrollBar.Value), (float)(sr.Top + 4 - vscrollBar.Value));
                //2. 农历日期
                string dayCN = node.ChinesDate.Cday;
                if (dayCN == "初一")
                {
                    dayCN = node.ChinesDate.Cmonth + "月"; // node.ChinesDate.Cday;
                }
                SizeF strSize = TextRenderer.MeasureText(g, dayCN, textFont, new Size(0, 0), TextFormatFlags.NoPadding);
                if (node.Selected)
                    g.DrawString(dayCN, textFont, rowSelectBrush, (float)(sr.Right - strSize.Width - 4 - hscrollBar.Value), (float)(sr.Top + 4 - vscrollBar.Value));
                else
                    g.DrawString(dayCN, textFont, new SolidBrush(node.ForeColor), (float)(sr.Right - strSize.Width - 4 - hscrollBar.Value), (float)(sr.Top + 4 - vscrollBar.Value));

            }
            else if(calendarViewMode== CalendarViewModel.Year)
            {
                if (node.Row == node.Date.Month - 1) //本月
                { 
                    //绘制选中节点
                    if (node.Selected)
                    {
                        g.FillRectangle(new SolidBrush(rowSelectColor), sr.Left + 2, sr.Top + 2, sr.Width - 2, 22);
                    } 
                    Font textFont = new Font("微软雅黑", 9.0f);
                    //1. 日期
                    string daytext = node.Day.ToString();
                    if (node.IsToday)
                    {
                        textFont = new Font("微软雅黑", 9.0f, FontStyle.Bold);
                        //daytext = daytext + "今日";

                    }
                    if (node.Selected)
                        g.DrawString(daytext, textFont, rowSelectBrush, (float)(sr.Left + 4 - hscrollBar.Value), (float)(sr.Top + 4 - vscrollBar.Value));
                    else
                        g.DrawString(daytext, textFont, new SolidBrush(node.ForeColor), (float)(sr.Left + 4 - hscrollBar.Value), (float)(sr.Top + 4 - vscrollBar.Value));


                }
                else
                {

                }

            }
       
        }

        private void RenderWeekDateNode(TreeListNode node, Graphics g, Rectangle r, int level)
        { 
            int row = node.Row;  //日期所在行  
            int col = node.Col;  //日期所在列

            int lb = lfWidth+1;
            int hb = headerBuffer + ltHeight;
            Rectangle sr = new Rectangle(r.Left + (int)(itemwidth * col) + lb - hscrollBar.Value, r.Top + itemheight * row + hb  +2-1- vscrollBar.Value, (int)itemwidth, itemheight);
            if (sr.Top > hb)
            {
                g.Clip = new Region(sr);
            }
            else
            { 
                int d = hb + 1 - sr.Top;
                Rectangle rh = new Rectangle(sr.Left, sr.Top + d, sr.Width, sr.Height - d);
                g.Clip = new Region(rh);
            }

            if(col>5)
            {

            }

            if (sr.Top < hb - 20) return;
            //Rectangle srNode = new Rectangle(sr.Left + 8, sr.Top, sr.Width - 8, sr.Height);
            Rectangle srNode = new Rectangle(sr.Left , sr.Top, sr.Width , sr.Height);
            nodeRowRects.Add(srNode, node);

            //填充日期区域背景颜色
            g.FillRectangle(new SolidBrush(node.BackColor), sr); 
            //绘制日期区域边框

            Pen p = new Pen(Color.SaddleBrown);
            if(row%2==1)
            {
                p = new Pen(Color.NavajoWhite);
            }
            Pen pr = new Pen(Color.LightSteelBlue,1.0f);
            if(node.Week==0  || node.Week==5)
            {
                pr = new Pen(Color.SteelBlue, 1.0f);
            }
            g.FillRectangle(new SolidBrush(Color.White), sr.Left, sr.Top, 8, itemheight); 
            if (row != 0)
                g.DrawLine(p, srNode.Left, srNode.Top, srNode.Right, srNode.Top);  //顶部 
            //g.DrawLine(Pens.Black, srNode.Left, srNode.Top, srNode.Left, srNode.Bottom);  //左侧
            g.DrawLine(pr, srNode.Right - 1, srNode.Top, srNode.Right - 1, srNode.Bottom);   //右侧
            g.DrawLine(p, srNode.Left, srNode.Bottom, srNode.Right, srNode.Bottom);
            //绘制选中节点
            if (node.Selected)
            {
                g.FillRectangle(new SolidBrush(rowSelectColor), srNode.Left+1,srNode.Top,srNode.Width-2,srNode.Height);
            }
            else
            {
                //ControlPaint.DrawFocusRectangle(g, sr); 
            } 
        }

        private void RenderTimeSpanDateNode(TreeListNode node, Graphics g, Rectangle r, int level)
        {
            int row = node.Row;  //日期所在行  
            int col = node.Col;  //日期所在列

            int lb = lfWidth;
            int hb = headerBuffer+ltHeight;
            int tb = 0;
            Rectangle sr = new Rectangle(r.Left + (int)(itemwidth * col) + lb - hscrollBar.Value, r.Top + itemheight * row + tb + hb - vscrollBar.Value, (int)itemwidth, itemheight);
            if (col == colCount - 1 && sr.Right < r.Right)
            {
                sr = new Rectangle(sr.Left, sr.Top, sr.Width + (r.Right - sr.Right), sr.Height);
            }

            nodeRowRects.Add(sr, node);

            g.Clip = new Region(sr);
            //填充日期区域背景颜色
            g.FillRectangle(new SolidBrush(node.BackColor), sr);
            if (node.Date.Hour == 23 && node.Date.Minute == 30)
            {
                g.DrawLine(Pens.SteelBlue, sr.Right - 1, sr.Top, sr.Right - 1, sr.Bottom);
            }
             
            //绘制日期区域边框
            if (row != 0)
                g.DrawLine(Pens.SteelBlue, sr.Left, sr.Top, sr.Right, sr.Top);
            if (col != 0)
                g.DrawLine(Pens.SteelBlue, sr.Left, sr.Top, sr.Left, sr.Bottom);
            g.DrawLine(Pens.SteelBlue, sr.Right, sr.Top, sr.Right, sr.Bottom);
            g.DrawLine(Pens.SteelBlue, sr.Left, sr.Bottom - 1, sr.Right, sr.Bottom - 1);
          

            //绘制选中节点
            if (node.Selected)
            {
                g.FillRectangle(new SolidBrush(rowSelectColor), sr.Left, sr.Top, sr.Width, sr.Height-2);
            } 

        }

        private void RenderWeekLeftInfo(Graphics g, Rectangle r, int level)
        {
            int lb = lfWidth+2;
            if(ltHeight>0)
            {
                Rectangle srArea = new Rectangle(r.Left + 0 - hscrollBar.Value, r.Top + headerBuffer + 1, lb, ltHeight);
                g.Clip = new Region(srArea);

                //填充左边空白区域背景颜色
                g.FillRectangle(new SolidBrush(Color.FromArgb(237, 246, 245)), srArea);
                g.DrawLine(Pens.LightSteelBlue, srArea.Left, srArea.Bottom-2, srArea.Right - 2, srArea.Bottom-2); 
                g.DrawLine(Pens.LightSteelBlue, srArea.Right - 2, srArea.Top, srArea.Right - 2, srArea.Bottom);

                Rectangle srAreaTask = new Rectangle(srArea.Left+srArea.Width-1, srArea.Top, r.Width - srArea.Width-vsize-1, srArea.Height);
                g.Clip = new Region(srAreaTask);

                //填充跨区域背景颜色
                g.FillRectangle(new SolidBrush(Color.FromArgb(207, 226, 225)), srAreaTask);
                g.DrawLine(Pens.LightSteelBlue, srAreaTask.Left, srAreaTask.Bottom - 2, srAreaTask.Right - 2, srAreaTask.Bottom - 2); 
                
                //绘制日期线条
                int left = srAreaTask.Left-1;
                for (int i = 0; i < level; i++)
                {
                    TreeListNode node = nodes[i * 48];
                    if(node.Week==6 || node.Week==0 || node.IsHoliady)
                    {
                        g.FillRectangle(new SolidBrush(Color.LightYellow),left+1,srAreaTask.Top+1,itemwidth-1,srAreaTask.Height-2-1);

                    }

                    Pen pr = new Pen(Color.LightSteelBlue, 1.0f);
                    if (node.Week == 0 || node.Week == 5)
                    {
                        pr = new Pen(Color.SteelBlue, 1.0f);
                    }
                    left += itemwidth;
                    g.DrawLine(pr,left, srAreaTask.Top +2 , left, srAreaTask.Bottom - 2);
                    
                    
                }
            }
            for(int i=0;i<24;i++)
            {
                int hb = headerBuffer+ ltHeight;;
                Rectangle sr = new Rectangle(r.Left + 0 - hscrollBar.Value, r.Top + itemheight * 2 * i + hb +1- vscrollBar.Value, lb, itemheight * 2);
                if (sr.Top > hb)
                {
                    g.Clip = new Region(sr);
                }
                else
                {
                    int d = hb+1 - sr.Top;
                    Rectangle rh = new Rectangle(sr.Left, sr.Top + d, sr.Width, sr.Height - d);
                    g.Clip = new Region(rh);
                }

                if (sr.Top < hb - 22) continue;

                //填充日期区域背景颜色
                g.FillRectangle(new SolidBrush(Color.FromArgb(237, 246, 245)), sr); 
                if(i!=0)
                    g.DrawLine(Pens.LightSteelBlue, sr.Left, sr.Top, sr.Right-6, sr.Top);
                g.DrawLine(Pens.LightSteelBlue, sr.Left + lb / 2, sr.Top + itemheight, sr.Right-6, sr.Top + itemheight);
                g.DrawLine(Pens.LightSteelBlue, sr.Right - 2, sr.Top, sr.Right - 2, sr.Bottom);
                //g.DrawLine(Pens.LightSteelBlue, sr.Left, sr.Bottom, sr.Right-6, sr.Bottom); 

                //绘制日期文本信息
                Font textFont = new Font("微软雅黑", 14.0f);
                Pen p=new Pen(Color.Black);
                string sp = i < 10 ? "0" + i : i.ToString();
                g.DrawString(sp, textFont,new SolidBrush(Color.Black), (float)(sr.Left + 4), (float)(sr.Top + 4 + 2));

                textFont = new System.Drawing.Font("微软雅黑", 9.0f);
                g.DrawString("30", textFont, new SolidBrush(Color.Black), (float)(sr.Left + lb / 2 + 2 ), (float)(sr.Top + 4));
                 
            }


        }
        private void RenderYearLeftInfo(Graphics g, Rectangle r, int level)
        { 
            for (int i = 0; i < 12; i++)
            {
                int lb = lfWidth;
                int hb = headerBuffer + ltHeight; ;
                Rectangle sr = new Rectangle(r.Left + 0 - hscrollBar.Value, r.Top + itemheight*i+1 + hb - vscrollBar.Value, lb+2, itemheight);
                if (sr.Top > hb)
                {
                    g.Clip = new Region(sr);
                }
                else
                {
                    int d = hb + 1 - sr.Top;
                    Rectangle rh = new Rectangle(sr.Left, sr.Top + d, sr.Width, sr.Height - d);
                    g.Clip = new Region(rh);
                }

                if (sr.Top < hb - 22) continue;

                //填充日期区域背景颜色
                g.FillRectangle(new SolidBrush(Color.FromArgb(237, 246, 245)), sr);
                if (i != 0)
                    g.DrawLine(Pens.LightSteelBlue, sr.Left, sr.Top, sr.Right - 2, sr.Top);
                //g.DrawLine(Pens.LightSteelBlue, sr.Left + lb / 2, sr.Top + itemheight, sr.Right - 6, sr.Top + itemheight);
                g.DrawLine(Pens.LightSteelBlue, sr.Right - 2, sr.Top, sr.Right - 2, sr.Bottom);
                //g.DrawLine(Pens.LightSteelBlue, sr.Left, sr.Bottom, sr.Right-6, sr.Bottom); 

                //绘制日期文本信息
                Font textFont = new Font("微软雅黑", 14.0f);
                Pen p = new Pen(Color.Black);
                string sp = (i+1) + "月";
                g.DrawString(sp, textFont, new SolidBrush(Color.Black), (float)(sr.Left -2+ (sr.Width / 2) - (g.MeasureString(sp, textFont).Width / 2)), (float)(sr.Top + (sr.Height / 2) - (g.MeasureString(sp, textFont).Height / 2)));
                 

            }


        }

        private void RenderTimeSpanTopInfo(Graphics g, Rectangle r, int level)
        {
            DateTime dtFirstNode = nodes[0].Date;
            int startColIndex = 0;
            int i = 0;
            int last = 0;

            int lb = lfWidth;
            int hb = headerBuffer + ltHeight;
            for (i = 0; i < nodes.Count; i++)
            {
                if (!(nodes[i].Date.Year == dtFirstNode.Year && nodes[i].Date.Month == dtFirstNode.Month && nodes[i].Date.Day == dtFirstNode.Day) || i == nodes.Count - 1)
                {
                    int width = (int)(itemwidth * (i - startColIndex));

                    g.Clip = new Region(new Rectangle(r.Left, r.Top + headerBuffer, r.Width, ltHeight));
                    //绘制新的日期标题
                    Rectangle rectTime = new Rectangle(lb + last, r.Top + headerBuffer, width, 20);

                    Rectangle rectSpan = new Rectangle(lb + last, r.Top + headerBuffer+20, width, ltHeight-20);

                    g.FillRectangle(new SolidBrush(Color.FromArgb(197, 216, 245)), rectTime);
                    g.DrawRectangle(Pens.SteelBlue, rectTime.Left, rectTime.Top, rectTime.Width-1, rectTime.Height);

                    g.FillRectangle(new SolidBrush(Color.FromArgb(237, 246, 245)), rectSpan);
                    g.DrawRectangle(Pens.SteelBlue, rectSpan.Left, rectSpan.Top, rectSpan.Width - 1, rectSpan.Height - 1);


                    //绘制时间
                    Font textFont = new Font("微软雅黑", 9.0f);
                    for (int m = startColIndex; m <= i; m+=2)
                    {
                        int ml = rectTime.Left + (int)(itemwidth * (m - startColIndex));

                        string sp = nodes[m].Date.ToString("HH:mm");
                        g.DrawLine(Pens.SteelBlue, ml, rectTime.Top, ml, rectTime.Top + rectTime.Height);

                        g.DrawString(sp, textFont, new SolidBrush(Color.Black),(float)ml, (float)rectTime.Top+2);


                    }
                    //绘制标尺
                    for (int m = startColIndex; m <= i;m++ )
                    {
                        int ml = rectSpan.Left + (int)(itemwidth * (m - startColIndex));
                        g.DrawLine(Pens.SteelBlue, ml, rectSpan.Top, ml, rectSpan.Top + rectSpan.Height);
                    }

                        last += width;



                    dtFirstNode = nodes[i].Date;
                    startColIndex = i;
                }
                else
                {



                }
            } 

            //for (int i = 0; i < 12; i++)
            //{
            //    int lb = lfWidth;
            //    int hb = headerBuffer + ltHeight; ;
            //    Rectangle sr = new Rectangle(r.Left + 0 - hscrollBar.Value, r.Top + itemheight * i + 1 + hb - vscrollBar.Value, lb + 2, itemheight);
            //    if (sr.Top > hb)
            //    {
            //        g.Clip = new Region(sr);
            //    }
            //    else
            //    {
            //        int d = hb + 1 - sr.Top;
            //        Rectangle rh = new Rectangle(sr.Left, sr.Top + d, sr.Width, sr.Height - d);
            //        g.Clip = new Region(rh);
            //    }

            //    if (sr.Top < hb - 22) continue;

            //    //填充日期区域背景颜色
            //    g.FillRectangle(new SolidBrush(Color.FromArgb(237, 246, 245)), sr);
            //    if (i != 0)
            //        g.DrawLine(Pens.LightSteelBlue, sr.Left, sr.Top, sr.Right - 2, sr.Top);
            //    //g.DrawLine(Pens.LightSteelBlue, sr.Left + lb / 2, sr.Top + itemheight, sr.Right - 6, sr.Top + itemheight);
            //    g.DrawLine(Pens.LightSteelBlue, sr.Right - 2, sr.Top, sr.Right - 2, sr.Bottom);
            //    //g.DrawLine(Pens.LightSteelBlue, sr.Left, sr.Bottom, sr.Right-6, sr.Bottom); 

            //    //绘制日期文本信息
            //    Font textFont = new Font("微软雅黑", 14.0f);
            //    Pen p = new Pen(Color.Black);
            //    string sp = (i + 1) + "月";
            //    g.DrawString(sp, textFont, new SolidBrush(Color.Black), (float)(sr.Left - 2 + (sr.Width / 2) - (g.MeasureString(sp, textFont).Width / 2)), (float)(sr.Top + (sr.Height / 2) - (g.MeasureString(sp, textFont).Height / 2)));


            //}


        }
        private void RenderMonthTaskEventNode(TaskEventNode taskEvent, Graphics g, Rectangle r, int level)
        {
            taskEvent.RectAreas.Clear();
            taskEvent.TitleAreas.Clear();

            Font textFont = Font;
            int hb = headerBuffer;
            int lb = lfWidth;
            //绘制事件信息

            textFont = new System.Drawing.Font("微软雅黑", 8.0f);
            int lh = 22;
            if (calendarViewMode == CalendarViewModel.Month)
                taskHeight = 20;
            else if (calendarViewMode == CalendarViewModel.Year)
                taskHeight = 16;
            // 没有关联的日期显示在当前区域，不绘制
            if (taskEvent.RelateNodes.Count == 0) return;

            //1.只关联一个日期区域，绘制在当前日期内
            if (taskEvent.RelateNodes.Count == 1)
            {
                //任务事件绘制区域
                int row = taskEvent.RelateNodes[0].Row;  //日期所在行  
                int col = taskEvent.RelateNodes[0].Col;  //日期所在列

                //日期绘制区域
                Rectangle sr = new Rectangle(r.Left + (int)(itemwidth * col) + lb - hscrollBar.Value, r.Top + itemheight * row + hb + 2 - vscrollBar.Value, (int)itemwidth, itemheight);
                g.Clip = new Region(sr); 

                Rectangle rcTask = new Rectangle(sr.Left + 4 - hscrollBar.Value, sr.Top + lh + 4 - vscrollBar.Value, sr.Width-4, taskHeight);


                while (taskInTaskRow(rcTask)!=null)
                {
                    lh += taskHeight;
                    rcTask = new Rectangle(rcTask.Left, sr.Top + lh + 4 - vscrollBar.Value, rcTask.Width, taskHeight);
                    if (rcTask.Top > sr.Top + sr.Height)
                    {
                        break;
                    }
                }  
                taskEvent.RectAreas.Add(rcTask);

                if(calendarViewMode== CalendarViewModel.Month)
                {
                    string ts = taskEvent.StartTime.ToString("HH:mm") + " - " + taskEvent.EndTime.ToString("HH:mm") + " ";
                    SizeF tsSize = g.MeasureString(ts, textFont);
                    int tsWidth = ((int)tsSize.Width);
                    //任务事件标题绘制区域 
                    Rectangle rcTaskTitle = new Rectangle(rcTask.Left + tsWidth, rcTask.Top, rcTask.Width - tsWidth + 2 + 2, rcTask.Height);
                    taskEvent.TitleAreas.Add(rcTaskTitle);


                    string sp = TruncatedString(taskEvent.Title, rcTaskTitle.Width, 5, g);

                    if (taskEvent.Selected == true)
                    {
                        g.FillRectangle(new SolidBrush(taskEventSelectColor), rcTask.Left + 2, rcTask.Top, rcTask.Width - 2, 20);
                        g.DrawString(ts, textFont, taskEventSelectBrush, (float)(rcTask.Left), (float)(rcTask.Top + 3));
                        g.DrawString(sp, textFont, taskEventSelectBrush, (float)(rcTaskTitle.Left), (float)(rcTaskTitle.Top + 3));

                    }
                    else
                    {
                        g.DrawString(ts, textFont, new SolidBrush(taskEvent.ForeColor), (float)(rcTask.Left), (float)(rcTask.Top + 3));
                        g.DrawString(sp, textFont, new SolidBrush(taskEvent.ForeColor), (float)(rcTaskTitle.Left), (float)(rcTaskTitle.Top + 3));
                    }
                }
                else if(calendarViewMode== CalendarViewModel.Year)
                {
                    int tsWidth = 0;
                    //任务事件标题绘制区域
                    Rectangle rcTaskTitle = new Rectangle(rcTask.Left + tsWidth, rcTask.Top, rcTask.Width - tsWidth + 2 + 2, rcTask.Height);
                    taskEvent.TitleAreas.Add(rcTaskTitle);
                     
                    string sp = TruncatedString(taskEvent.Title, rcTaskTitle.Width, 5, g);

                    if (taskEvent.Selected == true)
                    {
                        g.FillRectangle(new SolidBrush(taskEventSelectColor), rcTask.Left + 2, rcTask.Top, rcTask.Width - 2, 20);
                        //g.DrawString(ts, textFont, taskEventSelectBrush, (float)(rcTask.Left), (float)(rcTask.Top + 3));
                        g.DrawString(sp, textFont, taskEventSelectBrush, (float)(rcTaskTitle.Left), (float)(rcTaskTitle.Top + 3));

                    }
                    else
                    {
                        //g.DrawString(ts, textFont, new SolidBrush(taskEvent.ForeColor), (float)(rcTask.Left), (float)(rcTask.Top + 3));
                        g.DrawString(sp, textFont, new SolidBrush(taskEvent.ForeColor), (float)(rcTaskTitle.Left), (float)(rcTaskTitle.Top + 3));
                    }

                } 
            }
            else  //关联多个绘制区域
            { 
                int firstRow = taskEvent.RelateNodes[0].Row;
                 
                int row = taskEvent.RelateNodes[0].Row;
                int col = taskEvent.RelateNodes[0].Col;

                List<Rectangle> rectRows = new List<Rectangle>();
                //日期绘制区域 
                Rectangle sr = new Rectangle(r.Left + (int)(itemwidth * col) + lb - hscrollBar.Value, r.Top + itemheight * row + hb + 2 - vscrollBar.Value, (int)itemwidth, itemheight);
                Rectangle rcTask = new Rectangle(sr.Left - hscrollBar.Value, sr.Top + lh + 4 - vscrollBar.Value, sr.Width, taskHeight);
                rectRows.Add(rcTask);

                for(int i=1;i<taskEvent.RelateNodes.Count;i++)
                {
                    row = taskEvent.RelateNodes[i].Row;
                    col = taskEvent.RelateNodes[i].Col;

                    if (row == firstRow)
                    {
                        //增加宽度
                        Rectangle rr = rectRows[rectRows.Count - 1];
                        rr = new Rectangle(rr.Left, rr.Top, rr.Width + (int)itemwidth, rr.Height);
                        rectRows[rectRows.Count - 1] = rr;
                    }
                    else  //另一行显示
                    {
                        Rectangle sr2 = new Rectangle(r.Left + (int)(itemwidth * col) - hscrollBar.Value, r.Top + itemheight * row + hb + 2 - vscrollBar.Value, (int)itemwidth, itemheight);
                        Rectangle rcTask2 = new Rectangle(sr2.Left +lb- hscrollBar.Value, sr2.Top + lh + 4 - vscrollBar.Value, sr2.Width, taskHeight);
                        rectRows.Add(rcTask2);
                        firstRow = row;
                    }  
                }

                for(int i=0;i< rectRows.Count;i++)
                {
                    lh = 0;
                    Rectangle rcTaskRow = rectRows[i];
                    while (taskInTaskRow(rcTaskRow) != null)
                    {
                        lh += taskHeight;
                        rcTaskRow = new Rectangle(rcTaskRow.Left, rcTaskRow.Top + lh, rcTaskRow.Width, taskHeight);
                        if (rcTaskRow.Height > itemheight)
                        {
                            break;
                        }
                    }
                    taskEvent.RectAreas.Add(rcTaskRow);
                }


                //绘制任务显示区域
                for (int i = 0; i < taskEvent.RectAreas.Count; i++)
                { 
                    Rectangle rectTask = taskEvent.RectAreas[i];
                    g.Clip = new Region(rectTask);
                    //绘制区域
                    if (taskEvent.Selected == true)
                    {
                        g.FillRectangle(new SolidBrush(taskEventSelectColor), rectTask.Left + 2, rectTask.Top, rectTask.Width - 2, 20);  
                    }
                    else
                    {
                        g.FillRectangle(new SolidBrush(Color.White), rectTask.Left + 2, rectTask.Top, rectTask.Width - 2, 20); 
                    }
                   
                    //绘制多个区域
                    if(i==0)  //开始行
                    {
                        string ts = taskEvent.StartTime.ToString("HH:mm") + " ";
                        SizeF tsSize = g.MeasureString(ts, textFont);
                        int tsWidth = ((int)tsSize.Width);
                        int rightWidth = taskEvent.RectAreas.Count == 1 ? tsWidth : 0;
                        //任务事件标题绘制区域
                        Rectangle rcTaskTitle = new Rectangle(rectTask.Left + tsWidth, rectTask.Top, rectTask.Width - tsWidth - rightWidth, rectTask.Height); 
                        taskEvent.TitleAreas.Add(rcTaskTitle);

                        string sp = TruncatedString(taskEvent.Title, rectTask.Width - tsWidth, 5, g);
                        if (taskEvent.Selected == true)
                        { 
                            g.DrawString(ts, textFont, taskEventSelectBrush, (float)(rectTask.Left), (float)(rectTask.Top + 3));
                            //居中绘制
                            g.DrawString(sp, textFont, taskEventSelectBrush, (float)(rcTaskTitle.Left + rcTaskTitle.Width / 2 - (Helpers.StringTools.MeasureDisplayStringWidth(g, sp, this.Font) / 2)), (float)(rcTaskTitle.Top + 3));
                        }
                        else
                        { 
                            g.DrawString(ts, textFont, new SolidBrush(taskEvent.ForeColor), (float)(rectTask.Left), (float)(rectTask.Top + 3)); 
                            //居中绘制
                            g.DrawString(sp, textFont, new SolidBrush(taskEvent.ForeColor), (float)(rcTaskTitle.Left + rcTaskTitle.Width / 2 - (Helpers.StringTools.MeasureDisplayStringWidth(g, sp, this.Font) / 2)), (float)(rcTaskTitle.Top + 3));

                        }
                    }
                    if (i == taskEvent.RectAreas.Count - 1)  //完成行
                    {
                        string ts = taskEvent.EndTime.ToString("HH:mm") + " ";
                        SizeF tsSize = g.MeasureString(ts, textFont);
                        int tsWidth = ((int)tsSize.Width);

                        if (taskEvent.Selected == true)
                        {
                            g.DrawString(ts, textFont, taskEventSelectBrush, (float)(rectTask.Right - tsWidth - 2 - hscrollBar.Value), (float)(rectTask.Top + 3 - vscrollBar.Value));
                        }
                        else
                        { 
                            g.DrawString(ts, textFont, new SolidBrush(taskEvent.ForeColor), (float)(rectTask.Right - tsWidth - 2 - hscrollBar.Value), (float)(rectTask.Top + 3 - vscrollBar.Value));
                        }
                        if (i != 0)
                        {
                            //任务事件标题绘制区域
                            Rectangle rcTaskTitle = new Rectangle(rectTask.Left, rectTask.Top, rectTask.Width - tsWidth, rectTask.Height);
                            string sp = TruncatedString(taskEvent.Title, rectTask.Width - tsWidth, 5, g);

                            taskEvent.TitleAreas.Add(rcTaskTitle); 
                            if (taskEvent.Selected == true)
                            {
                                taskEvent.TitleAreas.Add(rcTaskTitle);
                                g.DrawString(sp, textFont, taskEventSelectBrush, (float)(rcTaskTitle.Left + rcTaskTitle.Width / 2 - (Helpers.StringTools.MeasureDisplayStringWidth(g, sp, this.Font) / 2)), (float)(rcTaskTitle.Top + 3));
                            }
                            else
                            {
                                taskEvent.TitleAreas.Add(rcTaskTitle);
                                g.DrawString(sp, textFont, new SolidBrush(taskEvent.ForeColor), (float)(rcTaskTitle.Left + rcTaskTitle.Width / 2 - (Helpers.StringTools.MeasureDisplayStringWidth(g, sp, this.Font) / 2)), (float)(rcTaskTitle.Top + 3));
                            }
                        }
                    }
                    if(i!= 0 && i!= taskEvent.RectAreas.Count - 1) //中间区域
                    {
                        //string ts = taskEvent.EndTime.ToString("hh:mm") + " ";
                        //SizeF tsSize = g.MeasureString(ts, textFont);
                        int tsWidth = 0;
                        //任务事件标题绘制区域
                        Rectangle rcTaskTitle = new Rectangle(rectTask.Left, rectTask.Top, rectTask.Width - tsWidth, rectTask.Height);
                        taskEvent.TitleAreas.Add(rcTaskTitle);
                        string sp = TruncatedString(taskEvent.Title, rectTask.Width - tsWidth, 5, g);


                        if (taskEvent.Selected == true)
                        {
                            if (i != 0)
                                g.DrawString(sp, textFont, taskEventSelectBrush, (float)(rcTaskTitle.Left + rcTaskTitle.Width / 2 - (Helpers.StringTools.MeasureDisplayStringWidth(g, sp, this.Font) / 2)), (float)(rcTaskTitle.Top + 3));

                        }
                        else
                        {
                            if (i != 0)
                                g.DrawString(sp, textFont, new SolidBrush(taskEvent.ForeColor), (float)(rcTaskTitle.Left + rcTaskTitle.Width / 2 - (Helpers.StringTools.MeasureDisplayStringWidth(g, sp, this.Font) / 2)), (float)(rcTaskTitle.Top + 3));

                        }  

                    }  
                } 
            } 
        }

        private void RenderWeekTaskEventNode(TaskEventNode taskEvent, Graphics g, Rectangle r, int level)
        {
            taskEvent.RectAreas.Clear();
            taskEvent.TitleAreas.Clear();

            Font textFont = Font;
            int hb = headerBuffer + ltHeight;
            int lb = lfWidth;
            //绘制事件信息

            textFont = new System.Drawing.Font("微软雅黑", 8.0f); 
            // 没有关联的日期显示在当前区域，不绘制
            if (taskEvent.RelateNodes.Count == 0) return;

            //任务事件跨天数量
            int dayCount = 1;
            for (int i = 1; i < taskEvent.RelateNodes.Count; i++)
            {
                //是否绘制开始时间   
                if (taskEvent.RelateNodes[i].Date.Year == taskEvent.RelateNodes[i - 1].Date.Year
                   && taskEvent.RelateNodes[i].Date.Month == taskEvent.RelateNodes[i - 1].Date.Month
                   && taskEvent.RelateNodes[i].Date.Day == taskEvent.RelateNodes[i - 1].Date.Day)
                {
                    continue;
                }
                else
                {
                    dayCount++;
                }
            } 

            //任务是否有节点不再当前区域
            bool isInNode = true;
            if (taskEvent.RelateNodes[0].Date > taskEvent.StartTime)
            {
                isInNode = false;
            }

            DateTime dtMin = new DateTime(taskEvent.EndTime.Year, taskEvent.EndTime.Month, taskEvent.EndTime.Day, 0, 0, 0);
            if (taskEvent.RelateNodes[taskEvent.RelateNodes.Count - 1].Date < dtMin)
            {
                isInNode = false;
            }  
            //1.只关联一个日期区域，绘制在当前日期内
            if (isInNode && dayCount==1)
            { 
                //任务事件绘制区域
                int row = taskEvent.RelateNodes[0].Row;  //开始区域所在行  
                int col = taskEvent.RelateNodes[0].Col;  //开始区域所在列 
                int rowCount = taskEvent.RelateNodes.Count;

                int subCol = taskEvent.ColSubIndex;
                int subCount = taskEvent.ColSubCount > 0 ? taskEvent.ColSubCount : 1;
                int taskWidth = (int)((itemwidth - 4 * subCount) / subCount);
                int lp = subCol == 0 ? 0 : 4;
                Rectangle sr = new Rectangle(r.Left + (int)(itemwidth * col) + taskWidth * subCol + subCol * 4 + lb - hscrollBar.Value, r.Top + itemheight * row + hb + 2 - vscrollBar.Value, taskWidth, itemheight * rowCount + 1);
                if (sr.Top > hb)
                {
                    g.Clip = new Region(sr);
                }
                else
                {
                    int d = hb + 1 - sr.Top;
                    Rectangle rh = new Rectangle(sr.Left, sr.Top + d, sr.Width, sr.Height - d);
                    g.Clip = new Region(rh);
                } 

                taskEvent.RectAreas.Add(sr);

                g.FillRectangle(new SolidBrush(Color.Blue), sr.Left, sr.Top, 8, sr.Height);

                Rectangle rcTask = new Rectangle(sr.Left + 8, sr.Top, sr.Width - 8, sr.Height);

                taskEvent.TitleAreas.Add(rcTask);


                if (taskEvent.Selected == true)
                {
                    g.FillRectangle(new SolidBrush(taskEventSelectColor), rcTask.Left, rcTask.Top, rcTask.Width, rcTask.Height);
                    g.DrawRectangle(Pens.Black, rcTask.Left, rcTask.Top, rcTask.Width - 1, rcTask.Height - 1);


                    //任务事件标题绘制区域 
                    string sp = TruncatedString(taskEvent.Title, rcTask.Width, 5, g);
                    g.DrawString(sp, textFont, taskEventSelectBrush, (float)(rcTask.Left + 4), (float)(rcTask.Top + 4));
                }
                else
                {

                    g.FillRectangle(new SolidBrush(Color.White), rcTask.Left, rcTask.Top, rcTask.Width, rcTask.Height);
                    g.DrawRectangle(Pens.Black, rcTask.Left, rcTask.Top, rcTask.Width - 1, rcTask.Height - 1);

                    //任务事件标题绘制区域 
                    string sp = TruncatedString(taskEvent.Title, rcTask.Width, 5, g);
                    g.DrawString(sp, textFont, new SolidBrush(taskEvent.ForeColor), (float)(rcTask.Left + 4), (float)(rcTask.Top + 4));
                }  
            }
            else  //关联多个绘制区域
            { 
                int row = taskEvent.AreaIndex-1;  //使用区域个数下标
                int col = taskEvent.RelateNodes[0].Col;
                //日期绘制区域 
                Rectangle sr = new Rectangle(r.Left + (int)(itemwidth * col) + lb - hscrollBar.Value, r.Top + headerBuffer + 4 + row * (itemheight + 5), (int)(itemwidth * dayCount), itemheight);
                
                if(sr.Left<0)
                {
                    sr = new Rectangle(0, sr.Top, sr.Width - (0 - sr.Left), sr.Height);
                }
                if(sr.Right>r.Right-vsize)
                {
                    sr = new Rectangle(sr.Left, sr.Top, sr.Width - (sr.Right - r.Right)-vsize-4, sr.Height);
                }
                g.Clip = new Region(sr);
                taskEvent.RectAreas.Add(sr);

                if (taskEvent.Selected == true)
                { 
                    g.FillRectangle(new SolidBrush(taskEventSelectColor), sr.Left, sr.Top, sr.Width, sr.Height);
                    g.DrawRectangle(Pens.Black, sr.Left, sr.Top, sr.Width - 1, sr.Height - 1);
                }
                else
                {
                    g.FillRectangle(new SolidBrush(Color.White), sr.Left, sr.Top, sr.Width, sr.Height);
                    g.DrawRectangle(Pens.Black, sr.Left, sr.Top, sr.Width - 1, sr.Height - 1);
                }


                bool isDrawStart = false;
                bool isDrawEnd = false; 
                //是否绘制开始时间
                if (taskEvent.RelateNodes[0].Date <= taskEvent.StartTime)
                {
                    isDrawStart = true;
                }
                //是否绘制结束时间
                DateTime dtMinTaskEnd = new DateTime(taskEvent.EndTime.Year, taskEvent.EndTime.Month, taskEvent.EndTime.Day, 0, 0, 0);
                if (taskEvent.RelateNodes[taskEvent.RelateNodes.Count - 1].Date >= dtMinTaskEnd)
                {
                    isDrawEnd = true;
                } 

                string ts = taskEvent.StartTime.ToString("HH:mm") + " ";
                SizeF tsSize = g.MeasureString(ts, textFont);
                int tsWidth = ((int)tsSize.Width);

                int lf = isDrawStart ? tsWidth : 0;
                int lw = isDrawStart ? tsWidth : 0;
                lw = lw + (isDrawEnd ? tsWidth : 0);
                //任务事件标题绘制区域
                Rectangle rcTaskTitle = new Rectangle(sr.Left + lf, sr.Top, sr.Width - lw, sr.Height);
                taskEvent.TitleAreas.Add(rcTaskTitle);

                string sp = TruncatedString(taskEvent.Title, rcTaskTitle.Width, 5, g);
                if (taskEvent.Selected == true)
                {  
                    if (isDrawStart)
                        g.DrawString(ts, textFont, taskEventSelectBrush, (float)(sr.Left), (float)(sr.Top + 3));
                    //居中绘制
                    g.DrawString(sp, textFont, taskEventSelectBrush, (float)(rcTaskTitle.Left + rcTaskTitle.Width / 2 - (Helpers.StringTools.MeasureDisplayStringWidth(g, sp, this.Font) / 2)), (float)(rcTaskTitle.Top + 3));
                    //绘制结束时间
                    if (isDrawEnd)
                    {
                        ts = taskEvent.EndTime.ToString("HH:mm") + " ";
                        g.DrawString(ts, textFont, taskEventSelectBrush, (float)(sr.Right - tsWidth - 2), (float)(sr.Top + 3));
                    }
                }
                else
                { 
                    //绘制开始时间
                    if (isDrawStart)
                        g.DrawString(ts, textFont, new SolidBrush(taskEvent.ForeColor), (float)(sr.Left), (float)(sr.Top + 3));
                    //居中绘制
                    g.DrawString(sp, textFont, new SolidBrush(taskEvent.ForeColor), (float)(rcTaskTitle.Left + rcTaskTitle.Width / 2 - (Helpers.StringTools.MeasureDisplayStringWidth(g, sp, this.Font) / 2)), (float)(rcTaskTitle.Top + 3));
                     
                    if (isDrawEnd)
                    {
                        ts = taskEvent.EndTime.ToString("HH:mm") + " ";
                        g.DrawString(ts, textFont, new SolidBrush(taskEvent.ForeColor), (float)(sr.Right - tsWidth - 2), (float)(sr.Top + 3));
                    }
                }
            }
        }


        private void RenderTimeSpanTaskEventNode(TaskEventNode taskEvent, Graphics g, Rectangle r, int level)
        {
            taskEvent.RectAreas.Clear();
            taskEvent.TitleAreas.Clear();

            Font textFont = Font;
            int hb = headerBuffer + ltHeight;
            int lb = lfWidth;
            //绘制事件信息

            textFont = new System.Drawing.Font("微软雅黑", 8.0f);
            int lh = 0;
            taskHeight = 20;
            // 没有关联的日期显示在当前区域，不绘制
            if (taskEvent.RelateNodes.Count == 0) return;


            int firstRow = taskEvent.RelateNodes[0].Row;

            int row = taskEvent.RelateNodes[0].Row;
            int col = taskEvent.RelateNodes[0].Col;

            List<Rectangle> rectRows = new List<Rectangle>();
            //日期绘制区域 
            Rectangle sr = new Rectangle(r.Left + (int)(itemwidth * col) + lb - hscrollBar.Value, r.Top + itemheight * row + hb + 2 - vscrollBar.Value, (int)itemwidth, itemheight);
            Rectangle rcTask = new Rectangle(sr.Left, sr.Top + lh + 4, sr.Width, taskHeight);
            rectRows.Add(rcTask);

            for (int i = 1; i < taskEvent.RelateNodes.Count; i++)
            {
                row = taskEvent.RelateNodes[i].Row;
                col = taskEvent.RelateNodes[i].Col;

                if (row == firstRow)
                {
                    //增加宽度
                    Rectangle rr = rectRows[rectRows.Count - 1];
                    rr = new Rectangle(rr.Left, rr.Top, rr.Width + (int)itemwidth, rr.Height);
                    rectRows[rectRows.Count - 1] = rr;
                }
            }

            lh = 0;
            //绘制任务事件区域
            Rectangle rcTaskRow = rectRows[0];
            while (taskInTaskRow(rcTaskRow) != null)
            {
                lh += taskHeight+2;
                rcTaskRow = new Rectangle(rcTaskRow.Left, rcTaskRow.Top + lh, rcTaskRow.Width, taskHeight);
                if (rcTaskRow.Height > itemheight)
                {
                    break;
                }
            }
            taskEvent.RectAreas.Add(rcTaskRow);

            Rectangle rectTask = taskEvent.RectAreas[0];
            g.Clip = new Region(rectTask);
            //绘制区域
            if (taskEvent.Selected == true)
            {
                g.FillRectangle(new SolidBrush(taskEventSelectColor), rectTask.Left, rectTask.Top, rectTask.Width, taskHeight);
                g.DrawRectangle(Pens.Black, rectTask.Left, rectTask.Top, rectTask.Width - 2, taskHeight - 1);
            }
            else
            {
                g.FillRectangle(new SolidBrush(Color.White), rectTask.Left, rectTask.Top, rectTask.Width - 2, taskHeight);
                g.DrawRectangle(Pens.Black, rectTask.Left, rectTask.Top, rectTask.Width - 2, taskHeight - 1);
            }

            bool isDrawStart = false;
            bool isDrawEnd = false;
            //是否绘制开始时间
            if (taskEvent.RelateNodes[0].Date <= taskEvent.StartTime)
            {
                isDrawStart = true;
            }
            //是否绘制结束时间
            DateTime dtMinTaskEnd = new DateTime(taskEvent.EndTime.Year, taskEvent.EndTime.Month, taskEvent.EndTime.Day, 0, 0, 0);
            if (taskEvent.RelateNodes[taskEvent.RelateNodes.Count - 1].Date >= dtMinTaskEnd)
            {
                isDrawEnd = true;
            } 
            if(isDrawStart==false)
            {
                g.DrawLine(Pens.White, rectTask.Left, rectTask.Top, rectTask.Left, rectTask.Bottom);
            }
            if(isDrawEnd==false)
            {
                g.DrawLine(Pens.White, rectTask.Right-2, rectTask.Top, rectTask.Right-2, rectTask.Bottom); 
            }


            //任务事件标题绘制区域
            Rectangle rcTaskTitle = rectTask;
            taskEvent.TitleAreas.Add(rcTaskTitle);
            string sp = TruncatedString(taskEvent.Title, rcTaskTitle.Width, 5, g);
            if (taskEvent.Selected == true)
            {
                //居中绘制
                g.DrawString(sp, textFont, taskEventSelectBrush, (float)(rcTaskTitle.Left + rcTaskTitle.Width / 2 - (Helpers.StringTools.MeasureDisplayStringWidth(g, sp, this.Font) / 2)), (float)(rcTaskTitle.Top + 3));
            }
            else
            {
                //居中绘制
                g.DrawString(sp, textFont, new SolidBrush(taskEvent.ForeColor), (float)(rcTaskTitle.Left + rcTaskTitle.Width / 2 - (Helpers.StringTools.MeasureDisplayStringWidth(g, sp, this.Font) / 2)), (float)(rcTaskTitle.Top + 3));
            }

        }

        //private void RenderNodeRows(TreeListNode node, Graphics g, Rectangle r, int level, int index, ref int totalRend, ref int childCount, int count)
        //{
        //    g.DrawString(node.Date.ToString(), Font, new SolidBrush(node.ForeColor), 12, (float)(r.Top + 30 + itemheight * index - vscrollBar.Value));

        //    return;
        //    if (node.IsVisible)
        //    {
        //        int eb = 10;	// edge buffer				

        //        // only render if row is visible in viewport
        //        if (((r.Top + itemheight * totalRend + eb / 4 - vscrollBar.Value + itemheight > r.Top)
        //            && (r.Top + itemheight * totalRend + eb / 4 - vscrollBar.Value < r.Top + r.Height)))
        //        {
        //            rendcnt++;
        //            int lb = 0;		// level buffer
        //            int ib = 0;		// icon buffer
        //            int hb = headerBuffer;	// header buffer	
        //            Pen linePen = new Pen(SystemBrushes.ControlDark, 1.0f);
        //            Pen PMPen = new Pen(SystemBrushes.ControlDark, 1.0f);
        //            Pen PMPen2 = new Pen(new SolidBrush(Color.Black), 1.0f);

        //            linePen.DashStyle = DashStyle.Dot;

        //            // add space for plis/minus icons and/or root lines to the edge buffer
        //            if (showrootlines || showplusminus)
        //            {
        //                eb += 10;
        //            }

        //            // set level buffer
        //            lb = indent * level;

        //            // set icon buffer
        //            if ((node.Selected || node.Focused) && stateImageList != null)
        //            {
        //                if (node.ImageIndex >= 0 && node.ImageIndex < stateImageList.Images.Count)
        //                {
        //                    stateImageList.Draw(g, r.Left + lb + eb + 2 - hscrollBar.Value, r.Top + hb + itemheight * totalRend + eb / 4 - 2 - vscrollBar.Value, 16, 16, node.ImageIndex);
        //                }
        //            }
        //            else
        //            {
        //                if (smallImageList != null && node.ImageIndex >= 0 && node.ImageIndex < smallImageList.Images.Count)
        //                {
        //                    //判断是否节点被选中,选中则画StateImageIndex图标;
        //                    if (node.IsExpanded)
        //                    {
        //                        smallImageList.Draw(g, r.Left + lb + eb + 2 - hscrollBar.Value, r.Top + hb + itemheight * totalRend + eb / 4 - 2 - vscrollBar.Value, smallImageList.ImageSize.Width, 16, node.SelectedImageIndex);
        //                        ib = smallImageList.ImageSize.Width+2;
                                
        //                    }
        //                    else
        //                    {
        //                        smallImageList.Draw(g, r.Left + lb + eb + 2 - hscrollBar.Value, r.Top + hb + itemheight * totalRend + eb / 4 - 2 - vscrollBar.Value, smallImageList.ImageSize.Width, 16, node.ImageIndex);
        //                        ib = smallImageList.ImageSize.Width + 2; 
                               
        //                    }
        //                }
        //            }

        //            // add a rectangle to the node row rectangles
        //            Rectangle sr = new Rectangle(r.Left + lb + ib + eb + 4 - hscrollBar.Value, r.Top + hb + itemheight * totalRend + 2 - vscrollBar.Value, allColsWidth - (lb + ib + eb + 4), itemheight);

        //            if (gridStyle)
        //            {
        //                sr.X = 2;
        //            }

        //            nodeRowRects.Add(sr, node);
                     
        //            g.Clip = new Region(sr); 

        //            // render per-item background
        //            if (node.BackColor != this.BackColor)
        //            {
        //                if (node.UseItemStyleForSubItems)
        //                {
        //                    if (editStyle)
        //                    {
        //                        g.FillRectangle(new SolidBrush(node.BackColor), r.Left + 4 + this.columns[0].Width - hscrollBar.Value, r.Top + hb + itemheight * totalRend + 2 - vscrollBar.Value, allColsWidth - (lb + ib + eb + 4), itemheight);
        //                    }
        //                    else
        //                    {
        //                        g.FillRectangle(new SolidBrush(node.BackColor), r.Left + lb + ib + eb + 4 - hscrollBar.Value, r.Top + hb + itemheight * totalRend + 2 - vscrollBar.Value, allColsWidth - (lb + ib + eb + 4), itemheight);
        //                    }
                            
        //                } 
        //                else
        //                {
        //                    g.FillRectangle(new SolidBrush(node.BackColor), r.Left + lb + ib + eb + 4 - hscrollBar.Value, r.Top + hb + itemheight * totalRend + 2 - vscrollBar.Value, columns[0].Width - (lb + ib + eb + 4), itemheight);
        //                    // return;
        //                }
        //            }
        //            if (node.BackColor != this.BackColor)
        //            {
        //                g.FillRectangle(new SolidBrush(node.BackColor), sr);
        //            }

        //            // render selection and focus
        //            if (node.Selected && isFocused)
        //            {
        //                g.FillRectangle(new SolidBrush(rowSelectColor), sr);
        //                if (this.rowSelectColor != Color.Transparent)
        //                {
        //                    g.FillRectangle(new SolidBrush(rowSelectColor), sr);
        //                }
        //                else
        //                {
        //                    ControlPaint.DrawFocusRectangle(g, sr);
        //                }

        //            }
        //            else if (node.Selected && !isFocused && !hideSelection)
        //            {
        //                g.FillRectangle(SystemBrushes.Control, sr);
        //            }
        //            else if (node.Selected && !isFocused && hideSelection)
        //            {
        //                ControlPaint.DrawFocusRectangle(g, sr);
        //            }

        //            if (node.Focused && ((isFocused && multiSelect) || !node.Selected))
        //            {
        //                ControlPaint.DrawFocusRectangle(g, sr);
        //            }

        //            //画选中节点背景;
        //            //if (this.selectedNode != null)
        //            //{
        //            //    if (this.SelectedNode.Selected && isFocused && !hideSelection)
        //            //    {
        //            //        ControlPaint.DrawFocusRectangle(g, sr);
        //            //    }
        //            //}

        //            g.Clip = new Region(new Rectangle(r.Left + 2 - hscrollBar.Value, r.Top + hb + 2, columns[0].Width, r.Height - hb - 4));

        //            // render root lines if visible
        //            if (r.Left + eb - hscrollBar.Value > r.Left)
        //            {
        //                if (showrootlines && level == 0)
        //                {
        //                    if (index == 0)
        //                    {
        //                        g.DrawLine(linePen, r.Left + eb / 2 - hscrollBar.Value, r.Top + eb / 2 + hb - vscrollBar.Value, r.Left + eb - hscrollBar.Value, r.Top + eb / 2 + hb - vscrollBar.Value);
        //                        g.DrawLine(linePen, r.Left + eb / 2 - hscrollBar.Value, r.Top + eb / 2 + hb - vscrollBar.Value, r.Left + eb / 2 - hscrollBar.Value, r.Top + eb + hb - vscrollBar.Value);
        //                    }
        //                    else if (index == count - 1)
        //                    {
        //                        g.DrawLine(linePen, r.Left + eb / 2 - hscrollBar.Value, r.Top + eb / 2 + hb + itemheight * (totalRend) - vscrollBar.Value, r.Left + eb - hscrollBar.Value, r.Top + eb / 2 + hb + itemheight * (totalRend) - vscrollBar.Value);
        //                        g.DrawLine(linePen, r.Left + eb / 2 - hscrollBar.Value, r.Top + hb + itemheight * (totalRend) - vscrollBar.Value, r.Left + eb / 2 - hscrollBar.Value, r.Top + eb / 2 + hb + itemheight * (totalRend) - vscrollBar.Value);
        //                    }
        //                    else
        //                    {
        //                        g.DrawLine(linePen, r.Left + eb / 2 - hscrollBar.Value, r.Top + eb + hb + itemheight * (totalRend) - eb / 2 - vscrollBar.Value, r.Left + eb - hscrollBar.Value, r.Top + eb + hb + itemheight * (totalRend) - eb / 2 - vscrollBar.Value);
        //                        g.DrawLine(linePen, r.Left + eb / 2 - hscrollBar.Value, r.Top + eb + hb + itemheight * (totalRend - 1) - vscrollBar.Value, r.Left + eb / 2 - hscrollBar.Value, r.Top + eb + hb + itemheight * (totalRend) - vscrollBar.Value);
        //                    }

        //                    if (childCount > 0)
        //                        g.DrawLine(linePen, r.Left + eb / 2 - hscrollBar.Value, r.Top + hb + itemheight * (totalRend - childCount) - vscrollBar.Value, r.Left + eb / 2 - hscrollBar.Value, r.Top + hb + itemheight * (totalRend) - vscrollBar.Value);
        //                }
        //            }

        //            // render child lines if visible
        //            if (r.Left + lb + eb - hscrollBar.Value > r.Left)
        //            {
        //                if (showlines && level > 0)
        //                {
        //                    if (index == count - 1)
        //                    {
        //                        g.DrawLine(linePen, r.Left + lb + eb / 2 - hscrollBar.Value, r.Top + eb / 2 + hb + itemheight * (totalRend) - vscrollBar.Value, r.Left + lb + eb - hscrollBar.Value, r.Top + eb / 2 + hb + itemheight * (totalRend) - vscrollBar.Value);
        //                        g.DrawLine(linePen, r.Left + lb + eb / 2 - hscrollBar.Value, r.Top + hb + itemheight * (totalRend) - vscrollBar.Value, r.Left + lb + eb / 2 - hscrollBar.Value, r.Top + eb / 2 + hb + itemheight * (totalRend) - vscrollBar.Value);
        //                    }
        //                    else
        //                    {
        //                        g.DrawLine(linePen, r.Left + lb + eb / 2 - hscrollBar.Value, r.Top + eb / 2 + hb + itemheight * (totalRend) - vscrollBar.Value, r.Left + lb + eb - hscrollBar.Value, r.Top + eb / 2 + hb + itemheight * (totalRend) - vscrollBar.Value);
        //                        g.DrawLine(linePen, r.Left + lb + eb / 2 - hscrollBar.Value, r.Top + hb + itemheight * (totalRend) - vscrollBar.Value, r.Left + lb + eb / 2 - hscrollBar.Value, r.Top + eb + hb + itemheight * (totalRend) - vscrollBar.Value);
        //                    }

        //                    if (childCount > 0)
        //                        g.DrawLine(linePen, r.Left + lb + eb / 2 - hscrollBar.Value, r.Top + hb + itemheight * (totalRend - childCount) - vscrollBar.Value, r.Left + lb + eb / 2 - hscrollBar.Value, r.Top + hb + itemheight * (totalRend) - vscrollBar.Value);
        //                }
        //            }

        //            // render +/- signs if visible
        //            if (r.Left + lb + eb / 2 + 5 - hscrollBar.Value > r.Left)
        //            {
        //                if (showplusminus && (node.GetNodeCount(false) > 0 || alwaysShowPM))
        //                {
        //                    if (index == 0 && level == 0)
        //                    {
        //                        RenderPlus(g, r.Left + lb + eb / 2 - 4 - hscrollBar.Value, r.Top + hb + eb / 2 - 4 - vscrollBar.Value, 8, 8, node);
        //                    }
        //                    else if (index == count - 1)
        //                    {

        //                        RenderPlus(g, r.Left + lb + eb / 2 - 4 - hscrollBar.Value, r.Top + hb + itemheight * totalRend + eb / 2 - 4 - vscrollBar.Value, 8, 8, node);
        //                    }
        //                    else
        //                    {
        //                        RenderPlus(g, r.Left + lb + eb / 2 - 4 - hscrollBar.Value, r.Top + hb + itemheight * totalRend + eb / 2 - 4 - vscrollBar.Value, 8, 8, node);
        //                    }
        //                }
        //            }

        //            // render text if visible
        //            if (r.Left + columns[0].Width - hscrollBar.Value > r.Left)
        //            {
        //                Font textFont = Font;
        //                if (gridStyle)
        //                {
        //                     textFont = new Font(Font.FontFamily, Font.Size, FontStyle.Bold);
        //                } 

        //                if (columns[0].TextAlign == HorizontalAlignment.Left)
        //                {
        //                    if (node.Selected && isFocused)
        //                        g.DrawString(TruncatedString(node.Date.ToString(), columns[0].Width, lb + eb + ib + 6, g), textFont, rowSelectBrush, (float)(r.Left + lb + ib + eb + 4 - hscrollBar.Value), (float)(r.Top + hb + itemheight * totalRend + eb / 4 - vscrollBar.Value));
        //                    else
        //                        g.DrawString(TruncatedString(node.Text, columns[0].Width, lb + eb + ib + 6, g), textFont, new SolidBrush(node.ForeColor), (float)(r.Left + lb + ib + eb + 4 - hscrollBar.Value), (float)(r.Top + hb + itemheight * totalRend + eb / 4 - vscrollBar.Value));
        //                }
        //                else if (columns[0].TextAlign == HorizontalAlignment.Right)
        //                {
        //                    string sp = TruncatedString(node.Date.ToString(), columns[0].Width, lb + eb + ib + 6, g);
        //                    SizeF strSize = TextRenderer.MeasureText(g, sp, textFont, new Size(0, 0), TextFormatFlags.NoPadding);
        //                    if (node.Selected && isFocused)
        //                        g.DrawString(sp, textFont, rowSelectBrush, (float)(r.Left + columns[0].Width - strSize.Width - 4 - hscrollBar.Value), (float)(r.Top + hb + itemheight * totalRend + eb / 4 - vscrollBar.Value));
        //                    else
        //                        g.DrawString(sp, textFont, new SolidBrush(node.ForeColor), (float)(r.Left + columns[0].Width - strSize.Width - 4 - hscrollBar.Value), (float)(r.Top + hb + itemheight * totalRend + eb / 4 - vscrollBar.Value));

        //                }
                       
        //            }

        //            // render subitems
        //            int j;
        //            int last = 0;
        //            if (columns.Count > 0)
        //            {
        //                for (j = 0; j < node.SubItems.Count && j < columns.Count; j++)
        //                {
        //                    last += columns[j].Width;
        //                    g.Clip = new Region(new Rectangle(last + 6 - hscrollBar.Value, r.Top + headerBuffer + 2, (last + columns[j + 1].Width > r.Width - 6 ? r.Width - 6 : columns[j + 1].Width - 6), r.Height - 5));
                            
        //                    if (node.SubItems[j].ItemControl != null)
        //                    {
        //                        Control c = node.SubItems[j].ItemControl;
        //                        c.Location = new Point(r.Left + last + 4 - hscrollBar.Value, r.Top + (itemheight * totalRend) + headerBuffer + 4 - vscrollBar.Value);

        //                        c.ClientSize = new Size(columns[j + 1].Width - 6, rowHeight - 2);
        //                        if (node.Selected && isFocused)
        //                            c.BackColor = rowSelectColor;
        //                        else
        //                            c.BackColor = node.BackColor;
        //                        c.Parent = this;
        //                        c.Visible = true;
        //                    }
        //                    else if ( smallImageList != null && node.SubItems[j].ImageIndex>= 0 && node.SubItems[j].ImageIndex < smallImageList.Images.Count)
        //                    {
        //                        smallImageList.Draw(g, last + 6 - hscrollBar.Value, r.Top + (itemheight * totalRend) + headerBuffer + 4 - vscrollBar.Value, smallImageList.ImageSize.Width, 16, node.SubItems[j].ImageIndex);

        //                        if (node.Selected && isFocused)
        //                        {
        //                            g.DrawString(TruncatedString(node.SubItems[j].Text, columns[j + 1].Width, 9, g), this.Font, rowSelectBrush, (float)(last + 6 - hscrollBar.Value + smallImageList.ImageSize.Width+2), (float)(r.Top + (itemheight * totalRend) + headerBuffer + 4 - vscrollBar.Value));
        //                        }
        //                        else
        //                        {
        //                            g.DrawString(TruncatedString(node.SubItems[j].Text, columns[j + 1].Width, 9, g), this.Font, (node.UseItemStyleForSubItems ? new SolidBrush(node.ForeColor) : SystemBrushes.WindowText), (float)(last + 6 - hscrollBar.Value+ smallImageList.ImageSize.Width+2), (float)(r.Top + (itemheight * totalRend) + headerBuffer + 4 - vscrollBar.Value));
        //                        }

        //                    }
        //                    else
        //                    {
        //                        //颜色绘制只在左对齐当前情况下判断，因为代码对比中代码都是左对齐的，就给右对齐增加颜色判断而增加复杂度了
        //                        string sp = "";
        //                        if (columns[j + 1].TextAlign == HorizontalAlignment.Left)
        //                        {
        //                            if (node.Selected && isFocused)
        //                            {
        //                                //Add By T.L

        //                                List<int> colorIndex = node.SubItems[j].ColorIndex;
        //                                List<int> colorValue = node.SubItems[j].ColorValue;
        //                                if (colorIndex != null && node.SubItems[j].Text.Length > colorIndex[colorIndex.Count - 1])
        //                                {
        //                                    int x = (int)(last + 6 - hscrollBar.Value);
        //                                    int y = (int)(r.Top + (itemheight * totalRend) + headerBuffer + 4 - vscrollBar.Value);

        //                                    SizeF strSize;
        //                                    Color c = Color.Black;

        //                                    string ss = "";

        //                                    for (int i = 0; i < colorIndex.Count; ++i)
        //                                    {
        //                                        if (i == colorIndex.Count - 1)  //最后一个Index，直接截取后面所有字符
        //                                        {
        //                                            ss = node.SubItems[j].Text.Substring(colorIndex[i]);
        //                                        }
        //                                        else
        //                                        {
        //                                            ss = node.SubItems[j].Text.Substring(colorIndex[i], colorIndex[i + 1] - colorIndex[i]);
        //                                        }
        //                                        c = colorValue[i] == 0 ? node.ForeColor : Color.Red;

        //                                        //g.DrawString(ss, this.Font, new SolidBrush(c), x, y);

        //                                        TextRenderer.DrawText(g, ss, this.Font, new Point((int)x, (int)y), c);
        //                                        //strSize = g.MeasureString(ss, this.Font);
        //                                        strSize = TextRenderer.MeasureText(g, ss, this.Font, new Size(0, 0), TextFormatFlags.NoPadding);
        //                                        x += (int)strSize.Width;
        //                                    }

        //                                }
        //                                else
        //                                {
        //                                    g.DrawString(TruncatedString(node.SubItems[j].Text, columns[j + 1].Width, 9, g), this.Font, rowSelectBrush, (float)(last + 6 - hscrollBar.Value), (float)(r.Top + (itemheight * totalRend) + headerBuffer + 4 - vscrollBar.Value));

        //                                }
        //                            }
        //                            else
        //                            {
        //                                //Add By T.L 
        //                                List<int> colorIndex = node.SubItems[j].ColorIndex;
        //                                List<int> colorValue = node.SubItems[j].ColorValue;
        //                                if (colorIndex != null && node.SubItems[j].Text.Length > colorIndex[colorIndex.Count - 1])
        //                                {
        //                                    int x = (int)(last + 6 - hscrollBar.Value);
        //                                    int y = (int)(r.Top + (itemheight * totalRend) + headerBuffer + 4 - vscrollBar.Value);

        //                                    SizeF strSize;
        //                                    Color c = Color.Black;

        //                                    string ss = "";

        //                                    for (int i = 0; i < colorIndex.Count; ++i)
        //                                    {
        //                                        if (i == colorIndex.Count - 1)  //最后一个Index，直接截取后面所有字符
        //                                        {
        //                                            ss = node.SubItems[j].Text.Substring(colorIndex[i]);
        //                                        }
        //                                        else
        //                                        {
        //                                            ss = node.SubItems[j].Text.Substring(colorIndex[i], colorIndex[i + 1] - colorIndex[i]);
        //                                        }
        //                                        c = colorValue[i] == 0 ? node.ForeColor : Color.Red;

        //                                        //g.DrawString(ss, this.Font, new SolidBrush(c), x, y);

        //                                        TextRenderer.DrawText(g, ss, this.Font, new Point(x, y), c);
        //                                        //strSize = g.MeasureString(ss, this.Font);
        //                                        strSize = TextRenderer.MeasureText(g, ss, this.Font, new Size(0, 0), TextFormatFlags.NoPadding);
        //                                        x += (int)strSize.Width;
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    g.DrawString(TruncatedString(node.SubItems[j].Text, columns[j + 1].Width, 9, g), this.Font, (node.UseItemStyleForSubItems ? new SolidBrush(node.ForeColor) : SystemBrushes.WindowText), (float)(last + 6 - hscrollBar.Value), (float)(r.Top + (itemheight * totalRend) + headerBuffer + 4 - vscrollBar.Value));

        //                                }
        //                            }

        //                        }
        //                        else if (columns[j + 1].TextAlign == HorizontalAlignment.Right)
        //                        {
        //                            sp = TruncatedString(node.SubItems[j].Text, columns[j + 1].Width, 9, g);
        //                            if (node.Selected && isFocused)
        //                            {

        //                                g.DrawString(sp, this.Font, rowSelectBrush, (float)(last + columns[j + 1].Width - Helpers.StringTools.MeasureDisplayStringWidth(g, sp, this.Font) - 4 - hscrollBar.Value), (float)(r.Top + (itemheight * totalRend) + headerBuffer + 4 - vscrollBar.Value));
        //                            }
        //                            else
        //                            {
        //                                g.DrawString(sp, this.Font, (node.UseItemStyleForSubItems ? new SolidBrush(node.ForeColor) : SystemBrushes.WindowText), (float)(last + columns[j + 1].Width - Helpers.StringTools.MeasureDisplayStringWidth(g, sp, this.Font) - 4 - hscrollBar.Value), (float)(r.Top + (itemheight * totalRend) + headerBuffer + 4 - vscrollBar.Value));
        //                            }
        //                        }
        //                        else
        //                        {
        //                            sp = TruncatedString(node.SubItems[j].Text, columns[j + 1].Width, 9, g);
        //                            if (node.Selected && isFocused)
        //                                g.DrawString(sp, this.Font, SystemBrushes.HighlightText, (float)(last + (columns[j + 1].Width / 2) - (Helpers.StringTools.MeasureDisplayStringWidth(g, sp, this.Font) / 2) - hscrollBar.Value), (float)(r.Top + (itemheight * totalRend) + headerBuffer + 4 - vscrollBar.Value));
        //                            else
        //                                g.DrawString(sp, this.Font, (node.UseItemStyleForSubItems ? new SolidBrush(node.ForeColor) : SystemBrushes.WindowText), (float)(last + (columns[j + 1].Width / 2) - (Helpers.StringTools.MeasureDisplayStringWidth(g, sp, this.Font) / 2) - hscrollBar.Value), (float)(r.Top + (itemheight * totalRend) + headerBuffer + 4 - vscrollBar.Value));
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        // increment number of rendered nodes
        //        totalRend++;

        //        // render child nodes
        //        if (node.IsExpanded)
        //        {
        //            childCount = 0;
        //            for (int n=0; n<node.GetNodeCount(false); n++)
        //            {
        //                RenderNodeRows(node.Nodes[n], g, r, level+1, n, ref totalRend, ref childCount, node.Nodes.Count);
        //            }
        //        }
        //        childCount = node.GetVisibleNodeCount(true);
        //    }
        //    else
        //    {
        //        childCount = 0;
        //    }
        //}

		private void RenderPlus(Graphics g, int x, int y, int w, int h, TreeListNode node)
		{
			if (VisualStyles)
			{
				if (node.IsExpanded)
					g.DrawImage(bmpMinus, x, y);
				else
					g.DrawImage(bmpPlus, x, y);
			}
			else
			{
				g.DrawRectangle(new Pen(SystemBrushes.ControlDark),x, y, w, h);
				g.FillRectangle(new SolidBrush(Color.White), x+1, y+1, w-1, h-1);
				g.DrawLine(new Pen(new SolidBrush(Color.Black)), x+2, y+4, x+w-2, y+4);			

				if (!node.IsExpanded)
					g.DrawLine(new Pen(new SolidBrush(Color.Black)), x+4, y+2, x+4, y+h-2);
			}

			pmRects.Add(new Rectangle(x, y, w, h), node);
		}
		#endregion

		#region Methods
		public void CollapseAll()
		{
			foreach (TreeListNode node in nodes)
			{
				node.CollapseAll();
			}
			allCollapsed = true; 
			Invalidate();
		}

		public void ExpandAll()
		{
			foreach (TreeListNode node in nodes)
			{
				node.ExpandAll();
			}
			allCollapsed = false;
			Invalidate();
		}

		public TreeListNode GetNodeAt(int x, int y)
		{
            IEnumerator ek = nodeRowRects.Keys.GetEnumerator();
            IEnumerator ev = nodeRowRects.Values.GetEnumerator();

            while (ek.MoveNext() && ev.MoveNext())
            {
                Rectangle r = (Rectangle)ek.Current;

                if (r.Left <= x && r.Left + r.Width >= x
                    && r.Top <= y && r.Top + r.Height >= y)
                {
                    return (TreeListNode)ev.Current;
                }
            }

            return null;
		} 
		
		private TreeListNodeCollection GetSelectedNodes(TreeListNode node)
		{
			TreeListNodeCollection list = new TreeListNodeCollection();

			for (int i=0; i<node.Nodes.Count; i++)
			{
				// check if current node is selected
				if (node.Nodes[i].Selected)
				{
					list.Add(node.Nodes[i]);
				}

				// chech if node is expanded and has
				// selected children
				if (node.Nodes[i].IsExpanded)
				{
					TreeListNodeCollection list2 = GetSelectedNodes(node.Nodes[i]);
					for (int j=0; j<list2.Count; j++)
					{
						list.Add(list2[i]);
					}
				}
			}

			return list;
		}

		public int GetNodeCount()
		{
			int c = 0;

			c += nodes.Count;
			foreach (TreeListNode node in nodes)
			{
				c += GetNodeCount(node);
			}

			return c;
		}

		public int GetNodeCount(TreeListNode node)
		{
			int c = 0;
			c += node.Nodes.Count;
			foreach (TreeListNode n in node.Nodes)
			{
				c += GetNodeCount(n);
			}
			return c;
		}

        public new void EnsureVisible(TreeListNode node)
        {
            b_fand = false;
            int nodePos = GetTreeListNodePos(node);
            int value = itemheight * nodePos;

            if (value < vscrollBar.Value)
            {
                int min = vscrollBar.Minimum;
                int v = value;
                if (v < min) v = min;
                vscrollBar.Value = v;
            }
            else if (value > vscrollBar.Value + vscrollBar.Height)
            { 
                int max = vscrollBar.Maximum;
                int v = value - vscrollBar.Height + itemheight; 
                if (v > max) v = max;
                vscrollBar.Value = v;
            }
        }

        Boolean b_fand = false;
        private int GetTreeListNodePos(TreeListNode node)
        {
            int allShowNodes = 0;
            for (int i = 0; i < nodes.Count; i++)
            {
                TreeListNode n = nodes[i];
                if (n == node) return allShowNodes;
                if (b_fand) break;
                if (n.IsExpanded && n.Nodes.Count > 0)
                {
                    allShowNodes += GetTreeListNodePos(n, node);
                }
                else
                {
                    allShowNodes++;
                }
            }
            return allShowNodes;
        }

        private int GetTreeListNodePos(TreeListNode n, TreeListNode node)
        {
            int allShowNodes = 1;
            for (int i = 0; i < n.Nodes.Count; i++)
            {
                TreeListNode tn = n.Nodes[i];
                if (tn == node)
                {
                    b_fand = true;
                    return allShowNodes;
                }
                if (b_fand) break;
                if (tn.IsExpanded && tn.Nodes.Count > 0)
                {
                    allShowNodes += GetTreeListNodePos(tn, node);
                }
                else
                {
                    allShowNodes++;
                }
            }

            return allShowNodes;
        }

        private TreeListNodeCollection alSelectNodes = new TreeListNodeCollection();
        public TreeListNodeCollection GetAllSelectNodes()
        {
            alSelectNodes = new TreeListNodeCollection();
            foreach (TreeListNode node in nodes)
            {
                if (node.Selected) alSelectNodes.Add(node, false);
                GetAllSelectNodes(node);
            }
            return alSelectNodes;
        }

        private void GetAllSelectNodes(TreeListNode node)
        {
            foreach (TreeListNode n in node.Nodes)
            {
                if (n.Selected) alSelectNodes.Add(n, false);
                GetAllSelectNodes(n);
            }
        } 
		#endregion
	}
	#endregion

	#region Type Converters
	public class TreeListNodeConverter: TypeConverter
	{
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (destinationType == typeof(InstanceDescriptor))
			{
				return true;
			}
			return base.CanConvertTo(context, destinationType);
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(InstanceDescriptor) && value is TreeListNode)
			{
				TreeListNode tln = (TreeListNode)value;

				ConstructorInfo ci = typeof(TreeListNode).GetConstructor(new Type[] {});
				if (ci != null)
				{
					return new InstanceDescriptor(ci, null, false);
				}
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
	#endregion
}
