using System.Linq;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using UnityEngine;
using BehaviourTree;

//Differentiate behaviour tree node and graphview node
using BNode = BehaviourTree.Node;

/// <summary>
/// Class to handle the Tree's graph view
/// </summary>
public class TreeView : GraphView {

    /// <summary>
    /// Factory to create Treeviews
    /// </summary>
    public new class UxmlFactory : UxmlFactory<TreeView, UxmlTraits> { }

    /// <summary>
    /// The behaviour tree
    /// </summary>
    private BehaviourTreeObject tree;

    /// <summary>
    /// The editor window the graphview is in
    /// </summary>
    public EditorWindow window { private get; set; }

    /// <summary>
    /// Constructor.
    /// Sets up the background, adds manipulators and applies the stylesheet
    /// </summary>
    public TreeView() {
        Insert(0, new GridBackground());

        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/BehaviourTree/Editor/BehaviourTreeEditor.uss");
        styleSheets.Add(styleSheet);
    }

    /// <summary>
    /// Creates the visual representation of the tree.
    /// Removes any elements from previous tree, adds a rootnode if none exists
    /// </summary>
    /// <param name="treeToShow"></param>
    public void CreateView(BehaviourTreeObject treeToShow) {
        tree = treeToShow;

        graphViewChanged -= OnGraphViewChanged;
        DeleteElements(graphElements);
        graphViewChanged += OnGraphViewChanged;

        if(tree.rootNode == null) {
            tree.rootNode = tree.CreateNode(typeof(RootNode)) as RootNode;
            EditorUtility.SetDirty(tree);
            AssetDatabase.SaveAssets();
        }

        CreateNodeViews();
        CreateEdges();
    }

    /// <summary>
    /// Creates the connections between the node ports
    /// </summary>
    private void CreateEdges() {
        foreach (BNode node in tree.nodes) {
            List<BNode> children = tree.GetChildren(node);
            if (children.Count == 0) {
                continue;
            }
            NodeView parentView = GetNodeByGuid(node.guid) as NodeView;
            foreach (BNode childNode in children) {
                NodeView childView = GetNodeByGuid(childNode.guid) as NodeView;
                Edge edge = parentView.OuputPort.ConnectTo(childView.InputPort);
                AddElement(edge);
            }
        }
    }

    /// <summary>
    /// Creates a node view for each node
    /// </summary>
    private void CreateNodeViews() {
        foreach (BNode node in tree.nodes) {
            CreateNodeView(node);
        }
    }

    /// <summary>
    /// Ensures ports cannot be attached to the same type of port (input or output) and
    /// nodes cannot be connected to themselves
    /// </summary>
    /// <param name="startPort"></param>
    /// <param name="nodeAdapter"></param>
    /// <returns></returns>
    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter) {
        return ports.ToList().Where(endport => 
        endport.direction != startPort.direction &&
        endport.node != startPort.node
        ).ToList();
    }

    /// <summary>
    /// Applies changes made in the window to the tree
    /// </summary>
    /// <param name="graphViewChange"></param>
    /// <returns></returns>
    private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange) {
        if(graphViewChange.elementsToRemove != null) {
            foreach(GraphElement elem in graphViewChange.elementsToRemove) {
                if (elem is NodeView nodeView) {
                    tree.DeleteNode(nodeView.Node);
                }

                if(elem is Edge edge) {
                    NodeView parentView = edge.output.node as NodeView;
                    NodeView childView = edge.input.node as NodeView;
                    tree.RemoveChild(parentView.Node, childView.Node);
                }
            }
        }

        if(graphViewChange.edgesToCreate != null) {
            foreach(Edge edge in graphViewChange.edgesToCreate) {
                NodeView parentView = edge.output.node as NodeView;
                NodeView childView = edge.input.node as NodeView;
                tree.AddChild(parentView.Node, childView.Node);
            }
        }
        return graphViewChange;
    }

    /// <summary>
    /// populates the right click menu with available options by checking what types are derived from the relevant types
    /// ignores abstract types. Passes the currect mouse position in the window so nodes are created at the correct point
    /// </summary>
    /// <param name="evt"></param>
    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt) {

        Vector2 nodePosition = this.ChangeCoordinatesTo(contentViewContainer, evt.localMousePosition);

        TypeCache.TypeCollection actions = TypeCache.GetTypesDerivedFrom<Action>();
        foreach(System.Type type in actions) {
            if (!type.IsAbstract) {
                evt.menu.AppendAction("[Action] " + type.Name, (a) => CreateNode(type, nodePosition));
            }
        }

        TypeCache.TypeCollection conditionals = TypeCache.GetTypesDerivedFrom<Conditional>();
        foreach (System.Type type in conditionals) {
            if (!type.IsAbstract) {
                evt.menu.AppendAction("[Conditional] " + type.Name, (a) => CreateNode(type, nodePosition));
            }
        }
        TypeCache.TypeCollection composites = TypeCache.GetTypesDerivedFrom<Composite>();
        foreach (System.Type type in composites) {
            if (!type.IsAbstract) {
                evt.menu.AppendAction("[Composite] " + type.Name, (a) => CreateNode(type, nodePosition));
            }
        }

        TypeCache.TypeCollection decorators = TypeCache.GetTypesDerivedFrom<Decorator>();
        foreach (System.Type type in decorators) {
            if (!type.IsAbstract) {
                evt.menu.AppendAction("[Decorator] " + type.Name, (a) => CreateNode(type, nodePosition));
            }
        }
    }

    /// <summary>
    /// Creates a new behaviour tree node of the type supplied
    /// </summary>
    /// <param name="type"></param>
    /// <param name="nodePosition"></param>
    private void CreateNode(System.Type type, Vector2 nodePosition) {
        BNode node = tree.CreateNode(type);
        node.position = nodePosition;
        CreateNodeView(node);

    }

    /// <summary>
    /// Creates a new nodeview from the supplied behaviour tree node
    /// </summary>
    /// <param name="node"></param>
    private void CreateNodeView(BNode node) {
        NodeView nodeView = new NodeView(node);
        AddElement(nodeView);
    }
}
