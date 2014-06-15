using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indexes.BPlusTree
{
    public sealed class InnerNode<TKey, TValue> : Node<TKey, TValue>
        where TKey : IComparable<TKey>
    {
        public IList<Node<TKey, TValue>> Children { get; private set; }

        public InnerNode()
            : base()
        {
        }

        public InnerNode(int order)
            : base(order)
        {
        }

        protected override void Init(int order)
        {
            base.Init(order);
            Children = new List<Node<TKey, TValue>>();
        }

        public void InsertChild(TKey key, Node<TKey, TValue> childNode)
        {
            // find the appropriate location to insert the node
            // this will either be the child left of the key or 
            // the infinity pointer
            int index = FindKeyInsertIndex(key);
            
        }

        /// <summary>
        /// Finds the insert index for the key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private int FindKeyInsertIndex(TKey key)
        {
            for (int i = 0; i < Keys.Count; i++)
               if(key.CompareTo(Keys[i])<0)
                    return i;
            return Keys.Count;
        }

        public override Node<TKey, TValue> Split()
        {
            var to = new InnerNode<TKey, TValue>(Order);
            int middle = (Keys.Count + 1) / 2;

            // move half of the keys to the new node
            while (Keys.Count > middle)
            {
                to.Keys.Insert(0, Keys[Keys.Count - 1]);
                Keys.RemoveAt(Keys.Count - 1);
            }
            // move half of the children to the new node
            // children will always have k+1
            while (Children.Count > middle)
            {
                to.Children.Insert(0, Children[Children.Count - 1]);
                Children.RemoveAt(Children.Count - 1);
            }
            return to;
        }

        public override void Merge(Node<TKey, TValue> node)
        {
            if (Keys.Count + node.Keys.Count > Order)
                throw new InvalidOperationException(
                    string.Format("Cannot merge nodes. Sum of Key Counts cannot exceed {0}. this.Keys.Count = {1}. node.Keys.Count = {2}", Order, this.Keys.Count, node.Keys.Count));

            var from = node as InnerNode<TKey, TValue>;

            // move all of the keys to the current node
            while (from.Keys.Count > 0)
            {
                Keys.Add(from.Keys[0]);
                from.Keys.RemoveAt(0);
            }
            // move all of the children to the current node
            while (from.Children.Count > 0)
            {
                Children.Add(from.Children[0]);
                from.Children.RemoveAt(0);
            }
        }

        public override bool IsLeaf()
        {
            return false;
        }
        
        public override void Redistribute(Node<TKey, TValue> node, int direction)
        {
            var innerNode = node as InnerNode<TKey, TValue>;
            int total = Keys.Count + node.Keys.Count;
            int middle = (total + 1) / 2;

            // move half of the keys to the new node
            while (node.Keys.Count > middle)
            {
                if (direction < 0)
                {
                    var lastKey = node.RemoveLastKey();
                    InsertKeyAtBeginning(lastKey);
                }
                else
                {
                    Keys.Add(node.Keys[0]);
                    node.Keys.RemoveAt(0);
                }
            }

            // move half of the children to the new node
            // children will always have k+1
            while (innerNode.Children.Count > middle)
            {
                if (direction < 0)
                {
                    var lastChild = innerNode.RemoveLastChild();
                    InsertChildAtBeginning(lastChild);
                }
                else 
                {
                    var firstChild = innerNode.RemoveFirstChild();
                    InsertChildAtEnd(firstChild);
                }
            }
        }

        private void InsertChildAtEnd(Node<TKey, TValue> node)
        {
            Children.Add(node);
        }

        private void InsertChildAtBeginning(Node<TKey, TValue> node)
        {
            Children.Insert(0, node);
        }

        private Node<TKey, TValue> RemoveLastChild()
        {
            var lastChild = Children.Last();
            int lastIndex = Keys.Count - 1;
            Keys.RemoveAt(lastIndex);
            return lastChild;
        }

        private Node<TKey, TValue> RemoveFirstChild()
        {
            var firstChild = Children.First();
            int lastIndex = 0;
            Children.RemoveAt(lastIndex);
            return firstChild;
        }

        private static bool KeysAreNotDistributed(InnerNode<TKey, TValue> sourceNode, InnerNode<TKey, TValue> targetNode)
        {
            int totalKeys = sourceNode.Keys.Count + targetNode.Keys.Count;
            int middle = (totalKeys + 1) / 2;
            return sourceNode.Keys.Count > middle;
        }

        private static bool ChildrenAreNotRedistributed(InnerNode<TKey, TValue> sourceNode, InnerNode<TKey, TValue> targetNode)
        {
            int totalKeys = sourceNode.Children.Count + targetNode.Children.Count;
            int middle = (totalKeys + 1) / 2;
            return sourceNode.Children.Count > middle;
        }

        private static void RedistributeFromHighToLow(InnerNode<TKey, TValue> highNode, InnerNode<TKey, TValue> lowNode)
        {
            while (KeysAreNotDistributed(highNode, lowNode))
            { 
            }
            while (ChildrenAreNotRedistributed(highNode, lowNode))
            { 
            }
        }

        private static void RedistributeFromLowToHigh(InnerNode<TKey, TValue> lowNode, InnerNode<TKey, TValue> highNode)
        {
            while (KeysAreNotDistributed(lowNode, highNode))
            {
            }
            while (ChildrenAreNotRedistributed(lowNode, highNode))
            {
            }
        }

        public override int Maximum
        {
            get { return Order; }
        }
    }
}
