using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indexes.BPlusTree
{
    public abstract class Node<TKey, TValue>
       where TKey : IComparable<TKey>
    {
        public IList<TKey> Keys { get; private set; }
        public int Order { get; private set; }

        protected Node(int order)
        {
            Init(order);
        }

        protected Node()
            : this(Constants.DefaultOrder)
        { }

        public abstract Node<TKey, TValue> Split();
        public abstract void Merge(Node<TKey, TValue> node);
        public abstract void Redistribute(Node<TKey, TValue> node);
        public abstract bool IsLeaf();

        protected virtual void Init(int order)
        {
            Keys = new List<TKey>();
            Order = order;
        }

        public bool IsFull()
        {
            return Keys.Count >= Order;
        }

        public int FindIndex(TKey key)
        {
            for (int i = 0; i < Keys.Count; i++)
                if (key.CompareTo(Keys[i]) < 0)
                    return i;
            return Keys.Count;
        }

        public virtual int Minimum
        {
            get { return Order / 2; }
        }

        public abstract int Maximum
        {
            get;
        }

        public virtual bool IsSubOptimal()
        {
            return Keys.Count < (Order / 2);
        }
    }
}
