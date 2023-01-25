using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters {

    /// <summary>
    /// Class to handle sneaking
    /// </summary>
    public class Sneak : TargetRelatedMovement {

        /// <summary>
        /// The character on the target
        /// </summary>
        Movement target;

        /// <summary>
        /// Whether the target has been caught
        /// </summary>
        private bool caughtTarget;

        protected override bool PreAction() {
            if(!character.Target || character.Target.GetComponent<Movement>() == null) {
                return false;
            }
            target = character.Target.GetComponent<Movement>();
            movement.MoveType = Movement.MovementType.Sneak;
            return base.PreAction();
        }

        /// <summary>
        /// Fails if the target starts running
        /// </summary>
        /// <returns></returns>
        protected override bool Failed() {
            return target.MoveType == Movement.MovementType.Run;
        }

        /// <summary>
        /// sets the target position to the target's position
        /// </summary>
        protected override void SetNewPosition() {
            movement.TargetPosition = character.transform.position;
        }

        /// <summary>
        /// Succeeds if target is caught
        /// </summary>
        /// <returns></returns>
        protected override bool Success() {
            return caughtTarget;
        }

        /// <summary>
        /// sets caughtTarget to true on collision with the target
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionEnter(Collision collision) {
            if (collision.gameObject == character.Target) {
                caughtTarget = true;
            }
        }
    }
}