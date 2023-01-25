using System;
using UnityEngine;

namespace BehaviourTree { 

    /// <summary>
    /// Class to handle getting and running a behaviour tree
    /// </summary>
    public class BTreeController : MonoBehaviour
    {
        /// <summary>
        /// The tree
        /// </summary>
        private BehaviourTreeObject tree;

        /// <summary>
        /// coroutine running the tree
        /// </summary>
        private Coroutine coroutine;

        /// <summary>
        /// Gets the tree from the blackboard and sets the character. Then starts the tree
        /// </summary>
        private void Start() {
            tree = FindObjectOfType<RabbitBlackboard>().GetBehaviourTree();
            IBTreeCharacter treeCharacter = GetComponent<IBTreeCharacter>();
            if(tree != null && treeCharacter != null) {
                StartTree(treeCharacter);
            }
        }

        /// <summary>
        /// Starts the tree running
        /// </summary>
        /// <param name="unit"></param>
        public void StartTree(IBTreeCharacter unit) {
            tree.InitialiseTree(unit, Interrupt);
            coroutine = StartCoroutine(tree.RunTree());
        }

        /// <summary>
        /// Ensures the tree keeps running
        /// </summary>
        private void Update() {
            if (tree.Running && coroutine == null) {
                coroutine = StartCoroutine(tree.RunTree());
            }
        }

        /// <summary>
        /// Stops the coroutine before destroying the tree
        /// </summary>
        private void OnDestroy() {
            if (coroutine != null) {
                StopCoroutine(coroutine);
            }
        }

        /// <summary>
        /// Interrupt delegate for the tree
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void Interrupt(object o, EventArgs e) {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }
    
}
