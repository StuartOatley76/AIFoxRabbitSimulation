using System;
using System.Collections;
using UnityEngine;

namespace BehaviourTree {

	/// <summary>
	/// Possible states for a node
	/// </summary>
	public enum NodeState {
		ready, // Node is ready to run
		passed, // Node has passed
		running, // Node is running
		failed // Node failed
	}

	/// <summary>
	/// Abstract class to represent a node on a behaviour tree
	/// </summary>
	public abstract class Node : ScriptableObject {

		/// <summary>
		/// State of the node
		/// </summary>
		public NodeState nodeState = NodeState.ready;

		/// <summary>
		/// EventHandler for interrupting the tree
		/// </summary>
		protected EventHandler<EventArgs> interruptHandler;

		/// <summary>
		/// The character running the tree
		/// </summary>
		protected IBTreeCharacter character;

		/// <summary>
		/// Function run by the tree's coroutine
		/// </summary>
		/// <returns></returns>
		public abstract IEnumerator RunNode();

		/// <summary>
		/// id used in tree creation
		/// </summary>
		public string guid;

		/// <summary>
		/// Position on the graph in the editor scene
		/// </summary>
		public Vector2 position;

		/// <summary>
		/// Used to stop the tree
		/// </summary>
		public void Interrupt() {
			interruptHandler?.Invoke(this, EventArgs.Empty);
        }

		/// <summary>
		/// Perform and necessary initialising here
		/// </summary>
		protected virtual void Initialise() {

        }

		/// <summary>
		/// Sets the character, then initialises the node
		/// </summary>
		/// <param name="newCharacter"></param>
		public void SetCharacter(IBTreeCharacter newCharacter) {
			character = newCharacter;
			Initialise();
		}

		/// <summary>
		/// Resets the node to original state
		/// Should always call base.Reset() once done
		/// </summary>
		public virtual void Reset() {
			nodeState = NodeState.ready;
		}

		/// <summary>
		/// Returns a copy of the node
		/// </summary>
		/// <returns></returns>
		public virtual Node Clone() {
			return Instantiate(this);
        }

		/// <summary>
		/// Connects the interrupt
		/// </summary>
		/// <param name="eventDel"></param>
		public virtual void ConnectInterrupt(EventHandler<EventArgs> eventDel) {
			interruptHandler += eventDel;
        }
	}
}
