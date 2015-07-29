using UnityEngine;
using System.Collections.Generic;

namespace Uzu
{
    /// <summary>
    /// This improved version of the System.Collections.Generic.List that doesn't release the buffer on Clear(),
    /// resulting in better performance and less garbage collection.
    /// </summary>
    public class SmartList<T>
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public SmartList ()
        {
        }

        /// <summary>
        /// Constructor that allows initializing of buffer capacity.
        /// </summary>
        public SmartList (int capacity)
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
                if (_buffer != null) {
                    return _buffer.Length;
                }
                return 0;
            }
        }

        /// <summary>
        /// For 'foreach' functionality.
        /// </summary>
        public IEnumerator<T> GetEnumerator ()
        {
            if (_buffer != null) {
                for (int i = 0; i < _size; ++i) {
                    yield return _buffer[i];
                }
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
        /// Clear the array and release the used memory.
        /// </summary>
        public void Release ()
        {
            _size = 0;
            _buffer = null;
        }

        /// <summary>
        /// Add the specified item to the end of the list.
        /// </summary>
        public void Add (T item)
        {
            if (_buffer == null || _size == _buffer.Length) {
                Resize ();
            }
            _buffer [_size++] = item;
        }

        /// <summary>
        /// Insert an item at the specified index, pushing the entries back.
        /// </summary>
        public void Insert (int index, T item)
        {
            if (_buffer == null || _size == _buffer.Length) {
                Resize ();
            }

            if (index < _size) {
                for (int i = _size; i > index; --i) {
                    _buffer [i] = _buffer [i - 1];
                }
                _buffer [index] = item;
                ++_size;
            } else {
                Add (item);
            }
        }

        /// <summary>
        /// Returns 'true' if the specified item is within the list.
        /// </summary>
        public bool Contains (T item)
        {
            return FindIndex (item) != -1;
        }

        /// <summary>
        /// Finds the index of the specified item within the list.
        /// If not found, return -1.
        /// </summary>
        public int FindIndex (T item)
        {
            for (int i = 0; i < _size; ++i) {
                if (_buffer [i].Equals (item)) {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Remove the specified item from the list.
        /// Note that RemoveAt() is faster and is advisable if you already know the index.
        /// </summary>
        public bool Remove (T item)
        {
            if (_buffer != null) {
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
            }
            return false;
        }

        /// <summary>
        /// Remove an item at the specified index.
        /// </summary>
        public void RemoveAt (int index)
        {
            if (_buffer != null && index < _size) {
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
            Trim ();
            return _buffer;
        }

        /// <summary>
        /// Expands the size of the buffer to support at least a given capacity.
        /// If the buffer is already big enough to hold the set capacity, nothing happens.
        /// </summary>
        public void Expand (int capacity)
        {
            if (Capacity < capacity) {
                T[] newList = new T[capacity];
                if (_buffer != null && _size > 0) {
                    _buffer.CopyTo (newList, 0);
                }
                _buffer = newList;
            }
        }

        #region Implementation.
        private T[] _buffer;
        private int _size = 0;

        /// <summary>
        /// Resize the array, maintaining current contents.
        /// </summary>
        private void Resize ()
        {
            T[] newList = (_buffer != null) ? new T[Mathf.Max (_buffer.Length << 1, 32)] : new T[32];
            if (_buffer != null && _size > 0) {
                _buffer.CopyTo (newList, 0);
            }
            _buffer = newList;
        }

        /// <summary>
        /// Trim the unnecessary memory, resizing the buffer to be of 'Length' size.
        /// </summary>
        private void Trim ()
        {
            if (_size > 0) {
                if (_size < _buffer.Length) {
                    T[] newList = new T[_size];
                    for (int i = 0; i < _size; ++i) {
                        newList [i] = _buffer [i];
                    }
                    _buffer = newList;
                }
            } else {
                _buffer = null;
            }
        }
        #endregion
    }
}
