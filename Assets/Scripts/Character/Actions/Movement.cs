using System;
using UnityEngine;
using UnityEngine.AI;
using Sensors.Hearing;
namespace Characters {

    /// <summary>
    /// Class to handle character movement
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public class Movement : CharacterAction
    {

        /// <summary>
        /// enum representing the type of movement
        /// </summary>
        public enum MovementType
        {
            None,
            Sneak,
            Walk,
            Run
        }

        /// <summary>
        /// The NavMeshAgent attached to the gameobject
        /// </summary>
        private NavMeshAgent agent;

        /// <summary>
        /// The position to move the character to
        /// </summary>
        public Vector3 TargetPosition { get; set; }

        private Vector3 targetNavmeshPos;

        /// <summary>
        /// Distance from target position the position on the navmesh can be
        /// </summary>
        private float startingNavmeshDistancetoCheck = 0.1f;

        /// <summary>
        /// The distance from the target position at which it counts as having arrived
        /// </summary>
        private float currentTargetDistance;

        /// <summary>
        /// Area mask used in sampling the navmesh
        /// </summary>
        protected int areaMask = NavMesh.AllAreas;

        /// <summary>
        /// The movement type used in the movement
        /// </summary>
        public MovementType MoveType { get; set; } = MovementType.Walk;

        /// <summary>
        /// The speed of the movement, based on movement type
        /// </summary>
        protected float speed;

        /// <summary>
        /// Assigns the NavMeshAgent
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            agent = GetComponent<NavMeshAgent>();
        }

        /// <summary>
        /// Sets speed based on the movement type and samples the navmesh to set the destination
        /// </summary>
        /// <returns></returns>
        protected override bool PreAction() {
            if (!character.CanMove) {
                return false;
            }
            base.PreAction();
            SetSpeed();
            GetPositionOnNavmesh();
            return true;
        }

        private void SetSpeed() {
            speed = (MoveType == MovementType.Run) ? character.MaxSpeed :
                (MoveType == MovementType.Sneak) ? character.BaseSpeed * 0.5f : character.BaseSpeed;
            agent.speed = speed;
        }

        /// <summary>
        /// Stops the movement
        /// </summary>
        public override void Stop(){
            base.Stop();
            agent.isStopped = true;
        }

        /// <summary>
        /// starts the agent moving
        /// </summary>
        public override void RunAction() {
            base.RunAction();
            if (IsRunning) {
                agent.ResetPath();
                agent.speed = speed;
                agent.SetDestination(targetNavmeshPos);
                
            } 
        }

        /// <summary>
        /// Attempts to change the target position to a position on the navmesh. Returns whether one was found
        /// </summary>
        /// <returns></returns>
        private void GetPositionOnNavmesh(){
            float distanceToCheck = startingNavmeshDistancetoCheck;
            NavMeshHit hit;
            while (!NavMesh.SamplePosition(TargetPosition, out hit, distanceToCheck, NavMesh.AllAreas)) {
                distanceToCheck += startingNavmeshDistancetoCheck;
            }
            distanceToCheck += startingNavmeshDistancetoCheck;
            currentTargetDistance = distanceToCheck;
            targetNavmeshPos = hit.position;
        }

        /// <summary>
        /// Updates character stats based on the movement
        /// If running and out of stamina, changes speed to walk
        /// </summary>
        protected override void UpdateCharacterStats(){
            character.Thirst += speed * Time.deltaTime * character.ThirstModifier;
            character.Hunger += speed * Time.deltaTime * character.HungerModifier;
            if (MoveType == MovementType.Run) {
                character.Stamina -= speed * Time.deltaTime * character.StaminaModifier;
            }
            if(MoveType == MovementType.Run && character.Stamina <= 0) {
                MoveType = MovementType.Walk;
                SetSpeed();
            }
        }

        protected override void Update() {
            base.Update();
            if (IsRunning) {
                new Sound(gameObject, transform.position, speed);
            }
        }

        /// <summary>
        /// Stops the agent
        /// </summary>
        protected override void PostAction(){
            
            agent.isStopped = true;
            MoveType = MovementType.None;
        }

        /// <summary>
        /// Succeeds if distance between character and targetposition <= min distance
        /// </summary>
        /// <returns></returns>
        protected override bool Success(){

            Vector3 navmeshPos = new Vector3(transform.position.x, targetNavmeshPos.y, transform.position.z);
            
            return Vector3.Distance(navmeshPos, targetNavmeshPos) <= currentTargetDistance;
        }

        /// <summary>
        /// Fails if a path cannot be found or the character cannot move
        /// </summary>
        /// <returns></returns>
        protected override bool Failed(){
            return agent.pathStatus == NavMeshPathStatus.PathInvalid || !character.CanMove;
        }

    }
}