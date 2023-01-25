using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Memorable {
    /// <summary>
    /// Enum to represent different types of things that can be remembered
    /// </summary>
    [Serializable]
    public enum MemorableObjectType {
        Neutral,
        Food,
        Water,
        Threat,
        Mate
    }

}
