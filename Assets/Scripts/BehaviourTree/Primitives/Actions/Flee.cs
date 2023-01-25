using System.Collections;

namespace BehaviourTree {

    /// <summary>
    /// Class to handle fleeing
    /// </summary>
    public class Flee : Action {

        /// <summary>
        /// Character's flee component
        /// </summary>
        private Characters.Flee fleeAction;

        /// <summary>
        /// Sets the flee component
        /// </summary>
        protected override void Initialise() {
            base.Initialise();
            fleeAction = character.GameObject.GetComponent<Characters.Flee>();
        }

        /// <summary>
        /// runs the flee action until it completes
        /// </summary>
        /// <returns></returns>
        protected override IEnumerator PerformAction() {
            if(fleeAction == null) {
                nodeState = NodeState.failed;
                yield break;
            }
            nodeState = NodeState.running;
            fleeAction.RunAction();
            while (nodeState == NodeState.running && fleeAction.IsRunning) {
                yield return null;
            }
            nodeState = (fleeAction.Succeeded) ? NodeState.passed : NodeState.failed;
        }
    }
}