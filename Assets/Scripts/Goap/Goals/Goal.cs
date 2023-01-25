using System;
using Newtonsoft.Json;

namespace GOAP {

    /// <summary>
    /// Class to represent a Goal in GOAP
    /// </summary>
    [Serializable]
    public abstract class Goal : IEquatable<Goal> {

        /// <summary>
        /// property asseccor for desiredState
        /// </summary>
        [JsonProperty]
        public State DesiredState { get; protected set; }

        /// <summary>
        /// Function that calculates the current priority and sets current priority to it
        /// </summary>
        public abstract float CalculatePriority(IGoapCharacter character);

        public bool Equals(Goal other) {
            return this == other;
        }

        public override int GetHashCode() {
            int hashCode = -2075916494;
            hashCode = hashCode * -1521134295 + (int)DesiredState;
            return hashCode;
        }

        public override bool Equals(object other) {
            if(other.GetType() == typeof(Goal)) {
                return (Goal)other == this;
            }
            return false;
        }

        public static bool operator == (Goal goal1, Goal goal2) {
            if(goal1 is null) {
                return goal2 is null;
            }
            if(goal2 is null) {
                return false;
            }
            return goal1.DesiredState == goal2.DesiredState;
        }

        public static bool operator !=(Goal goal1, Goal goal2) {
            return !(goal1 == goal2);
        }


    }
}