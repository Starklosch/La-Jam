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

    PlayerInput m_InputHandler;
    CharacterController m_Controller;
    Vector3 m_GroundNormal;
    Vector3 m_CharacterVelocity;
    float m_LastTimeJumped = 0f;
    float m_CameraVerticalAngle = 0f;
    float m_TargetCharacterHeight;

    const float k_JumpGroundingPreventionTime = 0.2f;
    const float k_GroundCheckDistanceInAir = 0.07f;

    void Start()
    {
        m_Controller = GetComponent<CharacterController>();

        m_InputHandler = GetComponent<PlayerInput>();

        m_Controller.enableOverlapRecovery = true;
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
        //Asegurarse de que la distancia para comprobar el suelo es suficientemente pequeña para evitar snapping al suelo
        float chosenGroundCheckDistance = IsGrounded ? (m_Controller.skinWidth + groundCheckDistance) : k_GroundCheckDistanceInAir;

        //Resetear valores antes de comprobar el suelo
        IsGrounded = false;
        m_GroundNormal = Vector3.up;

        //Solo se realiza la comprobacion si ha pasado cierto tiempo desde el ultimo salto, para evitar que se snapee al suelo nada mas saltar
        if (Time.time >= m_LastTimeJumped + k_JumpGroundingPreventionTime)
        {
            //Casteamos la capsula de comprobacion del suelo
            Vector3 capsuleBottomHalf = transform.position + (transform.up * m_Controller.radius);
            Vector3 capsuleTopHalf = transform.position + (transform.up * (m_Controller.height - m_Controller.radius));

            if (Physics.CapsuleCast(capsuleBottomHalf, capsuleTopHalf, m_Controller.radius, Vector3.down, out RaycastHit hit,
                chosenGroundCheckDistance, groundCheckLayers, QueryTriggerInteraction.Ignore))
            {
                m_GroundNormal = hit.normal;

                //Si la normal del suelo es la misma que el vector hacia arriba del jugador, se considera un golpe con el suelo
                //Y si el angulo de pendiente de ese suelo es menor que el del character controller
                if (Vector3.Dot(hit.normal, transform.up) > 0f && (Vector3.Angle(transform.up, m_GroundNormal) <= m_Controller.slopeLimit))
                {
                    IsGrounded = true;

                    //Gestion de snapping con el suelo
                    if (hit.distance > m_Controller.skinWidth)
                    {
                        m_Controller.Move(Vector3.down * hit.distance);
                    }
                }
            }
        }
    }

    void HandleCharacterMovement()
    {

    }

}
