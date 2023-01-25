using System.Collections.Generic;
using UnityEngine;

namespace Sensors {

    /// <summary>
    /// Class to handle casting multiple rays in a grid at an object
    /// </summary>
    public class BoxRaycaster : MonoBehaviour {

        /// <summary>
        /// Record of objects rays have been cast at this frame so raycasts aren't repeated unnecessarily
        /// </summary>
        Dictionary<GameObject, float> objectsCastForThisFrame = new Dictionary<GameObject, float>();

        /// <summary>
        /// number of rays to cast across
        /// </summary>
        [SerializeField]
        private int numberOfRaysAcross;

        /// <summary>
        /// number of rays to cast down
        /// </summary>
        [SerializeField]
        private int numberOfRaysDown;

        /// <summary>
        /// Checks record of objects cast for this frame. If object not found, casts a grid of rays at the object based on
        /// it's collider size and notes the distance in the dictionary (not hit = float.MaxValue)
        /// </summary>
        /// <param name="objectToDetect">Gameobject to raycast for</param>
        /// <param name="distanceForRays">Distance restriction</param>
        /// <returns>Whether a ray can hit the object given the distance</returns>
        public bool CanDetect(GameObject objectToDetect, float distanceForRays) {
            if (objectsCastForThisFrame.ContainsKey(objectToDetect)) {
                return objectsCastForThisFrame.GetValueOrDefault(objectToDetect) < distanceForRays;
            }

            Collider collider = objectToDetect.GetComponent<Collider>();
            if (collider == null) {
                objectsCastForThisFrame.Add(objectToDetect, float.MaxValue);
                return false;
            }

            Vector3 direction = objectToDetect.transform.position - transform.position;

            float maxXSize = (collider.bounds.size.x > collider.bounds.size.z) ? collider.bounds.size.x : collider.bounds.size.z;
            float rayDistanceApartOnX = maxXSize / numberOfRaysAcross;
            float rayDistanceApartOnY = collider.bounds.size.y / numberOfRaysDown;

            int xDividedByTwo = IntDivisionByTwoRoundedUp(numberOfRaysAcross);
            int yDividedByTwo = IntDivisionByTwoRoundedUp(numberOfRaysDown);

            for (int x = -xDividedByTwo; x <= xDividedByTwo; x++) {
                for (int y = -yDividedByTwo; y <= yDividedByTwo; y++) {
                    Vector3 directionToCast = new Vector3(direction.x + x * rayDistanceApartOnX, direction.y + y * rayDistanceApartOnY, direction.z);
                    if(Physics.Raycast(transform.position, directionToCast, out RaycastHit hit) 
                        && hit.collider == collider) {
                        objectsCastForThisFrame.Add(objectToDetect, hit.distance);
                        return hit.distance <= distanceForRays;
                    }
                }
            }

            objectsCastForThisFrame.Add(objectToDetect, float.MaxValue);
            return false;
        }

        /// <summary>
        /// int divides an interger by two and adds one if the number is odd
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private int IntDivisionByTwoRoundedUp(int number) {
            int divided = number / 2;
            return (number % 2 == 0) ? divided : divided + 1;
        }

        /// <summary>
        /// clears the record of the rays
        /// </summary>
        void LateUpdate() {
            objectsCastForThisFrame.Clear();
        }
    }
}
