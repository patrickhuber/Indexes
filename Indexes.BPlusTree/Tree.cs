using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Indexes.BPlusTree
{
    public sealed class Tree<TKey, TValue>
        where TKey : IComparable<TKey>
    {
        public int Order { get; private set; }
        public Node<TKey, TValue> Root { get; private set; }
                
        public Tree(Node<TKey, TValue> root)
        {
            Order = root.Order;
            Root = root;
        }

        public Tree(int order)
            : this(new LeafNode<TKey, TValue>(order))
        {
        }

        public Tree()
            : this(Constants.DefaultOrder)
        { }

        public void Insert(TKey key, TValue value)
        {
            Insert(Root, key, value, 0);
            if (Root.IsFull())
            {
                var newRoot = new InnerNode<TKey, TValue>(Order);
                var split = Split(newRoot, Root);
                newRoot.Children.Add(Root);
                newRoot.Children.Add(split);

                // set the current root
                Root = newRoot;
            }
        }

        private void Insert(Node<TKey, TValue> node, TKey key, TValue value, int level)
        {
            if (node.IsLeaf())
            {
                var leaf = node as LeafNode<TKey, TValue>;
                leaf.Insert(key, value);
                return;
            }
            var parent = node as InnerNode<TKey, TValue>;
            int index = parent.FindIndex(key);
            var child = parent.Children[index];

            // recursively insert using the child
            Insert(child, key, value, level + 1);
            
            if (child.IsFull())
            {
                var split = Split(parent, child);
                if (key.CompareTo(parent.Keys.Last()) > 0)
                    index = parent.Children.Count;
                else
                    index = parent.FindIndex(split.Keys.First());
                parent.Children.Insert(index, split);
            }
        }

        private Node<TKey, TValue> Split(InnerNode<TKey, TValue> parent, Node<TKey, TValue> child)
        {
            var split = child.Split();
            int index = 0;
            var key = default(TKey);
            if (child.IsLeaf())
            {
                key = split.Keys[0];
                index = parent.FindIndex(key);
                parent.Keys.Insert(index, key);
            }
            else
            {
                key = child.Keys[child.Keys.Count - 1];
                index = parent.FindIndex(key);
                parent.Keys.Insert(index, key);
                child.Keys.RemoveAt(child.Keys.Count - 1);
            }
            return split;
        }

        public void Delete(TKey key)            
        {
            Delete(Root, key, default(TValue), false, 0);
        }

        public void Delete(TKey key, TValue value)
        {
            Delete(Root, key, value, true, 0);
        }

        private void Delete(Node<TKey, TValue> node, TKey key, TValue value, bool deleteValue, int level)
        {
            // is node a leaf?
            if (node.IsLeaf())
            {
                var leaf = node as LeafNode<TKey, TValue>;
                var keyIndex = leaf.Keys.IndexOf(key);

                if (keyIndex > 0)
                {
                    var valueList = leaf.Values[keyIndex];
                    var valueIndex = valueList.IndexOf(value);

                    // if deleteValue and value found and list count == 1
                    // OR
                    // if not deleteValue
                    //  delete the key and the value list   
                    bool removeKey =
                        (deleteValue && (valueIndex > 0 && valueList.Count > 1))
                        || !deleteValue;

                    if (removeKey)
                    {
                        leaf.Keys.RemoveAt(keyIndex);
                        leaf.Values.RemoveAt(keyIndex);
                    }
                    else
                    {
                        valueList.Remove(value);
                    }
                }
            }
            // node is an inner node
            else 
            {
                var inner = node as InnerNode<TKey, TValue>;
                var index = inner.FindIndex(key);
                
                var child = inner.Children[index];
                Delete(child, key, value, deleteValue, level + 1);

                if (child.IsSubOptimal())
                {
                    var sibling = (Node<TKey, TValue>)null;

                    // if left child null, use right child
                    if (index > 0)
                        sibling = inner.Children[index + 1];

                    // if right child null, use left child  
                    else if (index < inner.Children.Count)
                        sibling = inner.Children[index + 1];
                        
                    // if not null left child and not null right child, use the child with the fewest keys
                    else
                    {                        
                        var leftChild = inner.Children[index - 1];
                        var rightChild = inner.Children[index + 1];

                        sibling = leftChild;
                        if (leftChild.IsSubOptimal() && !rightChild.IsSubOptimal())
                            sibling = rightChild;
                    }

                    if (sibling.IsSubOptimal())
                    { 
                        child.Merge(sibling);
                    }
                    else 
                    {
                        child.Redistribute(sibling);
                    }
                }
            }
        }
    }
}
