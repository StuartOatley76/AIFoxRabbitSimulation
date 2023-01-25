using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Memorable;

namespace BehaviourTree {

    /// <summary>
    /// Class to handle finding water
    /// </summary>
    public class FindWater : FindType {
        
        /// <summary>
        /// Sets the search type
        /// </summary>
        public FindWater() {
            searchType = MemorableObjectType.Water;
        }
    }
}