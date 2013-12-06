using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Indexes.BPlusTree.UnitTests
{
    [TestClass]
    public class TreeTests
    {
        [TestMethod]
        public void Test_Tree_Insert_Adds_Sub_Tree_After_Leaf_Split()
        {
            var tree = new Tree<int, int>(3);
            tree.Insert(1, 1);
            tree.Insert(2, 2);
            tree.Insert(3, 3);
            var root = tree.Root as InnerNode<int, int>;
            Assert.IsNotNull(root);
            Assert.AreEqual(1, tree.Root.Keys.Count);
            Assert.AreEqual(2, tree.Root.Keys[0]);
            Assert.AreEqual(2, root.Children.Count);
            Assert.AreEqual(1, root.Children[0].Keys.Count);
            Assert.AreEqual(1, root.Children[0].Keys[0]);
            Assert.AreEqual(2, root.Children[1].Keys.Count);
            Assert.AreEqual(2, root.Children[1].Keys[0]);
            Assert.AreEqual(3, root.Children[1].Keys[1]);
        }

        [TestMethod]
        public void Test_Tree_Insert()
        {
            var tree = new Tree<int, int>(3);
            for (int i = 1; i <= 9; i++)
                tree.Insert(i, i);

            // root
            var root = tree.Root as InnerNode<int, int>;
            Assert.IsNotNull(root);
            Assert.AreEqual(2, root.Children.Count);
            Assert.AreEqual(1, root.Keys.Count);
            Assert.AreEqual(5, root.Keys[0]);

            // level 1 first child
            var firstChild1 = root.Children[0] as InnerNode<int, int>;
            Assert.AreEqual(2, firstChild1.Children.Count);
            Assert.AreEqual(1, firstChild1.Keys.Count);
            Assert.AreEqual(3, firstChild1.Keys[0]);

            // level 1 second child
            var secondChild1 = root.Children[1] as InnerNode<int, int>;
            Assert.AreEqual(2, secondChild1.Children.Count);
            Assert.AreEqual(1, secondChild1.Keys.Count);
            Assert.AreEqual(7, secondChild1.Keys[0]);

            // level 2 first child
            var firstChild2 = firstChild1.Children[0] as InnerNode<int, int>;
            Assert.AreEqual(2, firstChild2.Children.Count);
            Assert.AreEqual(1, firstChild2.Keys.Count);
            Assert.AreEqual(2, firstChild2.Keys[0]);

            // level 2 second child
            var secondChild2 = firstChild1.Children[1] as InnerNode<int, int>;
            Assert.AreEqual(2, secondChild2.Children.Count);
            Assert.AreEqual(1, secondChild2.Keys.Count);
            Assert.AreEqual(4, secondChild2.Keys[0]);

            // level 2 third child
            var thirdChild2 = secondChild1.Children[0] as InnerNode<int, int>;
            Assert.AreEqual(2, thirdChild2.Children.Count);
            Assert.AreEqual(1, thirdChild2.Keys.Count);
            Assert.AreEqual(6, thirdChild2.Keys[0]);

            // level 2 fourth child
            var fourthChild2 = secondChild1.Children[1] as InnerNode<int, int>;
            Assert.AreEqual(2, fourthChild2.Children.Count);
            Assert.AreEqual(1, fourthChild2.Keys.Count);
            Assert.AreEqual(8, fourthChild2.Keys[0]);

            // level 3 first child
            var firstChild3 = firstChild2.Children[0] as LeafNode<int, int>;
            Assert.IsNotNull(firstChild3);
            Assert.AreEqual(1, firstChild3.Keys.Count);
            Assert.AreEqual(1, firstChild3.Keys[0]);

            // level 3 second child
            var secondChild3 = firstChild2.Children[1] as LeafNode<int, int>;
            Assert.IsNotNull(secondChild3);
            Assert.AreEqual(1, secondChild3.Keys.Count);
            Assert.AreEqual(2, secondChild3.Keys[0]);

            // level 3 third child
            var thirdChild3 = secondChild2.Children[0] as LeafNode<int, int>;
            Assert.IsNotNull(thirdChild3);
            Assert.AreEqual(1, thirdChild3.Keys.Count);
            Assert.AreEqual(3, thirdChild3.Keys[0]);

            // level 3 fourth child
            var fourthdChild3 = secondChild2.Children[1] as LeafNode<int, int>;
            Assert.IsNotNull(fourthdChild3);
            Assert.AreEqual(1, fourthdChild3.Keys.Count);
            Assert.AreEqual(4, fourthdChild3.Keys[0]);

            // level 3 fifth child
            var fifthChild3 = thirdChild2.Children[0] as LeafNode<int, int>;
            Assert.IsNotNull(fifthChild3);
            Assert.AreEqual(1, fifthChild3.Keys.Count);
            Assert.AreEqual(5, fifthChild3.Keys[0]);

            // level 3 sixth child
            var sixthChild3 = thirdChild2.Children[1] as LeafNode<int, int>;
            Assert.IsNotNull(sixthChild3);
            Assert.AreEqual(1, sixthChild3.Keys.Count);
            Assert.AreEqual(6, sixthChild3.Keys[0]);

            // level 3 seventh child
            var seventhChild3 = fourthChild2.Children[0] as LeafNode<int, int>;
            Assert.IsNotNull(seventhChild3);
            Assert.AreEqual(1, seventhChild3.Keys.Count);
            Assert.AreEqual(7, seventhChild3.Keys[0]);

            // level 3 eighth child
            var eighthChild3 = fourthChild2.Children[1] as LeafNode<int, int>;
            Assert.IsNotNull(eighthChild3);
            Assert.AreEqual(2, eighthChild3.Keys.Count);
            Assert.AreEqual(8, eighthChild3.Keys[0]);
            Assert.AreEqual(9, eighthChild3.Keys[1]);
        }

        [TestMethod]
        public void Test_Tree_Insert_Reverse()
        {
            var tree = new Tree<int, int>(3);
            for (int i = 9; i >= 1; i--)
                tree.Insert(i, i);
            var root = tree.Root as InnerNode<int, int>;
            Assert.IsNotNull(root);
            Assert.AreEqual(1, root.Keys.Count);
            Assert.AreEqual(6, root.Keys[0]);
        }

        [TestMethod]
        public void Test_Tree_Delete()
        {
            var root = new InnerNode<int, int>(3);
            root.Keys.Add(15);
            
                var level1Child1 = new InnerNode<int, int>(3);
                level1Child1.Keys.Add(9);
                level1Child1.Keys.Add(12);            

                    var level2Child1 = new LeafNode<int, int>(3);
                    level2Child1.Keys.Add(1);
                    level2Child1.Values.Add(new List<int>());
                    level2Child1.Values[0].Add(1);
                    
                    level2Child1.Keys.Add(4);
                    level2Child1.Values.Add(new List<int>());
                    level2Child1.Values[1].Add(4);
            
                level1Child1.Children.Add(level2Child1);
            
                    var level2Child2 = new LeafNode<int, int>(3);
                    level2Child2.Keys.Add(9);
                    level2Child2.Values.Add(new List<int>());
                    level2Child2.Values[0].Add(9);

                    level2Child2.Keys.Add(10);
                    level2Child2.Values.Add(new List<int>());
                    level2Child2.Values[1].Add(10);
            
                level1Child1.Children.Add(level2Child2);
           
                    var level2Child3 = new LeafNode<int, int>(3);
                    level2Child3.Keys.Add(12);
                    level2Child3.Values.Add(new List<int>());
                    level2Child3.Values[0].Add(12);

                    level2Child3.Keys.Add(13);
                    level2Child3.Values.Add(new List<int>());
                    level2Child3.Values[1].Add(13);
            
                level1Child1.Children.Add(level2Child3);
            
            root.Children.Add(level1Child1);

                var level1Child2 = new InnerNode<int, int>(3);
                level1Child2.Keys.Add(20);

                    var level2Child4 = new LeafNode<int, int>(3);
                    level2Child4.Keys.Add(15);
                    level2Child4.Values.Add(new List<int>());
                    level2Child4.Values[0].Add(15);

                    level2Child4.Keys.Add(16);
                    level2Child4.Values.Add(new List<int>());
                    level2Child4.Values[1].Add(16);

                level1Child2.Children.Add(level2Child4);

                    var level2Child5 = new LeafNode<int, int>(3);
                    level2Child5.Keys.Add(20);
                    level2Child5.Values.Add(new List<int>());
                    level2Child5.Values[0].Add(20);

                    level2Child5.Keys.Add(25);
                    level2Child5.Values.Add(new List<int>());
                    level2Child5.Values[1].Add(25);

                level1Child2.Children.Add(level2Child5);
            
            root.Children.Add(level1Child2);

            var tree = new Tree<int, int>(root);

            tree.Delete(13);
            Assert.AreEqual(2, root.Keys.Count);
            Assert.AreEqual(15, root.Keys[0]);

            var delete13Parent = (root.Children[0] as InnerNode<int, int>);
            Assert.AreEqual(2, delete13Parent.Keys.Count);
            Assert.AreEqual(09, delete13Parent.Keys[0]);
            Assert.AreEqual(12, delete13Parent.Keys[1]);
            Assert.AreEqual(3, delete13Parent.Children.Count);
            var delete13Child3 = delete13Parent.Children[2];
            Assert.AreEqual(1, delete13Child3.Keys.Count);
            Assert.AreEqual(12, delete13Child3.Keys[0]);            

            tree.Delete(15);

            var delete15Parent = (root.Children[0] as InnerNode<int, int>);
            Assert.AreEqual(2, delete15Parent.Keys.Count);
            Assert.AreEqual(20, delete15Parent.Keys[0]);            
            Assert.AreEqual(2, delete15Parent.Children.Count);
            var delete15Child1 = delete15Parent.Children[2];
            Assert.AreEqual(1, delete15Child1.Keys.Count);
            Assert.AreEqual(16, delete15Child1.Keys[0]);            

            tree.Delete(12);
        }
    }
}
