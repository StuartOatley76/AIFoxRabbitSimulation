using UnityEngine;
using Characters;

namespace GOAP {

	[System.Serializable]
	public class FindDanger : GoapAction {

		private static Pool<FindDanger> pool = new Pool<FindDanger>();

		private Turn turn;

		public FindDanger() {
			Requirements.Add(State.IsAlert);
			Outcomes.Add(State.IsInDanger);
		}
		public override void Initialise(IGoapCharacter goapCharacter) {
			base.Initialise(goapCharacter);
			turn = character.GameObject.GetComponent<Turn>();
			action = turn;
			
		}
		protected override void CalculateCost() {
			currentCost = 1;
		}

		protected override bool PreAction() {
			SetRotation();
			return base.PreAction();
		}

		private void SetRotation() {
			turn.DegreesToTurn = Random.Range(0f, 360f);
			turn.Clockwise = Random.Range(0, 2) > 0;
		}

		public override void Update() {
			if(started && !turn.IsRunning && !ActionSucceeded) {
				SetRotation();
				turn.RunAction();
			}
			base.Update();
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

		public override bool ActionSucceeded => character.State.HasFlag(State.IsInDanger) || character.State.HasFlag(State.IsSafe);
	}
}