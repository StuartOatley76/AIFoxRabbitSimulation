using System.Collections;
using Characters;
using UnityEngine;

namespace BehaviourTree {

    /// <summary>
    /// Class to handle finding danger that has been detected (state = alert)
    /// </summary>
    public class FindDanger : Action {

        /// <summary>
        /// The turn component
        /// </summary>
        private Turn turn;

        /// <summary>
        /// Sets turn to the character's turn component
        /// </summary>
        protected override void Initialise() {
            base.Initialise();
            turn = character.GameObject.GetComponent<Turn>();
        }

        /// <summary>
        /// Turns the character randomly until the character's state is no longer alert, or is in danger
        /// </summary>
        /// <returns></returns>
        protected override IEnumerator PerformAction() {
            if(turn == null) {
                nodeState = NodeState.failed;
                yield break;
            }
            nodeState = NodeState.running;
            do {
                if (!turn.IsRunning) {
                    SetTurn();
                }
                if (nodeState == NodeState.ready) {
                    yield break;
                }
                yield return null;

            } while (nodeState == NodeState.running && character.State.HasFlag(GOAP.State.IsAlert) && !character.State.HasFlag(GOAP.State.IsInDanger));

            nodeState = character.State.HasFlag(GOAP.State.IsInDanger) ? NodeState.passed : NodeState.failed;
        }

        /// <summary>
        /// Creates a random turn for the character
        /// </summary>
        private void SetTurn() {
            turn.DegreesToTurn = Random.Range(0f, 360f);
            turn.Clockwise = Random.Range(0, 2) > 0;
            turn.RunAction();
        }
    }


}