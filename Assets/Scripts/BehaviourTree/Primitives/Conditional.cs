using System.Collections;

namespace BehaviourTree {

    /// <summary>
    /// Abstract class to represent a conditional node on a behaviour tree
    /// </summary>
    public class Conditional : Node {

        public Node child;

        /// <summary>
        /// Checks the logic in Condition Check to see whether passed or failed and sets state accordingly
        /// </summary>
        /// <returns></returns>
        public sealed override IEnumerator RunNode() {
            if (!child || !ConditionCheck()) {
                nodeState = NodeState.failed;
                yield break;
            }
            nodeState = NodeState.running;
            child.Reset();
            do {
                yield return child.RunNode();
                if (nodeState == NodeState.ready) {
                    yield break;
                }
            } while (child.nodeState == NodeState.running);
            nodeState = child.nodeState;
        }

        /// <summary>
        /// function to hold the logic for the condition
        /// </summary>
        /// <returns>Whether the condition has passed</returns>
        protected virtual bool ConditionCheck() {
            return true;
        }

        /// <summary>
        /// Sets the character for the child node
        /// </summary>
        protected override void Initialise() {
            if (child) {
                child.SetCharacter(character);
            }
        }

        /// <summary>
        /// Clones this and the child node
        /// </summary>
        /// <returns></returns>
        public override Node Clone() {
            Conditional node = Instantiate(this);
            if (child) {
                node.child = child.Clone();
            }
            return node;
        }
    }
}
