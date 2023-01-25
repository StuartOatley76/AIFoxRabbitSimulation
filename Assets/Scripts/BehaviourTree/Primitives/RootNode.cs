using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree {

    /// <summary>
    /// Class to represent a root node on a behaviour tree
    /// </summary>
    public class RootNode : Node {

        /// <summary>
        /// The child node
        /// </summary>
        public Node child;

        /// <summary>
        /// Continues running the child node unless the node state of this node has been changed
        /// </summary>
        /// <returns></returns>
        public override IEnumerator RunNode() {
            if (!child) {
                yield break;
            }
            nodeState = NodeState.running;
            while (nodeState == NodeState.running) {
                child.Reset();
                yield return child.RunNode();
            }
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
            RootNode node = Instantiate(this);
            if (child) {
                node.child = child.Clone();
            }
            return node;
        }
    }
}
