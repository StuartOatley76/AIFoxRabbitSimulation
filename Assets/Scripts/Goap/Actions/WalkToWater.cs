using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP {

    [System.Serializable]
    public class WalkToWater : WalkToTarget {

        private static Pool<WalkToWater> pool = new Pool<WalkToWater>();
        public WalkToWater() {
            Requirements.Add(State.HasWaterTarget);
            Outcomes.Add(State.IsAtWater);
        }

		public override GoapAction GetCopy() {
			return pool.GetInstance();
		}
        protected override bool PreAction() {
            return base.PreAction();
        }

        protected override void PostAction() {
            base.PostAction();
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