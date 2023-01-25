using System.Collections.Generic;
using UnityEngine;
using Characters;
using Newtonsoft.Json;
namespace GOAP {

    /// <summary>
    /// Abstract class to represent a Goap Action
    /// </summary>
    [System.Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class GoapAction : poolable {

        /// <summary>
        /// The states that are required to perform this action
        /// </summary>
        [JsonProperty]
        public List<State> Requirements { get; private set; } = new List<State>();

        /// <summary>
        /// The states that this action will cause to be fulfilled if it succeeds
        /// </summary>
        [JsonProperty]
        public List<State> Outcomes { get; private set; } = new List<State>();

        /// <summary>
        /// The Character action run by this action
        /// </summary>
        protected CharacterAction action;

        /// <summary>
        /// Whether this action has suceeded
        /// </summary>
        public virtual bool ActionSucceeded { get { return action.Succeeded; } }

        /// <summary>
        /// The cost of this action
        /// </summary>
        public float Cost => GetCost();

        /// <summary>
        /// Whether the action has started
        /// </summary>
        protected bool started = false;

        /// <summary>
        /// Whether the action is running
        /// </summary>
        public bool Running { get { return action.IsRunning; } }

        /// <summary>
        /// The current cost. Will be recalculated if below 0;
        /// </summary>
        protected int currentCost = -1;

        /// <summary>
        /// The character attached to this gameobject
        /// </summary>
        protected IGoapCharacter character;

        /// <summary>
        /// Assigns the character
        /// </summary>
        public virtual void Initialise(IGoapCharacter goapCharacter) {
            character = goapCharacter;
        }

        public abstract GoapAction GetCopy();

        /// <summary>
        /// Calculates the cost if needed then returns it
        /// </summary>
        /// <returns></returns>
        private float GetCost() {
            if(currentCost < 0) {
                CalculateCost();
            }
            return currentCost;
        }

        /// <summary>
        /// Calculates the cost of the action and sets currentCost accordingly
        /// </summary>
        protected abstract void CalculateCost();

        /// <summary>
        /// Stops the action
        /// </summary>
        public virtual void Stop() {
            action.Stop();
            started = false;
        }

        /// <summary>
        /// Starts the action if possible
        /// </summary>
        public void PerformAction() {
            if (PreAction()) {
                action.RunAction();
                started = true;
            }
        }

        /// <summary>
        /// Runs any logic needed before the action starts. Returns whether the action can start
        /// </summary>
        /// <returns></returns>
        protected virtual bool PreAction() { return true; }

        /// <summary>
        /// Runs any logic needed at the end of the action
        /// </summary>
        protected virtual void PostAction() { }

        /// <summary>
        /// returns whether the action can run (ignoring requirements)
        /// </summary>
        /// <returns></returns>
        public virtual bool CanPerformAction() {
            return action != null;
        }

        /// <summary>
        /// Runs post action if the action has finished and resets current cost
        /// </summary>
        public virtual void Update() {
            if(started && !action.IsRunning) {
                PostAction();
                started = false;
            }
            currentCost = -1;
        }

        public abstract void Reset();

        public abstract void ReturnToPool();
    }
}
