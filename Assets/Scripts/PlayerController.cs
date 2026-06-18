using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    #region variables
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

        currSpeed = Mathf.SmoothDamp(
            currSpeed,
            (dir.magnitude > 0.01f) ? maxMoveSpeed : 0,
            ref refSpeedVar,
            moveDamping,
            maxMoveSpeed
        );

        transform.Translate(currSpeed * Time.deltaTime * dir);
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
