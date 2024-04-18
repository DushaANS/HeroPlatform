using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hero : Entity
{
    [SerializeField] private float speed = 3f; //скорость
    [SerializeField] private int health; //кол-во жизней
    [SerializeField] private float jump = 5f; //прыжок
    private bool isGrounted= false;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sp;

    [SerializeField] private Image[] hearts;

    [SerializeField] private Sprite aliveHeart;
    [SerializeField] private Sprite deadHeart;

    public bool isAttacking = false;
    public bool isReacherged = false;

    public Transform attackPos;
    public float attackRange;
    public LayerMask enemy;

    public static Hero Instance { get; set; }

    private States states
    {
        get { return (States)animator.GetInteger("state"); }
        set { animator.SetInteger("state", (int)value); }
    }

    private void Awake()
    {
        lives = 5;
        health = lives;
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        sp = rb.GetComponentInChildren<SpriteRenderer>();
        animator = GetComponent<Animator>();
        isReacherged = true;
    }

    private void Attack()
    {
        if(isGrounted)
        {
            states = States.attack;
            isAttacking = true;
            isReacherged = false;

            StartCoroutine(AttackAnimation());
            StartCoroutine(AttackCoolDown());
        }
    }

    private IEnumerator AttackCoolDown()
    {
        yield return new WaitForSeconds(0.5f);
        isReacherged = true;
    }

    private IEnumerator AttackAnimation()
    {
        yield return new WaitForSeconds(0.4f);
        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

    private void onAttack()
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(attackPos.position, attackRange, enemy);
        for(int i = 0; i < collider.Length; i++)
        {
            collider[i].GetComponent<Entity>().GetDamage();
        }
    }

    private void Run()
    {
        if (isGrounted) states = States.run;
        Vector3 dir = transform.right * Input.GetAxis("Horizontal");
        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);
        sp.flipX = dir.x < 0.0f;
    }

    private void Jump()
    {
        rb.AddForce(transform.up * jump, ForceMode2D.Impulse);
    }

    private void CheckGround()
    {
        Collider2D[] colider = Physics2D.OverlapCircleAll(transform.position, 0.3f);
        isGrounted = colider.Length > 1;
        if (!isGrounted) states = States.jump;
    }

    private void FixedUpdate()
    {
        CheckGround();
    }

    public override void GetDamage()
    {
        lives -= 1;
        if (lives== 0)
        {
            foreach (var h in hearts)
                h.sprite = deadHeart;
            Die();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isGrounted) states = States.idel;
        if (Input.GetButton("Horizontal")) Run();
        if (isGrounted && Input.GetButtonDown("Jump")) Jump();
        if (Input.GetButtonDown("Fire1")) Attack();

        if (health < lives)
            health = lives;
        for (int i = 0; i< hearts.Length; i++)
        {
            if (i < health)
                hearts[i].sprite = aliveHeart;
            else
                hearts[i].sprite = deadHeart;
            if (i < lives)
                hearts[i].enabled = true;
            else
                hearts[i].enabled = false;
        }
    }
}

public enum States
{
    idel,
    run,
    jump,
    attack
}
