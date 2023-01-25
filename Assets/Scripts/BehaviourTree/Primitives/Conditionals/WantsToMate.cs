
namespace BehaviourTree {

    /// <summary>
    /// Class to check for WantsToMate state
    /// </summary>
    public class WantsToMate : Conditional {
        protected override bool ConditionCheck() {
            return character.State.HasFlag(GOAP.State.WantsToMate);
        }
    }
}
