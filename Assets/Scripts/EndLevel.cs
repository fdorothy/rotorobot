using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class EndLevel : MonoBehaviour
{
    Flowchart chart;
    Player player;

    public void Start() {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        chart = GameObject.Find("Flowchart").GetComponent<Flowchart>();
    }

    void OnTriggerEnter2D(Collider2D collider2d) {
        if (collider2d.tag == "Player") {
            player.paused = true;

            chart.ExecuteBlock("LevelEnd");
        }
    }
}
