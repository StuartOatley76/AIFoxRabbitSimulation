using Memorable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP {
    [System.Serializable]
    public class SearchForFood : Search {

        private static Pool<SearchForFood> pool = new Pool<SearchForFood>();

        public SearchForFood() {
            Outcomes.Add(State.HasFoodTarget);
        }
        protected override MemorableObjectType SearchType {
            get { return MemorableObjectType.Food; }
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