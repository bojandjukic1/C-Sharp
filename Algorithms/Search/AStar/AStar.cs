﻿using System.Collections.Generic;
using DataStructures.Queue;

namespace Algorithms.Search.AStar
{
    /// <summary>
    /// Contains the code for A* Pathfinding.
    /// </summary>
    public static class AStar
    {
        /// <summary>
        /// Resets the Nodes in the list.
        /// </summary>
        /// <param name="nodes">Resets the nodes to be used again.</param>
        public static void ResetNodes(List<Node> nodes)
        {
            foreach (var node in nodes)
            {
                node.CurrentCost = 0;
                node.EstimatedCost = 0;
                node.Parent = null;
                node.State = NodeState.Unexplored;
            }
        }

        /// <summary>
        /// Generates the Path from a (solved) node graph, before it gets reset.
        /// </summary>
        /// <param name="target">The node where we want to go.</param>
        /// <returns>The Path to the target node.</returns>
        public static List<Node> GeneratePath(Node target)
        {
            var ret = new List<Node>();
            var current = target;
            while (!(current is null))
            {
                ret.Add(current);
                current = current.Parent;
            }

            ret.Reverse();
            return ret;
        }

        /// <summary>
        /// Computes the path from one node to another.
        /// </summary>
        /// <param name="from">Start node.</param>
        /// <param name="to">end node.</param>
        /// <returns>Path from start to end.</returns>
        public static List<Node> Compute(Node from, Node to)
        {
            var done = new List<Node>();

            // A priority queue that will sort our nodes based on the total cost estimate
            var traversedNodes = new PriorityQueue<Node>();
            foreach (var node in from.ConnectedNodes)
            {
                // Add connecting nodes if traversable
                if (node.Traversable)
                {
                    // Calculate the Costs
                    node.CurrentCost = from.CurrentCost + from.DistanceTo(node) * node.TraversalCostMultiplier;
                    node.EstimatedCost = from.CurrentCost + node.DistanceTo(to);

                    // Enqueue
                    traversedNodes.Enqueue(node);
                }
            }

            while (true)
            {
                // End Condition( Path not found )
                if (traversedNodes.Count == 0)
                {
                    ResetNodes(done);
                    ResetNodes(traversedNodes.GetData());
                    return new List<Node>();
                }

                // Selecting next Element from queue
                var current = traversedNodes.Dequeue();

                // Add it to the done list
                done.Add(current);

                current.State = NodeState.Explored;

                // EndCondition( Path was found )
                if (current == to)
                {
                    var ret = GeneratePath(to); // Create the Path

                    // Reset all Nodes that were used.
                    ResetNodes(done);
                    ResetNodes(traversedNodes.GetData());
                    return ret;
                }
                else
                {
                    AddOrUpdateConnected(current, to, traversedNodes);
                }
            }
        }

        private static void AddOrUpdateConnected(Node current, Node to, PriorityQueue<Node> queue)
        {
            foreach (var connected in current.ConnectedNodes)
            {
                if (!connected.Traversable ||
                    connected.State == NodeState.Explored)
                {
                    continue; // Do ignore already checked and not traversable nodes.
                }

                // Adds a previously not "seen" node into the Queue
                if (connected.State == NodeState.Unexplored)
                {
                    connected.Parent = current;
                    connected.CurrentCost = current.CurrentCost + current.DistanceTo(connected) * connected.TraversalCostMultiplier;
                    connected.EstimatedCost = connected.CurrentCost + connected.DistanceTo(to);
                    connected.State = NodeState.Open;
                    queue.Enqueue(connected);
                }
                else if (current != connected)
                {
                    // Updating the cost of the node if the current way is cheaper than the previous
                    var newCCost = current.CurrentCost + current.DistanceTo(connected);
                    var newTCost = newCCost + current.EstimatedCost;
                    if (newTCost < connected.TotalCost)
                    {
                        connected.Parent = current;
                        connected.CurrentCost = newCCost;
                    }
                }
                else
                {
                    throw new PathfindingException("Detected the same node twice.");
                }
            }
        }
    }
}
