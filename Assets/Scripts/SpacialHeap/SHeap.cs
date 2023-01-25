using System.Collections.Generic;
using UnityEngine;

namespace SpacialHeap {
    public class SHeap  {

        /// <summary>
        /// Size of a cell (in unity units)
        /// </summary>
        private int cellsize;

        /// <summary>
        /// default size for cells
        /// </summary>
        private const int defaultCellsize = 2;

        /// <summary>
        /// The heap to store object positions
        /// </summary>
        private readonly Dictionary<Vector3Int, List<IHeapableObject>> heap = new Dictionary<Vector3Int, List<IHeapableObject>>();

        /// <summary>
        /// List of the objects in the heap that can move
        /// </summary>
        private readonly List<KeyValuePair<Vector3Int, IHeapableObject>> movingObjects = new List<KeyValuePair<Vector3Int, IHeapableObject>>();

        /// <summary>
        /// List of all the objects in the heap
        /// </summary>
        private readonly List<IHeapableObject> allobjects = new List<IHeapableObject>();

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="sizeOfCells"></param>
        public SHeap(int sizeOfCells = defaultCellsize) {
            cellsize = sizeOfCells;
        }

        /// <summary>
        /// Whether the heap contains the provided object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool ContainsObject(IHeapableObject obj) {
            return allobjects.Contains(obj);
        }
        /// <summary>
        /// Adds a new object to the heap if it is not already in the heap
        /// </summary>
        /// <param name="obj">Object to add</param>
        /// <returns>Whether the adding succeeded</returns>
        public bool AddNewObject(IHeapableObject obj) {
            if (obj == null || allobjects.Contains(obj)) {
                return false;
            }
            obj.Heaps.Add(this);
            allobjects.Add(obj);
            Vector3Int key = GetKey(obj.Position);
            AddObject(key, obj);
            if (obj.CanMove) {
                movingObjects.Add(new KeyValuePair<Vector3Int, IHeapableObject>(key, obj));
            }
            return true;
        }

        /// <summary>
        /// Adds an object to the bucket held under the given key (creates new bucket if no existing one)
        /// </summary>
        /// <param name="key">Key to add to or create</param>
        /// <param name="obj">Object to add</param>
        private void AddObject(Vector3Int key, IHeapableObject obj) {
            if (heap.TryGetValue(key, out List<IHeapableObject> bucket)) {
                bucket.Add(obj);
            } else {
                bucket = new List<IHeapableObject> {
                    obj
                };
                heap.Add(key, bucket);
            }
        }

        /// <summary>
        /// Updates the positions of the objects in movingObjects
        /// </summary>
        public void UpdatePositions() {
            movingObjects.RemoveAll(r => r.Value == null);

            for(int i = movingObjects.Count - 1; i >= 0; i--){
                KeyValuePair<Vector3Int, IHeapableObject> record = movingObjects[i];
                Vector3Int newKey = GetKey(record.Value.Position);
                if (newKey == record.Key) {
                    continue;
                }
                if (heap.TryGetValue(record.Key, out List<IHeapableObject> bucket)) {
                    bucket.Remove(record.Value);
                    AddObject(newKey, record.Value);
                    movingObjects.Add(new KeyValuePair<Vector3Int, IHeapableObject>(newKey, record.Value));
                    movingObjects.Remove(record);
                }
            }
        }

        /// <summary>
        /// Removes the object from the heap
        /// </summary>
        /// <param name="heapableObject">Object to remove</param>
        public void Remove(IHeapableObject heapableObject) {
            if (!allobjects.Contains(heapableObject)) {
                return;
            }
            allobjects.Remove(heapableObject);

            RemoveFromHeap(heapableObject);
            if (heapableObject.CanMove) {
                RemoveFromMoveable(heapableObject);
            }
        }

        /// <summary>
        /// Removes the object from the movable list
        /// </summary>
        /// <param name="heapableObject"></param>
        private void RemoveFromMoveable(IHeapableObject heapableObject) {
            foreach(KeyValuePair<Vector3Int, IHeapableObject> record in movingObjects) {
                if(record.Value == heapableObject) {
                    movingObjects.Remove(record);
                    return;
                }
            }
        }

        /// <summary>
        /// Updates positions if movable so the heap has the correct key, then removes the object
        /// </summary>
        /// <param name="heapableObject"></param>
        private void RemoveFromHeap(IHeapableObject heapableObject) {
            if (heapableObject.CanMove) {
                UpdatePositions();
            }
            if (heap.TryGetValue(GetKey(heapableObject.Position), out List<IHeapableObject> bucket)) {
                if (bucket != null){ //Can be null when exiting scene
                    bucket.Remove(heapableObject);
                }
            }
        }

        /// <summary>
        /// Provides a key based on the position of the object and the cellsize
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        private Vector3Int GetKey(Vector3 position) {
            Vector3Int key = new Vector3Int();
            key.x = ((int)position.x) / cellsize;
            key.y = ((int)position.y) / cellsize;
            key.z = ((int)position.z) / cellsize;
            return key;
        }

        /// <summary>
        /// Returns a list of all objects within the given range of the supplied position
        /// </summary>
        /// <param name="position">The position to start at</param>
        /// <param name="range">The range to search</param>
        /// <returns></returns>
        public List<IHeapableObject> GetObjectsInRange(Vector3 position, int range) {
            List<IHeapableObject> objectsInRange = new List<IHeapableObject>();

            int sizeToSearch = 0;

            while(range > 0) {
                sizeToSearch++;
                range -= cellsize;
            }

            Vector3Int startKey = GetKey(position);

            for(int x = startKey.x - sizeToSearch; x <= startKey.x + sizeToSearch; x++) {
                for(int y = startKey.y - sizeToSearch; y <= startKey.y + sizeToSearch; y++) {
                    for(int z = startKey.z - sizeToSearch; z <= startKey.z + sizeToSearch; z++) {

                        Vector3Int currentKey = new Vector3Int(x, y, z);
                        if(heap.TryGetValue(currentKey, out List<IHeapableObject> bucket)) {
                            bucket.RemoveAll(obj => obj == null);
                            objectsInRange.AddRange(bucket);
                        }
                    }
                }
            }
            return objectsInRange;
        }

    }
}
