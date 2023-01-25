using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters {

    /// <summary>
    /// class to handle a character wandering
    /// </summary>
    [RequireComponent(typeof(Movement))]
    public class Wander : CharacterAction {

        /// <summary>
        /// The movement action attached to this character
        /// </summary>
        private Movement movement;
        
        /// <summary>
        /// Minimum distance for each path on the wander
        /// </summary>
        [SerializeField] 
        private float minDistanceBetweenPoints = 2.5f;

        /// <summary>
        /// Maximum distance for each path on the wander
        /// </summary>
        [SerializeField]
        private float maxDistanceBetweenPoints = 5f;

        [SerializeField]
        private float sphereSize = 2;
        /// <summary>
        /// Assigns the movement action
        /// </summary>
        protected override void Awake() {
            base.Awake();
            movement = GetComponent<Movement>();
        }

        protected override bool PreAction() {
            movement.MoveType = Movement.MovementType.Walk;
            return base.PreAction();
        }

        /// <summary>
        /// Stops the movement as well as this
        /// </summary>
        public override void Stop() {
            movement.Stop();
            base.Stop();
        }

        /// <summary>
        /// Handles setting new destination if needed
        /// </summary>
		protected override void Update() {
			base.Update();
			if (IsRunning) {
                if(movement != null && !movement.IsRunning) {
                    SetNewDestination();
				}
			}
		}

        /// <summary>
        /// Sets speed to walk
        /// </summary>
		public override void RunAction() {
            movement.MoveType = Movement.MovementType.Walk;
			base.RunAction();
		}

        /// <summary>
        /// Wander never succeeds or fails, must be stopped 
        /// </summary>
        /// <returns></returns>
		protected override bool Success() {
            return false;
        }

        /// <summary>
        /// Wander never succeeds or fails, must be stopped 
        /// </summary>
        /// <returns></returns>
        protected override bool Failed() {
            return false;
        }

        /// <summary>
        /// Sets a random direction
        /// </summary>
        private void SetNewDestination() {

            float distance = Random.Range(minDistanceBetweenPoints, maxDistanceBetweenPoints);
            movement.TargetPosition = transform.position + transform.forward * distance + Random.onUnitSphere * sphereSize;
            movement.RunAction();
        }


    }
}