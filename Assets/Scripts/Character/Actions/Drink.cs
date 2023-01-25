using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters {

    /// <summary>
    /// Class to handle drinking
    /// </summary>
    public class Drink : Consume {

        /// <summary>
        /// How much thirst is reduced by each second.
        /// </summary>
        private float reductionPerSecond;

        /// <summary>
        /// Calculates reductionPerSecond to zero thirst if action fully runs
        /// </summary>
        /// <returns></returns>
        protected override bool PreAction() {
            reductionPerSecond = character.Thirst / secondsConsumtionTakes;
            return base.PreAction();
        }

        /// <summary>
        /// Reduces thirst
        /// </summary>
        protected override void Consumption() {
            character.Thirst -= reductionPerSecond;
        }

        protected override bool Success() {
            return character.Thirst <= 0;
        }
    }
}
