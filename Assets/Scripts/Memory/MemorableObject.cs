using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpacialHeap;
using System;

namespace Memorable {

    /// <summary>
    /// Class to implement IheapableObject as an object that can be remembered
    /// </summary>
    public class MemorableObject : MonoBehaviour, IHeapableObject {

        public HashSet<SHeap> Heaps { get; private set; } = new HashSet<SHeap>();

        /// <summary>
        /// Whether this gameobject moves
        /// </summary>
        protected bool canMove;
        public bool CanMove { get { return canMove; } }

        public Vector3 Position { get { return transform.position; } } 

        /// <summary>
        /// adds object to worldheap
        /// </summary>
        protected virtual void Awake() {
            StartCoroutine(AddToWorldHeap());
        }

        /// <summary>
        /// We wait one frame if worldheap.instance is null so that if this awake runs before 
        /// worldheap's awake runs it is still added
        /// </summary>
        /// <returns></returns>
        private IEnumerator AddToWorldHeap() {
            if (WorldHeap.instance == null) {
                yield return null;
            }
            if (WorldHeap.instance != null) {
                WorldHeap.instance.addObject(this);
            }
        }

        /// <summary>
        /// removes the gameobject from any heaps it is in
        /// </summary>
        protected virtual void OnDestroy() {
            (this as IHeapableObject).RemoveFromHeaps();
        }

    }
}
