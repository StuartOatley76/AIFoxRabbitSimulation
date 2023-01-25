using System.Collections;

namespace BehaviourTree {

	/// <summary>
	/// Class to represent a Selector on a behaviour tree
	/// </summary>
	public class Selector : Composite {

		/// <summary>
		/// Runs through the attached nodes until one passes
		/// </summary>
		/// <returns></returns>
        public override IEnumerator RunNode() {
			Reset();
			nodeState = NodeState.running;
			for (int i = 0; i < attachedNodes.Count; i++) {
				yield return attachedNodes[i].RunNode();
				nodeState = attachedNodes[i].nodeState;
				if (nodeState == NodeState.passed) {
					yield break;
				}
			}
			nodeState = NodeState.failed;
		}
	}
}
