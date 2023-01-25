using UnityEngine;
using SpacialHeap;
using Memorable;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Class to handle a spacial heap that includes all Iheapable objects
/// </summary>
public class WorldHeap : MonoBehaviour
{

    /// <summary>
    /// Singleton instance
    /// </summary>
    public static WorldHeap instance;

    /// <summary>
    /// The heap
    /// </summary>
    private SHeap heap = new SHeap();

    /// <summary>
    /// Whether the heap has been updated this frame
    /// </summary>
    private bool heapUpdatedThisFrame = false;

    /// <summary>
    /// Singleton implementation
    /// </summary>
    private void Awake() {
        if(instance != null) {
            Destroy(this);
            return;
        }
        instance = this;
    }

    /// <summary>
    /// Resets heapUpdatedThisFrame
    /// </summary>
    private void LateUpdate() {
        heapUpdatedThisFrame = false;
    }

    /// <summary>
    /// Adds an object to the heap
    /// </summary>
    /// <param name="objToAdd"></param>
    /// <returns></returns>
    public bool addObject(MemorableObject objToAdd) {
        return heap.AddNewObject(objToAdd);
    }

    /// <summary>
    /// Updates the heap if necessary then returns objects within range
    /// </summary>
    /// <param name="position"></param>
    /// <param name="range"></param>
    /// <returns></returns>
    public List<MemorableObject> GetObjectsInRange(Vector3 position, int range) {
        if (!heapUpdatedThisFrame) {
            heap.UpdatePositions();
            heapUpdatedThisFrame = true;
        }
        return heap.GetObjectsInRange(position, range).Where(x => x is MemorableObject).Select(x => x as MemorableObject).ToList();
    }
}
