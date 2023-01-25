using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP {

    /// <summary>
    /// Class to represent the goal of having mated
    /// </summary>
    [System.Serializable]
    public class HasMated : Goal {


        public HasMated() {
            DesiredState = State.HasMated;
        }

        public override float CalculatePriority(IGoapCharacter character) {
            float desireToMate = character.DesireToMate - character.HornyLevel;
            return (desireToMate > 0) ? desireToMate : 0;
        }
    }
}