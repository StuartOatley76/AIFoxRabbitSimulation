using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP {

    /// <summary>
    /// class to represent the goal of having drunk
    /// </summary>
    [System.Serializable]
    public class HasDrunk : Goal {

        public HasDrunk() {
            DesiredState = State.HasDrunk;
        }

        public override float CalculatePriority(IGoapCharacter character) {
            float thirst = character.Thirst - character.ThirstyLevel;
            return (thirst > 0) ? thirst : 0;

        }
    }
}