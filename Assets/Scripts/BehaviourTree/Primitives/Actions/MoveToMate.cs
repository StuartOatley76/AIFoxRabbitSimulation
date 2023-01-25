using Memorable;

namespace BehaviourTree {

    /// <summary>
    /// Class to handle moving to a mate
    /// </summary>
    public class MoveToMate : MoveToTarget {

        /// <summary>
        /// Constructor. Sets targettype to mate
        /// </summary>
        public MoveToMate() {
            targetType = MemorableObjectType.Mate;
        }
    }
}
