using System;
using UnityEngine;

namespace Sensors {

    namespace Hearing {

        /// <summary>
        /// Class for a sensor that detects sounds
        /// </summary>
        public class SoundSensor : MonoBehaviour {

            /// <summary>
            /// Range that the sensor can detect sounds in
            /// </summary>
            private float hearingRange = 2f;

            /// <summary>
            /// Square of hearingRange
            /// </summary>
            private float squareHearingRange;

            /// <summary>
            /// Event triggered when sound is detected
            /// </summary>
            private EventHandler<SoundEventArgs> soundHeardEventHandler;

            /// <summary>
            /// Connects to sound manager and caches square of hearing range
            /// </summary>
            private void Awake() {
                SoundManager.Connect(SoundMade);
                squareHearingRange = hearingRange * hearingRange;
            }

            public void SetHearingRange(float range) {
                hearingRange = range;
            }

            /// <summary>
            /// Delegate called by soundmanager when a sound is created. Checks distance and triggers soundHeardEventHandler
            /// if can be detected
            /// </summary>
            /// <param name="o"></param>
            /// <param name="e"></param>
            private void SoundMade(object o, SoundEventArgs e) {
                Vector3 direction = e.Sound.Position - transform.position;

                if (direction.sqrMagnitude > e.Sound.Range * e.Sound.Range || direction.sqrMagnitude > squareHearingRange) {
                    return;
                }

                soundHeardEventHandler?.Invoke(this, e);
            }

            public void Connect(EventHandler<SoundEventArgs> del) {
                soundHeardEventHandler += del;
            }

            /// <summary>
            /// Disconnects from sound manager
            /// </summary>
            private void OnDestroy() {
                SoundManager.Disconnect(SoundMade);
            }
        }
    }
}