using Memorable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree {

    /// <summary>
    /// Class to handle moving to water
    /// </summary>
    public class MoveToWater : MoveToTarget {

        /// <summary>
        /// Constructor. Sets targettype to water
        /// </summary>
        public MoveToWater() {
            targetType = MemorableObjectType.Water;
        }
    }
}