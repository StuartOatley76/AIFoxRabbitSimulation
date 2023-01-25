using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters {

    /// <summary>
    /// Class to handle chasing a target
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class Chase : TargetRelatedMovement {

        /// <summary>
        /// Distance at which the chase is given up on
        /// </summary>
        [SerializeField]
        private float giveUpDistance;

        protected override void Awake() {
            base.Awake();
            moveType = Movement.MovementType.Run;
        }

        /// <summary>
        /// Sets a new position to move to
        /// </summary>
        protected override void SetNewPosition() {
            movement.TargetPosition = character.Target.transform.position + character.Target.transform.forward;
        }
    }
}