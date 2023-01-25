using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    [System.Serializable]
    public class Eat : GoapAction {

        private static Pool<Eat> pool = new Pool<Eat>();
        public Eat() {
            Requirements.Add(State.HasFood);
            Outcomes.Add(State.HasEaten);
        }

        protected override void CalculateCost() {
            currentCost = 1;
        }

        public override void Initialise(IGoapCharacter goapCharacter) {
            base.Initialise(goapCharacter);
            action = character.GameObject.GetComponent<Characters.Eat>();
            
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