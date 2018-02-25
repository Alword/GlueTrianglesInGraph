using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TriangleUI.Models.Shapes;
using System.Windows.Media;
using System.Windows.Threading;

namespace TriangleUI.Models
{
    class Bonder<T> where T : Interfaces.ILinkedList<T>
    {
        public void Start(List<T> dots)
        {
            //foreach()
        }
    }

    public class Bonder
    {
        public static async Task<bool> Start(IReadOnlyList<Dot> dots, Canvas canvas)
        {
            foreach (var dt in dots)
            {
                if (dt.LinkedList.Count > 0)
                {
                    foreach (var dt2 in dt.LinkedList)
                    {
                        foreach (var dt3 in dt2.LinkedList)
                        {
                            if (dt3.LinkedList.IndexOf(dt) >= 0)
                            {
                                return await Bondeling(dt, dt2, dt3, canvas);
                            }
                        }
                    }
                }
            }
            return false;
        }

        private static async Task<bool> Bondeling(Dot dt1, Dot dt2, Dot dt3, Canvas canvas)
        {

            dt1.SetColor = Colors.Violet;
            dt2.SetColor = Colors.Violet;
            dt3.SetColor = Colors.Violet;

            await Task.Delay(new TimeSpan(0, 0, 1));

            List<Dot> toLink = new List<Dot>();
            int newX = (dt1.PosX);
            int newY = (dt1.PosY);
            toLink = toLink.Union(dt1.LinkedList).Union(dt2.LinkedList).Union(dt3.LinkedList).ToList();
            toLink.Remove(dt1);
            toLink.Remove(dt2);
            toLink.Remove(dt3);
            dt1.Erase();
            dt2.Erase();
            dt3.Erase();

            var poligonDot = new Dot(newX, newY, canvas);
            foreach (var dt in toLink)
            {
                Dot.Establish(dt, poligonDot, canvas);
            }
            return true;
        }
    }
}
