using System.Collections.Generic;
using UnityEngine;
using SpacialHeap;
using System;

namespace Memorable {

    /// <summary>
    /// Class to handle memory
    /// </summary>
    public class Memory : MonoBehaviour {

		/// <summary>
		/// Dictionary to hold an SHeap of objects based on their MemorableObjectType
		/// </summary>
		private Dictionary<MemorableObjectType, SHeap> memories = new Dictionary<MemorableObjectType, SHeap>();

        /// <summary>
        /// Dictionary to hold positions of heard or smelt but not seen objects
        /// </summary>
        private Dictionary<MemorableObjectType, List<Vector3>> unseenMemories = new Dictionary<MemorableObjectType, List<Vector3>>();

		/// <summary>
		/// List of Tuples, containing how long until the object should be forgotten, the MemorableObjectType of the object and the object
		/// </summary>
		List<Tuple<float, MemorableObjectType, IHeapableObject>> toForget = new List<Tuple<float, MemorableObjectType, IHeapableObject>>();

        /// <summary>
		/// List of Tuples, containing how long until the object should be forgotten, the MemorableObjectType of the object and the position
		/// </summary>
		List<Tuple<float, MemorableObjectType, Vector3>> toForgetUnseen = new List<Tuple<float, MemorableObjectType, Vector3>>();

        /// <summary>
        /// Adds the object to the memory, or updates the time to remember if it already exists
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        /// <param name="timeToRemember"></param>
        public void AddObjectToMemory(IHeapableObject obj, MemorableObjectType type, float timeToRemember = -1) {
            if (!memories.TryGetValue(type, out SHeap memory)) {
                memories.Add(type, new SHeap());
                memory = memories.GetValueOrDefault(type);
            }

            if (!memory.AddNewObject(obj)) {
                UpdateForgetTime(obj, type, timeToRemember);
                return;
            }
            if (timeToRemember > 0) {
                toForget.Add(new Tuple<float, MemorableObjectType, IHeapableObject>(timeToRemember, type, obj));
            }

        }

        /// <summary>
        /// Adds an object position for objects that have been heard or smelt but not seen
        /// </summary>
        /// <param name="position"></param>
        /// <param name="type"></param>
        /// <param name="timeToRemember"></param>
        public void AddUnseenObject(Vector3 position, MemorableObjectType type, float timeToRemember) {
            if(!unseenMemories.TryGetValue(type, out List<Vector3> positions)) {
                unseenMemories.Add(type, new List<Vector3>());
                positions = unseenMemories.GetValueOrDefault(type);
            }
            if (positions.Contains(position)) {
                UpdateUnseenTime(position, type, timeToRemember);
                return;
            }
            if (timeToRemember > 0) {
                toForgetUnseen.Add(new Tuple<float, MemorableObjectType, Vector3>(timeToRemember, type, position));
            }

        }


        /// <summary>
        /// Updates the time before the object is forgotten
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        /// <param name="timeToRemember"></param>
        private void UpdateForgetTime(IHeapableObject obj, MemorableObjectType type, float timeToRemember) {
            for (int i = 0; i < toForget.Count; i++) {
                if (toForget[i].Item3 == obj) {
                    toForget[i] = new Tuple<float, MemorableObjectType, IHeapableObject>(timeToRemember, type, obj);
                    return;
                }
            }
        }

        /// <summary>
        /// Returns whether the object exists in memory
        /// </summary>
        /// <param name="mo"></param>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public bool ContainsObject(MemorableObject mo, MemorableObjectType objectType) {
            if(!memories.TryGetValue(objectType, out SHeap memory)) {
                return false;
            }
            return memory.ContainsObject(mo);
        }

        /// <summary>
        /// Updates the time before the unseen object is forgotten
        /// </summary>
        /// <param name="position"></param>
        /// <param name="type"></param>
        /// <param name="timeToRemember"></param>
        private void UpdateUnseenTime(Vector3 position, MemorableObjectType type, float timeToRemember) {
            for (int i = 0; i < toForget.Count; i++) {
                if (toForgetUnseen[i].Item3 == position) {
                    toForgetUnseen[i] = new Tuple<float, MemorableObjectType, Vector3>(timeToRemember, type, position);
                    return;
                }
            }
        }

        /// <summary>
        /// updates times left before objects are forgotten and removes them if necessary
        /// </summary>
        private void Update() {
            for (int i = 0; i < toForget.Count; i++) {
                if (toForget[i].Item1 - Time.deltaTime > 0) {
                    toForget[i] = new Tuple<float, MemorableObjectType, IHeapableObject>(toForget[i].Item1 - Time.deltaTime, toForget[i].Item2, toForget[i].Item3);
                    continue;
                }
                RemoveMemory(toForget[i].Item2, toForget[i].Item3);
            }

            for (int i = 0; i < toForgetUnseen.Count; i++) {
                if (toForgetUnseen[i].Item1 - Time.deltaTime > 0) {
                    toForgetUnseen[i] = new Tuple<float, MemorableObjectType, Vector3>(toForgetUnseen[i].Item1 - Time.deltaTime, toForgetUnseen[i].Item2, toForgetUnseen[i].Item3);
                    continue;
                }
                RemoveUnseenMemory(toForgetUnseen[i].Item2, toForgetUnseen[i].Item3);
            }

            toForget.RemoveAll(r => r.Item1 <= 0);
            toForgetUnseen.RemoveAll(r => r.Item1 <= 0);
        }

        /// <summary>
        /// Removes an object from memory
        /// </summary>
        /// <param name="type"></param>
        /// <param name="obj"></param>
        private void RemoveMemory(MemorableObjectType type, IHeapableObject obj) {
            if (memories.TryGetValue(type, out SHeap memory)) {
                memory.Remove(obj);
            } else {
                Debug.LogError(obj + "not found in memory"); // Sanity check, should never happen
            }
        }

        /// <summary>
        /// Removes an unseen object from memory
        /// </summary>
        /// <param name="type"></param>
        /// <param name="position"></param>
        private void RemoveUnseenMemory(MemorableObjectType type, Vector3 position) {
            if(unseenMemories.TryGetValue(type, out List<Vector3> positions)) {
                positions.Remove(position);
            } else {
                Debug.LogError(position + "not found in memory"); // Sanity check, should never happen
            }
        }

        /// <summary>
        /// Gets all objects of the given type that are within the range from memory
        /// </summary>
        /// <param name="type"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        public List<MemorableObject> GetObjectsOfTypeInRange(MemorableObjectType type, float range) {
            if (!memories.TryGetValue(type, out SHeap heap)) {
                return new List<MemorableObject>();
            }
            return ConvertToMemorable(heap.GetObjectsInRange(transform.position, (int)range));
        }

        /// <summary>
        /// Converts the list of IHeapableObjects to MemorableObjects
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private List<MemorableObject> ConvertToMemorable(List<IHeapableObject> list) {
            List<MemorableObject> toReturn = new List<MemorableObject>();
            for(int i = 0; i < list.Count; i++) {
                if(list[i] is MemorableObject) {
                    toReturn.Add(list[i] as MemorableObject);
                }
            }
            return toReturn;
        }

        /// <summary>
        /// Gets all positions of unseen objects of the given type that are within the range from memory
        /// </summary>
        /// <param name="type"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        public List<Vector3> GetUnseenObjectsPositionsOfTypeInRange(MemorableObjectType type, float range) {
            List<Vector3> inRange = new List<Vector3>();
            if (unseenMemories.TryGetValue(type, out List<Vector3> positions)) {
                for (int i = 0; i < positions.Count; i++) {
                    if(Vector3.Distance(transform.position, positions[i]) <= range) {
                        inRange.Add(positions[i]);
                    }
                }
            }
            return inRange;
        }

    }
}