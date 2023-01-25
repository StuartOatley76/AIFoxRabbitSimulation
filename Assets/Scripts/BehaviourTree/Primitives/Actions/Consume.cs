using System.Collections;

namespace BehaviourTree {

    public abstract class Consume : Action {

        /// <summary>
        /// The character's consumtion class used by this
        /// </summary>
        protected Characters.Consume charConsume;

        /// <summary>
        /// Runs charConsume until it finishes or the character detects danger
        /// </summary>
        /// <returns></returns>
        protected override IEnumerator PerformAction() {
            if(charConsume == null) {
                nodeState = NodeState.failed;
                yield break;
            }
            nodeState = NodeState.running;
            charConsume.RunAction();
            while(nodeState == NodeState.running && charConsume.IsRunning && !character.State.HasFlag(GOAP.State.IsInDanger) && !character.State.HasFlag(GOAP.State.IsAlert)) {
                if(nodeState == NodeState.ready) {
                    yield break;
                }
                yield return null;
            }
            if (charConsume.IsRunning) {
                nodeState = NodeState.failed;
                charConsume.Stop();
            } else {
                nodeState = NodeState.passed;
            }
        }
    }
}