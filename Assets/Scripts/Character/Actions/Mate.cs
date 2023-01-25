
using GOAP;
namespace Characters {

    /// <summary>
    /// Class to handle mating
    /// </summary>
    public class Mate : CharacterAction {

        /// <summary>
        /// Whether the mating was successful
        /// </summary>
        private bool matingSuccessful = false;


        protected override bool Failed() {
            return matingSuccessful;
        }

        /// <summary>
        /// Makes the character reproduce
        /// </summary>
        public override void RunAction() {
            matingSuccessful = false;
            base.RunAction();

            if (character.Target != null && character.State.HasFlag(State.HasMateTarget)){
                Character mate = character.Target.GetComponent<Character>();
                if (mate != null) {
                    character.Reproduce(mate);
                    matingSuccessful = true;
                }
            }
        }
        protected override bool Success() {
            return matingSuccessful;
        }
    }
}