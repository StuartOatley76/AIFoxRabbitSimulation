using System;

namespace Sensors {

    namespace Hearing {

        /// <summary>
        /// Class for a sound to be passed as eventargs
        /// </summary>
        public class SoundEventArgs : EventArgs {

            /// <summary>
            /// The sound
            /// </summary>
            public Sound Sound { get; private set; }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="sound"></param>
            public SoundEventArgs(Sound sound) {
                Sound = sound;
            }
        }
    }
}