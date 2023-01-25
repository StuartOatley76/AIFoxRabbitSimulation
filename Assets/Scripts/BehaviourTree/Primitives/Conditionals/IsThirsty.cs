
namespace BehaviourTree {

    /// <summary>
    /// Class to check for IsThirsty state
    /// </summary>
    public class IsThirsty : Conditional {
        protected override bool ConditionCheck() {
            return character.State.HasFlag(GOAP.State.IsThirsty);
        }
    }
}