using Memorable;

namespace BehaviourTree {


    /// <summary>
    /// Class to handle moving to food
    /// </summary>
    public class MoveToFood : MoveToTarget {

        /// <summary>
        /// Constructor. Sets targettype to food
        /// </summary>
        public MoveToFood() {
            targetType = MemorableObjectType.Food;
        }
    }
}
