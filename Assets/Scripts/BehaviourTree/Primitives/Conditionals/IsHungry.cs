
namespace BehaviourTree {

    /// <summary>
    /// Class to check for IsHungry state
    /// </summary>
    public class IsHungry : Conditional {
        protected override bool ConditionCheck() {
            return character.State.HasFlag(GOAP.State.IsHungry);
        }
    }
}
