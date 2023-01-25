using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
namespace GOAP {

    [System.Serializable]
    public class IsSafe : Goal {

        [SerializeField]
        [JsonProperty]
        private float alertLevel = 50f;

        [SerializeField]
        [JsonProperty]
        private float dangerLevel = 100f;

        public IsSafe() {
            DesiredState = State.IsSafe;
        }
        public override float CalculatePriority(IGoapCharacter character) {
            if (character.State.HasFlag(State.IsAlert)) {
                return alertLevel;
            }
            if (character.State.HasFlag(State.IsInDanger)) {
                return dangerLevel;
            }
            return 0;
        }
    }
}