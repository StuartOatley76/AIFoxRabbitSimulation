using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace GOAP {

    [RequireComponent(typeof(ActionPlanner))]
    public class ActionManager : MonoBehaviour {
        private List<Goal> goals;
        private GoapBlackboard blackboard;
        private Goal currentGoal = null;
        private GoapAction currentAction = null;
        private List<GoapAction> plan;
        IGoapCharacter character;

        private readonly float priorityDifferenceToChangeGoal = 2;

        private void Awake() {
            GoapBlackboard[] blackboards = FindObjectsOfType<GoapBlackboard>();
            for(int i = 0; i < blackboards.Length; i++) {
                if (CompareTag(blackboards[i].tag)) {
                    blackboard = blackboards[i];
                    break;
                }
            }
            character = GetComponent<IGoapCharacter>();
            goals = blackboard.Goals;
        }

        private void Update() {
            if(currentAction != null) {
                currentAction.Update();
            }
            List<Goal> updatedGoals = goals.OrderByDescending(p => p.CalculatePriority(character)).ToList();
            if(currentGoal != null && updatedGoals[0] != currentGoal && updatedGoals[0].CalculatePriority(character) > currentGoal.CalculatePriority(character) + priorityDifferenceToChangeGoal) {
                ReturnPlanToPool();
                currentGoal = null;
            }
            if (currentGoal == null || plan == null || (plan.Count == 0 && !currentAction.Running)) {
                goals = updatedGoals;
                if(!blackboard.GetPlan(updatedGoals, character, out plan, out currentGoal)) {
                    return;
                }
            }
            if(plan != null && (currentAction == null || !currentAction.Running)) {
                if (plan.Count <= 0) {
                    return;
                }
                if(currentAction != null) {
                    if (!currentAction.ActionSucceeded) {
                        Debug.LogWarning(character.GameObject.name + " failed " + currentAction.GetType().Name);
                        currentAction.ReturnToPool();
                        currentAction = null;
                        ReturnPlanToPool();
                        plan = null;
                        return;
                    }
                    currentAction.ReturnToPool();
                }
                currentAction = plan[plan.Count - 1];
                plan.RemoveAt(plan.Count - 1);
                currentAction.PerformAction();
            }
        }

        private void ReturnPlanToPool() {
            if(currentAction != null) {
                currentAction.ReturnToPool();
                currentAction = null;
            }
            while(plan.Count > 0) {
                currentAction = plan[plan.Count - 1];
                plan.RemoveAt(plan.Count - 1);
                currentAction.ReturnToPool();
            }
            currentAction = null;
        }
    }
}