using Memorable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree {

    /// <summary>
    /// Class to handle finding a mate
    /// </summary>
    public class FindMate : FindType {
        
        /// <summary>
        /// Sets the search type
        /// </summary>
        public FindMate() {
            searchType = MemorableObjectType.Mate;
        }
    }
}