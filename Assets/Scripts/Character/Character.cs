using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Genetics;
using Memorable;
using Sensors;
using GOAP;
using System.Collections;

namespace Characters {

    /// <summary>
    /// class to represent a character
    /// </summary>
    [RequireComponent(typeof(Memory), typeof(SensorManager))]
    public class Character : MemorableObject, IGoapCharacter, BehaviourTree.IBTreeCharacter {


        public GameObject GameObject { get { return gameObject; } }

        /// <summary>
        /// The character's target
        /// </summary>
        public GameObject Target { get; set; }

        /// <summary>
        /// The prefab for this character
        /// Used for reproduction
        /// </summary>
        [SerializeField]
        private GameObject prefab;

        /// <summary>
        /// Offset for positioning new spawns in reproduction
        /// </summary>
        [SerializeField]
        private Vector3 spawnOffset;

        /// <summary>
        /// Current stamina level
        /// </summary>
        public float Stamina { get; set; }

        /// <summary>
        /// current hunger level
        /// </summary>
        public float Hunger { get; set; } = 0;

        /// <summary>
        /// current thirst level
        /// </summary>
        public float Thirst { get; set; } = 0;

        /// <summary>
        /// current desire to mate level
        /// </summary>
        public float DesireToMate { get; set; } = 0;

        /// <summary>
        /// Walking speed of the character
        /// </summary>
        public float BaseSpeed { get; protected set; } = 2;

        /// <summary>
        /// Running speed of the character
        /// </summary>
        public float MaxSpeed { get { return Dna.Speed; } }

        /// <summary>
        /// multiplier to increase thirst when running
        /// </summary>
        public float ThirstModifier { get; protected set; } = 0.1f;

        /// <summary>
        /// multiplier to increase hunger when running
        /// </summary>
        public float HungerModifier { get; protected set; } = 0.1f;

        /// <summary>
        /// multiplier to increase stamina use when running
        /// </summary>
        public float StaminaModifier { get; protected set; } = 1f;

        /// <summary>
        /// Level at which character is classed at hungry
        /// </summary>
        public float HungryLevel { get { return Dna.HungerLevel; } }

        /// <summary>
        /// level at which character is classed as thirsty
        /// </summary>
        public float ThirstyLevel { get { return Dna.ThirstLevel; } }

        /// <summary>
        /// Level at which character wants to mate
        /// </summary>
        public float HornyLevel { get { return Dna.HornyLevel; } }

        /// <summary>
        /// Stamina level below which character is classed as tired
        /// </summary>
        public float TiredLevel { get { return Dna.TiredLevel; } }

        /// <summary>
        /// Level of hunger at which the character dies
        /// </summary>
        [SerializeField]
        private float starvationLevel = 200f;

        /// <summary>
        /// Level of thirst at which the character dies
        /// </summary>
        [SerializeField]
        private float dieOfThirstLevel = 200f;


        private bool isDying = false;
        /// <summary>
        /// The character's DNA
        /// </summary>
        protected DNA Dna { get; set; }

        /// <summary>
        /// hunger increase per tick
        /// </summary>
        [SerializeField]
        private float hungerIncreasePerTick = 0.01f;

        /// <summary>
        /// thirst increase per tick
        /// </summary>
        [SerializeField]
        private float thirstIncreasePerTick = 0.02f;

        /// <summary>
        /// DesireToMate increase per tick
        /// </summary>
        [SerializeField]
        private float desireToMateIncreasePerTick = 0.3f;

        /// <summary>
        /// Action the character is currently performing
        /// </summary>
        public CharacterAction CurrentAction { get; set; }

        /// <summary>
        /// Whether yje character has been caught by a threat
        /// </summary>
        public bool IsCaught { get; private set; }

        /// <summary>
        /// Maximum stamina level
        /// </summary>
        [SerializeField]
        private float maxStamina;

        public float MaxStamina { get { return maxStamina; } } 

        /// <summary>
        /// The character's memory
        /// </summary>
        private Memory memory;

        /// <summary>
        /// The character's sensor manager
        /// </summary>
        private SensorManager sensorManager;

        /// <summary>
        /// distance at which an object is classed as near this character
        /// </summary>
        [SerializeField]
        private float neabyDistance = 2f;

        /// <summary>
        /// range at which an object is targetable
        /// </summary>
        [SerializeField]
        private float targetableRange = 25f;

        /// <summary>
        /// How many seconds 
        /// </summary>
        [SerializeField]
        private int secondsBeforeDeath = 5;

        /// <summary>
        /// The character's state
        /// </summary>
        private State state;

        /// <summary>
        /// Accessor for the state. Updates fiorst if it hasn't been updated this frame
        /// </summary>
        public State State {
            get {
                if (!statesUpdatedThisFrame) {
                    state = UpdateState();
                }
                return state;
            }
        }

        /// <summary>
        /// Whether the state has been updated this frame
        /// </summary>
        private bool statesUpdatedThisFrame = false;

        /// <summary>
        /// Hashset of the types of objects in range
        /// </summary>
        private HashSet<MemorableObjectType> typesInRange = new HashSet<MemorableObjectType>();

        /// <summary>
        /// Whether this character is male or female
        /// </summary>
        public Sex Sex { get { return Dna.Sex; } }

        /// <summary>
        /// Returns a list of known threats within the range supplied
        /// </summary>
        /// <param name="maxThreatDistance"></param>
        /// <returns></returns>
        public List<MemorableObject> GetThreats(float maxThreatDistance) {
            return memory.GetObjectsOfTypeInRange(MemorableObjectType.Threat, maxThreatDistance);
        }

        /// <summary>
        /// Returns whether the supplied object is a threat
        /// </summary>
        /// <param name="mo"></param>
        /// <returns></returns>
        private bool IsThreat(MemorableObject mo) {
            return sensorManager.GetMemorableObjectType(mo) == MemorableObjectType.Threat;
		}

        /// <summary>
        /// performs initialisation
        /// </summary>
        protected override void Awake() {
            base.Awake();
            Dna = new DNA();
            canMove = true;
            memory = GetComponent<Memory>();
            sensorManager = GetComponent<SensorManager>();
            sensorManager.SetRanges(Dna.HearingRange, Dna.SmellRange);
            Stamina = MaxStamina;
        }

        /// <summary>
        /// Returns the movement type the character is using
        /// </summary>
        /// <returns></returns>
        public Movement.MovementType GetMovementType() {
            if(GetComponent<Movement>() == null) {
                return Movement.MovementType.None;
            }
            return GetComponent<Movement>().MoveType;
        }

        /// <summary>
        /// Kills the character
        /// </summary>
		public void Die() {
            canMove = false;
            isDying = true;
            Counter.Instance.Remove(tag);
            StartCoroutine(Death());
		}

        /// <summary>
        /// On death waits before destroying the game object
        /// </summary>
        /// <returns></returns>
        private IEnumerator Death() {
            yield return new WaitForSeconds(secondsBeforeDeath);
            Destroy(gameObject);
        }

        /// <summary>
        /// Updates the state
        /// </summary>
        /// <returns></returns>
        private State UpdateState() {
            State stateThisFrame = 0;
            CheckForNearby();
            MemorableObjectType targetType = GetTargetType();
            switch (targetType) {
                case MemorableObjectType.Food:
                    stateThisFrame |= State.HasFoodTarget;
                    break;
                case MemorableObjectType.Water:
                    stateThisFrame |= State.HasWaterTarget;
                    break;
                case MemorableObjectType.Mate:
                    stateThisFrame |= State.HasMateTarget;
                    break;
                default:
                    break;
            }
            if (Hunger >= HungryLevel) {
                stateThisFrame |= State.IsHungry;
            }
            if (Thirst >= ThirstyLevel) {
                stateThisFrame |= State.IsThirsty;
            }
            if (DesireToMate >= HornyLevel) {
                stateThisFrame |= State.WantsToMate;
            }
            if(Stamina <= TiredLevel) {
                stateThisFrame |= State.IsTired;
            }
            if (typesInRange.Contains(MemorableObjectType.Food)) {
                stateThisFrame |= State.HasFood;
            }
            if (typesInRange.Contains(MemorableObjectType.Water)) {
                stateThisFrame |= State.IsAtWater;
            }
            if (typesInRange.Contains(MemorableObjectType.Mate)) {
                stateThisFrame |= State.IsAtMate;
            }
            if (IsInDanger()) {
                stateThisFrame |= State.IsInDanger;
            } else if (IsAlert()) {
                stateThisFrame |= State.IsAlert;
            } else {
                stateThisFrame |= State.IsSafe;
            }

            statesUpdatedThisFrame = true;
            return stateThisFrame;
        }

        /// <summary>
        /// Returns the type of the current target
        /// </summary>
        /// <returns></returns>
        private MemorableObjectType GetTargetType() {
            if(Target == null) {
                return (MemorableObjectType)(-1);
            }
            MemorableObject mo = Target.GetComponent<MemorableObject>();
            if(mo == null) {
                return (MemorableObjectType)(-1);
            }
            return sensorManager.GetMemorableObjectType(mo);
        }

        /// <summary>
        /// Checks for any objects in the immediate area. Used for whether the character is at that type
        /// </summary>
        private void CheckForNearby() {
            typesInRange.Clear();
            Collider[] colliders = Physics.OverlapSphere(transform.position, neabyDistance);
            for(int i = 0; i < colliders.Length; i++) {
                MemorableObject mo = colliders[i].gameObject.GetComponent<MemorableObject>();
                if(mo == null) {
                    continue;
                }
                typesInRange.Add(sensorManager.GetMemorableObjectType(mo));
            }
        }


        /// <summary>
        /// Checks if there are any seen threats in targetable range
        /// </summary>
        /// <returns></returns>
        private bool IsInDanger() {
            if (memory.GetObjectsOfTypeInRange(MemorableObjectType.Threat, targetableRange).Count > 0) {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks if there are any heard or smelt threats in targetable range
        /// </summary>
        /// <returns></returns>
        private bool IsAlert() {
            if (memory.GetUnseenObjectsPositionsOfTypeInRange(MemorableObjectType.Threat, targetableRange).Count > 0) {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Creates a new character, combining the dna from both parents
        /// </summary>
        /// <param name="mate"></param>
        public void Reproduce(Character mate) {
            DNA otherDNA = mate.Dna;
            if (prefab.GetComponent<Character>() != null) {
                GameObject child = Instantiate(prefab, transform.position + spawnOffset, Quaternion.identity);
                child.GetComponent<Character>().Dna = new DNA(otherDNA, Dna);
                sensorManager.SetRanges(Dna.HearingRange, Dna.SmellRange);
                DesireToMate = 0;
                mate.DesireToMate = 0;
                Counter.Instance.Add(child.tag);
            }
        }

        /// <summary>
        /// If a character collides with a threat, they die
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionEnter(Collision collision) {
            MemorableObject mo = collision.gameObject.GetComponent<MemorableObject>();
            if (mo != null && IsThreat(mo)) {
                IsCaught = true;
                Die();
            }
        }

        /// <summary>
        /// Checks memory for any seen objects of the type and if it has some, sets the target to the nearest one
        /// </summary>
        /// <param name="searchType"></param>
        /// <returns></returns>
        public bool SetTarget(MemorableObjectType searchType) {
            List<MemorableObject> mos = memory.GetObjectsOfTypeInRange(searchType, targetableRange);
            if(mos.Count == 0) {
                return false;
            }
            mos.OrderBy(d => Vector3.Distance(d.Position, transform.position));
            Target = mos[0].gameObject;
            return true;
        }

        /// <summary>
        /// Checks memory for any unseen objects of the type and if it has some, returns the position the nearest one was sensed
        /// </summary>
        /// <param name="searchType"></param>
        /// <param name="targetPosition"></param>
        /// <returns></returns>
        public bool TargetTypeSensed(MemorableObjectType searchType, out Vector3 targetPosition) {
            List<Vector3> positions = memory.GetUnseenObjectsPositionsOfTypeInRange(searchType, targetableRange);
            if(positions.Count == 0) {
                targetPosition = Vector3.zero;
                return false;
            }
            positions.OrderBy(d => Vector3.Distance(d, transform.position));
            targetPosition = positions[0];
            return true;
        }

        /// <summary>
        /// Changes stats. Checks for starvation or death by thirst
        /// </summary>
        protected virtual void Update() {
            if (isDying) {
                return;
            }
            Thirst += thirstIncreasePerTick * Time.deltaTime;
            Hunger += hungerIncreasePerTick * Time.deltaTime;
            DesireToMate += desireToMateIncreasePerTick * Time.deltaTime;
            if(Thirst >= dieOfThirstLevel || Hunger >= starvationLevel) {
                Die();
            }
        }

        /// <summary>
        /// Resets statesUpdatedThisFrame to false
        /// </summary>
        private void LateUpdate() {
            statesUpdatedThisFrame = false;
        }

    }
}
