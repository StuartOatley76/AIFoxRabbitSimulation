using UnityEngine;
namespace Sensors {

    namespace Hearing {
        /// <summary>
        /// Class to represent a sound
        /// </summary>
        public class Sound {
            /// <summary>
            /// Position the sound is made
            /// </summary>
            public Vector3 Position { get; private set; }

            /// <summary>
            /// Maximum distance the sound can be heard from
            /// </summary>
            public float Range { get; private set; }

            /// <summary>
            /// Constructor. Assigns variables, plays the audiosource if applicable and passes the sound to the sound manager
            /// </summary>
            /// <param name="pos"></param>
            /// <param name="maxRange"></param>
            /// <param name="source"></param>
            public Sound(object soundCreator, Vector3 pos, float maxRange ,AudioSource source = null) {
                Position = pos;
                Range = maxRange;
                if (source != null && !source.isPlaying) {
                    source.Play();
                }
                SoundManager.MakeSound(soundCreator, this);
            }
        }
    }
}