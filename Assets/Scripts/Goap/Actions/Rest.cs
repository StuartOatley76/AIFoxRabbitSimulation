using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GOAP {
    [System.Serializable]
    public class Rest : GoapAction {

        private static Pool<Rest> pool = new Pool<Rest>();

        public Rest() {
            Requirements.Add(State.IsSafe);
            Outcomes.Add(State.HasRested);
        }
        public override void Initialise(IGoapCharacter goapCharacter) {
            base.Initialise(goapCharacter);
            action = character.GameObject.GetComponent<Characters.Rest>();
        }
        protected override void CalculateCost() {
            currentCost = 1;
        }

        public override GoapAction GetCopy() {
            return pool.GetInstance();
        }

        public override void Reset() {
            character = null;
            action = null;
        }

        public override void ReturnToPool() {
            pool.Return(this);
        }
    }
}