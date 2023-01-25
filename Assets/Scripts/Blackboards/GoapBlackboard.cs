using System.Collections.Generic;
using UnityEngine;

namespace GOAP {

    /// <summary>
    /// Holds the action planner for a GOAP character. Supplies action paths
    /// </summary>
    [RequireComponent(typeof(ActionPlanner))]
    public class GoapBlackboard : MonoBehaviour {

        /// <summary>
        /// List of all useable actions
        /// </summary>
        protected List<GoapAction> goapActions;

        /// <summary>
        /// Filename for the json of paths
        /// </summary>
        protected string fileName;

        /// <summary>
        /// List of all goals
        /// </summary>
        public List<Goal> Goals { get; protected set; }

        /// <summary>
        /// The action planner
        /// </summary>
        private ActionPlanner planner;

        /// <summary>
        /// Sets the action planner
        /// </summary>
        protected virtual void Awake() {
            planner = GetComponent<ActionPlanner>();
        }

        /// <summary>
        /// sets the action planner filename and tells it to load the paths
        /// </summary>
        private void Start() {
            ActionPlanner.Filename = fileName;
            planner.LoadPaths();
        }

        /// <summary>
        /// Provides an action plan from the planner
        /// </summary>
        /// <param name="goalsInOrder"></param>
        /// <param name="character"></param>
        /// <param name="plan"></param>
        /// <param name="goal"></param>
        /// <returns></returns>
        public bool GetPlan(List<Goal> goalsInOrder, IGoapCharacter character, out List<GoapAction> plan, out Goal goal) {
            plan = planner.GetActionPlan(goalsInOrder, goapActions, character, out goal);
            if(plan != null && plan.Count > 0) {
                return true;
            }
            return false;
        }
    }
}