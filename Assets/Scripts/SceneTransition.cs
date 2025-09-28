using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
	[Header("Transition Settings")]
	[SerializeField] float waitTime = 5f;     // time scene stays visible before fade out
	[SerializeField] float fadeDuration = 2f; // duration of fade in/out
	[SerializeField] string nextSceneName;    // scene to load

	[Header("UI")]
	[SerializeField] RawImage fadeImage;         // black UI Image covering the whole screen

	void Start()
	{
		//// Start fully opaque (black screen)
		//Color c = fadeImage.color;
		//c.a = 1f;
		//fadeImage.color = c;

		// Start the transition sequence
		StartCoroutine(TransitionSequence());
	}

	IEnumerator TransitionSequence()
	{
		// ---- FADE IN ----
		float t = 0f;
		while (t < fadeDuration)
		{
			t += Time.deltaTime;
			float alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
			SetAlpha(alpha);
			yield return null;
		}

		// Wait before fading out
		yield return new WaitForSeconds(waitTime);

		// ---- FADE OUT ----
		t = 0f;
		while (t < fadeDuration)
		{
			t += Time.deltaTime;
			float alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
			SetAlpha(alpha);
			yield return null;
		}

		// Load the next scene
		SceneManager.LoadScene(nextSceneName);
	}

	void SetAlpha(float alpha)
	{
		Color c = fadeImage.color;
		c.a = alpha;
		fadeImage.color = c;
	}
}
