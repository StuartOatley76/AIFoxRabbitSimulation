
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Class to handle an object spawner
/// </summary>
public class Spawner : MonoBehaviour {

    /// <summary>
    /// prefab of object to be spawned
    /// </summary>
    [SerializeField]
    private GameObject prefab;

    /// <summary>
    /// The floor object
    /// </summary>
    [SerializeField]
    private GameObject gameArea;

    /// <summary>
    /// The number to be spawned
    /// </summary>
    [SerializeField]
    private int numberToSpawn = 0;

    /// <summary>
    /// The number spawned
    /// </summary>
    private int numberSpawned = 0;

    /// <summary>
    /// The y position for objects to be spawned at
    /// </summary>
    [SerializeField]
    private float yOffset = 0;

    /// <summary>
    /// The range from the position that NavMesh.SamplePosition uses
    /// </summary>
    [SerializeField]
    private float range = 1;

    /// <summary>
    /// The name for the gameobject spawned
    /// </summary>
    [SerializeField]
    private string typeName;

    /// <summary>
    /// Spawns the required amount
    /// </summary>
    void Start() {
        while(numberSpawned < numberToSpawn) {
            Spawn();
        }
    }

    /// <summary>
    /// Instantiates the object
    /// </summary>
    private void Spawn() {
        Vector3 position = GetPosition();
        Quaternion rotation = Quaternion.Euler(0, Random.Range(0, 360f), 0);
        GameObject go = Instantiate(prefab, position, rotation);
        Counter.Instance.Add(go.tag);
        numberSpawned++;
        go.name = typeName + " " + numberSpawned;
        
    }

    /// <summary>
    /// Gets a random position within the bounds of the floor object
    /// </summary>
    /// <returns></returns>
    private Vector3 GetPosition() {
        Bounds bounds = gameArea.GetComponent<Renderer>().bounds;
        if(bounds == null) {
            return Vector3.zero;
        }
        Vector3 pos = new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
            );

        return GetPosOnNavmesh(pos);
    }

    /// <summary>
    /// Gets a point nearby on the navmesh. If fails, tries again with a new position
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    private Vector3 GetPosOnNavmesh(Vector3 pos) {
        if(!NavMesh.SamplePosition(pos, out NavMeshHit hit, range, NavMesh.AllAreas)){
            return GetPosition();
        }
        return hit.position + new Vector3(0, yOffset, 0);
    }
}
