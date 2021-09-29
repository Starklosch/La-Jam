using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    protected int health;
    protected Slider healthSlider;
    protected bool canAnim = false;

    protected int animAttackTrigger;
    protected int animMove;

    float currentStunDuration;
    bool hasPoison;
    bool hasStun;

    [Tooltip("Vida máxima del enemigo.")]
    public int maxHealth = 10;

    public Animator anim;
    public GameManager manager;

    public float attackTime;
    public float damage;

    public float chaseDistance;
    public float attackDistance;

    public bool hasMoveAnimation = false;

    protected float attackCooldown;
    protected bool canAttack = true;
    protected bool isMoving = false;
    protected bool m_canMove = true;

    protected PlayerController player;

    protected bool IsStopped
    {
        set
        {
            //Debug.Log("IsStopd: " + value);
            //m_canMove = value;
            nav.isStopped = value;
        }
        get => nav.isStopped;
    }

    protected bool MoveAnim
    {
        get => anim.GetBool(animMove);
        set
        {
            if (canAnim && hasMoveAnimation)
                anim.SetBool(animMove, value);
        }
    }

    //protected bool useDefaultLogic = true;

    protected NavMeshAgent nav;

    public LayerMask playerMask;

    public virtual void Start()
    {
        if (anim != null || TryGetComponent(out anim) || (anim = transform.GetComponentInChildren<Animator>()) != null)
        {
            canAnim = true;

            animAttackTrigger = Animator.StringToHash("Attack");
            animMove = Animator.StringToHash("Move");
        }
        if (manager == null)
        {
            manager = GameManager.Instance;
        }

        if (damage <= 0 || attackDistance <= 0 || chaseDistance <= 0)
            Debug.LogWarning("Cuidado, hay variables sin definir");

        nav = GetComponent<NavMeshAgent>();

        player = manager.PlayerInstance;

        healthSlider = transform.GetComponentInChildren<Slider>();
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
        health = maxHealth;
        //healthSlider = transform.Find("CanvasEnemy").Find("HealthBar").GetComponent<Slider>();
    }

    public virtual void Update()
    {
        if (hasStun)
        {
            currentStunDuration -= Time.deltaTime;
            if (currentStunDuration <= 0)
            {
                hasStun = false;
                Debug.Log("FREE OF STUN");
            }
        }

        if (player == null && manager.PlayerInstance != null)
            player = manager.PlayerInstance;

        if (player == null)
            return;

        var pDistance = Vector3.Distance(player.transform.position, transform.position);

        if (!IsStopped && pDistance < chaseDistance)
        {
            nav.SetDestination(player.transform.position);
            MoveAnim = true;
        }

        if (pDistance < attackDistance)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * nav.speed);
            IsStopped = true;
            MoveAnim = false;
            if (canAttack && Time.time > attackCooldown)
                Attack();
        }
        else if (canAttack)
        {
            IsStopped = false;
        }
    }

    public virtual void Attack()
    {
        //Debug.Log("Attack");
        canAttack = false;
        //CanMove = false;
    }

    //protected virtual void AttackEnd()
    //{
    //    Debug.Log("Attack end");
    //    canAttack = true;
    //    IsStopped = true;

    //    attackCooldown = Time.deltaTime + attackTime;
    //}

    public virtual void Heal(int h)
    {
        health += h;

        if (health > healthSlider.maxValue) health = (int)healthSlider.maxValue;

        UpdateHealthBarHUD();
    }

    public virtual bool TakeDamage(int d)
    {
        health -= d;

        UpdateHealthBarHUD();

        if (health <= 0) return true;

        return false;
    }

    public virtual void Die()
    {
        Destroy(this.gameObject);
    }

    void UpdateHealthBarHUD()
    {
        healthSlider.value = health;
    }
    void PoisonDamage()
    {
        TakeDamage(1);
    }

    void StopPoison()
    {
        CancelInvoke("PoisonDamage");
        hasPoison = false;
    }

    public void Poison(float duration)
    {
        StopPoison();

        Debug.Log("POISONED");
        hasPoison = true;
        InvokeRepeating("PoisonDamage", 0f, 1f);
        Invoke("StopPoison", duration);
    }

    public void Stun(float duration)
    {
        hasStun = true;
        currentStunDuration = duration;
        Debug.Log("STUNNED");
    }

    // Animations

    protected virtual void AttackAnim()
    {
        if (canAnim)
            anim.SetTrigger(animAttackTrigger);
    }

}
