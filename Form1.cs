using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView;


public partial class Form1 : Form
{
    private const int Size = 25;
    private int RectX;
    private int RectY;
    private int[,] grid;

    private Dictionary<Point, List<Point>> graph = new Dictionary<Point, List<Point>>();

    private Point start = new Point(0, 0);
    private Queue<Point> queue = new Queue<Point>();
    private Dictionary<Point, Point> visited = new Dictionary<Point, Point>();
    private Point curNode;
    private Graphics g;
    private Point goal;
    public Form1()
    {
        InitializeComponent();

        RectX = Width / Size;
        RectY= Height / Size;

        grid = new int[RectX,RectY];

        var rand = new Random();
        
        for (int i = 0; i < RectX; i++)
        {
            for (int j = 0; j < RectX; j++)
            {
                grid[i, j] = (rand.NextDouble() < 0.2) ? 1 : 0;
               
            }
        }
        
        goal = new Point(RectX - 2, RectY / 2);
        grid[goal.X, goal.Y] = 0;
        grid[start.X, start.Y] = 0;
        for (int i = 0; i < RectX; i++)
        {
            for (int j = 0; j < RectX; j++)
            {
                if (grid[i, j] == 0)
                {
                    var nextNodes = GetNextNodes(i, j);
                    graph[new Point(i, j)] = nextNodes;
                }
            }
        }

        
        Paint.Image = new Bitmap(Width, Height);
        g = Graphics.FromImage(Paint.Image);

        queue.Enqueue(start);
        visited[start] = new Point(0, 0);
        curNode = start;
        timer1.Start();
        DrawFull();
    }

    private void timer1_Tick(object sender, EventArgs e)
    {
        if (queue.Count >0)
        {
            curNode = queue.Dequeue();
            

            List<Point> Next = graph[curNode];
            foreach (var item in Next)
            {
                if (!visited.ContainsKey(item))
                {
                    queue.Enqueue(item);
                    visited[item] = curNode;
                }
            }
            if (curNode == goal)
            {
                timer1.Stop();
                DrawFull();
                return;
            }
            DrawFull();
        }     
    }

    private void DrawFull()
    {
        g.FillRectangle(Brushes.Black, 0, 0, Width, Height);
        for (int i = 0; i < RectX; i++)
        {
            for (int j = 0; j < RectY; j++)
            {
                if (grid[i, j] == 1)
                    g.FillRectangle(Brushes.Yellow, i * Size, j * Size, Size, Size);
            }
        }
        
        var temp = curNode;
        while (temp != start)
        {
            temp = visited[temp];
            g.FillRectangle(Brushes.White, temp.X * Size, temp.Y * Size, Size, Size);
        }
        
        g.FillRectangle(Brushes.Blue, 0, 0, Size, Size);
        g.FillRectangle(Brushes.Blue, goal.X*Size, goal.Y * Size, Size, Size);
        Paint.Invalidate();
    }

    private bool ProvOutRangeArray(Point point, Point two) => point.X + two.X < 0 || point.X + two.X >= RectX || point.Y + two.Y < 0 || point.Y + two.Y >= RectY;

    private List<Point> GetNextNodes(int x, int y)
    {
        
        List<Point> nextNodes = new List<Point>();

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (Math.Abs(i) == Math.Abs(j))
                    continue;
                if (ProvOutRangeArray(new Point(i, j), new Point(x + i, y + j)))
                    continue;
                if (grid[x + i, y + j] == 1)
                    continue;

                nextNodes.Add(new Point(x + i, y + j));
            }
        }
        
        return nextNodes;
    }
}