using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP {

    [System.Serializable]
    public class Explore : Goal {


        public Explore() {
            DesiredState = State.Explored;
        }

        public override float CalculatePriority(IGoapCharacter character) {
            return 1;
        }
    }
}