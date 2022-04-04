using System;
using System.Collections;
using System.Collections.Generic;

namespace CustomSortedList
{
    public class MySortedList<TKey, TValue> :
        IEnumerable<KeyValuePair<TKey, TValue>>
        where TKey : IComparable<TKey>
    {
        private const double REDUCTION_CAPACITY_LIMIT = 0.9;

        private Node head;
        private Node tail;
        private KeysList keysList;
        private ValuesList valuesList;
        private int capacity = 4;
        private IComparer valueComparer = Comparer<TValue>.Default;

        public IComparer<TKey> Comparer { get; } = Comparer<TKey>.Default;
        public int Count { get; private set; } = 0;
        public int Capacity
        {
            get
            {
                return capacity;
            }
            set
            {
                if (value < Count) throw new ArgumentOutOfRangeException("Capacity must be greater than Count");
                capacity = value;
            }
        }
        public IList<TKey> Keys
        {
            get
            {
                return GetKeysListHelper();
            }
        }
        public IList<TValue> Values
        {
            get
            {
                return GetValuesListHelper();
            }
        }
        public TValue this[TKey key]
        {
            get
            {
                if (Count == 0)
                {
                    throw new NullReferenceException("MySortedList is empty");
                }
                else
                {
                    Node cursor = head;

                    while (cursor != null)
                    {
                        if (Comparer.Compare(cursor.key, key) == 0) return cursor.value;
                        cursor = cursor.next;
                    }

                    throw new KeyNotFoundException($"Key {key} does not exists");
                }
            }
            set
            {
                if (Count == 0)
                {
                    throw new NullReferenceException("MySortedList is empty");
                }
                else
                {
                    Node cursor = head;

                    while (cursor != null)
                    {
                        if (Comparer.Compare(cursor.key, key) == 0)
                        {
                            cursor.value = value;
                            return;
                        }
                        cursor = cursor.next;
                    }

                    throw new NullReferenceException($"Key {key} does not exists");
                }
            }
        }

        public delegate void MySortedListEventHandler(object sender, MySortedListEventArgs<TKey, TValue> args);

        public event MySortedListEventHandler Addition;
        public event MySortedListEventHandler Removal;
        public event MySortedListEventHandler Clearing;

        public MySortedList() { }

        public MySortedList(IComparer<TKey> comparer)
        {
            Comparer = comparer;
        }

        public MySortedList(IComparer<TKey> comparer, int capacity) : this(comparer)
        {
            Capacity = capacity;
        }

        public MySortedList(IDictionary<TKey, TValue> dict)
        {
            foreach (KeyValuePair<TKey, TValue> item in dict)
                Add(item.Key, item.Value);
        }

        public MySortedList(IComparer<TKey> comparer, IDictionary<TKey, TValue> dict): this(comparer)
        {
            foreach (KeyValuePair<TKey, TValue> item in dict)
                Add(item.Key, item.Value);
        }

        public MySortedList(int capacity)
        {
            Capacity = capacity;
        }

        protected void OnAddition(MySortedListEventArgs<TKey, TValue> args)
        {
            Addition?.Invoke(this, args);
        }

        protected void OnRemoval(MySortedListEventArgs<TKey, TValue> args)
        {
            Removal?.Invoke(this, args);
        }

        protected void OnClearing(MySortedListEventArgs<TKey, TValue> args)
        {
            Clearing?.Invoke(this, args);
        }

        private void SortByKeys()
        {
            if (head == null || head.next == null) return;

            bool isSorting = true;
            while (isSorting)
            {
                isSorting = false;
                Node cursor = head, prev = null;
                while (cursor != null)
                {
                    if (cursor.next != null)
                    {
                        if (Comparer.Compare(cursor.key, cursor.next.key) == 1)
                        {
                            isSorting = true;
                            Node temp = cursor, temp2 = cursor.next;
                            cursor.next = temp2.next;
                            temp2.next = temp;

                            if (prev != null) prev.next = temp2;
                            else head = temp2;

                            prev = temp2;
                        }
                        else
                        {
                            prev = cursor;
                            cursor = cursor.next;
                        }
                    }
                    else
                    {
                        tail = cursor;
                        break;
                    }
                }
            }
        }

        public void Add(TKey key, TValue value)
        {
            if (ContainsKey(key)) throw new ArgumentException($"Key {key} already exists");
            if (key == null) throw new ArgumentNullException("Key should be not null");

            Node node = new Node(key, value);
            if (head == null)
            {
                head = node;
                tail = node;
            }
            else
            {
                tail.next = node;
                tail = node;
            }
            Count++;
            if (Capacity <= Count) Capacity *= 2;

            SortByKeys();
            OnAddition(new MySortedListEventArgs<TKey, TValue>($"Added: {key} -> {value}", new KeyValuePair<TKey, TValue>(key, value)));
        }

        public void Clear()
        {
            Count = 0;
            head = tail = null;
            OnClearing(new MySortedListEventArgs<TKey, TValue>($"MySortedList was cleared"));
        }

        public bool ContainsKey(TKey key)
        {
            if (key == null) throw new ArgumentNullException("Key should be not null");
            Node cursor = head;

            while (cursor != null)
            {
                if (Comparer.Compare(cursor.key, key) == 0) return true;
                cursor = cursor.next;
            }

            return false;
        }

        public bool Contains(TKey key)
        {
            return ContainsKey(key);
        }

        public bool ContainsValue(TValue value)
        {
            Node cursor = head;

            while (cursor != null)
            {
                if (valueComparer != null && valueComparer.Compare(cursor.value, value) == 0) return true;
                else if (cursor.value.Equals(value)) return true;
                cursor = cursor.next;
            }

            return false;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            Node cursor = head;
            while (cursor != null)
            {
                yield return new KeyValuePair<TKey, TValue>(cursor.key, cursor.value);
                cursor = cursor.next;
            }
            //return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int IndexOfKey(TKey key)
        {
            if (key == null) throw new ArgumentNullException("Key should be not null");
            Node cursor = head;
            int index = 0;

            while (cursor != null)
            {
                if (Comparer.Compare(cursor.key, key) == 0) return index;
                cursor = cursor.next;
                index++;
            }

            return -1;
        }

        public int IndexOfValue(TValue value)
        {
            Node cursor = head;
            int index = 0;

            while (cursor != null)
            {
                if (valueComparer != null && valueComparer.Compare(cursor.value, value) == 0) return index;
                else if (cursor.value.Equals(value)) return index;
                cursor = cursor.next;
                index++;
            }

            return -1;
        }

        public TKey GetKey(int index)
        {
            if (index < 0 || index > Count) throw new ArgumentOutOfRangeException("Index out of range");

            Node cursor = head;
            int iterate = 0;

            while (cursor != null && iterate <= index)
            {
                if (iterate == index) return cursor.key;
                cursor = cursor.next;
                iterate++;
            }

            throw new NullReferenceException($"Key by index {index} does not exists");
        }

        public TValue GetByIndex(int index)
        {
            if (index < 0 || index > Count) throw new ArgumentOutOfRangeException("Index out of range");

            Node cursor = head;
            int iterate = 0;

            while (cursor != null && iterate <= index)
            {
                if (iterate == index) return cursor.value;
                cursor = cursor.next;
                iterate++;
            }

            throw new NullReferenceException($"Value by index {index} does not exists");
        }

        public void SetByIndex(int index, TValue value)
        {
            if (index < 0 || index > Count) throw new ArgumentOutOfRangeException("Index out of range");

            Node cursor = head;
            int iterate = 0;

            while (cursor != null && iterate <= index)
            {
                if (iterate == index) cursor.value = value;
                cursor = cursor.next;
                iterate++;
            }
        }

        public bool Remove(TKey key)
        {
            if (key == null) throw new ArgumentNullException("Key should be not null");

            Node cursor = head;
            Node prev = null;

            while (cursor != null)
            {
                if (Comparer.Compare(cursor.key, key) == 0)
                {
                    removeNode(prev, cursor);
                    Count--;
                    OnRemoval(
                        new MySortedListEventArgs<TKey, TValue>(
                            $"Removed: {cursor.key} -> {cursor.value}",
                            new KeyValuePair<TKey, TValue>(cursor.key, cursor.value)));

                    return true;
                }

                prev = cursor;
                cursor = cursor.next;
            }

            return false;
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index > Count) throw new ArgumentOutOfRangeException("Index out of range");
            int iterate = 0;

            Node cursor = head;
            Node prev = null;

            while (cursor != null && iterate <= index)
            {
                if (iterate == index)
                {
                    removeNode(prev, cursor);
                    OnRemoval(
                        new MySortedListEventArgs<TKey, TValue>(
                            $"Removed: {cursor.key} -> {cursor.value}",
                            new KeyValuePair<TKey, TValue>(cursor.key, cursor.value)));
                    return;
                }

                prev = cursor;
                cursor = cursor.next;
                iterate++;
            }
        }

        private void removeNode(Node prev, Node cursor)
        {
            if (prev != null)
            {
                prev.next = cursor.next;
                if (cursor.next == null) tail = prev;
            }
            else
            {
                head = head.next;
                if (head == null) tail = null;
            }
            Count--;
        }

        public bool IsEmpty()
        {
            return Count == 0;
        }

        public MySortedList<TKey, TValue> Clone()
        {
            MySortedList<TKey, TValue> m = new MySortedList<TKey, TValue>();
            foreach (KeyValuePair<TKey, TValue> node in this)
            {
                m.Add(node.Key, node.Value);
            }

            return m;
        }

        public void TrimExcess()
        {
            if (Count / Capacity >= REDUCTION_CAPACITY_LIMIT) return;
            Capacity = Count;
        }

        public override string ToString()
        {

            if (head == null)
            {
                return "MySortedList Empty";
            }

            string list = "";
            Node cursor = head;

            while (cursor.next != null)
            {

                list += $"{cursor} -> ";
                cursor = cursor.next;
            }

            list += $"{cursor}";

            return list;
        }

        private sealed class Node
        {
            public Node next;
            public TKey key;
            public TValue value;

            public Node(TKey key, TValue value)
            {
                this.key = key;
                this.value = value;
                next = null;
            }

            public Node(Node node)
            {
                next = node;
            }

            public override string ToString() => $"({key}, {value})";
        }

        private sealed class KeysList : IList<TKey>
        {
            private MySortedList<TKey, TValue> source;

            internal KeysList(MySortedList<TKey, TValue> source)
            {
                this.source = source;
            }

            public int Count
            {
                get { return source.Count; }
            }

            public bool IsReadOnly
            {
                get { return true; }
            }

            public void Add(TKey key)
            {
                throw new NotSupportedException("This operation is not supported");
            }

            public void Clear()
            {
                throw new NotSupportedException("This operation is not supported");
            }

            public void Insert(int index, TKey key)
            {
                throw new NotSupportedException("This operation is not supported");
            }

            public bool Contains(TKey key)
            {
                return source.ContainsKey(key);
            }

            public TKey this[int index]
            {
                get
                {
                    return source.GetKey(index);
                }
                set
                {
                    throw new NotSupportedException("This operation is not supported");
                }
            }

            public void CopyTo(TKey[] array, int arrayIndex)
            {
                if (arrayIndex < 0) throw new ArgumentOutOfRangeException("Number was less than the array's lower bound in the first dimension");
                if (array.Length < arrayIndex + source.Count) throw new ArgumentException("Destination array was not long enough");
                int index = arrayIndex;
                Node cursor = source.head;

                while (cursor != null)
                {
                    array[index] = cursor.key;
                    index++;
                }
            }

            public IEnumerator<TKey> GetEnumerator()
            {
                Node cursor = source.head;
                while (cursor != null)
                {
                    yield return cursor.key;
                    cursor = cursor.next;
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public int IndexOf(TKey key)
            {
                return source.IndexOfKey(key);
            }

            public bool Remove(TKey key)
            {
                throw new NotSupportedException("This operation is not supported");
            }

            public void RemoveAt(int index)
            {
                throw new NotSupportedException("This operation is not supported");
            }
        }

        private KeysList GetKeysListHelper()
        {
            if (keysList == null)
                keysList = new KeysList(this);
            return keysList;
        }

        private sealed class ValuesList : IList<TValue>
        {
            private MySortedList<TKey, TValue> source;

            internal ValuesList(MySortedList<TKey, TValue> source)
            {
                this.source = source;
            }

            public int Count
            {
                get { return source.Count; }
            }

            public bool IsReadOnly
            {
                get { return true; }
            }

            public void Add(TValue value)
            {
                throw new NotSupportedException("This operation is not supported");
            }

            public void Clear()
            {
                throw new NotSupportedException("This operation is not supported");
            }

            public void Insert(int index, TValue value)
            {
                throw new NotSupportedException("This operation is not supported");
            }

            public bool Contains(TValue value)
            {
                return source.ContainsValue(value);
            }

            public TValue this[int index]
            {
                get
                {
                    return source.GetByIndex(index);
                }
                set
                {
                    throw new NotSupportedException("This operation is not supported");
                }
            }

            public void CopyTo(TValue[] array, int arrayIndex)
            {
                if (arrayIndex < 0) throw new ArgumentOutOfRangeException("Number was less than the array's lower bound in the first dimension");
                if (array.Length < arrayIndex + source.Count) throw new ArgumentException("Destination array was not long enough");
                int index = arrayIndex;
                Node cursor = source.head;

                while (cursor != null)
                {
                    array[index] = cursor.value;
                    index++;
                }
            }

            public IEnumerator<TValue> GetEnumerator()
            {
                Node cursor = source.head;
                while (cursor != null)
                {
                    yield return cursor.value;
                    cursor = cursor.next;
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public int IndexOf(TValue value)
            {
                return source.IndexOfValue(value);
            }

            public bool Remove(TValue value)
            {
                throw new NotSupportedException("This operation is not supported");
            }


            public void RemoveAt(int index)
            {
                throw new NotSupportedException("This operation is not supported");
            }
        }

        private ValuesList GetValuesListHelper()
        {
            if (valuesList == null)
                valuesList = new ValuesList(this);
            return valuesList;
        }

        private class Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>
        {
            private readonly Node head;
            private Node current;


            public Enumerator(MySortedList<TKey, TValue> mySortedList)
            {
                head = mySortedList.head;
                current = new Node(head);
            }

            public KeyValuePair<TKey, TValue> Current => new KeyValuePair<TKey, TValue>(current.key, current.value);
            object IEnumerator.Current
            {
                get
                {
                    if (current == null)
                    {
                        throw new InvalidOperationException();
                    }
                    return new KeyValuePair<TKey, TValue>(current.key, current.value); ;
                }
            }

            public bool MoveNext()
            {
                if (current != null)
                {
                    current = current.next;
                }

                return current != null;
            }

            public void Reset()
            {
                current = head;
            }

            public void Dispose()
            {
                GC.SuppressFinalize(this);
            }
        }
    }
}
