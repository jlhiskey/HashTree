using System;
using Xunit;
using HashTree.Classes;

namespace HashtreeProjectTestSuite
{
    public class NodeTestSuite
    {
        [Fact]
        public void TestingNodeConstructorValue()
        {
            Node node = new Node(1);

            Assert.Equal(1, node.Value);
        }

        [Fact]
        public void TestingNodeConstructorParent()
        {
            Node node = new Node(1);

            Assert.Null(node.Parent);
        }

        [Fact]
        public void TestingNodeConstructorLeft()
        {
            Node node = new Node(1);

            Assert.Null(node.Left);
        }

        [Fact]
        public void TestingNodeConstructorRight()
        {
            Node node = new Node(1);

            Assert.Null(node.Right);
        }

        [Fact]
        public void TestingNodeParentAssignment()
        {
            Node node = new Node(1);
            Node node2 = new Node(2);

            node.Parent = node2;

            Assert.Equal(node2, node.Parent);
        }

        [Fact]
        public void TestingNodeLeftAssignment()
        {
            Node node = new Node(1);
            Node node2 = new Node(2);

            node.Left = node2;

            Assert.Equal(node2, node.Left);
        }

        [Fact]
        public void TestingNodeRightAssignment()
        {
            Node node = new Node(1);
            Node node2 = new Node(2);

            node.Right = node2;

            Assert.Equal(node2, node.Right);
        }
    }
}
