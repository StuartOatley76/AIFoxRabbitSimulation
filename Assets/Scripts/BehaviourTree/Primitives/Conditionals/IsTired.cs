
namespace BehaviourTree {

    /// <summary>
    /// Class to check for isTired state
    /// </summary>
    public class IsTired : Conditional {
        protected override bool ConditionCheck() {
            return character.State.HasFlag(GOAP.State.IsTired);
        }
    }
}