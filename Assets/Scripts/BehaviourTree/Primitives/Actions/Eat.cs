

namespace BehaviourTree {

    /// <summary>
    /// Class to handle eating
    /// </summary>
    public class Eat : Consume {

        /// <summary>
        /// Sets charconsume to the character's eat component
        /// </summary>
        protected override void Initialise() {
            base.Initialise();
            charConsume = character.GameObject.GetComponent<Characters.Eat>();
        }
    }
}