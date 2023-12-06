using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveCounterHUD : MonoBehaviour
{
    public TMP_Text waveCounter;
    public TMP_Text waveHeader;
    public TMP_Text waveTimer;
    public int timeOnClock = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void newWave(int secsIn, int waveCount, bool isBreak) {
        Debug.Log("starting new wave " + secsIn + " " + waveCount + " " + isBreak);
        if (waveCount == 0) {
            EnemyWaveSpawnerTake2.singleton.waveCount++;
            waveHeader.SetText("Game starts in:");
            waveCounter.SetText("Preparation");
            timeOnClock = secsIn;
            waveTimer.SetText(formatTime(timeOnClock));
        } else if (isBreak) {
            waveHeader.SetText("Break ends in:");
            waveCounter.SetText("Preparation");
            timeOnClock = secsIn;
            waveTimer.SetText(formatTime(timeOnClock));
        } else if (waveCount != 26) {
            waveHeader.SetText("Wave ends in:");
            waveCounter.SetText("Wave: " + waveCount);
            timeOnClock = secsIn;
            waveTimer.SetText(formatTime(timeOnClock));
        } else {
            waveHeader.SetText("Wave ends in:");
            waveCounter.SetText("Survive");
            waveTimer.SetText("your\ndeath");
            waveTimer.fontSize = 30;
        }
        
    }

    public string formatTime(int secsIn) {
        int secs = secsIn % 60;
        int mins = secsIn / 60;
        string timerOut = mins + ":";
        if (secs < 10) {
            timerOut += "0";
        }
        timerOut += secs;
        Debug.Log(secsIn + " formatted as " + timerOut);
        return timerOut;
    }

    public void decrementSecond() {
        if (timeOnClock > 0) {
            timeOnClock--;
            waveTimer.SetText(formatTime(timeOnClock));
        }
    }
}
