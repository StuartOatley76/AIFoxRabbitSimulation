using System.Collections;
using UnityEngine;

namespace Characters {
    public abstract class Consume : CharacterAction {

        /// <summary>
        /// time for consumption to take
        /// </summary>
        [SerializeField]
        protected int secondsConsumtionTakes = 5;

        /// <summary>
        /// Time spent consuming
        /// </summary>
        private float currentTimeConsuming = 0;


        /// <summary>
        /// whole seconds spent consuming
        /// </summary>
        private int currentSecondsConsuming = 0;

        /// <summary>
        /// Consumption can be stopped but never fails
        /// </summary>
        /// <returns></returns>
        protected override bool Failed() {
            return false;
        }

        /// <summary>
        /// sets timers to zera
        /// </summary>
        public override void RunAction() {
            base.RunAction();
            currentTimeConsuming = 0;
            currentSecondsConsuming = 0;
        }

        /// <summary>
        /// Stops any current movement
        /// </summary>
        /// <returns></returns>
        protected override bool PreAction() {
            Movement movement = GetComponent<Movement>();
            if(movement != null) {
                movement.Stop();
            }
            return base.PreAction();
        }

        /// <summary>
        /// updates timers. Consumption takes place once per second
        /// </summary>
        protected override void Update() {
            base.Update();
            if (IsRunning) {
                currentTimeConsuming += Time.deltaTime;
                if (Mathf.Floor(currentTimeConsuming) >= currentSecondsConsuming) {
                    currentSecondsConsuming++;
                    Consumption();
                }
            }
        }

        /// <summary>
        /// Stops the coroutine
        /// </summary>
        public override void Stop() {
            base.Stop();
        }

        /// <summary>
        /// Change stats here
        /// </summary>
        protected abstract void Consumption();

    }
}