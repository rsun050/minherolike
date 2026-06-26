using UnityEngine;

class InteractableItem : MonoBehaviour, IInteractable {
	public void Interact() {
		Debug.Log($"Interacting with {name}");
	}
}