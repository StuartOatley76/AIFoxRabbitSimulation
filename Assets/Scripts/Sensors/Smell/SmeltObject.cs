using UnityEngine;

namespace Sensors {

    namespace Smell {

        /// <summary>
        /// enum for types of objects that can be smelt
        /// </summary>
        public enum ObjectType {
            Plant,
            Rabbit,
            Fox
        }

        /// <summary>
        /// Class to represent an object that has been smelt
        /// </summary>
        public class SmeltObject {

            /// <summary>
            /// Direction of the object
            /// </summary>
            Vector3 direction;

            /// <summary>
            /// type of the object
            /// </summary>
            ObjectType type;

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="dir"></param>
            /// <param name="objectType"></param>
            public SmeltObject(Vector3 dir, ObjectType objectType) {
                direction = dir;
                type = objectType;
            }

        }
    }
}