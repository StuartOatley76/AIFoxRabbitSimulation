using UnityEngine;

namespace GOAP {

    /// <summary>
    /// Class to handle Goap action of chasing a target
    /// </summary>
    [RequireComponent(typeof(Characters.Chase))]
    [System.Serializable]
    public class Chase : GoapAction {

        private static Pool<Chase> pool = new Pool<Chase>();

        public Chase() {
            Requirements.Add(State.HasFoodTarget);
            Outcomes.Add(State.HasFood);
        }
        public override void Initialise(IGoapCharacter goapCharacter) {
            base.Initialise(goapCharacter);

            action = character.GameObject.GetComponent<Characters.Chase>();
        }
        protected override void CalculateCost() {
            if(character.Target == null) {
                currentCost = 10000;
                return;
            }
            currentCost = (int)(Vector3.Distance(character.Target.transform.position, character.GameObject.transform.position) * character.MaxSpeed);
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