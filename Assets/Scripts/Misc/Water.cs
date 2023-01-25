using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Memorable;

/// <summary>
/// Class to implement water as a memorable object
/// </summary>
public class Water : MemorableObject {

    protected override void Awake() {
        base.Awake();
        canMove = false;
    }
}
