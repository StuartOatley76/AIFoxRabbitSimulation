using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP {

	[System.Serializable]
	public class Mate : GoapAction {

		private static Pool<Mate> pool = new Pool<Mate>();
		public Mate() {
			Requirements.Add(State.IsAtMate);
			Outcomes.Add(State.HasMated);
		}
		public override void Initialise(IGoapCharacter goapCharacter) {
			base.Initialise(goapCharacter);
			action = character.GameObject.GetComponent<Characters.Mate>();

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