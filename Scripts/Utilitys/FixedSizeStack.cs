using System;
using System.Collections;
using System.Collections.Generic;
namespace MacacaGames
{
    [Serializable]
    public class FixedSizeStack<T> : IEnumerable<T>
    {
        #region Fields

        private int _limit;
        private LinkedList<T> _list;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a FixedSizeStack
        /// </summary>
        /// <param name="maxSize">The size of the stack</param>
        public FixedSizeStack(int maxSize)
        {
            _limit = maxSize;
            _list = new LinkedList<T>();
        }

        #endregion

        #region Public Stack Implementation

        /// <summary>
        /// Add ont item to Stack
        /// </summary>
        public void Push(T value)
        {
            if (_list.Count == _limit)
            {
                _list.RemoveLast();
            }
            _list.AddFirst(value);
        }

        /// <summary>
        /// Take the last item and remove from stack
        /// </summary>
        /// <returns>The item</returns>
        public T Pop()
        {
            if (_list.Count > 0)
            {
                T value = _list.First.Value;
                _list.RemoveFirst();
                return value;
            }
            else
            {
                throw new InvalidOperationException("The Stack is empty");
            }
        }


        /// <summary>
        /// Take the last item but "not" remove from stack
        /// </summary>
        /// <returns>The item</returns>
        public T Peek()
        {
            if (_list.Count > 0)
            {
                T value = _list.First.Value;
                return value;
            }
            else
            {
                throw new InvalidOperationException("The Stack is empty");
            }

        }


        /// <summary>
        /// Clear the stack
        /// </summary>
        public void Clear()
        {
            _list.Clear();
        }

        public int Count
        {
            get { return _list.Count; }
        }

        /// <summary>
        /// Checks if the top object on the stack matches the value passed in
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool IsTop(T value)
        {
            bool result = false;
            if (this.Count > 0)
            {
                result = Peek().Equals(value);
            }
            return result;
        }


        /// <summary>
        /// Is the item in stack
        /// </summary>
        /// <param name="value">item</param>
        /// <returns>True if the item is in stack</returns>
        public bool Contains(T value)
        {
            bool result = false;
            if (this.Count > 0)
            {
                result = _list.Contains(value);
            }
            return result;
        }

        /// <summary>
        /// Get Enumerator
        /// </summary>
        /// <returns>The IEnumerator</returns>
        public IEnumerator GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        /// <summary>
        /// Get Enumerator
        /// </summary>
        /// <returns>The IEnumerator</returns>
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return _list.GetEnumerator();
        }
        #endregion

    }
}
