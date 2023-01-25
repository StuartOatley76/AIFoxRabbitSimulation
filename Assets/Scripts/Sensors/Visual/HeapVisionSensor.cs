using UnityEngine;

namespace Sensors {

    namespace Vision {

        /// <summary>
        /// Concrete class derived from VisionSensor
        /// </summary>
        public class HeapVisionSensor : VisionSensor {

            /// <summary>
            /// Updates the objectsInRange list using spacial heap
            /// </summary>
            /// <param name="position"></param>
            /// <param name="range"></param>
            protected override void UpdateObjectsInRange(Vector3 position, float range) {
                int intRange = (int)Mathf.Ceil(range);
                objectsInRange = WorldHeap.instance.GetObjectsInRange(position, intRange);
                objectsInRange.RemoveAll(r => r == null);
            }
        }
    }
}
