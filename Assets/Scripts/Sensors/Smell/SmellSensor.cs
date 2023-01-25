using System;
using UnityEngine;

namespace Sensors {

    namespace Smell {

        /// <summary>
        /// Class to handle smelling an object
        /// </summary>
        [RequireComponent(typeof(SphereCollider))]
        public class SmellSensor : MonoBehaviour {

            /// <summary>
            /// Event triggered when smelling an object
            /// </summary>
            private EventHandler<ItemSmeltEventArgs> smeltObjectEvent;

            /// <summary>
            /// Sets the radius of the area items can be smelt in
            /// </summary>
            /// <param name="radius"></param>
            public void SetSmellRadius(float radius) {
                GetComponent<SphereCollider>().radius = radius;
            }

            /// <summary>
            /// Connects the supplied delegate to the event
            /// </summary>
            /// <param name="smellDel"></param>
            public void Connect(EventHandler<ItemSmeltEventArgs> smellDel) {
                smeltObjectEvent += smellDel;
            }

            /// <summary>
            /// Triggered when a particle hits the collider. Checks the tag and creates the appropriate
            /// smeltobject to pass from the event
            /// </summary>
            /// <param name="other">The particle system that emitted the smell particle</param>
            private void OnParticleCollision(GameObject other) {
                Vector3 direction = other.transform.position - transform.position;
                switch (other.tag) {
                    case "Plant":
                        smeltObjectEvent?.Invoke(other, new ItemSmeltEventArgs(new SmeltObject(direction, ObjectType.Plant)));
                        return;
                    case "Rabbit":
                        smeltObjectEvent?.Invoke(other, new ItemSmeltEventArgs(new SmeltObject(direction, ObjectType.Rabbit)));
                        return;
                    case "Fox":
                        smeltObjectEvent?.Invoke(other, new ItemSmeltEventArgs(new SmeltObject(direction, ObjectType.Fox)));
                        return;
                    default:
                        return;
                }
            }
        }
    }
}