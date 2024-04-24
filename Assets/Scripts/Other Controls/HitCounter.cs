using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HitCounter : MonoBehaviour
{
    [SerializeField] private int scoreAdded = 1;
    [SerializeField] private TextMeshProUGUI scoreboardText;
    public float score;

    public static HitCounter Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);
    }

    // Adds to score on initial player collision trigger with a platform 
    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision detected with platform."); // Add this line for debugging

        // Check if the collision is with a platform and if it's the first time
        if (collision.gameObject.CompareTag("Fox"))
        {

            // Increase the score (you can adjust this as needed)
            score += scoreAdded; // Example: Add # points for each platform collision

            // Update the UI
            DisplayScore();
        }
    }


    //Displays score to UI rounded to nearest integer
    public void DisplayScore()
    {
        // Round the score to the nearest integer
        int roundedScore = Mathf.RoundToInt(score);

        // Update the TextMeshProUGUI text component to display the score
        scoreboardText.text = "Hits: " + roundedScore.ToString();
    }
}
