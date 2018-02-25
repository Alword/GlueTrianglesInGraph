using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TriangleUI.Models;

namespace TriangleUI
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int MARGIN = 30;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point pos0 = new Point
            {
                X = (int)Math.Round(e.MouseDevice.GetPosition(sender as IInputElement).X),
                Y = (int)Math.Round(e.MouseDevice.GetPosition(sender as IInputElement).Y)
            };


            ////R(pos1, pos2) = sqrt((x1-x2)^2+(y1-y2)^2)
            foreach (UIElement child in MainCanvas.Children)
            {
                if (child is Ellipse)
                {
                    var hwnd = child as Ellipse;

                    Point pos1 = new Point
                    {
                        X = Canvas.GetLeft(hwnd) + hwnd.Width / 2,
                        Y = Canvas.GetTop(hwnd) + hwnd.Width / 2
                    };
                    if (R(pos0, pos1) < MARGIN)
                    {
                        e.Handled = true;
                        return;
                    }
                }
            }
            new Models.Shapes.Dot((int)pos0.X, (int)pos0.Y, MainCanvas);
        }

        private int R(Point pos1, Point pos2)
        {
            return (int)Math.Round(Math.Sqrt(Math.Pow(pos1.X - pos2.X, 2) + Math.Pow(pos1.Y - pos2.Y, 2)));
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Models.Shapes.Dot.DotList != null)
            {
                IsEnabled = false;
                while (await Bonder.Start(Models.Shapes.Dot.DotList, MainCanvas)) ;
            }
            IsEnabled = true;
        }
    }
}
