using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP {

    /// <summary>
    /// Class to represent the goal of having rested
    /// </summary>
    [System.Serializable]
    public class Rested : Goal {

        public Rested() {
            DesiredState = State.HasRested;
        }

        public override float CalculatePriority(IGoapCharacter character) {
            float tired = character.MaxStamina - character.Stamina - character.TiredLevel;
            return (tired > 0) ? tired : 0;
        }
    }
}