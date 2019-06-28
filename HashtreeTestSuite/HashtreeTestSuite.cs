using System;
using Xunit;
using HashTree.Classes;

namespace HashtreeProjectTestSuite
{
    public class HashtreeTestSuite
    {
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
        //Test Add       
        //Test Remove
        //Test Update
        //Test Read
    }
}
