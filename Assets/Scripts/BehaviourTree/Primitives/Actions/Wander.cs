using System.Collections;

namespace BehaviourTree {

    /// <summary>
    /// Class to handle wandering
    /// </summary>
    public class Wander : Action {

        /// <summary>
        /// The character's wander component
        /// </summary>
        private Characters.Wander wander;

        /// <summary>
        /// Sets the wander component
        /// </summary>
        protected override void Initialise() {
            base.Initialise();
            wander = character.GameObject.GetComponent<Characters.Wander>();
        }

        /// <summary>
        /// Runs wander until continue returns false
        /// </summary>
        /// <returns></returns>
        protected override IEnumerator PerformAction() {
            nodeState = NodeState.running;
            while (Continue()) {
                if (!wander.IsRunning) {
                    wander.RunAction();
                }
                yield return null;
            }
            wander.Stop();
            nodeState = NodeState.passed;
        }

        /// <summary>
        /// Stops when any of the states are met
        /// </summary>
        /// <returns></returns>
        private bool Continue() {

            if (character.State.HasFlag(GOAP.State.IsAlert) ||
                character.State.HasFlag(GOAP.State.IsInDanger) ||
                character.State.HasFlag(GOAP.State.IsHungry) ||
                character.State.HasFlag(GOAP.State.IsThirsty) ||
                character.State.HasFlag(GOAP.State.WantsToMate) ||
                character.State.HasFlag(GOAP.State.IsTired) ||
                nodeState != NodeState.running
                ) {
                return false;
            }

            return true;
        }
    }
}