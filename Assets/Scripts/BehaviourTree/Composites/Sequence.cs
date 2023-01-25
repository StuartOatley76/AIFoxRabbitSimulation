using System.Collections;
using UnityEngine;

namespace BehaviourTree {

	/// <summary>
	/// Class to represent a sequence on a behaviour tree
	/// </summary>
	public class Sequence : Composite {

		/// <summary>
		/// Runs through attached nodes until one fails
		/// </summary>
		/// <returns></returns>
        public override IEnumerator RunNode() {
			Reset();
			nodeState = NodeState.running;
			for (int i = 0; i < attachedNodes.Count; i++) {
				yield return attachedNodes[i].RunNode();
				nodeState = attachedNodes[i].nodeState;
				if (nodeState == NodeState.failed) {
					yield break;
				}
			}
		}
	}
}
