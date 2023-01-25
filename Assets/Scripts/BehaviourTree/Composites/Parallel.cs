using System.Collections;

namespace BehaviourTree {

    /// <summary>
    /// Abstract class to represent a parallel composite on a behaviour tree
    /// </summary>
    public class Parallel : Composite {

        /// <summary>
        /// Runs all attached nodes then checks nodestate
        /// </summary>
        /// <returns></returns>
        public override IEnumerator RunNode() {
            Reset();
            nodeState = NodeState.running;
            for (int i = 0; i < attachedNodes.Count; i++) {
                yield return attachedNodes[i];
            }
            nodeState = GetNodeState();
        }

        /// <summary>
        /// Parallels can have their own logic for whether they pass or fail, set it here
        /// </summary>
        /// <returns></returns>
        protected virtual NodeState GetNodeState() {
            return NodeState.passed;
        }
    }
}
