using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistentStack
{
    public class PersistentStack<T>
    {
        const int SIZE = 1023;
        private int CountVersions = 1;
        private PersistentStackNode<T>[] stack = null;

        public int VersionsCount { get => CountVersions - 1; }

        public delegate void StackEvents(object sender, object Data, int version, PersistentStack.StackEvents x);

        public event StackEvents StackEventHandler;

        public PersistentStack()
        {
            stack = new PersistentStackNode<T>[1023];
        }

        public void Push(T Data, int version = 0)
        {
            version = LastOrDefault(version);
            stack[CountVersions++] = new PersistentStackNode<T>(Data, stack[version]);

            //Event
            StackEventHandler?.Invoke(this, Data, version, PersistentStack.StackEvents.Push);
        }

        public T Pop(int version = 0)
        {
            version = LastOrDefault(version);

            stack[CountVersions] = stack[version]?.Linked;
            CountVersions++;

            //Event
            StackEventHandler?.Invoke(this, stack[version].Data, version, PersistentStack.StackEvents.Push);

            return stack[version].Data;
        }

        public List<T> ToList(int version)
        {
            version = LastOrDefault(version);
            List<T> result = new List<T>();

            PersistentStackNode<T> reader = stack[version];
            while (reader != null)
            {
                result.Add(reader.Data);
                reader = reader.Linked;
            }
            return result;
        }

        private int LastOrDefault(int version)
        {
            return ((version < 1) || version > CountVersions) ? CountVersions - 1 : version;
        }

    }

    public enum StackEvents
    {
        Pop,
        Push
    }

    public class PersistentStackNode<T>
    {
        public T Data { get; }
        public PersistentStackNode<T> Linked { get; }
        public PersistentStackNode(T Data, PersistentStackNode<T> linked)
        {
            this.Data = Data;
            this.Linked = linked;
        }

        public PersistentStackNode()
        {

        }
    }
}
