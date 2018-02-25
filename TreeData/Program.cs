using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeData
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Дерево отрезков - структура полезна когда необходимо часто искать значение какой-то функции на отрезках линейного массива и иметь возможность быстро изменять значения группы подряд идущих элементов.");
            Console.WriteLine();

            SegmentTree IntTree = GetSeries();
            ConsoleWriteTree(IntTree);

            Console.WriteLine();
            Console.WriteLine("Быстрая функция - минимум на отрезке");
            MinInRangeTest(IntTree);

            Console.WriteLine("Проверка возможности быстро изменить значение");
            ChangeInIndext(IntTree);
            ConsoleWriteTree(IntTree);

            Console.WriteLine("Быстрая функция - минимум на отрезке");
            MinInRangeTest(IntTree);

            Console.ReadLine();
        }

        private static SegmentTree GetSeries()
        {
            Console.WriteLine("Вводите числа через пробел");
            string[] Input = Console.ReadLine().ToString().Split(' ');
            List<int> Valide;
            if (!(Input.Length == 1 && Input[0] == ""))
            {
                Valide = new List<int>(Input.Length);
                foreach (string x in Input)
                {
                    if (int.TryParse(x, out int num))
                    {
                        Valide.Add(num);
                    }
                    else
                    {
                        Notification($"'{x}' - имеет неверный формат и не будет обработан");
                    }
                }
                Console.WriteLine();

            }
            else
            {
                int maxRnd = 99;
                int count = 10;
                Notification("генератор случайных чисел");
                Random rnd = new Random();
                Valide = new List<int>(count);
                for (int i = 0; i < count; i++)
                {
                    Valide.Add(rnd.Next(maxRnd));
                }
            }
            return new SegmentTree(Valide.ToArray());

        }

        private static void GetRange(out int minIndex, out int maxIndex)
        {
            Console.Write("Введите левую границу отрезка: ");
            int.TryParse(Console.ReadLine(), out minIndex);
            Console.Write("Введите правую границу отрезка: ");
            int.TryParse(Console.ReadLine(), out maxIndex);
            Console.WriteLine($"Левая {minIndex++}\n" +
                              $"Правая {maxIndex++}\n");
        }

        private static void GetUpdate(out int index, out int val)
        {
            Console.Write("Введите индекс: ");
            int.TryParse(Console.ReadLine(), out index);
            Console.Write("Введите значение: ");
            int.TryParse(Console.ReadLine(), out val);

        }

        private static void ConsoleWriteTree(SegmentTree tree)
        {
            int index = 0;
            foreach (var x in tree)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"{x}");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($"[{index++}] ");
                Console.ForegroundColor = ConsoleColor.Gray;
            }

            Console.WriteLine();
            Console.WriteLine();

        }

        private static void Notification(string note)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"#Внимание: {note}");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        //TEST
        private static void MinInRangeTest(SegmentTree IntTree)
        {
            GetRange(out int minIndex, out int maxIndex);
            try
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"Минимум на отрезке [{minIndex - 1},{maxIndex - 1}] - {IntTree.FindMinBetween(minIndex, maxIndex)}");

            }
            catch (IndexOutOfRangeException e)
            {
                Notification(e.Message);
            }
            finally
            {
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            Console.WriteLine();
        }

        private static void ChangeInIndext(SegmentTree IntTree)
        {
            GetUpdate(out int indexToUpdate, out int valToUpdate);
            IntTree[indexToUpdate] = valToUpdate;

        }

    }
}