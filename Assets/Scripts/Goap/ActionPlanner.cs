using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
namespace GOAP {

    /// <summary>
    /// Class to create a list of actions to perform as a plan
    /// </summary>
    public class ActionPlanner : MonoBehaviour {

        /// <summary>
        /// Class to represent a node in the graph created
        /// </summary>
        public class Node {

            /// <summary>
            /// The Node's parent if it hsa one
            /// </summary>
            public Node Parent { get; private set; }

            /// <summary>
            /// The cost to get to this node in the graph
            /// </summary>
            public float Cost { get; private set; }

            /// <summary>
            /// The states created in the graph to this point
            /// </summary>
            public State State { get; private set;}

            /// <summary>
            /// The action associated with this node
            /// </summary>
            public GoapAction Action { get; private set; }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="parent"></param>
            /// <param name="cost"></param>
            /// <param name="states"></param>
            /// <param name="action"></param>

            public Node() {

            }
            public void SetNode(Node parent, float cost, GoapAction action, State state) {
                Parent = parent;
                Cost = cost;
                State = state;
                Action = action;
            }
            public void Reset() {
                Parent = null;
                Cost = 0;
                State = (State)(-1);
                Action = null;
            }
        }

        /// <summary>
        /// list of the leaf nodes
        /// </summary>
        private List<Node> leaves = new List<Node>();

        /// <summary>
        /// Pool for nodes
        /// </summary>
        private readonly NodePool nodePool = new NodePool();

        /// <summary>
        /// pool for lists of actions
        /// </summary>
        private readonly ListPool<GoapAction> actionListPool = new ListPool<GoapAction>();

        /// <summary>
        /// string for the path of the save file
        /// </summary>
        private static string path;

        /// <summary>
        /// The gameobject's character
        /// </summary>
        private IGoapCharacter character;

        /// <summary>
        /// Dictionary of all previously calculated plans
        /// </summary>
        private static Dictionary<PathKey, List<GoapAction>> foundPaths;

        public static string Filename { protected get; set; }

        /// <summary>
        /// Sets the path
        /// </summary>
        private void Awake() {
            path = Application.dataPath + "/";
        }

        /// <summary>
        /// Returns the action plan
        /// </summary>
        /// <param name="goalsInOrder"></param>
        /// <param name="goal"></param>
        /// <returns></returns>
        public List<GoapAction> GetActionPlan(List<Goal> goalsInOrder, List<GoapAction> goapActions,
            IGoapCharacter goapCharacter, out Goal goal) {

            character = goapCharacter;
            State currentState = character.State;
            
            goal = null;
            List<GoapAction> plan = null;
            if(foundPaths == null) {
                foundPaths = new Dictionary<PathKey, List<GoapAction>>();
            }
            for(int i = 0; i < goalsInOrder.Count; i++) {
                PathKey key = new PathKey(goalsInOrder[i], currentState);
                if (foundPaths.ContainsKey(key)) {
                    goal = goalsInOrder[i];
                    return DeepCopyPlan(foundPaths[key]);
                }
                plan = CreatePlan(goalsInOrder[i], currentState, goapActions);
                if(plan != null) {
                    goal = goalsInOrder[i];
                    foundPaths.Add(key, plan);
                    SavePaths(foundPaths);
                    plan = DeepCopyPlan(plan);
                    break;
                }
            }
            return plan;
        }

        /// <summary>
        /// Creates a deep copy of the plan
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        private List<GoapAction> DeepCopyPlan(List<GoapAction> plan) {
            List<GoapAction> toReturn = new List<GoapAction>();
            for (int i = 0; i < plan.Count; i++) {
                GoapAction action = plan[i].GetCopy();
                action.Initialise(character);
                toReturn.Add(action);
            }
            return toReturn;
        }

        /// <summary>
        /// Attempts to create an action plan for the provided goal from the actions. Returns null if not possible
        /// </summary>
        /// <param name="goal"></param>
        /// <param name="currentState"></param>
        /// <param name="usableActions"></param>
        /// <returns></returns>
        private List<GoapAction> CreatePlan(Goal goal, State currentState, List<GoapAction> usableActions) {
            leaves.Clear();
            Node start = nodePool.GetNode();
            start.SetNode(null, 0, null, 0);

            bool success = BuildGraph(start, leaves, usableActions, goal, currentState);
            if (!success) {
                ReturnNodes(leaves);
                return null;
            }
            Node cheapest = null;
            for(int i = 0; i < leaves.Count; i++) {
                if(cheapest == null || leaves[i].Cost < cheapest.Cost) {
                    cheapest = leaves[i];
                }
            }

            List<GoapAction> plan = new List<GoapAction>();
            Node node = cheapest;
            while(node != null) {
                if(node.Action != null) {
                    plan.Add(node.Action);
                }
                node = node.Parent;
            }
            ReturnNodes(leaves);
            return plan;
        }

        /// <summary>
        /// Returns nodes to the pool
        /// </summary>
        /// <param name="nodes"></param>
        private void ReturnNodes(List<Node> nodes) {
            for(int i = 0; i < nodes.Count; i++) {
                nodePool.ReturnNode(nodes[i]);
            }
        }

        /// <summary>
        /// Recursive A* pathfinding method to find the action plan
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="leaves"></param>
        /// <param name="usableActions"></param>
        /// <param name="goal"></param>
        /// <param name="currentState"></param>
        /// <returns></returns>
        private bool BuildGraph(Node parent, List<Node> leaves, List<GoapAction> usableActions, Goal goal, State currentState) {

            bool foundPlan = false;
            for(int i = 0; i < usableActions.Count; i++) {

                if (IsAcheivable(usableActions[i], parent, currentState)) {
                    State states = parent.State;
                    for (int j = 0; j < usableActions[i].Outcomes.Count; j++) {
                        if (!states.HasFlag(usableActions[i].Outcomes[j])) {
                            states |= usableActions[i].Outcomes[j];
                        }
                    }

                    Node node = nodePool.GetNode();

                    node.SetNode(parent, parent.Cost + 1, usableActions[i], states);
  
                    if(states.HasFlag(goal.DesiredState)) {
                        leaves.Add(node);
                        foundPlan = true;
                    } else {
                        List<GoapAction> actionRemoved = RemoveAction(usableActions, usableActions[i]);
                        bool found = BuildGraph(node, leaves, actionRemoved, goal, currentState);
                        if (found) {
                            foundPlan = true;
                        }
                        actionListPool.ReturnList(actionRemoved);
                    }
                }
            }

            return foundPlan;
        }

        /// <summary>
        /// returns a new list with the supplied action removed
        /// </summary>
        /// <param name="usableActions"></param>
        /// <param name="goapAction"></param>
        /// <returns></returns>
        private List<GoapAction> RemoveAction(List<GoapAction> usableActions, GoapAction goapAction) {
            List<GoapAction> actionRemoved = actionListPool.GetList();
            for(int i = 0; i < usableActions.Count; i++) {
                if(usableActions[i] != goapAction) {
                    actionRemoved.Add(usableActions[i]);
                }
            }
            return actionRemoved;
        }

        /// <summary>
        /// Returns whether an action can be performed based on the character's state and the states created by previous actions in the plan
        /// </summary>
        /// <param name="goapAction"></param>
        /// <param name="node"></param>
        /// <param name="currentState"></param>
        /// <returns></returns>
        private bool IsAcheivable(GoapAction goapAction, Node node, State currentState) {
            for(int i = 0; i < goapAction.Requirements.Count; i++) {
                if(!currentState.HasFlag(goapAction.Requirements[i]) && !node.State.HasFlag(goapAction.Requirements[i])) {
                    return false;
                }
            }

            return true;
        }


        /// <summary>
        /// loads the binary json path and creates the foundpaths dictionary from it
        /// </summary>
        public void LoadPaths() {
            if(!File.Exists(path + Filename)) {
                Debug.LogWarning("No file");
                return;
            }
            
            try {
                byte[] bytes = File.ReadAllBytes(path + Filename);
                string json = Encoding.Unicode.GetString(bytes);
                JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All }; //Enables derived classes
                List<KeyValuePair<PathKey, List<GoapAction>>> dictionaryAsList = 
                    JsonConvert.DeserializeObject<List<KeyValuePair<PathKey, List<GoapAction>>>>(json, settings);

                foundPaths = ConvertListToDictionary(dictionaryAsList);
            } catch (Exception e) {
                Debug.LogError("load failed - " + e.Message);
            }
        }


        /// <summary>
        /// formats ths supplied dictionary into json, then saves it as a binary file.
        /// Note - dictionary is converted to a list of key value pairs as json is BAD at serialising dictionaries (insists on converting keys to strings) but
        /// fine with lists of key value pairs
        /// </summary>
        /// <param name="paths"></param>
        private void SavePaths(Dictionary<PathKey, List<GoapAction>> paths) {
            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All }; //Enables derived classes
            settings.Formatting = Formatting.Indented;
            List<KeyValuePair<PathKey, List<GoapAction>>> dictionaryAsList = ConvertDictionaryToList(paths);
            string json = JsonConvert.SerializeObject(dictionaryAsList, settings);

            WriteFileAsync(json); // this line causes a warning CS4014 because it isn't using an await. However, the await
                                  // is not needed as nothing needs to wait for the file to finish writing
        }

        /// <summary>
        /// Writes the file Asyncronously
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static async Task WriteFileAsync(string text) {
            byte[] encodedText = Encoding.Unicode.GetBytes(text);
            string destination = path + Filename;
            using (FileStream fs = new FileStream(destination, FileMode.Create, FileAccess.Write, FileShare.None)) {
                await fs.WriteAsync(encodedText, 0, encodedText.Length);
            }
        }

        /// <summary>
        /// DO NOT USE UNLESS IN PRE RELEASE! THIS WILL TAKE A LONG TIME
        /// Works through each goal and calculates paths for every possible state for that goal
        /// adds them all to dictionary and then saves the dictionary
        /// </summary>
        /// <param name="goals"></param>
        /// <param name="actions"></param>
        /// <param name="fileName"></param>
        public void CreateFullDictionary(List<Goal> goals, List<GoapAction> actions, string fileName) {
            Filename = fileName;
            Dictionary<PathKey, List<GoapAction>> paths = new Dictionary<PathKey, List<GoapAction>>();
            for (int i = 0; i < goals.Count; i++) {
                for(int j = 1; j < (int)State.EndOfFlags; j++) {
                    List<GoapAction> plan = CreatePlan(goals[i], (State)j, actions);
                    if(plan != null) {
                        PathKey key = new PathKey(goals[i], (State)j);
                        paths.Add(key, plan);
                    }
                }
            }
            SavePaths(paths);
        }

        /// <summary>
        /// Converts dictionary to list of key value pairs
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        private static List<KeyValuePair<PathKey, List<GoapAction>>> ConvertDictionaryToList(Dictionary<PathKey, List<GoapAction>> dictionary) {
            return dictionary.ToList();
        }

        /// <summary>
        /// converts list of key value pairs to dictionary
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private static Dictionary<PathKey, List<GoapAction>> ConvertListToDictionary(List<KeyValuePair<PathKey, List<GoapAction>>> list) {
            return list.ToDictionary(x => x.Key, x => x.Value);
        }
    }
}