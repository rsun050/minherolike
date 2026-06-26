using UnityEngine;

class Character : MonoBehaviour, IInteractable {
	public void Interact() {
		Debug.Log($"Interacting with {name}");
	}
}