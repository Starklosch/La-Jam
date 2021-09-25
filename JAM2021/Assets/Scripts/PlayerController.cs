using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Referencias")]
    [Tooltip("Referencia a la camara del jugador")]
    public Camera playerCamera;

    [Header("General")]
    [Tooltip("Fuerza de gravedad aplicada al jugador")]
    public float gravityDownForce = 20f;

    [Tooltip("Capas fisicas consideradas en la comprobacion de tocar el suelo")]
    public LayerMask groundCheckLayers = -1;

    [Tooltip("Distancia desde los pies de la capsula para comprobar el suelo")]
    public float groundCheckDistance = 0.05f;

    [Header("Movimiento")]
    [Tooltip("Maxima speed en el suelo")]
    public float maxSpeedOnGround = 10f;

    [Tooltip("Friccion en el suelo")]
    public float movementSharpnessOnGround = 15;

    [Tooltip("Maxima speed en el aire")]
    public float maxSpeedInAir = 10f;

    [Tooltip("Aceleracion en el aire")]
    public float accelerationSpeedInAir = 25f;

    [Tooltip("Multiplicador de sprint")]
    public float sprintSpeedModifier = 2f;

    [Header("Rotacion")]
    [Tooltip("Rotacion de la camara")]
    public float rotationSpeed = 200f;

    [Header("Salto")]
    [Tooltip("Fuerza de salto")]
    public float jumpForce = 9f;

    [Header("Capsula")]
    [Tooltip("Altura de la camara en proporcion (0-1) a la altura del jugador")]
    public float cameraHeightRatio = 0.9f;

    [Tooltip("Altura del jugador")]
    public float capsuleHeightStanding = 1.8f;

    public Vector3 CharacterVelocity { get; set; }
    public bool IsGrounded { get; private set; }
    public bool HasJumpedThisFrame { get; private set; }
    public bool IsDead { get; private set; }
    public bool IsCrouching { get; private set; }

    PlayerInput inputHandler;
    CharacterController controller;
    Vector3 groundNormal;
    float lastTimeJumped = 0f;
    float cameraVerticalAngle = 0f;

    const float jumpGroundingPreventionTime = 0.2f;
    const float groundCheckDistanceInAir = 0.07f;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        inputHandler = GetComponent<PlayerInput>();

        controller.enableOverlapRecovery = true;
    }

    void Update()
    {
        HasJumpedThisFrame = false;

        bool wasGrounded = IsGrounded;
        GroundCheck();

        //Aterrizar
        if (IsGrounded && !wasGrounded)
        {
            //AudioSource.PlayOneShot(LandSfx);
        }

        HandleCharacterMovement();
    }

    void GroundCheck()
    {
        //Asegurarse de que la distancia para comprobar el suelo es suficientemente peque�a para evitar snapping al suelo
        float chosenGroundCheckDistance = IsGrounded ? (controller.skinWidth + groundCheckDistance) : groundCheckDistanceInAir;

        //Resetear valores antes de comprobar el suelo
        IsGrounded = false;
        groundNormal = Vector3.up;

        //Solo se realiza la comprobacion si ha pasado cierto tiempo desde el ultimo salto, para evitar que se snapee al suelo nada mas saltar
        if (Time.time >= lastTimeJumped + jumpGroundingPreventionTime)
        {
            //Casteamos la capsula de comprobacion del suelo
            Vector3 capsuleBottomHalf = transform.position + (transform.up * controller.radius);
            Vector3 capsuleTopHalf = transform.position + (transform.up * (controller.height - controller.radius));

            if (Physics.CapsuleCast(capsuleBottomHalf, capsuleTopHalf, controller.radius, Vector3.down, out RaycastHit hit,
                chosenGroundCheckDistance, groundCheckLayers, QueryTriggerInteraction.Ignore))
            {
                groundNormal = hit.normal;

                //Si la normal del suelo es la misma que el vector hacia arriba del jugador, se considera un golpe con el suelo
                //Y si el angulo de pendiente de ese suelo es menor que el del character controller
                if (Vector3.Dot(hit.normal, transform.up) > 0f && (Vector3.Angle(transform.up, groundNormal) <= controller.slopeLimit))
                {
                    IsGrounded = true;

                    //Gestion de snapping con el suelo
                    if (hit.distance > controller.skinWidth)
                    {
                        controller.Move(Vector3.down * hit.distance);
                    }
                }
            }
        }
    }

    public Vector3 GetDirectionReorientedOnSlope(Vector3 direction, Vector3 slopeNormal)
    {
        Vector3 directionRight = Vector3.Cross(direction, transform.up);
        return Vector3.Cross(slopeNormal, directionRight).normalized;
    }

    void HandleCharacterMovement()
    {
        //Rotacion horizontal
        {
            //Rotar el transform con el input en su eje Y
            transform.Rotate(new Vector3(0f, (inputHandler.GetLookInputsHorizontal() * rotationSpeed), 0f), Space.Self);
        }

        //Rotacion vertical
        {
            //Suma el input a la camara
            cameraVerticalAngle += inputHandler.GetLookInputsVertical() * rotationSpeed;

            // limita el movimiento vertical de la camara
            cameraVerticalAngle = Mathf.Clamp(cameraVerticalAngle, -89f, 89f);

            //Aplica el angulo vertical de forma local a la camara para que pivotee
            playerCamera.transform.localEulerAngles = new Vector3(cameraVerticalAngle, 0, 0);
        }

        //Movimiento del player
        bool isSprinting = inputHandler.GetSprintInputHeld();
        {

            float speedModifier = isSprinting ? sprintSpeedModifier : 1f;

            //Convierte el input de movimiento a worldspace basado en la orientacion del player transform
            Vector3 worldspaceMoveInput = transform.TransformVector(inputHandler.GetMoveInput());
            //Debug.Log(IsGrounded);
            //Gestion de tocar el suelo
            if (IsGrounded)
            {
                //Calcular velocidad deseada basandose en inputs, velocidad maxima y slope
                Vector3 targetVelocity = worldspaceMoveInput * maxSpeedOnGround * speedModifier;

                targetVelocity = GetDirectionReorientedOnSlope(targetVelocity.normalized, groundNormal) * targetVelocity.magnitude;

                //Interpolacion entre la velocidad actual y la deseada, basandose en la aceleracion
                CharacterVelocity = Vector3.Lerp(CharacterVelocity, targetVelocity, movementSharpnessOnGround * Time.deltaTime);

                //Salto
                if (IsGrounded && inputHandler.GetJumpInputDown())
                {
                    //Reseteo de velocidad en eje Y
                    CharacterVelocity = new Vector3(CharacterVelocity.x, 0f, CharacterVelocity.z);

                    //Se a�ade la fuerza vertical de salto
                    CharacterVelocity += Vector3.up * jumpForce;

                    // play sound
                    //AudioSource.PlayOneShot(JumpSfx);

                    //Se usara luego para prevenir snapping
                    lastTimeJumped = Time.time;
                    HasJumpedThisFrame = true;

                    //Reseteo de valores
                    IsGrounded = false;
                    groundNormal = Vector3.up;
                }

            }
            //Movimiento en el aire
            else
            {
                //Sumar la acceleracion en el aire
                CharacterVelocity += worldspaceMoveInput * accelerationSpeedInAir * Time.deltaTime;

                //Limitar la velocidad en el aire
                float verticalVelocity = CharacterVelocity.y;
                Vector3 horizontalVelocity = Vector3.ProjectOnPlane(CharacterVelocity, Vector3.up);
                horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, maxSpeedInAir * speedModifier);
                CharacterVelocity = horizontalVelocity + (Vector3.up * verticalVelocity);

                //Aplicar la gravedad
                CharacterVelocity += Vector3.down * gravityDownForce * Time.deltaTime;
            }
        }

        //Aplicar la velocidad final como al CharacterController
        Vector3 capsuleBottomBeforeMove = transform.position + (transform.up * controller.radius);
        Vector3 capsuleTopBeforeMove = transform.position + (transform.up * (controller.height - controller.radius));
        controller.Move(CharacterVelocity * Time.deltaTime);

        //Detectar obstrucciones para ajustar la velocidad acordemente
        if (Physics.CapsuleCast(capsuleBottomBeforeMove, capsuleTopBeforeMove, controller.radius, CharacterVelocity.normalized, 
            out RaycastHit hit, CharacterVelocity.magnitude * Time.deltaTime, -1, QueryTriggerInteraction.Ignore))
        {
            CharacterVelocity = Vector3.ProjectOnPlane(CharacterVelocity, hit.normal);
        }
    }
}