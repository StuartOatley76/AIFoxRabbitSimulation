using Memorable;


namespace BehaviourTree {

    /// <summary>
    /// Class to handle finding food
    /// </summary>
    public class FindFood : FindType {

        /// <summary>
        /// Sets the search type
        /// </summary>
        public FindFood() {
            searchType = MemorableObjectType.Food;
        }
        

    }
}