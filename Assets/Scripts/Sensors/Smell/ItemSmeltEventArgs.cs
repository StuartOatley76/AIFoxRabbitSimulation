using System;

namespace Sensors {

    namespace Smell {

        /// <summary>
        /// class to pass a smeltobject as an eventarg
        /// </summary>
        public class ItemSmeltEventArgs : EventArgs {

            /// <summary>
            /// Object that is smelt
            /// </summary>
            public SmeltObject Smelt { get; private set; }

            /// <summary>
            /// constructor
            /// </summary>
            /// <param name="smelt"></param>
            public ItemSmeltEventArgs(SmeltObject smelt) {
                Smelt = smelt;
            }
        }
    }
}