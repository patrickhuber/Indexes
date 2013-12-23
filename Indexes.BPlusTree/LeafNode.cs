using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indexes.BPlusTree
{
    public sealed class LeafNode<TKey, TValue> 
        : Node<TKey, TValue>
        where TKey : IComparable<TKey>
    {
        public IList<IList<TValue>> Values { get; private set; }
        public Node<TKey, TValue> Next { get; private set; }

        public LeafNode()
            : base()
        {}

        public LeafNode(int order)
            : base(order)
        {}

        protected override void Init(int order)
        {
            base.Init(order);
            Values = new List<IList<TValue>>();
        }

        public void Insert(TKey key, TValue value)
        {
            int index = Keys.IndexOf(key);
            if(index > 0)
            {
                if(!Values[index].Contains(value))
                    Values[index].Add(value);
            }
            else
            {
                index = FindIndex(key);
                Keys.Insert(index, key);
                Values.Insert(index, new List<TValue>());
                Values[index].Add(value);
            }
        }

        public override Node<TKey, TValue> Split()
        {
            var to = new LeafNode<TKey, TValue>(Order);
            int middle = (Keys.Count - 1) / 2;

            // move half of the keys to the new node
            while (Keys.Count > middle)
            {
                to.Keys.Insert(0, Keys[Keys.Count - 1]);
                Keys.RemoveAt(Keys.Count - 1);
            }
            // move half of the values to the new node
            // values will always have k items
            while (Values.Count > middle)
            {
                to.Values.Insert(0, Values[Values.Count - 1]);
                Values.RemoveAt(Values.Count - 1);
            }
            to.Next = Next;
            Next = to;
            return to;
        }

        public override void Merge(Node<TKey, TValue> node)
        {
            var from = node as LeafNode<TKey, TValue>;

            // move all of the keys to the current node
            while (from.Values.Count > 0)
            {
                Keys.Add(from.Keys[0]);
                from.Keys.RemoveAt(0);
                Values.Add(from.Values[0]);
                from.Values.RemoveAt(0);
            }
            Next = from.Next;
            from.Next = null;
        }

        public override bool IsLeaf()
        {
            return true;
        }
                
        public override bool Redistribute(Node<TKey, TValue> node, int direction)
        {
            int total = Keys.Count + node.Keys.Count;
            if (node.Keys.Count <= Minimum)
                return false;

            var leaf = node as LeafNode<TKey, TValue>;            
            int middle = (total + 1) / 2;

            // move half of the keys to the new node
            while (leaf.Keys.Count > middle)
            {
                if (direction < 0)
                {
                    Keys.Insert(0, leaf.Keys[leaf.Keys.Count - 1]);
                    leaf.Keys.RemoveAt(leaf.Keys.Count - 1);
                }
                else 
                {
                    Keys.Add(leaf.Keys[0]);
                    leaf.Keys.RemoveAt(0);
                }
            }

            // move half of the values to the new node
            // values will always have k items
            while (leaf.Values.Count > middle)
            {
                if (direction < 0)
                {
                    Values.Insert(0, leaf.Values[leaf.Values.Count - 1]);
                    leaf.Values.RemoveAt(leaf.Values.Count - 1);
                }
                else 
                {
                    Values.Add(leaf.Values[0]);
                    leaf.Values.RemoveAt(0);
                }
            }

            return true;
        }

        public override int Maximum
        {
            get { return Order - 1; }
        }

        /// <summary>
        /// Used for unit testing
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(TKey key, TValue value)
        {
            Keys.Add(key);
            Values.Add(new List<TValue>());
            Values.Last().Add(value);
        }
    }
}
