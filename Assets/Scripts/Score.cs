using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class Score : MonoBehaviour
{

	[SerializeField] TMP_Text displayScore;
    [SerializeField] GameObject player;
    private int score;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
		displayScore = GetComponent<TMP_Text>();
	}

	private void Update()
	{
		if (score == 10) 
		{
			SceneManager.LoadScene("Victory");

		}
	}

	// Update is called once per frame
	public void AddPoint()
    {
        score++;
        displayScore.text = score.ToString();
    }
}
