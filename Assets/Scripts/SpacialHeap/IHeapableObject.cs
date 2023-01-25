using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpacialHeap {
    /// <summary>
    /// Interface for objects that can be added to a spacial heap
    /// </summary>
    public interface IHeapableObject {

        /// <summary>
        /// hashset of the SHeaps this belongs to (Updated by SHeap)
        /// </summary>
        public HashSet<SHeap> Heaps { get; }
        /// <summary>
        /// Whether the object can move (and therefore needs position updating)
        /// </summary>
        bool CanMove { get; }

        /// <summary>
        /// Object's position
        /// </summary>
        Vector3 Position { get; }

        /// <summary>
        /// Removes the object from any SHeaps it is in
        /// </summary>
        public void RemoveFromHeaps() {
            foreach(SHeap heap in Heaps) {
                if (heap != null) {
                    heap.Remove(this);
                }
            }
        }
    }

}