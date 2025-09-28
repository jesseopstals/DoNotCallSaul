using UnityEngine;
using TMPro;

public class PickUp : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private Score scoreScript;
	[SerializeField] string pickupMessage = "You picked up the object!";
	[SerializeField] TextMeshProUGUI displayText;
	[SerializeField] FadeText fadeTextScript; // reference to FadeText script


	void Start()
    {
	
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			if (displayText != null)
			{
				displayText.text = pickupMessage; 
			}
			fadeTextScript.ShowAndFade(pickupMessage, 2f); // show message for 3s and fade
			scoreScript.AddPoint();
			Destroy(gameObject);
		}
	}
}
