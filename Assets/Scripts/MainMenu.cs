using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		// Make sure the cursor is visible and free when we enter the main menu
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}
	
	public void StartGame() 
    {
        SceneManager.LoadScene("MainScene");
    }

	public void QuitGame() 
	{
		Application.Quit();
	}
}
