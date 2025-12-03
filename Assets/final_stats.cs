using UnityEngine;
using TMPro;

public class final_stats : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public TextMeshPro finalScoreDisplay;
    void Start()
    {
        finalScoreDisplay.text = "FINAL SCORE: " + BallController.score;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
