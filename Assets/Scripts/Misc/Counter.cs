using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Counter : MonoBehaviour
{
    public static Counter Instance { get; private set; }

    [SerializeField]
    private TMP_Text rabbitCounter;

    [SerializeField]
    private TMP_Text foxCounter;

    [SerializeField]
    private string foxText = "Number of foxes = ";

    [SerializeField]
    private string rabbitText = "Number of rabbits = ";

    private void Awake() {
        if (Instance != null) {
            Destroy(this);
            return;
        }
        Instance = this;
    }
    private int numberOfFoxes;

    private int numberOfRabbits;

    private void Update() {
        if (rabbitCounter) {
            rabbitCounter.text = rabbitText + numberOfRabbits.ToString();
        }
        if (foxCounter) {
            foxCounter.text = foxText + numberOfFoxes.ToString();
        }
    }

    public void Add(string tag) {
        if (tag == "Fox") {
            numberOfFoxes++;
        }
        if (tag == "Rabbit") {
            numberOfRabbits++;
        }
    }

    public void Remove(string tag) {
        if (tag == "Fox") {
            numberOfFoxes++;
        }
        if (tag == "Rabbit") {
            numberOfRabbits++;
        }
    }
}
