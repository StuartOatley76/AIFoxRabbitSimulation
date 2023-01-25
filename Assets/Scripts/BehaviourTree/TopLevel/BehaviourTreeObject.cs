using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BehaviourTree {


    /// <summary>
    /// Class to represent a behaviour tree. Scriptable object so can be created in editor
    /// </summary>
    [CreateAssetMenu()]
    public class BehaviourTreeObject : ScriptableObject {

        /// <summary>
        /// The root node of the tree
        /// </summary>
        public Node rootNode;

        /// <summary>
        /// List of all the nodes on the tree
        /// </summary>
        public List<Node> nodes = new List<Node>();

        /// <summary>
        /// State of the tree
        /// </summary>
        public NodeState NodeState { get { return rootNode.nodeState; } }

        /// <summary>
        /// Whether the tree should be running
        /// </summary>
        public bool Running { get; set; } = false;

        /// <summary>
        /// Sets the character and interrupt delegate for the root node (These propogate down all the attached nodes)
        /// </summary>
        /// <param name="character"></param>
        /// <param name="del"></param>
        public void InitialiseTree(IBTreeCharacter character, EventHandler<EventArgs> del) {
            rootNode.SetCharacter(character);
            rootNode.ConnectInterrupt(del);
        }

        /// <summary>
        /// Coroutine to run the tree
        /// </summary>
        /// <returns></returns>
        public IEnumerator RunTree() {
            Running = true;
            do {
                yield return rootNode.RunNode();
            } while (rootNode.nodeState == NodeState.running && Running == true); 
        }

        /// <summary>
        /// Creates a node of the given type. Used in editor
        /// Assigns name and guid and adds to asset database
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Node CreateNode(Type type) {
            Node node = CreateInstance(type) as Node;
            node.name = type.Name;
            node.guid = GUID.Generate().ToString();
            nodes.Add(node);
            AssetDatabase.AddObjectToAsset(node, this);
            AssetDatabase.SaveAssets();
            return node;
        }

        /// <summary>
        /// Deletes a node from the list of nodes and the asset databse. Used in editor
        /// </summary>
        /// <param name="node"></param>
        public void DeleteNode(Node node) {
            nodes.Remove(node);
            AssetDatabase.RemoveObjectFromAsset(node);
            AssetDatabase.SaveAssets();
        }

        /// <summary>
        /// Adds child node to the supplied parent. Used in editor
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="child"></param>
        public void AddChild(Node parent, Node child) {

            if (parent is RootNode root) {
                root.child = child;
                return;
            }
            if (parent is Conditional conditional) {
                conditional.child = child;
                return;
            }
            if (parent is Decorator repeater) {
                repeater.nodeToRepeat = child;
                return;
            }
            if (parent is Composite composite) {
                if (composite.attachedNodes.Contains(child)) {
                    return;
                }
                composite.attachedNodes.Add(child);
                return;
            }
        }

        /// <summary>
        /// Removes a node from the supplied parent. Used in editor
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="child"></param>
        public void RemoveChild(Node parent, Node child) {
            if (parent is RootNode root) {
                root.child = null;
                return;
            }
            if (parent is Conditional conditional) {
                conditional.child = null;
                return;
            }
            if (parent is Decorator repeater) {
                repeater.nodeToRepeat = null;
                return;
            }
            if (parent is Composite composite) {
                composite.attachedNodes.Remove(child);
                return;
            }
        }

        /// <summary>
        /// Returns a list of all child nodes for the given parent. Used in editor
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public List<Node> GetChildren(Node parent) {
            if (parent is RootNode root && root.child != null) {
                return new List<Node>() { root.child };
            }
            if (parent is Conditional conditional && conditional.child != null) {
                return new List<Node>() { conditional.child };
            }
            if (parent is Decorator repeater && repeater.nodeToRepeat != null) {
                return new List<Node>() { repeater.nodeToRepeat };
            }
            if (parent is Composite composite) {
                return composite.attachedNodes;
            }
            return new List<Node>();
        }

        /// <summary>
        /// Clones the tree
        /// </summary>
        /// <returns></returns>
        public BehaviourTreeObject Clone() {
            BehaviourTreeObject tree = Instantiate(this);
            tree.rootNode = rootNode.Clone();
            return tree;
        }


    }
}