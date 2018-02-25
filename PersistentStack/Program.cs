using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersistentStack;

namespace PersistentStackProg
{

    class Program
    {
        static PersistentStack<string> TestStack;

        static int LastUserVersion = 0;

        static void Main(string[] args)
        {
            TestStack = new PersistentStack<string>();
            TestStack.StackEventHandler += X_StackEventHandler; ;
            Menu();
        }

        private static void X_StackEventHandler(object sender, object Data, int version, StackEvents x)
        {
            Console.WriteLine("============================");
            //Console.WriteLine($"Стек - {sender.GetHashCode()}");
            Console.WriteLine($"Действие - {x}");
            Console.WriteLine($"Данные - {Data}");
            Console.WriteLine($"Версия - {version}");
            Console.WriteLine("============================");
            LastUserVersion = TestStack.VersionsCount;
        }

        private static void Menu()
        {
            bool showMenu = true;

            while (showMenu)
            {
                Console.Clear();
                TestWriteAll();

                Console.WriteLine();

                Console.WriteLine("1] Добавить в стек");
                Console.WriteLine("2] Взять из стека");

                string x = Console.ReadLine();
                switch (x)
                {
                    case "1": TestPush(); break;
                    case "2": TestPop(); break;
                    default: showMenu = false; break;
                }
                Console.WriteLine("Нажмите любую кнопку чтобы продолжить...");
                Console.ReadKey();
            }
        }

        private static void TestPop()
        {
            Console.Write("Введите версию стека ");
            int.TryParse(Console.ReadLine(), out int version);
            TestStack.Pop(version);

        }

        private static void TestPush()
        {
            Console.Write("Введите данные ");
            string Data = Console.ReadLine();


            Console.Write("Введите версию стека ");
            int.TryParse(Console.ReadLine(), out int version);

            TestStack.Push(Data, version);
        }

        private static void TestWriteAll()
        {
            for (int i = 1; i <= TestStack.VersionsCount; i++)
            {
                WriteVersion(i);
            }
        }

        private static void WriteVersion(int version)
        {
            Console.Write($"Версия стека {version}/{TestStack.VersionsCount}: ");
            var list = TestStack.ToList(version);
            foreach (var x in list)
            {
                Console.Write($"{x} ");
            }
            Console.WriteLine();

        }
    }
}
