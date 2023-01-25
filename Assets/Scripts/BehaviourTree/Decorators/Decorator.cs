using System;
using System.Collections;

namespace BehaviourTree {

    /// <summary>
    /// Abstract lass to represent a loop decorator on a behaviour tree
    /// </summary>
    public class Decorator : Node {

        /// <summary>
        /// The node to loop
        /// </summary>
        public Node nodeToRepeat;

        /// <summary>
        /// Sealed function that loops the node until CheckCondition returns false
        /// </summary>
        /// <returns></returns>
        public sealed override IEnumerator RunNode() {
            if (!nodeToRepeat) {
                nodeState = NodeState.failed;
                yield break;
            }
            nodeToRepeat.Reset();
            nodeState = NodeState.running;
            while (CheckCondition()) {
                yield return nodeToRepeat.RunNode();
            }
            nodeState = nodeToRepeat.nodeState;
        }

        /// <summary>
        /// Set the logic to break out of the loop here
        /// </summary>
        /// <returns></returns>
        protected virtual bool CheckCondition() {
            return true;
        }

        /// <summary>
        /// Resets the attached node
        /// </summary>
        public override void Reset() {
            nodeToRepeat.Reset();
            base.Reset();
        }

        /// <summary>
        /// Sets the character on the child node
        /// </summary>
        protected override void Initialise() {
            nodeToRepeat.SetCharacter(character);
        }

        /// <summary>
        /// Clones this node and the child
        /// </summary>
        /// <returns></returns>
        public override Node Clone() {
            Decorator node = Instantiate(this);
            if (nodeToRepeat) {
                node.nodeToRepeat = nodeToRepeat.Clone();
            }
            return node;
        }

        /// <summary>
        /// Connects the interrupt to this node and the child node
        /// </summary>
        /// <param name="eventDel"></param>
        public override void ConnectInterrupt(EventHandler<EventArgs> eventDel) {
            base.ConnectInterrupt(eventDel);
            nodeToRepeat.ConnectInterrupt(eventDel);
        }
    }
}
