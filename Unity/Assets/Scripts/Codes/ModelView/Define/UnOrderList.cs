using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{
    public class UnOrderList<T> : IEnumerable<T> where T : class
    {
        public int Capacity { private set; get; }
        public int Count { private set; get; }

        private T[] _data;

        private int _maxValidIndex;
        private int _minEmptyIndex;
        private int _pointer;

        private UnSortListEnumerator _enumerator;
        
        public UnOrderList()
        {
            Capacity = 4;
            _data = new T[Capacity];
            _enumerator = new UnSortListEnumerator();
        }
        
        public UnOrderList(int capacity)
        {
            this.Capacity = capacity;
            _data = new T[capacity];
            _enumerator = new UnSortListEnumerator();
        }

        public T this[int idx]
        {
            get => _data[idx];
        }

        public void Clear()
        {
            for (int i = 0; i <= _maxValidIndex; i++)
            {
                _data[i] = null;
            }

            _maxValidIndex = 0;
            _minEmptyIndex = 0;
            _pointer = 0;
        }

        public int Add(T item)
        {
            var ret = _pointer;
            _data[_pointer] = item;
            OnAddItem();
            Count++;
            return ret;
        }

        public int[] Remove(T item)
        {
            var list = new List<int>();
            for (int i = 0; i <= _maxValidIndex; i++)
            {
                var d = _data[i];
                if (d != null)
                {
                    if (d.Equals(item))
                    {
                        _data[i] = null;
                        OnRemoveItem(i);
                        Count--;
                        list.Add(i);
                    }
                }
            }

            return list.ToArray();
        }

        public T[] RemoveIf(Func<T, bool> compare)
        {
            List<T> res = new List<T>();
            for (int i = 0; i <= _maxValidIndex; i++)
            {
                var item = _data[i];
                if (item == null) continue;
                if (compare.Invoke(item))
                {
                    res.Add(item);
                    _data[i] = null;
                    OnRemoveItem(i);
                    Count--;
                }
            }

            return res.ToArray();
        }

        public T RemoveAt(int index)
        {
            T item = null;
            if (_data[index] != null)
            {
                item = _data[index];
                _data[index] = null;
                Count--;
            }

            return item;
        }
        
        public T[] FindIf(Func<T, bool> compare)
        {
            List<T> res = new List<T>();
            for (int i = 0; i <= _maxValidIndex; i++)
            {
                var item = _data[i];
                if (item == null) continue;
                if (compare.Invoke(item))
                {
                    res.Add(item);
                }
            }

            return res.ToArray();
        }

        public T[] ToArray()
        {
            List<T> res = new List<T>();
            for (int i = 0; i <= _maxValidIndex; i++)
            {
                var item = _data[i];
                if (item != null)
                {
                    res.Add(item);
                }
            }

            return res.ToArray();
        }
        
        private void FindEmptyHole()
        {
            var max = Capacity;
            if (_minEmptyIndex >= max)
            {
                var arr = new T[Capacity * 2];
                Array.Copy(_data, arr, Capacity);
                Capacity *= 2;
                _data = arr;
            }
            
            _minEmptyIndex++;
            while (_minEmptyIndex < max)
            {
                if (_data[_minEmptyIndex] == null)
                {
                    return;
                }

                _minEmptyIndex++;
            }
            FindEmptyHole();
        }
        
        private void OnAddItem()
        {
            _maxValidIndex = Mathf.Max(_maxValidIndex, _pointer);
            FindEmptyHole();
            _pointer = _minEmptyIndex;
        }
        
        private void OnRemoveItem(int index)
        {
            if (index == _maxValidIndex)
            {
                _maxValidIndex--;
                while (_maxValidIndex >= 0)
                {
                    if (_data[_maxValidIndex] != null)
                        break;
                    _maxValidIndex--;
                }
            }

            if (index < _minEmptyIndex)
            {
                _minEmptyIndex = index;
                _pointer = _minEmptyIndex;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            _enumerator.Reset();
            _enumerator.Init(_data, _maxValidIndex);
            return _enumerator;
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        
        private class UnSortListEnumerator :  IEnumerator<T>
        {
            private T[] _data;
            private int _index;
            private int _curIndex;
            private int _max;

            public void Init(T[] d, int m)
            {
                _data = d;
                _max = m;
            }
            
            public bool MoveNext()
            {
                while (_index <= _max)
                {
                    var item = _data[_index];
                    _curIndex = _index;
                    _index++;
                    if (item != null)
                    {
                        return true;
                    }
                }

                return false;
            }

            public void Reset()
            {
                _index = 0;
                _curIndex = 0;
                _max = 0;
            }

            public T Current => _data[_curIndex];

            object IEnumerator.Current => Current;

            public void Dispose()
            {
            }
        }
    }
}