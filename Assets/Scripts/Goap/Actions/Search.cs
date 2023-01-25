using UnityEngine;
using Memorable;
using Characters;
using Newtonsoft.Json;
namespace GOAP {

    /// <summary>
    /// abstract class to handle searching for a MemorableObjectType
    /// </summary>
    [System.Serializable]
    public abstract class Search : GoapWander {
        
        [JsonProperty]
        protected abstract MemorableObjectType SearchType { get; }

        private bool found = false;

        private Wander wander;
        private Movement movement;

        private Vector3 lastDetectedPosition = Vector3.zero;

        public Search() {
            Outcomes.Remove(State.Explored);
        }

        public override void Initialise(IGoapCharacter goapCharacter) {
            base.Initialise(goapCharacter);
            
            wander = character.GameObject.GetComponent<Wander>();
            movement = character.GameObject.GetComponent<Movement>();
        }
        protected override bool PreAction() {
            found = false;
            return base.PreAction();
        }

        public override void Update() {
            if (character.SetTarget(SearchType)){
                found = true;
                action.Stop();
                base.Update();
                return;
            }

            bool canSenseTarget = character.TargetTypeSensed(SearchType, out Vector3 targetPosition);

            if((action == wander && canSenseTarget) || (action == movement && targetPosition != lastDetectedPosition)) {
                action.Stop();
                action = movement;
                movement.TargetPosition = targetPosition;
                lastDetectedPosition = targetPosition;
                action.RunAction();
                base.Update();
                return;
            }

            if(action == movement && !canSenseTarget) {
                action.Stop();
                action = wander;
                action.RunAction();
            }
            base.Update();
        }

        public override bool ActionSucceeded => found;

    }
}