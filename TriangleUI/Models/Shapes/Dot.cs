using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media;

namespace TriangleUI.Models.Shapes
{
    public partial class Dot : Interfaces.ILinkedList<Dot>
    {
        private static List<Dot> dotsList;

        private const int SIZE = 20;

        private static Dot dotBuffer;

        private Ellipse shape;

        private Canvas canvas;

        private bool _isActive = false;

        public Dot(int _posX, int _posY, Canvas _canvas)
        {
            this.PosX = _posX;
            this.PosY = _posY;
            this.canvas = _canvas;
            LinkedDictinary = new Dictionary<Dot, Line>(0);
            Draw();

            if (dotsList == null)
            {
                dotsList = new List<Dot>(0);
            }
            dotsList.Add(this);
        }

        public static IReadOnlyList<Dot> DotList
        {
            get
            {
                return dotsList?.AsReadOnly();
            }
        }

        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                _isActive = value;
                if (value)
                {
                    shape.Fill = new SolidColorBrush(Colors.Red);
                }
                else
                {
                    shape.Fill = new SolidColorBrush(Colors.Yellow);
                }
            }
        }

        public int PosX { get; }

        public int PosY { get; }

        public Dictionary<Dot, Line> LinkedDictinary { get; set; }

        public List<Dot> LinkedList
        {
            get
            {
                return LinkedDictinary.Keys.ToList();
            }
        }

        public Color SetColor
        {
            set
            {
                shape.Fill = new SolidColorBrush(value);
            }
        }
    }

    public partial class Dot
    {
        private void Shape_MouseRightButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Erase();
        }

        private void Shape_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (dotBuffer == null)
            {
                dotBuffer = this;
                IsActive = true;
            }
            else if (dotBuffer == this)
            {
                dotBuffer = null;
                IsActive = false;
            }
            else
            {
                Establish(this, dotBuffer, canvas);
            }
        }
    }

    public partial class Dot
    {
        public void Draw()
        {
            if (shape == null)
            {
                shape = new Ellipse
                {
                    Width = SIZE,
                    Height = SIZE,
                    Stroke = new SolidColorBrush(Colors.Black),
                    StrokeThickness = 1,
                    Fill = new SolidColorBrush(Colors.Yellow)
                };
                shape.MouseRightButtonUp += Shape_MouseRightButtonUp;
                shape.PreviewMouseLeftButtonDown += Shape_PreviewMouseLeftButtonUp;

                Canvas.SetLeft(shape, PosX - SIZE / 2);
                Canvas.SetTop(shape, PosY - SIZE / 2);
                Canvas.SetZIndex(shape, 1);
                canvas.Children.Add(shape);
            }
        }

        public List<Dot> Erase()
        {
            if (dotBuffer == this)
                dotBuffer = null;

            List<Dot> linked = new List<Dot>(0);

            foreach (var linkedRow in LinkedDictinary)
            {
                canvas.Children.Remove(linkedRow.Value);
                linkedRow.Key.LinkedDictinary.Remove(this);
                linked.Add(linkedRow.Key);
            }
            LinkedDictinary.Clear();

            canvas.Children.Remove(shape);

            dotsList.Remove(this);

            return linked;
        }

        public static void Establish(Dot dot1, Dot dot2, Canvas canvas)
        {
            if (!dot1.LinkedDictinary.ContainsKey(dot2))
            {
                Line line = new Line
                {
                    Stroke = new SolidColorBrush(Colors.Black),
                    StrokeThickness = 1,
                    X1 = dot1.PosX,
                    Y1 = dot1.PosY,
                    X2 = dot2.PosX,
                    Y2 = dot2.PosY
                };
                canvas.Children.Add(line);

                dot2.LinkedDictinary.Add(dot1, line);
                dot1.LinkedDictinary.Add(dot2, line);
            }
        }
    }

}
