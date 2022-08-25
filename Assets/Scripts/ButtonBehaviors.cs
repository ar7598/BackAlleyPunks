using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Class: ButtonBehaviors
// Author: Ajay Ramnarine
// Purpose: Methods that will load scenes whenever a button is pressed that is attached to the method
// Restrictions: None
public class ButtonBehaviors : MonoBehaviour
{
    // Method: TitleScreen
    // Purpose: Attaches to a button to load the title screen scene
    // Restrictions: Only works for the button that is attached
    public void TitleScreen()
    {
        // loads the title screen scene
        SceneManager.LoadScene(0);
    }

    // Method: MainGame
    // Purpose: Attaches to a button to load the main game scene
    // Restrictions: Only works for the button that is attached
    public void MainGame()
    {
        // loads the main game scene
        SceneManager.LoadScene(1);
    }
}
