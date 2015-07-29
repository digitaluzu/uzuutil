using UnityEngine;
using System.Collections.Generic;

namespace Uzu
{
    /// <summary>
    /// A 'fixed size' list implementation.
    /// Maintains the the current size of the list for easy adding,
    /// but does not expand if the maximum capacity is exceeded.
    /// </summary>
    public class FixedList<T>
    {
        /// <summary>
        /// Constructor that allows initializing of buffer capacity.
        /// </summary>
        public FixedList (int capacity)
        {
            _buffer = new T[capacity];
        }

        /// <summary>
        /// Gets the number of current elements.
        /// </summary>
        public int Count {
            get { return _size; }
        }

        /// <summary>
        /// Gets the maximum capacity of the current buffer.
        /// This is different Count, which stores the number of items currently stored.
        /// </summary>
        public int Capacity {
            get {
                return _buffer.Length;
            }
        }

        /// <summary>
        /// For 'foreach' functionality.
        /// </summary>
        public IEnumerator<T> GetEnumerator ()
        {
            for (int i = 0; i < _size; ++i) {
                yield return _buffer[i];
            }
        }

        /// <summary>
        /// Indexing into the list.
        /// </summary>
        public T this [int i] {
            get { return _buffer [i]; }
            set { _buffer [i] = value; }
        }

        /// <summary>
        /// Clear the array by resetting its size to zero. Note that the memory is not actually released.
        /// </summary>
        public void Clear ()
        {
            _size = 0;
        }

        /// <summary>
        /// Add the specified item to the end of the list.
        /// </summary>
        public void Add (T item)
        {
            _buffer [_size++] = item;
        }

        /// <summary>
        /// Insert an item at the specified index, pushing the entries back.
        /// </summary>
        public void Insert (int index, T item)
        {
            for (int i = _size; i > index; --i) {
                _buffer [i] = _buffer [i - 1];
            }
            _buffer [index] = item;
            ++_size;
        }

        /// <summary>
        /// Returns 'true' if the specified item is within the list.
        /// </summary>
        public bool Contains (T item)
        {
            for (int i = 0; i < _size; ++i) {
                if (_buffer [i].Equals (item)) {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Finds the index of the specified item.
        /// Returns the Count of the list if not found.
        /// </summary>
        public int FindIndex (T item)
        {
            for (int i = 0; i < _size; ++i) {
                if (_buffer [i].Equals (item)) {
                    return i;
                }
            }
            return Count;
        }

        /// <summary>
        /// Remove the specified item from the list.
        /// Note that RemoveAt() is faster and is advisable if you already know the index.
        /// </summary>
        public bool Remove (T item)
        {
            EqualityComparer<T> comp = EqualityComparer<T>.Default;

            for (int i = 0; i < _size; ++i) {
                if (comp.Equals (_buffer [i], item)) {
                    --_size;
                    _buffer [i] = default(T);
                    for (int b = i; b < _size; ++b) {
                        _buffer [b] = _buffer [b + 1];
                    }
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Remove an item at the specified index.
        /// </summary>
        public void RemoveAt (int index)
        {
            if (index < _size) {
                --_size;
                _buffer [index] = default(T);
                for (int b = index; b < _size; ++b) {
                    _buffer [b] = _buffer [b + 1];
                }
            }
        }

        /// <summary>
        /// Mimic List's ToArray() functionality, except that in this case the list is resized to match the current size.
        /// </summary>
        public T[] ToArray ()
        {
            return _buffer;
        }

        #region Implementation.
        private T[] _buffer;
        private int _size = 0;
        #endregion
    }
}
