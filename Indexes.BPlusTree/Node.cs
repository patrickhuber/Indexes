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
        
        public bool CanRedistribute(Node<TKey, TValue> node)
        {
            int total = Keys.Count + node.Keys.Count;
            if (total <= Minimum)
                return false;
            return true;
        }

        public abstract void Redistribute(Node<TKey, TValue> node, int direction);

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

        public TKey RemoveLastKey()
        {
            var lastKey = Keys.Last();
            int lastIndex = Keys.Count - 1;            
            Keys.RemoveAt(lastIndex);
            return lastKey;
        }

        public TKey RemoveFirstKey()
        {
            var firstKey = Keys.First();
            int lastIndex = 0;
            Keys.RemoveAt(lastIndex);
            return firstKey;
        }

        public void InsertKeyAtBeginning(TKey key)
        {
            Keys.Insert(0, key);
        }

        public void InsertKeyAtEnd(TKey key)
        {
            Keys.Add(key);
        }
    }
}
