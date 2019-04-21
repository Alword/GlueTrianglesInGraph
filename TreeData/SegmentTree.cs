using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeData
{
    public class SegmentTree : IEnumerable
    {
        private const int INF = int.MaxValue;

        private int[] ThreeData;

        public int Count { get; }

        public SegmentTree(params int[] input)
        {
            Count = input.Length;
            // размер, доведённый до степени двойки
            int power = (int)Math.Ceiling(Math.Log(Count - 1) / Math.Log(2));
            int n = 1 << power;
            ThreeData = new int[2 * n];
            // инициализируем листы
            for (int i = n; i < 2 * n; i++)
                ThreeData[i] = (i - n < Count) ? input[i - n] : INF;

            // и все остальные вершины
            for (int i = n - 1; i > 0; i--)
                ThreeData[i] = this.Min(ThreeData[2 * i], ThreeData[2 * i + 1]);
        }

        public int this[int index]//Indexer define 
        {
            get
            {
                return ThreeData[index + Count];
            }
            set
            {
                int leafIndex = index + ThreeData.Length / 2;
                if (leafIndex < ThreeData.Length)
                {
                    ThreeData[leafIndex] = value;
                    Update(leafIndex, value);
                }
                else throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Обновление данных (снизу вверх) с перерасчётом функции
        /// </summary>
        /// <param name="i">Индекс</param>
        /// <param name="x">Значение</param>
        private void Update(int i, int x)
        {
            ThreeData[i] = x;
            while ((i /= 2) > 0)
                ThreeData[i] = Min(ThreeData[2 * i], ThreeData[2 * i + 1]);
        }

        private int Min(int p1, int p2)
        {
            return (p1 < p2) ? p1 : p2;
        }

        public int FindMinBetween(int minIndex, int maxIndex)
        {
            if (minIndex > 0 && maxIndex <= Count)
            {
                int ans = INF;
                int n = ThreeData.Length / 2;//интекс первого листа
                minIndex += n - 1;
                maxIndex += n - 1;
                while (minIndex <= maxIndex)
                {
                    // если l - правый сын своего родителя, 
                    // учитываем его фундаментальный отрезок
                    if ((minIndex & 1) == 1)
                        ans = this.Min(ans, ThreeData[minIndex]);

                    // если r - левый сын своего родителя, 
                    // учитываем его фундаментальный отрезок
                    if ((maxIndex & 1) == 0)
                        ans = this.Min(ans, ThreeData[maxIndex]);
                    // сдвигаем указатели на уровень выше
                    minIndex = (minIndex + 1) / 2;
                    maxIndex = (maxIndex - 1) / 2;
                }
                return ans;
            }
            else
            {
                throw new IndexOutOfRangeException();
            }

        }

        public IEnumerator GetEnumerator()
        {
            return ThreeData.Skip(ThreeData.Length / 2).Take(Count).GetEnumerator();
        }
    }
}
