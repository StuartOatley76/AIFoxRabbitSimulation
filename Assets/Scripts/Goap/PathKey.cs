using System;
using Newtonsoft.Json;

namespace GOAP {

    /// <summary>
    /// Class to act as a key in the action planner dictionaries
    /// </summary>
    [Serializable]
    public class PathKey : IEquatable<PathKey>{

        /// <summary>
        /// The goal the path fulfills
        /// </summary>
        [JsonProperty]
        private Goal goal;

        /// <summary>
        /// The initial state of the character
        /// </summary>
        [JsonProperty]
        private State state;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="theGoal"></param>
        /// <param name="currentState"></param>
        public PathKey(Goal theGoal, State currentState) {
            goal = theGoal;
            state = currentState;
        }

        /// <summary>
        /// Implemeted to allow use as a key in a dictionary
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(PathKey other) {
            return this == other;
        }

        /// <summary>
        /// Implemeted to allow use as a key in a dictionary
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public override bool Equals(object other) {
            if (other.GetType() == typeof(PathKey)) {
                return (PathKey)other == this;
            }
            return false;
        }

        /// <summary>
        /// Implemeted to allow use as a key in a dictionary
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() {
            int hashCode = -890889252;
            hashCode = hashCode * -1521134295 + goal.GetHashCode();
            hashCode = hashCode * -1521134295 + (int)state;
            return hashCode;
        }

        /// <summary>
        /// Implemeted to allow use as a key in a dictionary
        /// </summary>
        /// <param name="key1"></param>
        /// <param name="key2"></param>
        /// <returns></returns>
        public static bool operator == (PathKey key1, PathKey key2) {
            return key1.goal == key2.goal &&
                (int)key1.state == (int)key2.state;
        }

        /// <summary>
        /// Implemeted to allow use as a key in a dictionary
        /// </summary>
        /// <param name="key1"></param>
        /// <param name="key2"></param>
        /// <returns></returns>
        public static bool operator !=(PathKey key1, PathKey key2) {
            return !(key1 == key2);
        }


    }
}