using System;
using System.Collections.Generic;

namespace BehaviourTree {

    /// <summary>
    /// Abstract class to represent a composite on a behaviour tree
    /// </summary>
    public abstract class Composite : Node {

        /// <summary>
        /// The child nodes attached to this node
        /// </summary>
        public List<Node> attachedNodes = new List<Node>();

        /// <summary>
        /// Resets all attached nodes
        /// </summary>
        public override void Reset() {
            for(int i = 0; i < attachedNodes.Count; i++) {
                attachedNodes[i].Reset();
            }
            base.Reset();
        }

        /// <summary>
        /// Sets the character on all child nodes
        /// </summary>
        protected override void Initialise() {
            foreach(Node node in attachedNodes) {
                node.SetCharacter(character);
            }
        }

        /// <summary>
        /// Clones this and all child nodes
        /// </summary>
        /// <returns></returns>
        public override Node Clone() {
            Composite node = Instantiate(this);
            node.attachedNodes = attachedNodes.ConvertAll(c => c.Clone());
            return node;
        }

        /// <summary>
        /// Connects the interrupt to this and all child nodes
        /// </summary>
        /// <param name="eventDel"></param>
        public override void ConnectInterrupt(EventHandler<EventArgs> eventDel) {
            base.ConnectInterrupt(eventDel);
            foreach(Node node in attachedNodes) {
                node.ConnectInterrupt(eventDel);
            }
        }
    }
}