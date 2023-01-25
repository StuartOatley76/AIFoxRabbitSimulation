using Memorable;
using UnityEngine;
using Characters;

namespace GOAP {

    /// <summary>
    /// Interface to implement necessary features for an IGoapCharacter
    /// </summary>
    public interface IGoapCharacter {

        public GameObject GameObject { get; }

        /// <summary>
        /// The character's current target
        /// </summary>
        public GameObject Target { get; set; }

        /// <summary>
        /// How much stamina the character has
        /// </summary>
        public float Stamina { get; set; }

        /// <summary>
        /// Maximum stamina the character can have
        /// </summary>
        public float MaxStamina { get; }

        /// <summary>
        /// Attempts to set the character's target to the nearest known memorableObject of the given type
        /// Fails if one is not known
        /// </summary>
        /// <param name="searchType"></param>
        /// <returns></returns>
        public bool SetTarget(MemorableObjectType searchType);
        bool TargetTypeSensed(MemorableObjectType searchType, out Vector3 targetPosition);

        /// <summary>
        /// How much hunger the character has
        /// </summary>
        public float Hunger { get; set; }

        /// <summary>
        /// How much thirst the character has
        /// </summary>
        public float Thirst { get; set; }

        /// <summary>
        /// How much desire to mate the character has
        /// </summary>
        public float DesireToMate { get; set; }
        public float MaxSpeed { get; }

        /// <summary>
        /// Level at which character is classed at hungry
        /// </summary>
        public float HungryLevel { get; }

        /// <summary>
        /// level at which character is classed as thirsty
        /// </summary>
        public float ThirstyLevel { get; }

        /// <summary>
        /// Level at which character wants to mate
        /// </summary>
        public float HornyLevel { get; }

        /// <summary>
        /// Stamina level below which character is classed as tired
        /// </summary>
        public float TiredLevel { get; }

        public State State { get; }

    }
}
 