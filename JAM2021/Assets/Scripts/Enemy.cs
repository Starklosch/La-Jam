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

    public Animator anim;
    public GameManager manager;

    public float attackTime;
    public float damage;

    public float chaseDistance;
    public float attackDistance;

    public bool hasMoveAnimation = false;

    protected float attackCooldown;
    protected bool canAttack = true;
    protected bool canMove = true;
    protected PlayerController player;

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

        nav = GetComponent<NavMeshAgent>();

        player = manager.PlayerInstance;

        healthSlider = transform.GetComponentInChildren<Slider>();
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

        //if (useDefaultLogic)
        //{
        //    if (player != null)
        //    {
        //        var pDistance = Vector3.Distance(player.transform.position, transform.position);
        //        if (pDistance < attackDistance)
        //        {
        //            if (canAttack && Time.time > attackCooldown)
        //                Attack();
        //        }
        //        else if (pDistance < chaseDistance)
        //        {
        //            Chase();
        //        }
        //    }
        //}
    }

    public virtual void Chase()
    {
        canAttack = false;

        MoveAnim = true;
    }

    public virtual void Attack()
    {
        canAttack = false;
        canMove = false;
    }

    protected virtual void AttackEnd()
    {
        canAttack = true;
        canMove = true;

        attackCooldown = Time.deltaTime + attackTime;
    }

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
