using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Characters;

namespace GOAP {

    [RequireComponent(typeof(IGoapCharacter), typeof(Wander))]
    [System.Serializable]
    public class GoapWander : GoapAction {

        private static Pool<GoapWander> pool = new Pool<GoapWander>();
        public GoapWander() {
            Outcomes.Add(State.Explored);
        }
        public override void Initialise(IGoapCharacter goapCharacter) {
            base.Initialise(goapCharacter);
            action = character.GameObject.GetComponent<Wander>();
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