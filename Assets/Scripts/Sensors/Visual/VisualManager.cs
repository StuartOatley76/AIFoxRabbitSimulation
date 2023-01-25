using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Memorable;


namespace Sensors {

    namespace Vision {

        /// <summary>
        /// class to manage one or more vision sensors on an object and determine what can be seen
        /// </summary>
        [RequireComponent(typeof(VisionSensor))]
        public class VisualManager : MonoBehaviour {

            /// <summary>
            /// List of the vision sensors attached to this gameobject
            /// </summary>
            private List<VisionSensor> sensors;

            /// <summary>
            /// Visibility value above which objects are definitely seen
            /// </summary>
            [Range(0.5f, 100f)]
            [SerializeField]
            private float DefinitelyDetectedLevel;

            /// <summary>
            /// Visibility value below which objects are definitely not seen
            /// </summary>
            [SerializeField]
            private float NotDetectedLevel;

            /// <summary>
            /// 1 percent of the difference between not detected and def detected
            /// </summary>
            private float percentile;

            /// <summary>
            /// ensures not detected is above 0 and definitely detected is greater than not detected
            /// </summary>
            private void OnValidate() {
                if(NotDetectedLevel < 0) {
                    NotDetectedLevel = 0;
                }
                if(NotDetectedLevel >= DefinitelyDetectedLevel) {
                    NotDetectedLevel = DefinitelyDetectedLevel - 0.1f;
                }
            }

            /// <summary>
            /// Gathers the vision sensors and calculates percentile
            /// </summary>
            private void Awake() {
                sensors = new List<VisionSensor>(GetComponents<VisionSensor>());
                percentile = (DefinitelyDetectedLevel - NotDetectedLevel) / 100;
            }

            /// <summary>
            /// Retreives objects in the vision sensors ann returns ones which pass visibility check
            /// </summary>
            /// <returns></returns>
            public List<MemorableObject> GetObjectsSeen() {
                Dictionary<MemorableObject, float> seenObjects = new Dictionary<MemorableObject, float>();
                for (int i = 0; i < sensors.Count; i++) {
                    Dictionary<MemorableObject, float> temp = sensors[i].GetObjectsInView();
                    MergeDictionaries(seenObjects, temp);
                }
                return CheckDetected(seenObjects);
                
            }

            /// <summary>
            /// Works through all objects in the dictionary determining whether they were seen. For those between the 
            /// definitelyDetected and notDetected values it is random, weighted by how far through the gap the value is
            /// </summary>
            /// <param name="seenObjects"></param>
            /// <returns></returns>
            private List<MemorableObject> CheckDetected(Dictionary<MemorableObject, float> seenObjects) {
                List<MemorableObject> detected = new List<MemorableObject>();
                foreach (KeyValuePair<MemorableObject, float> entry in seenObjects) {
                    if (entry.Value > DefinitelyDetectedLevel) {
                        detected.Add(entry.Key);
                        continue;
                    }
                    if (entry.Value < NotDetectedLevel) {
                        continue;
                    }
                    float percentage = (entry.Value - NotDetectedLevel) / percentile;
                    if(Random.Range(0, 100f) < percentage) {
                        detected.Add(entry.Key);
                    }
                }
                return detected;
            }

            /// <summary>
            /// Combines the dictionaries, using the highest value if the object is in both
            /// </summary>
            /// <param name="seenObjects"></param>
            /// <param name="temp"></param>
            private void MergeDictionaries(Dictionary<MemorableObject, float> seenObjects, Dictionary<MemorableObject, float> temp) {
                foreach(KeyValuePair<MemorableObject, float> entry in temp) {
                    if (seenObjects.ContainsKey(entry.Key)) {
                        seenObjects[entry.Key] = (entry.Value > seenObjects.GetValueOrDefault(entry.Key)) ? entry.Value : seenObjects.GetValueOrDefault(entry.Key);
                        continue;
                    }
                    seenObjects.Add(entry.Key, entry.Value);
                }
            }
        }
    }
}