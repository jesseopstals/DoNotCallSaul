//using UnityEngine;

//public class DoorTrigger : MonoBehaviour
//{
//	public Door door; // assign parent door in inspector

//	void OnTriggerEnter(Collider other)
//	{
//		if (other.CompareTag("Player") || other.CompareTag("Enemy"))
//		{
//			door.OpenDoor();
//			Debug.Log("enemy at door!");
//		}
//	}

//	void OnTriggerExit(Collider other)
//	{
//		if (other.CompareTag("Player") || other.CompareTag("Enemy"))
//		{
//			door.CloseDoor();
//		}
//	}
//}
