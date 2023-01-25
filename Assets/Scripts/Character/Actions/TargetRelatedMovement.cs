
using UnityEngine;

namespace Characters {


    /// <summary>
    /// abstract class to handle character movement based on a target
    /// </summary>
    [RequireComponent(typeof(Movement), typeof(Collider))]
    public abstract class TargetRelatedMovement : CharacterAction {

        /// <summary>
        /// The movement action attached to this character
        /// </summary>
        protected Movement movement;

        protected Movement.MovementType moveType = Movement.MovementType.Walk;

        /// <summary>
        /// How frequently in frames the destination should be updated 
        /// </summary>
        [SerializeField]
        private int updateFrequency = 10;

        /// <summary>
        /// framecount when the action started
        /// </summary>
        private int startTime = 0;

        private bool collided = false;

        /// <summary>
        /// Assigns the movement action
        /// </summary>
        protected override void Awake() {
            base.Awake();
            movement = GetComponent<Movement>();
        }

        /// <summary>
        /// Checks the character has a target, sets the framecount and destination and ensures
        /// the character is running
        /// </summary>
        /// <returns></returns>
        protected override bool PreAction() {
            collided = false;
            if (character.Target == null) {
                return false;
            }
            movement.MoveType = moveType;
            SetNewPosition();
            startTime = Time.frameCount;
            return base.PreAction();
        }

        /// <summary>
        /// checks polling for new destination
        /// </summary>
        protected override void Update() {
            if (IsRunning && character.Target != null && ((Time.frameCount - startTime) % updateFrequency == 0 || 
                (!movement.IsRunning))) {
                SetNewPosition();
                movement.RunAction();
            }
            base.Update();
        }

        /// <summary>
        /// abstract method to set the destination
        /// </summary>
        protected abstract void SetNewPosition();

        /// <summary>
        /// Starts the action
        /// </summary>
        public override void RunAction() {
            base.RunAction();
            if (IsRunning) {
                movement.RunAction();
                character.CurrentAction = this;
            }
            
        }

        protected override bool Success() {
            return collided;
        }

        protected override bool Failed() {
            if(character.Target == null || moveType == Movement.MovementType.Run && character.Stamina <= 0) {
                return true;
            }
            if (!movement.IsRunning) {
                if (movement.Succeeded) {
                    return false;
                }
                return true;
            }
            return false;
        }

        private void OnCollisionEnter(Collision collision) {
            if (character.Target != null && collision.collider.gameObject == character.Target.gameObject) {
                collided = true;
            }
        }
    }
}
