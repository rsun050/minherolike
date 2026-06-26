using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    #region variables
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private float maxMoveSpeed;
    [SerializeField] private float moveDamping;

    [Header("Interaction Parameters")]
    [SerializeField] private LayerMask interactableLayers;
    [SerializeField] private float interactRange;
    private ContactFilter2D interactionFilter;


    private bool canMove = true;
    private float horizDirection = 0f;
    private float vertDirection = 0f;

    private float refSpeedVar;    
    private float currSpeed = 0f;
    #endregion variables

    void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, interactRange);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.05f);
    }
    void Start() {
		interactionFilter = new ContactFilter2D {
			useLayerMask = true,
            layerMask = interactableLayers
		};
	}

    // Update is called once per frame
    void Update() {
        GetInputs();

        Vector2 dir = (Vector2.up * vertDirection + Vector2.right * horizDirection).normalized;
        if(dir.x > 0) { // right
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        } else if(dir.x < 0) { // left
            transform.localScale = new Vector3(-1 * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);            
        }

        currSpeed = Mathf.SmoothDamp(
            currSpeed,
            (dir.magnitude > 0.01f) ? maxMoveSpeed : 0,
            ref refSpeedVar,
            moveDamping,
            maxMoveSpeed
        );

        if(dir == Vector2.zero) {
            animator.SetBool("walk", false);
            rb.linearVelocity = Vector2.zero;
        } else {
            animator.SetBool("walk", true);
            rb.AddForce(maxMoveSpeed * dir); 
        }
    }

    void LateUpdate() {
        if(rb.linearVelocity.magnitude > maxMoveSpeed) {
            rb.linearVelocity = maxMoveSpeed * rb.linearVelocity.normalized;
        }
    }

    private void GetInputs() {
        horizDirection = 0f;
        if (Keyboard.current[Key.A].isPressed || Keyboard.current[Key.LeftArrow].isPressed) {
            horizDirection -= 1;
        }
        else if (Keyboard.current[Key.D].isPressed || Keyboard.current[Key.RightArrow].isPressed) {
            horizDirection += 1;
        }

        vertDirection = 0f;
        if (Keyboard.current[Key.S].isPressed || Keyboard.current[Key.DownArrow].isPressed) {
            vertDirection -= 1;
        }
        else if (Keyboard.current[Key.W].isPressed || Keyboard.current[Key.UpArrow].isPressed) {
            vertDirection += 1;
        }

        if(Keyboard.current[Key.Z].wasPressedThisFrame || Keyboard.current[Key.Space].wasPressedThisFrame || Keyboard.current[Key.Enter].wasPressedThisFrame) {
            Interact();
        }
    }

    private void Interact() {
        // check for something in range to interact
        List<Collider2D> objsInRange = new List<Collider2D>();
        Physics2D.OverlapCircle(
            transform.position, 
            interactRange, 
            interactionFilter,
            objsInRange
        );

        if(objsInRange.Count > 0) {
            objsInRange.Sort(ProximitySorter);
            objsInRange[0].gameObject.GetComponent<IInteractable>().Interact();
        }
    }

    // sort by proximity to the player (nearest first)
    private int ProximitySorter(Collider2D a, Collider2D b) {
        float adist = Vector2.Distance(transform.position, a.transform.position);
        float bdist = Vector2.Distance(transform.position, b.transform.position);

        if(adist < bdist) {
            return -1;
        } else if(adist == bdist) {
            return 0;
        } else {
            return 1;
        }
    }
}
