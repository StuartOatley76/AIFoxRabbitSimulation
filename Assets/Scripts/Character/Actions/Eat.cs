using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Characters {

    /// <summary>
    /// Class to handle eating. Reduces hunger to zero if action completes
    /// </summary>
    public class Eat : Consume {

        /// <summary>
        /// How much hunger is reduced by each second.
        /// </summary>
        private float calsPerSecond;

        /// <summary>
        /// Calculates calsPerSecond to zero hunger if action fully runs
        /// </summary>
        /// <returns></returns>
        protected override bool PreAction() {
            calsPerSecond = character.Hunger / secondsConsumtionTakes;
            return base.PreAction();
        }

        /// <summary>
        /// Reduces hunger
        /// </summary>
        protected override void Consumption() {
            character.Hunger -= calsPerSecond;
        }

        protected override bool Success() {
            return character.Hunger <= 0;
        }
    }
}