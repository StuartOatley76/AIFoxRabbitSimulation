using System;

namespace Sensors {

    namespace Hearing {
        /// <summary>
        /// Class to manage passing sounds to sound sensors
        /// </summary>
        public static class SoundManager {
            /// <summary>
            /// Event triggered when sound is made
            /// </summary>
            private static EventHandler<SoundEventArgs> soundEventHandler;

            /// <summary>
            /// Connects the delegate passed in to the event
            /// </summary>
            /// <param name="del"></param>
            public static void Connect(EventHandler<SoundEventArgs> del) {
                soundEventHandler += del;
            }

            /// <summary>
            /// Disconnects the delegate passed in from the event
            /// </summary>
            /// <param name="del"></param>
            public static void Disconnect(EventHandler<SoundEventArgs> del) {
                soundEventHandler -= del;
            }

            /// <summary>
            /// Triggers the event with the supplied sound
            /// </summary>
            /// <param name="soundCreator">Object that made the sound</param>
            /// <param name="sound">The sound made</param>
            public static void MakeSound(Object soundCreator, Sound sound) {
                soundEventHandler?.Invoke(soundCreator, new SoundEventArgs(sound));
            }
        }
    }
}