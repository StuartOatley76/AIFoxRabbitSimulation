using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP {
	[RequireComponent(typeof(Characters.Drink))]
	[System.Serializable]
	public class Drink : GoapAction {

		private static Pool<Drink> pool = new Pool<Drink>();
		public Drink() {
			Requirements.Add(State.IsAtWater);
			Outcomes.Add(State.HasDrunk);
		}
		public override void Initialise(IGoapCharacter goapCharacter) {
			base.Initialise(goapCharacter);
			action = character.GameObject.GetComponent<Characters.Drink>();
		}
		protected override void CalculateCost() {
			currentCost = 1;
		}
		public override GoapAction GetCopy() {
			return pool.GetInstance();
		}

		protected override bool PreAction() {

			return base.PreAction();
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