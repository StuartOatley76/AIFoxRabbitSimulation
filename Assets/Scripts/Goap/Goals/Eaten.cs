using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP {

    /// <summary>
    /// Class to represent goal of having eaten
    /// </summary>
    [System.Serializable]
    public class Eaten : Goal {


        public Eaten() {
            DesiredState = State.HasEaten;
        }

        public override float CalculatePriority(IGoapCharacter character) {
            float hungerLevel = character.Hunger - character.HungryLevel;
            return (hungerLevel > 0) ? hungerLevel : 0;
        }
    }
}