using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpGL;
namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            DrawTimer.Interval = 1000;
        }

        private void openGLControl_OpenGLDraw(object sender, SharpGL.RenderEventArgs args)
        {
            OpenGL gl = openGLControl.OpenGL;
            // Clear the color and depth buffer.
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            drawAllShape(gl);
            gl.Flush();// Thực hiện lệnh vẽ ngay lập tức thay vì đợi sau 1 khoảng thời gian
        }

        private void openGLControl_OpenGLInitialized(object sender, EventArgs e)
        {
            OpenGL gl = openGLControl.OpenGL;
            // Set the clear color.
            gl.ClearColor(0, 0, 0, 0);
            // Set the projection matrix.
            gl.MatrixMode(OpenGL.GL_PROJECTION);
            // Load the identity.
            gl.LoadIdentity();

        }

        private void openGLControl_Resized(object sender, EventArgs e)
        {
            OpenGL gl = openGLControl.OpenGL;
            // Set the projection matrix.
            gl.MatrixMode(OpenGL.GL_PROJECTION);
            // Load the identity.
            gl.LoadIdentity();
            // Create a perspective transformation.
            gl.Viewport(0, 0, openGLControl.Width, openGLControl.Height);
            gl.Ortho2D(0, openGLControl.Width, 0, openGLControl.Height);
        }

        private class State
        {
            public string Choose;
            public int UnitOfLineWidth;
            public bool IsMouseDown;
            public bool IsMoveShape;
            public bool IsFill;
            public int DrawTime;
            public Point MoveBegin;
            public Point MoveEnd;
            public State()
            {
                Choose = "DrawLine";
                UnitOfLineWidth = 1;
                IsMouseDown = false;
                DrawTime = 0;
                IsMoveShape = false;
                IsFill = false;
                MoveBegin = new Point();
                MoveEnd = new Point();
            }
        }
        private class Properties
        {
            public Point PointBegin;
            public Point PointEnd;
            public int LineWidth;
            public Color ColorLine;
            public Color ColorFill;
            public Properties()
            {
                PointBegin = new Point();
                PointEnd = new Point();
                LineWidth = 1;
                ColorLine = Color.White;
                ColorFill = Color.Red;
            }
            public Properties(Point pb, Point pe, int lw, Color cl, Color cf)
            {
                PointBegin = new Point(pb.X, pb.Y);
                PointEnd = new Point(pe.X, pe.Y);
                LineWidth = lw;
                ColorLine = Color.FromArgb(cl.A, cl.R, cl.G, cl.B);
                ColorFill = Color.FromArgb(cf.A, cf.R, cf.G, cf.B);

            }
            public Properties DeepCopy()
            {
                Properties newProperties = new Properties(this.PointBegin, this.PointEnd, this.LineWidth, this.ColorLine, this.ColorFill);
                return newProperties;
            }
        }
        private class Shape
        {
            public string Kind;
            public Properties Properties;
            public HashSet<Point> PixelsInLine;
            public bool IsFill;
            public HashSet<Point> PixelsInArea;
            public bool IsChosen;

            public List<Point> ListOfBetweenPoints;
            public bool IsDone;
            public Shape()
            {
                Kind = "Line";
                Properties = new Properties();
                PixelsInLine = new HashSet<Point>();
                IsFill = false;
                PixelsInArea = new HashSet<Point>();
                IsChosen = false;

                IsDone = false;
                ListOfBetweenPoints = new List<Point>();
            }
        }

        State CurrentState = new State();
        List<Shape> ListOfInstances = new List<Shape>();
        Properties CurrentCustom = new Properties();

        HashSet<Point> savedPointInLine;//lưu lại pixels của hình được chọn.
        HashSet<Point> savedPointInArea;//lưu lại pixels trong hình được chọn.

        private void drawAllShape(OpenGL gl)
        {
            foreach (Shape AnInstance in ListOfInstances)
            {
                gl.Color(AnInstance.Properties.ColorLine.R, AnInstance.Properties.ColorLine.G, AnInstance.Properties.ColorLine.B, AnInstance.Properties.ColorLine.A);
                gl.Begin(OpenGL.GL_POINTS);
                foreach (Point A in AnInstance.PixelsInLine)
                {
                    gl.Vertex(A.X, gl.RenderContextProvider.Height - A.Y);
                }
                gl.End();

                gl.Color(AnInstance.Properties.ColorFill.R, AnInstance.Properties.ColorFill.G, AnInstance.Properties.ColorFill.B, AnInstance.Properties.ColorFill.A);
                gl.Begin(OpenGL.GL_POINTS);
                foreach (Point A in AnInstance.PixelsInArea)
                {
                    gl.Vertex(A.X, gl.RenderContextProvider.Height - A.Y);
                }
                gl.End();
            }
        }

        private void DrawLineButton_Click(object sender, EventArgs e)
        {
            CurrentState.Choose = "DrawLine";
            DrawTimeLabel.Text = "Draw Time: 0 s";
            CurrentState.IsMoveShape = false;
            CurrentState.IsFill = false;
        }

        private void openGLControl_MouseDown(object sender, MouseEventArgs e)
        {
            CurrentState.IsMouseDown = true;
            if (CurrentState.IsMoveShape == true)
            {
                CurrentState.MoveBegin =CurrentState.MoveEnd= e.Location;
                savedPointInLine = pickShape(CurrentState.MoveBegin, ListOfInstances);
            }
            else
            {
                if (e.Button == MouseButtons.Left)
                {
                    switch (CurrentState.Choose)
                    {
                        case "DrawLine":
                            DrawTimeLabel.Text = "Draw Time: 0 s";
                            DrawTimer.Start();
                            CurrentCustom.PointBegin = e.Location;
                            CurrentCustom.PointEnd = e.Location;
                            Shape newLine = new Shape();
                            newLine.Kind = "Line";
                            newLine.Properties = (Properties)CurrentCustom.DeepCopy();
                            ListOfInstances.Add(newLine);
                            break;
                        case "DrawEllipse":
                            DrawTimeLabel.Text = "Draw Time: 0 s";
                            DrawTimer.Start();
                            CurrentCustom.PointBegin = e.Location;
                            CurrentCustom.PointEnd = e.Location;
                            Shape newEllipse = new Shape();
                            newEllipse.Kind = "Ellipse";
                            newEllipse.Properties = (Properties)CurrentCustom.DeepCopy();
                            ListOfInstances.Add(newEllipse);
                            break;
                        case "DrawRectangle":
                            DrawTimeLabel.Text = "Draw Time: 0 s";
                            DrawTimer.Start();
                            CurrentCustom.PointBegin = e.Location;
                            CurrentCustom.PointEnd = e.Location;
                            Shape newRectangle = new Shape();
                            newRectangle.Kind = "Rectangle";
                            newRectangle.Properties = (Properties)CurrentCustom.DeepCopy();
                            ListOfInstances.Add(newRectangle);
                            break;
                        case "DrawPentagon":
                            DrawTimeLabel.Text = "Draw Time: 0 s";
                            DrawTimer.Start();
                            CurrentCustom.PointBegin = e.Location;
                            CurrentCustom.PointEnd = e.Location;
                            Shape newPentagon = new Shape();
                            newPentagon.Kind = "Pentagon";
                            newPentagon.Properties = (Properties)CurrentCustom.DeepCopy();
                            ListOfInstances.Add(newPentagon);
                            break;
                        case "DrawCircle":
                            DrawTimer.Start();
                            DrawTimeLabel.Text = "Draw Time: 0 s";
                            CurrentCustom.PointBegin = e.Location;
                            CurrentCustom.PointEnd = e.Location;
                            Shape newCircle = new Shape();
                            newCircle.Kind = "Circle";
                            newCircle.Properties = (Properties)CurrentCustom.DeepCopy();
                            ListOfInstances.Add(newCircle);
                            break;
                        case "DrawEquilTriangle":
                            DrawTimer.Start();
                            DrawTimeLabel.Text = "Draw Time: 0 s";
                            CurrentCustom.PointBegin = e.Location;
                            CurrentCustom.PointEnd = e.Location;
                            Shape newEquilTriangle = new Shape();
                            newEquilTriangle.Kind = "EquilTriangle";
                            newEquilTriangle.Properties = (Properties)CurrentCustom.DeepCopy();
                            ListOfInstances.Add(newEquilTriangle);
                            break;
                        case "DrawPolygon":
                            if (ListOfInstances.Count > 0)
                            {
                                if (ListOfInstances[ListOfInstances.Count - 1].Kind == "Polygon")
                                {
                                    if ((ListOfInstances[ListOfInstances.Count - 1]).IsDone == false)
                                    {
                                        ListOfInstances[ListOfInstances.Count - 1].ListOfBetweenPoints.Add(new Point(ListOfInstances[ListOfInstances.Count - 1].Properties.PointEnd.X, ListOfInstances[ListOfInstances.Count - 1].Properties.PointEnd.Y));
                                        ListOfInstances[ListOfInstances.Count - 1].Properties.PointEnd = e.Location;
                                        ListOfInstances[ListOfInstances.Count - 1].PixelsInLine = drawPolygon(
                                            ListOfInstances[ListOfInstances.Count - 1].Properties.PointBegin,
                                            ListOfInstances[ListOfInstances.Count - 1].Properties.PointEnd,
                                            ListOfInstances[ListOfInstances.Count - 1].ListOfBetweenPoints,
                                            ListOfInstances[ListOfInstances.Count - 1].Properties.LineWidth
                                            );
                                        break;
                                    }
                                }
                            }
                            DrawTimeLabel.Text = "Draw Time: 0 s";
                            DrawTimer.Start();
                            CurrentCustom.PointBegin = e.Location;
                            CurrentCustom.PointEnd = e.Location;
                            Shape newPolygon = new Shape();
                            newPolygon.Kind = "Polygon";
                            newPolygon.Properties = (Properties)CurrentCustom.DeepCopy();
                            ListOfInstances.Add(newPolygon);
                            break;
                    }
                }
                else if (e.Button == MouseButtons.Right)
                {
                    if (CurrentState.Choose == "DrawPolygon")
                    {
                        if (ListOfInstances[ListOfInstances.Count - 1].Kind == "Polygon" && ListOfInstances[ListOfInstances.Count - 1].IsDone == false)
                        {
                            ListOfInstances[ListOfInstances.Count - 1].IsDone = true;
                            DrawTimer.Stop();
                            CurrentState.DrawTime = 0;
                        }
                    }
                    if(CurrentState.IsFill==true)
                    {
                        floodFill(e.Location.X, e.Location.Y);
                    }
                }
            }
        }

        private void openGLControl_MouseUp(object sender, MouseEventArgs e)
        {
            CurrentState.IsMouseDown = false;
            if (CurrentState.IsMoveShape == true)
            {
                foreach (Shape temp in ListOfInstances)
                {
                    if (temp.IsChosen == true)
                        temp.IsChosen = false;
                }
                CurrentState.MoveBegin = CurrentState.MoveEnd = Point.Empty;
            }
            else
            {
                if (CurrentState.Choose != "DrawPolygon")
                {
                    DrawTimer.Stop();
                    CurrentState.DrawTime = 0;
                }
           }
        }

        private void openGLControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (CurrentState.IsMouseDown && e.Button == MouseButtons.Left && CurrentState.IsMoveShape==false)
            {
                switch (CurrentState.Choose)
                {
                    case "DrawLine":
                        ListOfInstances[ListOfInstances.Count - 1].Properties.PointEnd = e.Location;
                        
                        ListOfInstances[ListOfInstances.Count - 1].PixelsInLine = drawLine(
                            ListOfInstances[ListOfInstances.Count - 1].Properties.PointBegin.X,
                            ListOfInstances[ListOfInstances.Count - 1].Properties.PointBegin.Y,
                            ListOfInstances[ListOfInstances.Count - 1].Properties.PointEnd.X,
                            ListOfInstances[ListOfInstances.Count - 1].Properties.PointEnd.Y,
                            ListOfInstances[ListOfInstances.Count - 1].Properties.LineWidth
                            );
                        break;
                    case "DrawEllipse":
                        ListOfInstances[ListOfInstances.Count - 1].Properties.PointEnd = e.Location;
                        ListOfInstances[ListOfInstances.Count - 1].PixelsInLine = drawEllipse(
                            ListOfInstances[ListOfInstances.Count - 1].Properties.PointBegin.X,
                            ListOfInstances[ListOfInstances.Count - 1].Properties.PointBegin.Y,
                            ListOfInstances[ListOfInstances.Count - 1].Properties.PointEnd.X,
                            ListOfInstances[ListOfInstances.Count - 1].Properties.PointEnd.Y,
                            ListOfInstances[ListOfInstances.Count - 1].Properties.LineWidth
                            );
                        break;
                    case "DrawRectangle":
                        ListOfInstances[ListOfInstances.Count - 1].Properties.PointEnd = e.Location;
                        ListOfInstances[ListOfInstances.Count - 1].PixelsInLine = drawRectangle(
                            ListOfInstances[ListOfInstances.Count - 1].Properties.PointBegin.X,
                            ListOfInstances[ListOfInstances.Count - 1].Properties.PointBegin.Y,
                            ListOfInstances[ListOfInstances.Count - 1].Properties.PointEnd.X,
                            ListOfInstances[ListOfInstances.Count - 1].Properties.PointEnd.Y,
                            ListOfInstances[ListOfInstances.Count - 1].Properties.LineWidth
                            );
                        break;
                    case "DrawPentagon":
                        ListOfInstances[ListOfInstances.Count - 1].Properties.PointEnd = e.Location;
                        ListOfInstances[ListOfInstances.Count - 1].PixelsInLine = drawPentagon(
                            ListOfInstances[ListOfInstances.Count - 1].Properties.PointBegin.X,
                            ListOfInstances[ListOfInstances.Count - 1].Properties.PointBegin.Y,
                            ListOfInstances[ListOfInstances.Count - 1].Properties.PointEnd.X,
                            ListOfInstances[ListOfInstances.Count - 1].Properties.PointEnd.Y,
                            ListOfInstances[ListOfInstances.Count - 1].Properties.LineWidth
                            );
                        break;
                    case "DrawPolygon":
                        ListOfInstances[ListOfInstances.Count - 1].Properties.PointEnd = e.Location;
                        ListOfInstances[ListOfInstances.Count - 1].PixelsInLine = drawPolygon(
                            ListOfInstances[ListOfInstances.Count - 1].Properties.PointBegin,
                            ListOfInstances[ListOfInstances.Count - 1].Properties.PointEnd,
                            ListOfInstances[ListOfInstances.Count - 1].ListOfBetweenPoints,
                            ListOfInstances[ListOfInstances.Count - 1].Properties.LineWidth
                            );
                        break;
                    case "DrawCircle":
                        ListOfInstances[ListOfInstances.Count - 1].Properties.PointEnd = e.Location;
                        ListOfInstances[ListOfInstances.Count - 1].PixelsInLine = drawCircle(
                            ListOfInstances[ListOfInstances.Count - 1].Properties.PointBegin.X,
                            ListOfInstances[ListOfInstances.Count - 1].Properties.PointBegin.Y,
                            ListOfInstances[ListOfInstances.Count - 1].Properties.PointEnd.X,
                            ListOfInstances[ListOfInstances.Count - 1].Properties.PointEnd.Y,
                            ListOfInstances[ListOfInstances.Count - 1].Properties.LineWidth
                            );
                        break;
                    case "DrawEquilTriangle":
                        ListOfInstances[ListOfInstances.Count - 1].Properties.PointEnd = e.Location;
                        ListOfInstances[ListOfInstances.Count - 1].PixelsInLine = drawEquilateralTriangle(
                            ListOfInstances[ListOfInstances.Count - 1].Properties.PointBegin.X,
                            ListOfInstances[ListOfInstances.Count - 1].Properties.PointBegin.Y,
                            ListOfInstances[ListOfInstances.Count - 1].Properties.PointEnd.X,
                            ListOfInstances[ListOfInstances.Count - 1].Properties.PointEnd.Y,
                            ListOfInstances[ListOfInstances.Count - 1].Properties.LineWidth
                            );
                        break;
                }
            }
            else if(CurrentState.IsMoveShape==true)
            {
                CurrentState.MoveEnd = e.Location;
                MoveShape(CurrentState.MoveBegin, CurrentState.MoveEnd);
            }
        }

        private HashSet<Point> putpixels(int x, int y, int widthLine)
        {
            HashSet<Point> newList = new HashSet<Point>();
            for (int i = 0; i < widthLine; i++)
                for (int j = 0; j < widthLine; j++)
                {
                    newList.Add(new Point(x + i, y + j));
                }
            return newList;
        }
        private HashSet<Point> drawLineLow(int x1, int y1, int x2, int y2, int widthLine)
        {
            HashSet<Point> result = new HashSet<Point>();
            int dx, dy, yi, D;
            int x, y;
            dx = x2 - x1;
            dy = y2 - y1;
            yi = 1;
            if (dy < 0)
            {
                yi = -1;
                dy = -dy;
            }
            D = 2 * dy - dx;
            y = y1;
            for (x = x1; x <= x2; x++)
            {
                result.UnionWith(putpixels(x, y, widthLine));
                if (D > 0)
                {
                    y += yi;
                    D -= 2 * dx;
                }
                D += 2 * dy;
            }
            return result;
        }
        private HashSet<Point> drawLineHigh(int x1, int y1, int x2, int y2, int widthLine)
        {
            HashSet<Point> result = new HashSet<Point>();
            int dx, dy, xi, D;
            int x, y;
            dx = x2 - x1;
            dy = y2 - y1;
            xi = 1;
            if (dx < 0)
            {
                xi = -1;
                dx = -dx;
            }
            D = 2 * dx - dy;
            x = x1;
            for (y = y1; y <= y2; y++)
            {
                result.UnionWith(putpixels(x, y, widthLine));
                if (D > 0)
                {
                    x += xi;
                    D -= 2 * dy;
                }
                D += 2 * dx;
            }
            return result;
        }
        private HashSet<Point> drawLine(int x1, int y1, int x2, int y2, int widthLine)
        {
            HashSet<Point> result = new HashSet<Point>();

            if (Math.Abs(y2 - y1) < Math.Abs(x2 - x1))
            {
                if (x1 > x2)
                    result.UnionWith(drawLineLow(x2, y2, x1, y1, widthLine));
                else
                    result.UnionWith(drawLineLow(x1, y1, x2, y2, widthLine));
            }
            else
            {
                if (y1 > y2)
                    result.UnionWith(drawLineHigh(x2, y2, x1, y1, widthLine));
                else
                    result.UnionWith(drawLineHigh(x1, y1, x2, y2, widthLine));
            }
            return result;
        }

        private int CountPointInList(HashSet<Point> list, Point point)
        {
            int result = 0;
            foreach (Point temp in list)
            {
                if (temp == point)
                    result++;
            }
            return result;
        }

        private void SetLineColor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                CurrentCustom.ColorLine = colorDialog1.Color;
            }
            CurrentState.IsMoveShape = false;
            CurrentState.IsFill = false;
        }
        private HashSet<Point> drawEllipse(int x1, int y1, int x2, int y2, int widthLine)
        {
            HashSet<Point> result = new HashSet<Point>();
            int Ry = Math.Abs(y1 - y2) / 2;
            int Rx = Math.Abs(x1 - x2) / 2;
            int Rx2 = Rx * Rx;
            int Ry2 = Ry * Ry;
            int twoRx2 = 2 * Rx2;
            int twoRy2 = 2 * Ry2;
            int p;
            int x = 0;
            int y = Ry;
            int a = (x1 + x2) / 2;
            int b = (y1 + y2) / 2;
            int px = 0;
            int py = twoRx2 * y;
            result.UnionWith(ellipsePlotPoints(a, b, x, y, widthLine));

            p = round(Ry2 - (Rx2 * Ry) + (0.25 + Rx2));
            while (px < py)
            {
                x++;
                px += twoRy2;
                if (p < 0)
                {
                    p += Ry2 + px;
                }
                else
                {
                    y--;
                    py -= twoRx2;
                    p += Ry2 + px - py;
                }
                result.UnionWith(ellipsePlotPoints(a, b, x, y, widthLine));
            }
            p = round(Ry2 * (x + 0.5) * (x + 0.5) + Rx2 * (y - 1) * (y - 1) - Rx2 * Ry2);
            while (y > 0)
            {
                y--;
                py -= twoRx2;
                if (p > 0) p += Rx2 - py;
                else
                {
                    x++;
                    px += twoRy2;
                    p += Rx2 - py + px;
                }
                result.UnionWith(ellipsePlotPoints(a, b, x, y, widthLine));
            }
            return result;
        }
        private int round(double a)
        {
            return (int)(a + 0.5);
        }
        private HashSet<Point> ellipsePlotPoints(int x1, int y1, int x2, int y2, int widthLine)
        {
            HashSet<Point> result = new HashSet<Point>();
            result.UnionWith(putpixels(x1 + x2, y1 + y2, widthLine));

            result.UnionWith(putpixels(x1 - x2, y1 + y2, widthLine));

            result.UnionWith(putpixels(x1 + x2, y1 - y2, widthLine));

            result.UnionWith(putpixels(x1 - x2, y1 - y2, widthLine));
            return result;
        }

        private void DrawEllipseButton_Click(object sender, EventArgs e)
        {
            CurrentState.Choose = "DrawEllipse";
            DrawTimeLabel.Text = "Draw Time: 0 s";
            CurrentState.IsMoveShape = false;
            CurrentState.IsFill = false;
        }
        private HashSet<Point> drawRectangle(int x1, int y1, int x2, int y2, int widthLine)
        {
            HashSet<Point> result = new HashSet<Point>();
            result.UnionWith(drawLine(x1, y1, x1, y2, widthLine));
            result.UnionWith(drawLine(x1, y2, x2, y2, widthLine));
            result.UnionWith(drawLine(x2, y2, x2, y1, widthLine));
            result.UnionWith(drawLine(x2, y1, x1, y1, widthLine));
            return result;
        }

        private void DrawRectangleButton_Click(object sender, EventArgs e)
        {
            CurrentState.Choose = "DrawRectangle";
            DrawTimeLabel.Text = "Draw Time: 0 s";
            CurrentState.IsMoveShape = false;
            CurrentState.IsFill = false;
        }

        private void DrawPentagonButton_Click(object sender, EventArgs e)
        {
            CurrentState.Choose = "DrawPentagon";
            DrawTimeLabel.Text = "Draw Time: 0 s";
            CurrentState.IsMoveShape = false;
            CurrentState.IsFill = false;
        }

        private HashSet<Point> drawPentagon(int x1, int y1, int x2, int y2, int widthLine)
        {
            HashSet<Point> result = new HashSet<Point>();
            int bx, by, cx, cy;//bottom left coordinate

            cx = x2 - x1;
            //cy = this.openGLControl.Height - y2 - y1;
            cy = y2 - y1;
            if (cx < 0)
            {
                cx = -cx;
                if (Math.Abs(cy) * 1.0 / cx < Math.Sqrt(5 + 2 * Math.Sqrt(5)) / (1 + Math.Sqrt(5)))
                {
                    double tempA = Math.Abs(cy) * 2 / (Math.Sqrt(5 + 2 * Math.Sqrt(5)));
                    bx = x1 - Convert.ToInt32(tempA * ((1 + Math.Sqrt(5)) / 2));
                }
                else
                {
                    bx = x2;
                }
            }
            else
            {
                bx = x1;
            }
            if (cy < 0)
            {
                cy = -cy;
                by = y1;
            }
            else
            {
                if (cy * 1.0 / Math.Abs(cx) > Math.Sqrt(5 + 2 * Math.Sqrt(5)) / (1 + Math.Sqrt(5)))
                {
                    double tempA = Math.Abs(cx) * 2 / (1 + Math.Sqrt(5));
                    by = y1 + Convert.ToInt32(tempA * ((Math.Sqrt(5 + 2 * Math.Sqrt(5))) / 2));
                }
                else
                {
                    by = y2;
                }
            }

            double a = 0;
            if (cy * 1.0 / cx < Math.Sqrt(5 + 2 * Math.Sqrt(5)) / (1 + Math.Sqrt(5)))
            {
                a = cy * 2 / (Math.Sqrt(5 + 2 * Math.Sqrt(5)));
            }
            else
            {
                a = cx * 2 / (1 + Math.Sqrt(5));
            }

            result.UnionWith(
                drawLine(
                    bx + 0,
                    Convert.ToInt32(by - a * Math.Sqrt((5 + Math.Sqrt(5)) / 8)),
                    Convert.ToInt32(bx + a * (Math.Sqrt(5) - 1) / 4),
                    by - 0,
                    widthLine
                    ));
            result.UnionWith(
                drawLine(
                    Convert.ToInt32(bx + a * (Math.Sqrt(5) - 1) / 4),
                    by - 0,
                    Convert.ToInt32(bx + a * ((3 + Math.Sqrt(5)) / 4)),
                    by - 0,
                    widthLine
                    ));
            result.UnionWith(
                drawLine(
                    Convert.ToInt32(bx + a * ((3 + Math.Sqrt(5)) / 4)),
                    by - 0,
                    Convert.ToInt32(bx + a * ((1 + Math.Sqrt(5)) / 2)),
                    Convert.ToInt32(by - a * (Math.Sqrt((5 + Math.Sqrt(5)) / 8))),
                    widthLine
                    ));
            result.UnionWith(
                drawLine(
                    Convert.ToInt32(bx + a * ((1 + Math.Sqrt(5)) / 2)),
                    Convert.ToInt32(by - a * (Math.Sqrt((5 + Math.Sqrt(5)) / 8))),
                    Convert.ToInt32(bx + a * ((1 + Math.Sqrt(5)) / 4)),
                    Convert.ToInt32(by - a * ((Math.Sqrt(5 + 2 * Math.Sqrt(5))) / 2)),
                    widthLine
                    ));
            result.UnionWith(
                drawLine(
                    Convert.ToInt32(bx + a * ((1 + Math.Sqrt(5)) / 4)),
                    Convert.ToInt32(by - a * ((Math.Sqrt(5 + 2 * Math.Sqrt(5))) / 2)),
                    bx + 0,
                    Convert.ToInt32(by - a * Math.Sqrt((5 + Math.Sqrt(5)) / 8)),
                    widthLine
                    ));
            return result;
        }

        private void DrawTimer_Tick(object sender, EventArgs e)
        {
            CurrentState.DrawTime += 1;
            DrawTimeLabel.Text = "Draw Time: " + CurrentState.DrawTime.ToString() + " s";
            DrawTimer.Start();
        }

        private void DrawPolygonButton_Click(object sender, EventArgs e)
        {
            CurrentState.Choose = "DrawPolygon";
            DrawTimeLabel.Text = "Draw Time: 0 s";
            CurrentState.IsMoveShape = false;
            CurrentState.IsFill = false;
        }
        private HashSet<Point> drawPolygon(Point p1, Point p2, List<Point> lp, int lineWidth)
        {
            HashSet<Point> result = new HashSet<Point>();
            for (int i = 0; i < lp.Count - 1; i++)
            {
                result.UnionWith(drawLine(lp[i].X, lp[i].Y, lp[i + 1].X, lp[i + 1].Y, lineWidth));
            }
            if (lp.Count > 0)
            {
                result.UnionWith(drawLine(lp[0].X, lp[0].Y, p1.X, p1.Y, lineWidth));
                result.UnionWith(drawLine(lp[lp.Count - 1].X, lp[lp.Count - 1].Y, p2.X, p2.Y, lineWidth));
            }
            result.UnionWith(drawLine(p1.X, p1.Y, p2.X, p2.Y, lineWidth));
            return result;
        }

        private HashSet<Point> drawEquilateralTriangle(int x1, int y1, int x2, int y2, int widthLine)
        {
            int a;
            Point vertex1 = new Point(), vertex2 = new Point(), vertex3 = new Point();
            HashSet<Point> result = new HashSet<Point>();

            int deltaX = x2 - x1;
            int deltaY = y2 - y1;

            if (Math.Abs(deltaX) < Math.Abs(deltaY) / Math.Sin(Math.PI / 3))
                a = Math.Abs(deltaX);
            else
                a = Math.Abs(Convert.ToInt32(deltaY / Math.Sin(Math.PI / 3)));

            if (deltaY > 0)
            {
                if (deltaX > 0)
                {
                    vertex1 = new Point(x1 + a / 2, y1);
                    vertex2 = new Point(x1, Convert.ToInt32(y1 + a * Math.Sin(Math.PI / 3)));
                    vertex3 = new Point(vertex2.X + a, vertex2.Y);
                }
                if (deltaX < 0)
                {
                    vertex1 = new Point(x1 - a / 2, y1);
                    vertex2 = new Point(x1, Convert.ToInt32(y1 + a * Math.Sin(Math.PI / 3)));
                    vertex3 = new Point(vertex2.X - a, vertex2.Y);
                }
            }
            else
            {
                if (deltaX > 0)
                {
                    vertex1 = new Point(x1, y1);
                    vertex2 = new Point(x1 + a, y1);
                    vertex3 = new Point(x1 + a / 2, Convert.ToInt32(y1 - a * Math.Sin(Math.PI / 3)));
                }
                if (deltaX < 0)
                {
                    vertex1 = new Point(x1, y1);
                    vertex2 = new Point(x1 - a, y1);
                    vertex3 = new Point(x1 - a / 2, Convert.ToInt32(y1 - a * Math.Sin(Math.PI / 3)));
                }
            }

            result.UnionWith(drawLine(vertex1.X, vertex1.Y, vertex2.X, vertex2.Y, widthLine));
            result.UnionWith(drawLine(vertex2.X, vertex2.Y, vertex3.X, vertex3.Y, widthLine));
            result.UnionWith(drawLine(vertex3.X, vertex3.Y, vertex1.X, vertex1.Y, widthLine));
            return result;
        }

        private HashSet<Point> put8Pixels(int a, int b, int x, int y, int widthLine)
        {
            HashSet<Point> result = new HashSet<Point>();
            result.UnionWith(putpixels(x + a, y + b, widthLine));

            result.UnionWith(putpixels(x + a, b - y, widthLine));

            result.UnionWith(putpixels(a - x, y + b, widthLine));

            result.UnionWith(putpixels(a - x, b - y, widthLine));

            result.UnionWith(putpixels(y + a, x + b, widthLine));

            result.UnionWith(putpixels(a - y, x + b, widthLine));

            result.UnionWith(putpixels(a + y, b - x, widthLine));

            result.UnionWith(putpixels(a - y, b - x, widthLine));
            return result;
        }
        private HashSet<Point> drawCircle(int x1, int y1, int x2, int y2, int widthLine)
        {
            HashSet<Point> result = new HashSet<Point>();

            double R;
            int deltaX = x2 - x1;
            int deltaY = y2 - y1;

            if (Math.Abs(deltaX) < Math.Abs(deltaY))
            {
                R = Math.Abs(deltaX) / 2;
            }
            else
                R = Math.Abs(deltaY) / 2;

            int x, y, p, a, b;

            if (deltaY < 0)
            {
                if (deltaX < 0)
                {
                    a = Convert.ToInt32(x1 - R);
                    b = Convert.ToInt32(y1 - R);
                }
                else
                {
                    a = Convert.ToInt32(x1 + R);
                    b = Convert.ToInt32(y1 - R);
                }
            }
            else
            {
                if (deltaX < 0)
                {
                    a = Convert.ToInt32(x1 - R);
                    b = Convert.ToInt32(y1 + R);
                }
                else
                {
                    a = Convert.ToInt32(x1 + R);
                    b = Convert.ToInt32(y1 + R);
                }
            }
            x = 0;
            y = Convert.ToInt32(R);
            result.UnionWith(put8Pixels(a, b, x, y, widthLine));
            p = Convert.ToInt32(5 / 4 - R);
            while (x < y)
            {
                if (p < 0)
                    p += 2 * x + 3;
                else
                {
                    p += 2 * (x - y) + 5;
                    y--;
                }
                x++;
                result.UnionWith(put8Pixels(a, b, x, y, widthLine));
            }
            return result;
        }

        class AffineTransform
        {
            public double[,] _matrixTransform = new double[3, 3];
            public int Ox,Oy;
            public AffineTransform()
            {
                Ox = 0;
                Oy = 0;
                _matrixTransform[0, 0] = 1;
                _matrixTransform[1, 1] = 1;
                _matrixTransform[2, 2] = 1;
            }
            public void Translate(double dx, double dy)
            {
                double[,] newMatrix = new double[3, 3];
                newMatrix[0, 0] = 1;
                newMatrix[1, 1] = 1;
                newMatrix[2, 2] = 1;
                newMatrix[0, 2] = dx;
                newMatrix[1, 2] = dy;
                Array.Copy(MulMatrix(_matrixTransform,3,newMatrix,3,3),_matrixTransform,9);
            }
            double[,] MulMatrix(double[,] firstMatrix,int h, double[,] secondMatrix, int w,int subw) {
                double[,] result = new double[h, w];
                for (int i = 0; i < h; i++) {
                    for (int j = 0; j < w; j++) {
                        for (int k = 0; k < subw; k++) {
                            result[i, j] += firstMatrix[i, k] * secondMatrix[k, j];
                        }
                    }
                }
                return result;
            }
            public void Rotate(double angle)
            {
                double[,] newMatrix = new double[3, 3];
                newMatrix[0, 0] = Math.Cos(angle * Math.PI / 180.0);

                newMatrix[1, 1] = Math.Cos(angle * Math.PI / 180.0);
                newMatrix[2, 2] = 1;
                newMatrix[0, 1] = -Math.Sin(angle * Math.PI / 180.0);
                newMatrix[1, 0] = Math.Sin(angle * Math.PI / 180.0);
                Array.Copy(MulMatrix(_matrixTransform,3,newMatrix,3,3),_matrixTransform,9);
            }
            public void Scale(double sx, double sy)
            {
                double[,] newMatrix = new double[3, 3];
                newMatrix[0, 0] = sx;
                newMatrix[1, 1] = sy;
                newMatrix[2, 2] = 1;
                Array.Copy(MulMatrix(_matrixTransform,3,newMatrix,3,3),_matrixTransform,9);
            }
            public void TransformPoint(ref double x, ref double y)
            {
                double[,] newMatrix = new double[3, 1];
                newMatrix[0, 0] = x-Ox;
                newMatrix[1, 0] = y-Oy;
                newMatrix[2, 0] = 1;
                Array.Copy(MulMatrix(_matrixTransform,3, newMatrix,1,3), newMatrix, 3);
                x = newMatrix[0, 0]+Ox;
                y = newMatrix[1, 0]+Oy;
            }
            public void ReservedTransformPoint(ref double x, ref double y)
            {
                double[,] newMatrix = new double[3, 1];
                newMatrix[0, 0] = x-Ox;
                newMatrix[1, 0] = y-Oy;
                newMatrix[2, 0] = 1;
                Array.Copy(MulMatrix(InverseMatrix(_matrixTransform),3, newMatrix,1,3), newMatrix, 3);
                x = newMatrix[0, 0]+Ox;
                y = newMatrix[1, 0]+Oy;
            }
            public HashSet <Point> Transform(HashSet<Point> points)
            {
                HashSet<Point> result = new HashSet<Point>();
                int minx = points.OrderBy(x => x.X).First().X,
                    maxx = points.OrderBy(x => x.X).Last().X,
                    miny = points.OrderBy(x => x.Y).First().Y,
                    maxy = points.OrderBy(x => x.Y).Last().Y;
                double mindx = minx, mindy = miny, maxdx = maxx, maxdy = maxy,px,py;
                px = mindx; py = mindy;
                TransformPoint(ref px,ref py);
                minx = Convert.ToInt32(px);
                maxx = Convert.ToInt32(px);
                miny = Convert.ToInt32(py);
                maxy = Convert.ToInt32(py);
                px = mindx;py = maxdy;
                TransformPoint(ref px, ref py);
                minx = (Convert.ToInt32(px) > minx) ? minx : Convert.ToInt32(px);
                maxx = (Convert.ToInt32(px) > maxx) ? Convert.ToInt32(px) : maxx;
                miny = (Convert.ToInt32(py) > miny) ? miny : Convert.ToInt32(py);
                maxy = (Convert.ToInt32(py) > maxy) ? Convert.ToInt32(py) : maxy;
                px = maxdx; py = mindy;
                TransformPoint(ref px, ref py);
                minx = (Convert.ToInt32(px) > minx) ? minx : Convert.ToInt32(px);
                maxx = (Convert.ToInt32(px) > maxx) ? Convert.ToInt32(px) : maxx;
                miny = (Convert.ToInt32(py) > miny) ? miny : Convert.ToInt32(py);
                maxy = (Convert.ToInt32(py) > maxy) ? Convert.ToInt32(py) : maxy;
                px = maxdx; py = maxdy;
                TransformPoint(ref px, ref py);
                minx = (Convert.ToInt32(px) > minx) ? minx : Convert.ToInt32(px);
                maxx = (Convert.ToInt32(px) > maxx) ? Convert.ToInt32(px) : maxx;
                miny = (Convert.ToInt32(py) > miny) ? miny : Convert.ToInt32(py);
                maxy = (Convert.ToInt32(py) > maxy) ? Convert.ToInt32(py) : maxy;
                int ox = minx, oy = miny,
                    h = maxy - miny+1, w = maxx - minx+1;
                for (int i = 0; i < h; i++)
                {
                    for (int j = 0; j < w; j++)
                    {
                        double dx = ox + j, dy = oy + i;
                        ReservedTransformPoint(ref dx, ref dy);
                        int x = Convert.ToInt32(dx), y = Convert.ToInt32(dy);
                        if (points.Contains(new Point(x, y)))
                        {
                            result.Add(new Point(ox+j,oy+i));
                        }
                    }
                }
                return result;
            }
            double[,] InverseMatrix(double[,] matrix)
            {
                double det = matrix[0, 0] * (matrix[1, 1] * matrix[2, 2] - matrix[2, 1] * matrix[1, 2]) -
            matrix[0, 1] * (matrix[1, 0] * matrix[2, 2] - matrix[1, 2] * matrix[2, 0]) +
            matrix[0, 2] * (matrix[1, 0] * matrix[2, 1] - matrix[1, 1] * matrix[2, 0]);

                double invdet = 1 / det;

                double[,] minv = new double[3, 3];
                minv[0, 0] = (matrix[1, 1] * matrix[2, 2] - matrix[2, 1] * matrix[1, 2]) * invdet;
                minv[0, 1] = (matrix[0, 2] * matrix[2, 1] - matrix[0, 1] * matrix[2, 2]) * invdet;
                minv[0, 2] = (matrix[0, 1] * matrix[1, 2] - matrix[0, 2] * matrix[1, 1]) * invdet;
                minv[1, 0] = (matrix[1, 2] * matrix[2, 0] - matrix[1, 0] * matrix[2, 2]) * invdet;
                minv[1, 1] = (matrix[0, 0] * matrix[2, 2] - matrix[0, 2] * matrix[2, 0]) * invdet;
                minv[1, 2] = (matrix[1, 0] * matrix[0, 2] - matrix[0, 0] * matrix[1, 2]) * invdet;
                minv[2, 0] = (matrix[1, 0] * matrix[2, 1] - matrix[2, 0] * matrix[1, 1]) * invdet;
                minv[2, 1] = (matrix[2, 0] * matrix[0, 1] - matrix[0, 0] * matrix[2, 1]) * invdet;
                minv[2, 2] = (matrix[0, 0] * matrix[1, 1] - matrix[1, 0] * matrix[0, 1]) * invdet;

                return minv;
            }
            public void SetOxy(int newOx,int newOy)
            {
                Ox = newOx;
                Oy = newOy;
            }
        }

        private void SetLineWidthUI_ValueChanged(object sender, EventArgs e)
        {
            CurrentCustom.LineWidth= Convert.ToInt32(decimal.ToSingle(SetLineWidthUI.Value))*CurrentState.UnitOfLineWidth;
        }

        private void ClearScreen_Click(object sender, EventArgs e)
        {
            ListOfInstances.Clear();
            CurrentState.DrawTime = 0;
            DrawTimer.Stop();
            DrawTimeLabel.Text = "Draw Time: 0 s";
            CurrentState.IsMoveShape = false;
            CurrentState.IsFill = false;
        }

        private void SetFillColorButton_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                CurrentCustom.ColorFill = colorDialog1.Color;
            }
            CurrentState.IsFill = true;
            CurrentState.IsMoveShape = false;
        }

        private HashSet<Point> pickShape(Point p, List<Shape> allShape)
        {
            const int distance = 5;//khoảng cách cho phép;
            for(int i=0; i<allShape.Count();i++)
                foreach(Point tempP in allShape[i].PixelsInLine)
                {
                    if (Math.Pow(tempP.X - p.X, 2) + Math.Pow(tempP.Y - p.Y, 2) <= Math.Pow((distance + allShape[i].Properties.LineWidth), 2))
                    {
                        allShape[i].IsChosen = true;
                        savedPointInArea = allShape[i].PixelsInArea;
                        return allShape[i].PixelsInLine;
                    }
                }
            return new HashSet<Point>();
        }

        private void MoveButton_Click(object sender, EventArgs e)
        {
            CurrentState.IsMoveShape = true;
            CurrentState.IsFill = false;
        }

        private void MoveShape(Point start,Point end)
        {
            if (CurrentState.IsMoveShape == false) return;

            AffineTransform affine =new AffineTransform();
            affine.Translate(end.X - start.X, end.Y - start.Y);

            foreach (Shape currShape in ListOfInstances)
                if (currShape.IsChosen == true)
                {
                    currShape.PixelsInLine = affine.Transform(savedPointInLine);
                    currShape.PixelsInArea = affine.Transform(savedPointInArea);
                }

        }

        private void DrawCircleButton_Click(object sender, EventArgs e)
        {
            CurrentState.Choose = "DrawCircle";
            DrawTimeLabel.Text = "Draw Time: 0 s";
            CurrentState.IsMoveShape = false;
            CurrentState.IsFill = false;
        }

        private void DrawEquilTriangleButton_Click(object sender, EventArgs e)
        {
            CurrentState.Choose = "DrawEquilTriangle";
            DrawTimeLabel.Text = "Draw Time: 0 s";
            CurrentState.IsMoveShape = false;
            CurrentState.IsFill = false;
        }


        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ tô màu

        private byte[] getPixels(int x, int y)
        {
            OpenGL gl = openGLControl.OpenGL;
            byte[] temp = new byte[3];
            gl.ReadPixels(x, y, 1, 1, OpenGL.GL_RGB, OpenGL.GL_UNSIGNED_BYTE, temp);
            return temp;
        }

        private void setPixels(int x, int y, Color color)
        {
            OpenGL gl = openGLControl.OpenGL;
            gl.Color(color.R / 255.0, color.G / 255.0, color.B / 255.0, 0);
            gl.Begin(OpenGL.GL_POINTS);
            gl.Vertex(x, y);
            gl.End();
            gl.Flush();
        }

        private byte checkPointOnEdge(Point p, HashSet<Point> shape)
        {
            foreach (Point temp in shape)
            {
                if (p.X == temp.X && p.Y == temp.Y)
                    return 1;
            }
            return 0;
        }

        private byte checkPointInShape(Point p,HashSet<Point>shape)//hàm kiểm tra điểm nằm trong hình chữ nhật bao hình.
        {
            const int MAX_VALUE = 1000000;
            const int MIN_VALUE = 0;
            int xmax=MIN_VALUE,ymax=MIN_VALUE;
            int xmin = MAX_VALUE, ymin = MAX_VALUE;

            foreach(Point temp in shape)
            {
                if (temp.X > xmax)
                    xmax = temp.X;
                if (temp.X < xmin)
                    xmin = temp.X;
                if (temp.Y > ymax)
                    ymax = temp.Y;
                if (temp.Y < ymin)
                    ymin = temp.Y;
            }

            if (p.X >= xmin && p.X <= xmax && p.Y >= ymin && p.Y <= ymax)
                return 1;
            return 0;
        }

        private void floodFill(int x, int y)
        {
            List<Point> pixels = new List<Point>();
            Shape temp_s = new Shape();
            OpenGL gl = openGLControl.OpenGL;

            int[,] checkFill = new int[gl.RenderContextProvider.Width + 1, gl.RenderContextProvider.Height + 1];
            for (int i = 0; i <= gl.RenderContextProvider.Width; i++)
            {
                for (int j = 0; j <= gl.RenderContextProvider.Height; j++)
                {
                    checkFill[i, j] = 0;
                }
            }

            int index = 0;
            foreach(Shape s in ListOfInstances)
            {
                if(checkPointInShape(new Point(x,y),s.PixelsInLine)==1)
                {
                    temp_s = s;
                    s.Properties.ColorFill = CurrentCustom.ColorFill;
                    break;
                }
                index++;
            }

            Point temp = new Point(x, y);
            pixels.Add(temp);
            while (pixels.Count() > 0)
            {
                Point a = pixels[0];
                pixels.RemoveAt(0);

                if ((a.X <= gl.RenderContextProvider.Width && a.X >= 0 && a.Y <= gl.RenderContextProvider.Height && a.Y >= 0)
                    &&checkPointOnEdge(a,temp_s.PixelsInLine)==0)
                {
                    if (checkFill[a.X, a.Y] == 0)
                    {
                        temp_s.PixelsInArea.Add(a);
                        checkFill[a.X, a.Y] = 1;
                        pixels.Add(new Point(a.X + 1, a.Y));
                        pixels.Add(new Point(a.X, a.Y + 1));
                        pixels.Add(new Point(a.X - 1, a.Y));
                        pixels.Add(new Point(a.X, a.Y - 1));
                    }
                }
            }
            ListOfInstances[index] = temp_s;
        }
    }
}