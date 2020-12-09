using System;
using System.Collections;
using System.Collections.Generic;
namespace MacacaGames
{
    [Serializable]
    public class FixedSizeStack<T>
    {
        #region Fields

        private int _limit;
        private LinkedList<T> _list;

        #endregion

        #region Constructors

        public FixedSizeStack(int maxSize)
        {
            _limit = maxSize;
            _list = new LinkedList<T>();

        }

        #endregion

        #region Public Stack Implementation

        /// <summary>
        /// 將一個項目存入 Stack
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
        /// 取出最後一個項目並從 Stack 移除該項目
        /// </summary>
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
        /// 取出最後一個項目 但不從 Stack 移除該項目
        /// </summary>
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

        public bool Contains(T value)
        {
            bool result = false;
            if (this.Count > 0)
            {
                result = _list.Contains(value);
            }
            return result;
        }

        public IEnumerator GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        #endregion

    }
}
