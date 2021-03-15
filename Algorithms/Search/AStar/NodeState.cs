namespace Algorithms.Search.AStar
{
    /// <summary>
    /// The states the nodes can have.
    /// </summary>
    public enum NodeState
    {
        /// <summary>
        /// The node has not ben explored.
        /// </summary>
        Unexplored = 0,

        /// <summary>
        /// The node is currently being explored.
        /// </summary>
        Open = 1,

        /// <summary>
        /// The node has been fully explored.
        /// </summary>
        Explored = 2,
    }
}
