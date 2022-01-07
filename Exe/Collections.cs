using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exe
{
    public class Sequence<T> : IEnumerable<T>
    {
        protected class Node<TValue>
        {
            internal Node<TValue> next, previous;
            internal TValue value;
            public Node(TValue value, Node<TValue> next = null, Node<TValue> previous = null)
            {
                this.value = value;
                this.next = next;
                this.previous = previous;
            }
        }
        protected Node<T> head, tail;
        public int Count { get; private set; }
        protected void AddLast(T data)
        {
            Node<T> node = new Node<T>(data);

            if (head == null)
                head = node;
            else
            {
                tail.next = node;
                node.previous = tail;
            }
            tail = node;
            Count++;
        }
        protected void AddFirst(T data)
        {
            Node<T> node = new Node<T>(data);
            Node<T> temp = head;
            node.next = temp;
            head = node;
            if (Count == 0)
                tail = head;
            else
                temp.previous = node;
            Count++;
        }
        protected T PopFirst()
        {
            if (Count == 0) throw new Exception();
            T val = head.value;
            head = head.next;
            Count--;
            return val;
        }
        public void Clear()
        {
            head = tail = null;
            Count = 0;
        }
        public T this[int index]
        {
            get
            {
                if (index >= Count) throw new IndexOutOfRangeException();
                int counter = 0;
                foreach (var node in this)
                {
                    if (counter == index)
                        return node;
                    counter++;
                }
                throw new Exception();
            }
            set
            {
                if (index >= Count) throw new IndexOutOfRangeException();
                var current = head;
                for (int i = 1; i < index; i++)
                    current = current.next;
                current.value = value;
            }
        }
        public IEnumerator<T> GetEnumerator()
        {
            var current = head;
            while (current != null)
            {
                yield return current.value;
                current = current.next;
            }
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    // Стек на основе списка
    public class MyStack<T> : Sequence<T>
    {
        public void Push(T value) => AddFirst(value);
        public T Pop() => PopFirst();
        public T Peek() => this[0];
    }
    // Очередь на основе списка
    public class MyQueue<T> : Sequence<T>
    {
        public void Enqueue(T value) => AddLast(value);
        public T Dequeue() => PopFirst();
        public T Peek() => this[0];
    }
}
