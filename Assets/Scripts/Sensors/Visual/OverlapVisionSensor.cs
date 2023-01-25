using UnityEngine;
using Memorable;

namespace Sensors {

    namespace Vision {

        /// <summary>
        /// Concrete class derived from VisionSensor
        /// </summary>
        public class OverlapVisionSensor : VisionSensor {

            /// <summary>
            /// Updates the objectsInRange list using OverlapSphere
            /// </summary>
            /// <param name="position"></param>
            /// <param name="range"></param>
            protected override void UpdateObjectsInRange(Vector3 position, float range) {
                Collider[] colliders = Physics.OverlapSphere(position, range, LayerMask.NameToLayer("MemorableObject"));
                objectsInRange.Clear();
                for (int i = 0; i < colliders.Length; i++) {
                    if (colliders[i].gameObject.GetComponent<MemorableObject>()) {
                        objectsInRange.Add(colliders[i].gameObject.GetComponent<MemorableObject>());
                    }
                }
            }
        }
    }
}