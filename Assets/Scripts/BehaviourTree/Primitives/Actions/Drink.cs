
namespace BehaviourTree {

    /// <summary>
    /// Class to handle drinking
    /// </summary>
    public class Drink : Consume {

        /// <summary>
        /// Sets charconsume to the character's drink component
        /// </summary>
        protected override void Initialise() {
            base.Initialise();
            charConsume = character.GameObject.GetComponent<Characters.Drink>();
        }
    }
}
