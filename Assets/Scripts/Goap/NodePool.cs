using System.Collections.Generic;


namespace GOAP {
    /// <summary>
    /// Class to provide a pool of nodes used in the action planner
    /// </summary>
    public class NodePool {

        /// <summary>
        /// The queue that acts as the pool
        /// </summary>
        private Queue<ActionPlanner.Node> nodes = new Queue<ActionPlanner.Node>();

        /// <summary>
        /// The initial size of the pool
        /// </summary>
        private const int startSize = 50;

        /// <summary>
        /// The current size of the pool
        /// </summary>
        private int currentSize = 0;

        /// <summary>
        /// Constructor. Initialises the pool
        /// </summary>
        public NodePool() {
            CreateNodes(startSize);
        }

        /// <summary>
        /// Resizes the pool if needed, then provides a reset node
        /// </summary>
        /// <returns></returns>
        public ActionPlanner.Node GetNode() {
            if(nodes.Count <= 0) {
                CreateNodes(currentSize);
            }
            ActionPlanner.Node node = nodes.Dequeue();
            node.Reset();
            return node;
        }

        /// <summary>
        /// Returns the list to the pool
        /// </summary>
        public void ReturnNode(ActionPlanner.Node node) {
            nodes.Enqueue(node);
        }

        /// <summary>
        /// Adds nodes to the pool
        /// </summary>
        private void CreateNodes(int numberOfNodes) {
            for(int i = 0; i < numberOfNodes; i++) {
                nodes.Enqueue(new ActionPlanner.Node());
            }
            currentSize += numberOfNodes;
        }
    }
}