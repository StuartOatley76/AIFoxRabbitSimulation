using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree {

    /// <summary>
    /// Class to handle mating
    /// </summary>
    public class Mate : Action {

        /// <summary>
        /// The character's mate component
        /// </summary>
        private Characters.Mate mate;

        /// <summary>
        /// Sets the character's mate component
        /// </summary>
        protected override void Initialise() {
            base.Initialise();
            mate = character.GameObject.GetComponent<Characters.Mate>();
        }

        /// <summary>
        /// Runs the mate component. Mating is an instant action, so no need to wait
        /// </summary>
        /// <returns></returns>
        protected override IEnumerator PerformAction() {
            if(mate == null) {
                nodeState = NodeState.failed;
                yield break;
            }
            nodeState = NodeState.running;
            mate.RunAction();
            nodeState = mate.Succeeded ? NodeState.passed : NodeState.failed;

        }
    }
}