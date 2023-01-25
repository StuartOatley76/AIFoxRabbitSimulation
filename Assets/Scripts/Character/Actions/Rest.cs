using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters {

    /// <summary>
    /// Class to handle resting
    /// </summary>
    public class Rest : Consume {

        /// <summary>
        /// How much stamina is increased by each second.
        /// </summary>
        private float increasePerSecond;

        /// <summary>
        /// Calculates increasePerSecond to reach maximum stamina if action fully runs
        /// </summary>
        /// <returns></returns>
        protected override bool PreAction() {
            increasePerSecond = (character.MaxStamina - character.Stamina) / secondsConsumtionTakes;
            return base.PreAction();
        }

        /// <summary>
        /// Increases stamina
        /// </summary>
        protected override void Consumption() {
            character.Stamina += increasePerSecond;
        }

        protected override bool Success() {
            return character.Stamina >= character.MaxStamina;
        }
    }
}