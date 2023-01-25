using Memorable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP {

    [System.Serializable]
    public class SearchForWater : Search {

        private static Pool<SearchForWater> pool = new Pool<SearchForWater>();

        public SearchForWater() {
            Outcomes.Add(State.HasWaterTarget);
        }
        protected override MemorableObjectType SearchType {
            get { return MemorableObjectType.Water; }
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