using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exe
{
    
    // Ячейка для хэш-таблиц
    internal class Cell<TKey, TValue>
    {
        internal TValue value;
        internal TKey key;
        internal Cell(TKey key, TValue value)
        {
            this.value = value;
            this.key = key;
        }
        public override string ToString() => $"key {key}, value {value}";
    }
    // Хэш-таблица с разрешением коллизий методом цепочек
    public class MyDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        private readonly LinkedList<Cell<TKey, TValue>>[] buckets;
        public MyDictionary(int dictLenght = 200)
        {
            buckets = new LinkedList<Cell<TKey, TValue>>[dictLenght];
            for (int i = 0; i < buckets.Length; i++)
                buckets[i] = new LinkedList<Cell<TKey, TValue>>();
        }

        // Add, Remove pairs
        public void Add(TKey key, TValue value)
        {
            if (IsHaveValue(key))
                throw new Exception("This key yet contains in dictionary");
            var cell = GetList(key);
            cell.AddFirst(new Cell<TKey, TValue>(key, value));
        }
        public void Remove(TKey key)
        {
            if (!IsHaveValue(key)) 
                throw new Exception("This key not contains in dictionary");
            var cell = GetList(key);
            var pair = GetKeyValuePair(key);
            cell.Remove(pair);
        }

        // Get, Set value
        public void Set(TKey key, TValue value) => GetKeyValuePair(key).value = value;
        public TValue Get(TKey key) => GetKeyValuePair(key).value;
        public TValue this[TKey key]
        {
            get => Get(key);
            set => Set(key, value);
        }
        public bool IsHaveValue(TKey key)
        {
            var list = GetList(key);
            foreach (var cell in list)
                if (cell.key?.Equals(key) ?? false)
                    return true;
            return false;
        }

        // Find key value pairs
        private Cell<TKey, TValue> GetKeyValuePair(TKey key)
        {
            var list = GetList(key);
            foreach (var cell in list)
                if (cell.key?.Equals(key) ?? false)
                    return cell;
            // if key is not found
            throw new Exception("Key not found");
        }
        private LinkedList<Cell<TKey, TValue>> GetList(TKey key)
        {
            int hash = key?.GetHashCode() ?? throw new Exception("Key mustn't be null");
            int bucketNum = (hash & 0x7fffffff) % buckets.Length;
            return buckets[bucketNum];
        }

        // Interface
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            foreach (var bucket in buckets)
                foreach (var pair in bucket)
                    yield return new KeyValuePair<TKey, TValue>(pair.key, pair.value);
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    // Хэш-таблица с разрешением коллизий методом открытой адресации
    public class MyDictionary2<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        private readonly Cell<TKey, TValue>[] buckets;
        public MyDictionary2(int dictLenght = 200) => buckets = new Cell<TKey, TValue>[dictLenght];

        // Add, Remove pairs
        public void Add(TKey key, TValue value)
        {
            if (IsHaveValue(key)) 
                throw new Exception("This key yet contains in dictionary or hash-map overfilled");
            GetKeyValuePair(key, out int ind);
            buckets[ind] = new Cell<TKey, TValue>(key, value);
        }
        public void Remove(TKey key)
        {
            if (!IsHaveValue(key)) 
                throw new Exception("This key not contains in dictionary");
            GetKeyValuePair(key, out int ind);
            buckets[ind] = null;
        }

        // Get, Set value
        public void Set(TKey key, TValue value)
        {
            GetKeyValuePair(key).value = IsHaveValue(key) ? value : throw new Exception("Key not found");
        }
        public TValue Get(TKey key)
        {
            return IsHaveValue(key) ? GetKeyValuePair(key).value : throw new Exception("Key not found");
        }

        public TValue this[TKey key]
        {
            get => Get(key);
            set => Set(key, value);
        }
        public bool IsHaveValue(TKey key)
        {
            try
            {
                return GetKeyValuePair(key) != null;
            }
            catch
            {
                return false;
            }
        }

        // Find key value pairs
        private Cell<TKey, TValue> GetKeyValuePair(TKey key, out int index)
        {
            int hash = key?.GetHashCode() ?? throw new Exception("Key mustn't be null");
            int bucketNum = (hash & 0x7fffffff) % buckets.Length;

            int offsetCounter = 0;
            while (offsetCounter < buckets.Length)
            {
                if (buckets[bucketNum]?.key?.Equals(key) ?? true)   // либо пустой, либо равный ключу
                {
                    index = bucketNum;
                    return buckets[bucketNum];
                }
                bucketNum = (bucketNum + 1) % buckets.Length;
                offsetCounter++;
            }
            throw new Exception("Key not found");
        }
        private Cell<TKey, TValue> GetKeyValuePair(TKey key) => GetKeyValuePair(key, out int _);
        // Interface
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            foreach (var pair in buckets)
                yield return new KeyValuePair<TKey, TValue>(pair.key, pair.value);
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
