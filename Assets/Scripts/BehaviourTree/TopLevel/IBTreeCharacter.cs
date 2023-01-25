using Memorable;
using UnityEngine;
using GOAP;

namespace BehaviourTree {

    /// <summary>
    /// Interface to implement needed functionality for a Behaviour tree character
    /// </summary>
    public interface IBTreeCharacter {

        /// <summary>
        /// Supplies the character's gameobject
        /// </summary>
        public GameObject GameObject { get; }

        /// <summary>
        /// Supplies the character's state
        /// </summary>
        public State State { get; }

        /// <summary>
        /// Tries to set the character's target to one of the supplied type
        /// </summary>
        /// <param name="searchType"></param>
        /// <returns>Whether or not the target could be set</returns>
        public bool SetTarget(MemorableObjectType searchType);

        /// <summary>
        /// Checks whether a target type can be sensed (smelt or heard)
        /// </summary>
        /// <param name="searchType"></param>
        /// <param name="targetPosition">Position of the type is sensed</param>
        /// <returns>Whether or not the type could be sensed</returns>
        bool TargetTypeSensed(MemorableObjectType searchType, out Vector3 targetPosition);
    }
}