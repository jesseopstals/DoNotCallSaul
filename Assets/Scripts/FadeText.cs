using System.Collections;
using UnityEngine;
using TMPro;

public class FadeText : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI displayText;
	[SerializeField] private float fadeDuration = 2f;

	public void ShowAndFade(string message, float visibleTime = 3f)
	{
		StopAllCoroutines(); // stop old fades
		StartCoroutine(FadeOutText(message, visibleTime));
	}

	private IEnumerator FadeOutText(string message, float visibleTime)
	{
		// Set text + reset alpha
		displayText.text = message;
		displayText.color = new Color(displayText.color.r, displayText.color.g, displayText.color.b, 1f);

		// Wait before fading
		yield return new WaitForSeconds(visibleTime);

		Color originalColor = displayText.color;
		float elapsedTime = 0f;

		while (elapsedTime < fadeDuration)
		{
			float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
			displayText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

			elapsedTime += Time.deltaTime;
			yield return null;
		}

		displayText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
	}
}
