using UnityEngine;

namespace GOAP {

    /// <summary>
    /// Abstract Goap Action to walk to a target
    /// </summary>
    [System.Serializable]
    public abstract class WalkToTarget : GoapAction {

        /// <summary>
        /// assigns the character action
        /// </summary>
        public override void Initialise(IGoapCharacter goapCharacter) {
            base.Initialise(goapCharacter);
            action = character.GameObject.GetComponent<Characters.WalkToTarget>();
        }

        /// <summary>
        /// Uses distance to the target as the cost
        /// </summary>
        protected override void CalculateCost() {
            if (character.Target == null) {
                currentCost = 10000;
                return;
            }
            currentCost = (int)Vector3.Distance(character.GameObject.transform.position, character.Target.transform.position);
        }
    }
}