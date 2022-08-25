using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

// Class: GameManager
// Author: Ajay Ramnarine
// Purpose: Keeps track of the score and manages the scene of the main game
// Restrictions: Only for the main game, do not add to the title screen
public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject boss;
    public GameObject winScreen;
    public GameObject loseScreen;

    public TextMeshProUGUI finalScore;

    public int score = 0;
    int multiplier = 1;

    // Start is called before the first frame update
    void Start()
    {
        // disable the final score until the game ends
        finalScore.enabled = false;
        
        // disable the win screen and lose screen at the start
        winScreen.SetActive(false);
        loseScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(finalScore.enabled)
        {
            // set the text scores to the current score
            finalScore.text = "Final Score: " + score;
        }
    }

    // Method: IncreaseScore
    // Purpose: Increase the player's score
    // Restrictions: None
    public void IncreaseScore(int scoreIncrement)
    {
        // increase the score by the passed in increment multiplied by the current multiplier
        score += scoreIncrement * multiplier;
    }

    // Method: IncreaseMultiplier
    // Purpose: Increases the multiplier by 1 whenever the player lands a hit
    // Restrictions: None
    public void IncreaseMultiplier()
    {
        // increase the multiplier by 1 whenever this method is called
        multiplier++;
    }

    // Method: ResetMultiplier
    // Purpose: Sets the multiplier back to 1 whenever the player is hit
    // Restrictions: None
    public void ResetMultiplier()
    {
        // set the multiplier back to 1
        multiplier = 1;
    }



}
