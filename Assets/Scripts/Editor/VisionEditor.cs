using System.Collections;
using UnityEditor;
using Sensors.Vision;
using UnityEngine;

/// <summary>
/// Provides a visual representation of the vision cone in the scene view
/// </summary>
[CustomEditor(typeof(VisionSensor), true)]
public class VisionEditor : Editor
{
    private void OnSceneGUI() {
        VisionSensor vs = (VisionSensor)target;
        Handles.color = Color.white;
        Vector3 firstAngle = DirectionFromAngle(-vs.ViewAngle * 0.5f, false, vs.transform);
        Vector3 secondAngle = DirectionFromAngle(vs.ViewAngle * 0.5f, false, vs.transform);
        Handles.DrawWireArc(vs.transform.position, Vector3.up, 
            new Vector3((vs.transform.position + firstAngle * vs.Range).x,
            0,
            (vs.transform.position + firstAngle * vs.Range).z),
            vs.ViewAngle, vs.Range);

        Handles.DrawLine(vs.transform.position, vs.transform.position + firstAngle * vs.Range);
        Handles.DrawLine(vs.transform.position, vs.transform.position + secondAngle * vs.Range);
    }

    private Vector3 DirectionFromAngle(float angleInDegrees, bool angleIsInWorldPosition, Transform transform) {
        angleInDegrees = (angleIsInWorldPosition) ? angleInDegrees : angleInDegrees + transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
