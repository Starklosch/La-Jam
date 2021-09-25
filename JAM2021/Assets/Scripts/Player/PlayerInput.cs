using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [Tooltip("Factor de sensibilidad para mover la camara")]
    [Range(0f, 1f)]
    public float lookSensitivity = 1f;

    [Tooltip("Ivenrtir en vertical")]
    public bool invertYAxis = false;

    [Tooltip("Invertir en horizontal")]
    public bool invertXAxis = false;

    PlayerController playerController;

    void Start()
    {
        playerController = GetComponent<PlayerController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public bool CanProcessInput()
    {
        //Debug.Log(Cursor.lockState);
        return Cursor.lockState == CursorLockMode.Locked /*&& (JUEGO NO HA TERMINADO)*/;
    }

    public Vector3 GetMoveInput()
    {
        if (CanProcessInput())
        {
            Vector3 move = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));

            //Capa el movimiento a 1 como máximo para que no sobrepase el limite en los movimientos diagonales
            move = Vector3.ClampMagnitude(move, 1);

            return move;
        }

        return Vector3.zero;
    }

    public float GetLookInputsHorizontal()
    {
        return GetMouseOrStickLookAxis("Mouse X", "Look X");
    }

    public float GetLookInputsVertical()
    {
        return GetMouseOrStickLookAxis("Mouse Y", "Look Y");
    }

    public int GetMouseWheelDirection()
    {
        if (CanProcessInput())
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0) return 1;
            else if (Input.GetAxis("Mouse ScrollWheel") < 0) return -1;
        }

        return 0;
    }

    public bool GetJumpInputDown()
    {
        if (CanProcessInput())
        {
            return Input.GetButtonDown("Jump");
        }

        return false;
    }

    public bool GetJumpInputHeld()
    {
        if (CanProcessInput())
        {
            return Input.GetButton("Jump");
        }

        return false;
    }

    public bool GetSprintInputHeld()
    {
        if (CanProcessInput())
        {
            return Input.GetButton("Sprint");
        }

        return false;
    }

    float GetMouseOrStickLookAxis(string mouseInputName, string stickInputName)
    {
        if (CanProcessInput())
        {
            //Comprobar si el input viene del mouse
            bool isGamepad = Input.GetAxis(stickInputName) != 0f;
            float i = isGamepad ? Input.GetAxis(stickInputName) : Input.GetAxisRaw(mouseInputName);

            //Invierte en vertical
            if (invertYAxis)
                i *= -1f;

            //Multiplicador de sensibilidad
            i *= lookSensitivity;

            if (isGamepad)
            {
                //Si viene de un gamepad se aplica deltaTime (mouse ya usa el deltaTime)
                i *= Time.deltaTime;
            }
            else
            {
                // Reduce el input del mouse para que sea equivalente a la de un stick
                i *= 0.01f;
            }

            return i;
        }

        return 0f;
    }
}
