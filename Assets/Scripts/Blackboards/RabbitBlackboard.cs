using UnityEngine;
using BehaviourTree;

/// <summary>
/// Class to represent a blackboard for behaviour tree characters. Provides a copy of the behaviour tree
/// </summary>
public class RabbitBlackboard : MonoBehaviour
{

    /// <summary>
    /// The behaviour tree prefab
    /// </summary>
    [SerializeField]
    private BehaviourTreeObject prefab;

    /// <summary>
    /// An instance of the tree
    /// </summary>
    private BehaviourTreeObject tree;
    private void Awake() {
        tree = Instantiate(prefab);
    }

    /// <summary>
    /// Returns a copy of the tree
    /// </summary>
    /// <returns></returns>
    public BehaviourTreeObject GetBehaviourTree() {
        return tree.Clone();
    }
}
