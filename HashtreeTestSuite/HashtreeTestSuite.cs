using System;
using Xunit;
using HashTree.Classes;

namespace HashtreeProjectTestSuite
{
    public class HashtreeTestSuite
    {
        public Hashtree BuildTree(int numNodes)
        {
            Hashtree hashtree = new Hashtree();
            for (int i = 1; i <= numNodes + 1; i++)
            {
                hashtree.Add(i);
            }
            return hashtree;
        }

        //Test Size
        [Fact]
        public void TestingSizeOnHashtreeCreation()
        {
            Hashtree hashtree = new Hashtree();

            Assert.Equal(0, hashtree.Size);
        }

        [Fact]
        public void TestingSizeOnHashtreeAfterAddingOneNode()
        {
            Hashtree hashtree = new Hashtree();

            hashtree.Add(1);

            Assert.Equal(1, hashtree.Size);
        }

        [Fact]
        public void TestingSizeOnHashtreeAfterAddingMultipleNodes()
        {
            Hashtree hashtree = new Hashtree();

            hashtree.Add(1);
            hashtree.Add(2);

            Assert.Equal(2, hashtree.Size);
        }

        [Fact]
        public void TestingSizeOnHashtreeAfterRemovingNodeFromEmptyTree()
        {
            Hashtree hashtree = new Hashtree();

            hashtree.Remove(1);           

            Assert.Equal(0, hashtree.Size);
        }
        //Test Root
        [Fact]
        public void TestingRootOfAnEmptyTree()
        {
            Hashtree hashtree = new Hashtree();
            
            Assert.Equal(0, hashtree.Root);
        }

        [Fact]
        public void TestingRootOfAnSingleNodeTree()
        {
            Hashtree hashtree = BuildTree(1);

            Assert.Equal(1, hashtree.Root);
        }

        [Fact]
        public void TestingRootOfAnThreeNodeTree()
        {
            Hashtree hashtree = BuildTree(3);

            Assert.Equal(1, hashtree.Root);
        }
        //Test Add  

        [Fact]
        public void TestingAddingSingleNodeTree()
        {
            Hashtree hashtree = BuildTree(1);

            Assert.Equal(1, hashtree.Read(1));
        }

        [Fact]
        public void TestingAddingThreeNodesTree()
        {
            Hashtree hashtree = BuildTree(3);

            Assert.Equal(1, hashtree.Read(1));
            Assert.Equal(2, hashtree.Read(2));
            Assert.Equal(3, hashtree.Read(3));
        }
        //Test Remove

        
        [Fact]
        public void TestingAddingOneNodeRemovingOneNodeTree()
        {
            Hashtree hashtree = BuildTree(1);

            Assert.Equal(1, hashtree.Read(1));
            hashtree.Remove(1);
            Assert.Equal(1, hashtree.Read(1));
        }

        [Fact]
        public void TestingAddingThreeNodesRemovingOneNodeTree()
        {
            Hashtree hashtree = BuildTree(3);

            Assert.Equal(2, hashtree.Read(2));
            hashtree.Remove(2);
            Assert.Null(hashtree.Read(2));
        }

        //Test Update
        //Test Read
    }
}
