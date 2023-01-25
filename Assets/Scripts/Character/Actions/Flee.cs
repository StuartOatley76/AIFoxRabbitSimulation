using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Memorable;

namespace Characters {

    /// <summary>
    /// Class to handle a character fleeing
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class Flee : TargetRelatedMovement {

        /// <summary>
        /// Maximum distance a gameobject can be counted as a threat
        /// </summary>
        [SerializeField]
        private float maxThreatDistance;

        /// <summary>
        /// Whether the character has escaped the threats
        /// </summary>
        private bool escaped;

        /// <summary>
        /// Fails on character being caught
        /// </summary>
        /// <returns></returns>
        protected override bool Failed() {
            return character.IsCaught;
        }

        /// <summary>
        /// Fleeing is done at a run
        /// </summary>
        /// <returns></returns>
        protected override bool PreAction() {
            movement.MoveType = Movement.MovementType.Run;
            return base.PreAction();
        }

        /// <summary>
        /// Sets destination based on the position of the threats
        /// </summary>
        protected override void SetNewPosition() {
            List<MemorableObject> threats = character.GetThreats(maxThreatDistance);
            if(threats == null || threats.Count == 0) {
                escaped = true;
                return;
            }
            Vector3 direction = Vector3.zero;
            for(int i = 0; i < threats.Count; i++) {
                direction += -(threats[i].transform.position - transform.position);
            }
        }

        /// <summary>
        /// Succeeds when the character escapes
        /// </summary>
        /// <returns></returns>
        protected override bool Success() {
            return escaped;
        }

    }
}