﻿using System;
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

                if (keyIndex >= 0)
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

                var parent = inner;
                if (!child.IsSubOptimal())
                    return;

                var sibling = (Node<TKey, TValue>)null;
                var siblingDirection = 0;

                // if there is no left child
                if (index == 0)
                {
                    siblingDirection = +1;
                    sibling = parent.Children[index + 1];
                }

                // if there is no right child
                else if (index == parent.Children.Count - 1)
                {
                    siblingDirection = -1;
                    sibling = parent.Children[index - 1];
                }

                // if both left and right child, 
                // only use right child left child is suboptimal and right child is not
                else
                {
                    var leftChild = parent.Children[index - 1];
                    var rightChild = parent.Children[index + 1];
                    siblingDirection = -1;
                    sibling = leftChild;
                    if (leftChild.IsSubOptimal() && !rightChild.IsSubOptimal())
                    {
                        siblingDirection = +1;
                        sibling = rightChild;
                    }
                }

                // the parent key index is the location of the key between the sibling node and the
                // child node. 
                int parentKeyIndex = siblingDirection > 0 ? index : index + siblingDirection;

                // try to redistrubute nodes.                
                if (child.Redistribute(sibling, siblingDirection))
                {       
                    var newParentKey = default(TKey);
                    
                    // recalculate the key at the given index using the left tree max
                    // if sibling is > child
                    if (siblingDirection > 0)
                        newParentKey = sibling.Keys.First();
                    // if child > sibling
                    else
                        newParentKey = child.Keys.First();

                    parent.Keys[parentKeyIndex] = newParentKey;
                }
                else
                {
                    // if redistrbute fails, merge nodes
                    child.Merge(sibling);

                    // when a merge occurs, we need to delete the sibling from the parent. 
                    parent.Children.RemoveAt(index + siblingDirection);
                    parent.Keys.RemoveAt(parentKeyIndex);
                }                
            }

            // if at the root and the root is suboptimal
            // lower the height of the tree
            if (level == 0 && node.IsSubOptimal())
            { }
        }
    }
}
