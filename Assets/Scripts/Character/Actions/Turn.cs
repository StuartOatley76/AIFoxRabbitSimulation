using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters {

	/// <summary>
	/// class to give a character action that rotates the character
	/// </summary>
	public class Turn : CharacterAction {

		/// <summary>
		/// Whether the rotation should be clockwise
		/// </summary>
		public bool Clockwise { private get; set; }

		/// <summary>
		/// how much rotation should be applied
		/// </summary>
		private float degreesToTurn;

		/// <summary>
		/// Accessor to allow setting the amount of rotation
		/// </summary>
		public float DegreesToTurn {
			private get {
				return degreesToTurn;
			} 
			set {
				value = Mathf.Abs(value);
				value = value % 360f;
				degreesToTurn = value;
			} 
		}

		/// <summary>
		/// how many degrees the character has turned during this action
		/// </summary>
		private float degreesTurned = 0;

		/// <summary>
		/// How long the rotation should take
		/// </summary>
		[SerializeField]
		private float timeToTurn;

		/// <summary>
		/// degreesToTurn / timeToTurn for this rotation
		/// </summary>
		private float rotationAmount = 0;

		/// <summary>
		/// performs initialisation at the start of the action
		/// </summary>
		/// <returns></returns>
		protected override bool PreAction(){
			degreesTurned = 0;
			rotationAmount = degreesToTurn / timeToTurn;
			return base.PreAction();
		}

		/// <summary>
		/// Rotates the character if running
		/// </summary>
		protected override void Update() {
			base.Update();
			if (IsRunning) {
				float rotate = rotationAmount * Time.deltaTime;
				rotate = (Clockwise) ? rotate : -rotate;
				gameObject.transform.Rotate(Vector3.up, rotate, Space.World);
				degreesTurned += rotate;
			}
		}

		/// <summary>
		/// rotation never fails
		/// </summary>
		/// <returns></returns>
		protected override bool Failed() {
			return false;
		}

		/// <summary>
		/// Success happens when degrees turned >= degrees to turn
		/// </summary>
		/// <returns></returns>
		protected override bool Success() {
			return degreesTurned >= degreesToTurn;
		}
	}
}