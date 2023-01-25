
namespace BehaviourTree {

    /// <summary>
    /// Class to check for alert state
    /// </summary>
    public class IsAlert : Conditional {

        protected override bool ConditionCheck() {
            return character.State.HasFlag(GOAP.State.IsAlert);
        }
    }
}