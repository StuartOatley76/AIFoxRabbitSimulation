using System.Collections;
using Characters;
using Memorable;
using UnityEngine;

namespace BehaviourTree {

    /// <summary>
    /// Class to handle finding a type of object
    /// </summary>
    public abstract class FindType : Action {

        /// <summary>
        /// The type of object to find
        /// </summary>
        protected MemorableObjectType searchType;

        /// <summary>
        /// The character's wander component
        /// </summary>
        private Characters.Wander wander;

        /// <summary>
        /// the character's movement component
        /// </summary>
        private Movement movement;

        /// <summary>
        /// Sets the wander and movement components
        /// </summary>
        protected override void Initialise() {
            base.Initialise();
            wander = character.GameObject.GetComponent<Characters.Wander>();
            movement = character.GameObject.GetComponent<Movement>();
        }

        /// <summary>
        /// Character wanders until the type is sensed. if smelt or heard, moves to the location
        /// Passes once seen.
        /// </summary>
        /// <returns></returns>
        protected override IEnumerator PerformAction() {
            if(wander == null || movement == null) {
                nodeState = NodeState.failed;
                yield break;
            }
            nodeState = NodeState.running;
            while (Continue()) {
                if (character.TargetTypeSensed(searchType, out Vector3 position)) {
                    if (wander.IsRunning) {
                        wander.Stop();
                    }
                    movement.TargetPosition = position;
                    movement.RunAction();
                    yield return null;
                }
                if (!wander.IsRunning) {
                    wander.RunAction();
                }
                yield return null;
            }

            nodeState = (character.SetTarget(searchType)) ? NodeState.passed : NodeState.failed; 
        }

        /// <summary>
        /// Continues the search until this node's state has been changed, the type has been found or the character senses danger
        /// </summary>
        /// <returns></returns>
        private bool Continue() {
            if(nodeState != NodeState.running) {
                return false;
            }
            if (character.SetTarget(searchType)){
                return false;
            }
            if(character.State.HasFlag(GOAP.State.IsInDanger) || character.State.HasFlag(GOAP.State.IsAlert)) {
                return false;
            }
            return true;
        }
    }
}