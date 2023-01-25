using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters {

    /// <summary>
    /// class to handle walking to the target
    /// </summary>
    public class WalkToTarget : TargetRelatedMovement {

        protected override void SetNewPosition() {
            movement.TargetPosition = character.Target.transform.position;
        }

    }
}