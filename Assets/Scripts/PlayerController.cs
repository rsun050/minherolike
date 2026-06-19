using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    #region variables
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float maxMoveSpeed;
    [SerializeField] private float moveDamping;

    private float horizDirection = 0f;
    private float vertDirection = 0f;

    private float refSpeedVar;    
    private float currSpeed = 0f;
    #endregion variables

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
            rb.linearVelocity = currSpeed * dir;
        } else {
            rb.AddForce(currSpeed * dir);        
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
    }
}
