using System.Collections.Generic;

namespace GOAP {

    /// <summary>
    /// Class to provide a GOAP blackboard for foxes
    /// </summary>
    public class FoxBlackboard : GoapBlackboard {


        /// <summary>
        /// Creates the goals and actions for the fox, and sets the filename
        /// </summary>
        protected override void Awake() {
            
            goapActions = new List<GoapAction>() {
                new Chase(),
                new Drink(),
                new Eat(),
                new GoapWander(),
                new Mate(),
                new Rest(),
                new SearchForFood(),
                new SearchForMate(),
                new SearchForWater(),
                new WalkToFood(),
                new WalkToMate(),
                new WalkToWater()
            };

            Goals = new List<Goal>() {
                new Eaten(),
                new HasDrunk(),
                new HasMated(),
                new Rested(),
                new Explore()
            };

            fileName = "FoxGoap.Json";
            base.Awake();
        }

    }
}