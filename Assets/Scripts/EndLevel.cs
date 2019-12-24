using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class EndLevel : MonoBehaviour
{
    Flowchart chart;

    public void Start() {
        chart = GameObject.FindObjectOfType<Flowchart>();
    }

    void OnTriggerEnter2D(Collider2D collider2d) {
        if (collider2d.tag == "Player") {
            chart.ExecuteBlock("LevelEnd");
        }
    }
}
