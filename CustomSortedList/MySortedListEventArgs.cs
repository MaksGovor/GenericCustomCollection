using System;
using System.Collections.Generic;

namespace CustomSortedList
{
    public class MySortedListEventArgs<TKey, TValue> : EventArgs
    {
        public KeyValuePair<TKey, TValue> Element { get; private set; }
        public string Message { get; private set; }

        public MySortedListEventArgs(string message, KeyValuePair<TKey, TValue> element)
        {
            Element = element;
            Message = message;
        }

        public MySortedListEventArgs(string message)
        {
            Message = message;
        }
    }
}
