
namespace BehaviourTree {

    /// <summary>
    /// Class to check for IsSafe state
    /// </summary>
    public class IsSafe : Conditional {

        protected override bool ConditionCheck() {
            return character.State.HasFlag(GOAP.State.IsSafe);
        }
    }
}