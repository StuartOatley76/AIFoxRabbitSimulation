using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP {

    /// <summary>
    /// Class for walking to a food object
    /// </summary>
    [System.Serializable]
    public class WalkToFood : WalkToTarget {


        private static Pool<WalkToFood> pool = new Pool<WalkToFood>();

        /// <summary>
        /// Assigns requirements and outcomes
        /// </summary>
        public WalkToFood() {
            Requirements.Add(State.HasFoodTarget);
            Outcomes.Add(State.HasFood);
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