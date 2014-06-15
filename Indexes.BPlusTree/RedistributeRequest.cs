using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indexes.BPlusTree
{
    public class RedistributeRequest<TKey, TValue>
        where TKey : IComparable<TKey>
    {
        public Node<TKey, TValue> HighNode { get; set; }
        public Node<TKey, TValue> LowNode { get; set; }
    }
}
