using Sensors.Hearing;
using Sensors.Smell;
using Sensors.Vision;
using System.Collections.Generic;
using UnityEngine;
using Memorable;
using Characters;

namespace Sensors {

    /// <summary>
    /// Class to handle sensors attached to the character
    /// </summary>
    [RequireComponent(typeof(Memory), typeof(Character))]
    public class SensorManager : MonoBehaviour {

        /// <summary>
        /// The visual manager attached to the character
        /// </summary>
        private VisualManager visualManager;

        /// <summary>
        /// The sound sensor attached to this gameobject
        /// </summary>
        private SoundSensor soundSensor;

        /// <summary>
        /// The smell sensor attached to this gameobject
        /// </summary>
        private SmellSensor smellSensor;

        /// <summary>
        /// The Memory attached to this character
        /// </summary>
        private Memory memory;

        /// <summary>
        /// How many frames between each polling of the Vision manager
        /// </summary>
        [SerializeField]
        private int visionPollFrequency = 5;

        /// <summary>
        /// How long mobile memorable objects that move should be remembered fo in seconds
        /// </summary>
        [SerializeField]
        private float timeToRememberMobile = 180f;

        [SerializeField]
        private float timeToRememberUnseen = 20f;

        /// <summary>
        /// The tags that identify an object as food to this character
        /// </summary>
        [SerializeField]
        private List<string> foodTags;

        /// <summary>
        /// The tags that identify an object as water to this character
        /// </summary>
		[SerializeField]
        private List<string> waterTags;

        /// <summary>
        /// The tags that identify an object as a threat to this character
        /// </summary>
		[SerializeField]
        private List<string> threatTags;




        /// <summary>
        /// Assigns the memory and visual manager (if one) and connects the delegates to
        /// the sound and smell sensors
        /// </summary>
		private void Awake() {
            visualManager = GetComponent<VisualManager>();
            soundSensor = GetComponent<SoundSensor>();
            if (soundSensor != null) {
                soundSensor.Connect(SoundHeard);
            }
            smellSensor = GetComponent<SmellSensor>();
            if (smellSensor != null) {
                smellSensor.Connect(ObjectSmelt);

            }

            memory = GetComponent<Memory>();
        }

        public void SetRanges(float hearingRange, float smellRange) {
            if (soundSensor != null) {
                soundSensor.SetHearingRange(hearingRange);
            }
            if(smellSensor != null) {
                smellSensor.SetSmellRadius(smellRange);
            }
        }

        /// <summary>
        /// Delegate triggered when sound sensor recieves a noise
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void SoundHeard(object o, SoundEventArgs e) {
            if (o is GameObject) {
                GameObject go = (GameObject)o;
                MemorableObject mo = go.GetComponent<MemorableObject>();
                if (mo != null) {
                    MemorableObjectType objectType = GetMemorableObjectType(mo);
                    if (memory.ContainsObject(mo, objectType)) {
                        return;
                    }
                    memory.AddUnseenObject(mo.transform.position, objectType, timeToRememberUnseen);
                }

            }
        }

        /// <summary>
        /// Delegate triggered when the smell sensor detects a smell
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void ObjectSmelt(object o, ItemSmeltEventArgs e) {
            GameObject go = (GameObject)o;
            MemorableObject mo = go.GetComponent<MemorableObject>();
            if (mo != null) {
                MemorableObjectType objectType = GetMemorableObjectType(mo);
                if (memory.ContainsObject(mo, objectType)) {
                    return;
                }
                memory.AddUnseenObject(mo.transform.position, objectType, timeToRememberUnseen);
            }
        }


        /// <summary>
        /// Polls the vision sensor when necessary 
        /// </summary>
        private void Update() {
            if (visualManager != null && Time.frameCount % visionPollFrequency == 0) {
                List<MemorableObject> seenObjects = visualManager.GetObjectsSeen();
                for (int i = 0; i < seenObjects.Count; i++) {
                    float memoryTime = (seenObjects[i].CanMove) ? timeToRememberMobile : -1;
                    MemorableObjectType objectType = GetMemorableObjectType(seenObjects[i]);
                    memory.AddObjectToMemory(seenObjects[i], objectType, memoryTime);
                }
            }
        }


        /// <summary>
        /// Gets the MemorableObjectType of the MemorableObject provided based on the tags
        /// </summary>
        /// <param name="mo"></param>
        /// <returns></returns>
        public MemorableObjectType GetMemorableObjectType(MemorableObject mo) {

            if (threatTags.Contains(mo.tag)) {
                return MemorableObjectType.Threat;
            }
            if (foodTags.Contains(mo.tag)) {
                return MemorableObjectType.Food;
            }
            if (waterTags.Contains(mo.tag)) {
                return MemorableObjectType.Water;
            }
            Character moCharacter = mo.GetComponent<Character>();
            Character thisCharacter = gameObject.GetComponent<Character>();
            if (moCharacter != null && moCharacter.GetType() == thisCharacter.GetType() &&
                moCharacter.Sex != thisCharacter.Sex) {
                return MemorableObjectType.Mate;
            }
            return MemorableObjectType.Neutral;
        }
    }
}