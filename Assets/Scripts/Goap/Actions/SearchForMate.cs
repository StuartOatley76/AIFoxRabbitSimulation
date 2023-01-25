using Memorable;
using UnityEngine;

namespace GOAP {
    [System.Serializable]
    public class SearchForMate : Search {

        private static Pool<SearchForMate> pool = new Pool<SearchForMate>();

        public SearchForMate() {
            Outcomes.Add(State.HasMateTarget);
        }
        protected override MemorableObjectType SearchType {
            get { return MemorableObjectType.Mate; }
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