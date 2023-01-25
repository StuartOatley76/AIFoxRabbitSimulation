using System.Collections;

namespace BehaviourTree {

    /// <summary>
    /// Abstract class to represent an action on a behaviour tree
    /// </summary>
    public abstract class Action : Node {

        /// <summary>
        /// Sealed implementation of Node's RunNode
        /// </summary>
        /// <returns></returns>
        public sealed override IEnumerator RunNode() {
            yield return PerformAction();
        }

        /// <summary>
        /// Abstract function to hold the logic to perform the action
        /// </summary>
        protected abstract IEnumerator PerformAction();
    }
}
