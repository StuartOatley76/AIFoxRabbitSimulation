
namespace BehaviourTree {

    /// <summary>
    /// Class to check for IsInDanger state
    /// </summary>
    public class IsInDanger : Conditional {
        protected override bool ConditionCheck() {
            return character.State.HasFlag(GOAP.State.IsInDanger);
        }
    }
}
