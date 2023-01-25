using UnityEngine;

/// <summary>
/// Class to rotate the wind
/// </summary>
public class Wind : MonoBehaviour
{
    float rotateTime;
    float currentrotatetime;
    float rotateAmount;
    float maxRotateAmount = 10f;
    float minRotateTime = 10f;
    float maxRotateTime = 20f;
    float timeUntilNextChange;
    float minTimeUntilNextChange = 100f;
    float maxTimeUntilNextChange = 200f;

    private void Awake() {
        GetNewDirection();
    }
    private void Update() {
        if(timeUntilNextChange <= 0) {
            GetNewDirection();
        }
        if(currentrotatetime < rotateTime) {
            transform.Rotate(Vector3.up, rotateAmount, Space.World);
            currentrotatetime += Time.deltaTime;
        }

        timeUntilNextChange -= Time.deltaTime;
    }

    private void GetNewDirection() {
        currentrotatetime = 0;
        rotateTime = Random.Range(minRotateTime, maxRotateTime);
        float AngleToRotate = Random.Range(0, maxRotateAmount);
        AngleToRotate = (Random.Range(0, 2) == 1) ? -AngleToRotate : AngleToRotate;
        rotateAmount = AngleToRotate / rotateTime;
        timeUntilNextChange = Random.Range(minTimeUntilNextChange, maxTimeUntilNextChange);
    }

}
