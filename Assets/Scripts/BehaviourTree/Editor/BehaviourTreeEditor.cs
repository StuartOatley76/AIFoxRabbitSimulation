using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Callbacks;
using BehaviourTree;

/// <summary>
/// The editor window used to build trees
/// </summary>
public class BehaviourTreeEditor : EditorWindow
{
    /// <summary>
    /// The graph view used for the tree
    /// </summary>
    private TreeView treeView;

    /// <summary>
    /// Menu item to open the editor
    /// </summary>
    [MenuItem("BehaviourTreeEditor/Editor ...")]
    public static void OpenWindow() {
        BehaviourTreeEditor wnd = GetWindow<BehaviourTreeEditor>();
        wnd.titleContent = new GUIContent("BehaviourTreeEditor");
    }

    /// <summary>
    /// Opens the window when a Behaviour tree is double clicked
    /// </summary>
    /// <param name="instanceID"></param>
    /// <param name="line"></param>
    /// <returns></returns>
    [OnOpenAsset]
    public static bool OnOpenAsset(int instanceID, int line) {
        if (Selection.activeObject is BehaviourTreeObject) {
            OpenWindow();
            return true;
        }
        return false;
    }

    /// <summary>
    /// Creates the UI for the window
    /// </summary>
    public void CreateGUI() {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        //Import Uxml
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/BehaviourTree/Editor/BehaviourTreeEditor.uxml");
        visualTree.CloneTree(root);

        //Add stylesheet
        StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/BehaviourTree/Editor/BehaviourTreeEditor.uss");
        root.styleSheets.Add(styleSheet);

        //Sets up the tree view
        treeView = root.Q<TreeView>();
        treeView.window = this;
        OnSelectionChange();
    }

    /// <summary>
    /// Updates the window when a tree is selected
    /// </summary>
    private void OnSelectionChange() {
        BehaviourTreeObject tree = Selection.activeObject as BehaviourTreeObject;
        if (tree && AssetDatabase.CanOpenAssetInEditor(tree.GetInstanceID())) {
            treeView.CreateView(tree);
        }
    }
}
