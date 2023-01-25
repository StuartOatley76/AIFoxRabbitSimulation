using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters {

    /// <summary>
    /// Abstract class to represent a character action
    /// </summary>
    [RequireComponent(typeof(Character))]
    public abstract class CharacterAction : MonoBehaviour {

        /// <summary>
        /// The character attached to this gameobject
        /// </summary>
        protected Character character;

        /// <summary>
        /// Whether the action is running
        /// </summary>
        public bool IsRunning { get; protected set; } = false;

        /// <summary>
        /// Whether the action has suceeded
        /// </summary>
        public bool Succeeded { get { return Success(); } }

        /// <summary>
        /// Assigns the character
        /// </summary>
        protected virtual void Awake() {
            character = GetComponent<Character>();
        }

        /// <summary>
        /// Determines whether the action should keep running
        /// </summary>
        protected virtual void Update() {
            if (!IsRunning) {
                return;
            }
            UpdateCharacterStats();
            if (Success()) {
                IsRunning = false;
            } else if (Failed()) {
                Stop();
            }
            if (!IsRunning) {
                PostAction();
            }
        }

        /// <summary>
        /// Stops the action
        /// </summary>
        public virtual void Stop() {
            IsRunning = false;
        }

        /// <summary>
        /// Runs any needed logic once the action has stopped
        /// </summary>
        protected virtual void PostAction() {

        }

        /// <summary>
        /// For logic determining whether the action has succeeded
        /// </summary>
        /// <returns></returns>
        protected abstract bool Success();

        /// <summary>
        /// For logic determining whether the action has failed
        /// </summary>
        /// <returns></returns>
        protected abstract bool Failed();

        /// <summary>
        /// Runs any needed logic before the action starts. Returns whether the action can run
        /// </summary>
        /// <returns></returns>
        protected virtual bool PreAction() {
            character.CurrentAction = this;
            return true; 
        }

        /// <summary>
        /// Starts the action
        /// </summary>
        public virtual void RunAction() {
            if (IsRunning) {
                Stop();
            }
            if (!PreAction()) {
                return;
            }
            IsRunning = true;
        }

        /// <summary>
        /// Used to update character stats each frame action is running if needed
        /// </summary>
        protected virtual void UpdateCharacterStats() {
        }
    }
}