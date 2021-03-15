using Algorithms.Search.AStar;

using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Algorithms.Tests.Search
{
    /// <summary>
    /// Unit tests for the <see cref="AStar"/> class.
    /// </summary>
    public class AStarTests
    {

        [Test]
        public void Test()
        {
            var nodeA = new Node(new VecN(0, 0), true, 5);
            var nodeB = new Node(new VecN(30, -30), true, 2);
            var nodeC = new Node(new VecN(-30, -30), true, 3);

            nodeA.ConnectedNodes = new[] { nodeB, nodeC };
            nodeB.ConnectedNodes = new[] { nodeA };
            nodeC.ConnectedNodes = new[] { nodeA };

            var quickestRoute = AStar.Compute(nodeA, nodeB);
            System.Console.WriteLine();
        }

    }
}
