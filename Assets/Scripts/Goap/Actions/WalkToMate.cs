using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP {

    [System.Serializable]
    public class WalkToMate : WalkToTarget {

        private static Pool<WalkToMate> pool = new Pool<WalkToMate>();

        public WalkToMate() {
            Requirements.Add(State.HasMateTarget);
            Outcomes.Add(State.IsAtMate);
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