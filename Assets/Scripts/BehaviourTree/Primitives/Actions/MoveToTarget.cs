using System.Collections;
using System.Collections.Generic;
using Memorable;

namespace BehaviourTree {

    /// <summary>
    /// Class to handle moving to 
    /// </summary>
    public abstract class MoveToTarget : Action {

        /// <summary>
        /// The type of target
        /// </summary>
        protected MemorableObjectType targetType;

        /// <summary>
        /// The character's WalkToTarget component
        /// </summary>
        private Characters.WalkToTarget toTarget;

        /// <summary>
        /// Sets toTarget
        /// </summary>
        protected override void Initialise() {
            base.Initialise();
            toTarget = character.GameObject.GetComponent<Characters.WalkToTarget>();
        }

        /// <summary>
        /// Fails if the character cannot set a target of the given type
        /// Runs totarget while continue returns true
        /// </summary>
        /// <returns></returns>
        protected override IEnumerator PerformAction() {
            if(!toTarget || !character.SetTarget(targetType)) {
                nodeState = NodeState.failed;
                yield break;
            }
            toTarget.RunAction();
            nodeState = NodeState.running;
            while (Continue()) {
                yield return null;
            }
            if (toTarget.IsRunning) {
                toTarget.Stop();
                nodeState = NodeState.failed;
            } else {
                nodeState = toTarget.Succeeded ? NodeState.passed : NodeState.failed;
            }
        }

        /// <summary>
        /// Stops if toTarget has stopped, or the character has sensed danger
        /// </summary>
        /// <returns></returns>
        private bool Continue() {
            if(nodeState == NodeState.running && !toTarget.IsRunning || character.State.HasFlag(GOAP.State.IsInDanger) || character.State.HasFlag(GOAP.State.IsAlert)) {
                return false;
            }
            return true;
        }
    }
}