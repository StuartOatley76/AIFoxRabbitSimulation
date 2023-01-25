using System.Collections.Generic;
using UnityEngine;
using Memorable;
using Characters;

namespace Sensors {

    namespace Vision {

        /// <summary>
        /// Abstract class to represent a visual sensor
        /// </summary>
        [RequireComponent(typeof(BoxRaycaster))]
        public abstract class VisionSensor : MonoBehaviour {

            /// <summary>
            /// List of visible objects in range
            /// </summary>
            protected List<MemorableObject> objectsInRange;

            /// <summary>
            /// Abstract function to fill objectsInRange list
            /// </summary>
            /// <param name="position">Position of sensor</param>
            /// <param name="range">How far the sensor can detect</param>
            protected abstract void UpdateObjectsInRange(Vector3 position, float range);

            /// <summary>
            /// Range of the sensor
            /// </summary>
            [SerializeField]
            private float range;

            public float Range { get { return range; } protected set { range = value; } }

            /// <summary>
            /// Total angle of the sensor
            /// </summary>
            [SerializeField]
            [Range(0, 360)]
            private float viewAngle;

            public float ViewAngle { get { return viewAngle; } protected set { viewAngle = value % 360; } }

            /// <summary>
            /// BoxRaycaster attached to this gameobject
            /// </summary>
            private BoxRaycaster boxRaycaster;

            /// <summary>
            /// affects how much speed affects visibility
            /// </summary>
            [SerializeField]
            private float movementDetectionMultiplier;

            /// <summary>
            /// affects how much distance affects visibility
            /// </summary>
            [SerializeField]
            private float MaxClosenessValue;

            private void Awake() {
                boxRaycaster = GetComponent<BoxRaycaster>();
            }

            /// <summary>
            /// updates objectsInRange then loops through it checking whether it has a collider, is in range, is within the
            /// vision cone and finally raycasts to the object using the boxRaycaster.
            /// </summary>
            /// <param name="visionTransfrom">Position of the sensor</param>
            /// <param name="range">How far the sensor can see</param>
            /// <param name="CosVisionConeAngle">cos of the vision angle</param>
            /// <returns>List of objects that can be seen</returns>
            public Dictionary<MemorableObject, float> GetObjectsInView() {
                UpdateObjectsInRange(transform.position, Range);
                Dictionary<MemorableObject, float> objectsInView = new Dictionary<MemorableObject, float>();
                for (int i = 0; i < objectsInRange.Count; i++) {

                    if (objectsInRange[i].gameObject == gameObject) {
                        continue;
                    }

                    if (objectsInRange[i].GetComponent<Collider>() == null) {
                        continue;
                    }

                    Vector3 directionToTarget = objectsInRange[i].transform.position - transform.position;

                    if (directionToTarget.magnitude > Range) {
                        continue;
                    }

                    if(Vector3.Angle(transform.forward, directionToTarget) > ViewAngle * 0.5f) {
                        continue;
                    }

                    if (boxRaycaster.CanDetect(objectsInRange[i].gameObject, Range)) {
                        float visibilityLevel = CalculateVisibilityLevel(objectsInRange[i]);
                        objectsInView.Add(objectsInRange[i], visibilityLevel);
                    }

                }

                return objectsInView;
            }

            /// <summary>
            /// Calculates visibility level based on how far the object is and how fast it is going
            /// </summary>
            /// <param name="memorableObject"></param>
            /// <returns></returns>
            private float CalculateVisibilityLevel(MemorableObject memorableObject) {
                float distance = Vector3.Distance(memorableObject.gameObject.transform.position, transform.position);
                float returnValue = 0;
                if (distance != 0 && MaxClosenessValue != 0) {
                     returnValue = 1 / distance * MaxClosenessValue;
                } else {
                    return float.MaxValue;
                }
                if(memorableObject.GetComponent<Character>() && 
                    memorableObject.GetComponent<Character>().GetMovementType() == Movement.MovementType.Sneak) {
                    returnValue *= 0.5f;
                }
                if(memorableObject.GetComponent<Rigidbody>() != null) {
                    returnValue += memorableObject.GetComponent<Rigidbody>().velocity.magnitude * movementDetectionMultiplier;
                }
                return returnValue;
            }
        }
    }
}