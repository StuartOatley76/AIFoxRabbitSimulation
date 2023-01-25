using System.Collections;

namespace BehaviourTree {

    /// <summary>
    /// Class to handle resting
    /// </summary>
    public class Rest : Action {

        /// <summary>
        /// The character's rest component
        /// </summary>
        private Characters.Rest rest;

        /// <summary>
        /// Sets the rest component
        /// </summary>
        protected override void Initialise() {
            base.Initialise();
            rest = character.GameObject.GetComponent<Characters.Rest>();
        }

        /// <summary>
        /// runs rest until it finishes or danger is sensed
        /// </summary>
        /// <returns></returns>
        protected override IEnumerator PerformAction() {
            if(rest == null) {
                nodeState = NodeState.failed;
                yield break;
            }
            nodeState = NodeState.running;
            rest.RunAction();
            do {
                yield return null;
            } while (nodeState == NodeState.running && !rest.Succeeded && !character.State.HasFlag(GOAP.State.IsInDanger) && !character.State.HasFlag(GOAP.State.IsAlert));

            if (rest.IsRunning) {
                rest.Stop();
                nodeState = NodeState.failed;
            } else {
                nodeState = NodeState.passed;
            }
        }
    }
}