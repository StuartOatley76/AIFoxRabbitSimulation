using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

//Allows distinction between Graphview node and behaviour tree node
using BNode = BehaviourTree.Node;

/// <summary>
/// Class to handle the Graphview functionality of a node
/// </summary>
public class NodeView : Node {

    /// <summary>
    /// The behaviour tree node attached to this graphview node
    /// </summary>
    private BNode node;

    /// <summary>
    /// The path to the uxml file for a node
    /// </summary>
    private static readonly string nodeViewUXMLpath = "Assets/Scripts/BehaviourTree/Editor/NodeView.uxml";

    /// <summary>
    /// Accessor for the behaviour tree node
    /// </summary>
    public BNode Node {
        get { return node; }
        set { node = value; }

    }

    /// <summary>
    /// The input port on this node
    /// </summary>
    private Port inputPort;

    /// <summary>
    /// Accessor for the input port
    /// </summary>
    public Port InputPort {
        get { return inputPort; }
        set { inputPort = value; }
    }

    /// <summary>
    /// The output port for this node
    /// </summary>
    private Port outputPort;

    /// <summary>
    /// Accessor for the output port
    /// </summary>
    public Port OuputPort {
        get { return outputPort; }
        set { outputPort = value; }
    }

    /// <summary>
    /// Constructor
    /// Initialises data then creates the ports
    /// </summary>
    /// <param name="bNode"></param>
    public NodeView(BNode bNode) : base(nodeViewUXMLpath) {
        node = bNode;
        title = node.name;
        viewDataKey = node.guid;
        style.left = node.position.x;
        style.top = node.position.y;

        CreateInputPorts();
        CreateOutoutPorts();
    }

    /// <summary>
    /// Sets up the input port on all nodes except the root node
    /// </summary>
    private void CreateInputPorts() {
        if(node is BehaviourTree.RootNode) {
            return;
        }
        inputPort = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
        inputPort.portName = "";
        inputPort.style.flexDirection = FlexDirection.Column;
        inputContainer.Add(inputPort);

    }

    /// <summary>
    /// Creates output ports on all nodes except action nodes
    /// Composite outbut ports allow multiple connections, others only allow one
    /// </summary>
    private void CreateOutoutPorts() {
        if(node is BehaviourTree.Action) {
            return;
        }
        if(node is BehaviourTree.Composite) {
            outputPort = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, typeof(bool));
        } else {
            outputPort = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));
        }

        if (outputPort != null) {
            outputPort.portName = "";
            outputPort.style.flexDirection = FlexDirection.ColumnReverse;
            outputContainer.Add(outputPort);
        }
    }

    /// <summary>
    /// Sets the position of this node. Stores it in the behaviour tree node to
    /// allow nodes to reappear where they were when opened again
    /// </summary>
    /// <param name="newPos"></param>
    public override void SetPosition(Rect newPos) {
        base.SetPosition(newPos);
        Vector2 pos = new Vector2(newPos.xMin, newPos.yMin);
        node.position = pos;
    }
}
