using UnityEngine;
using UnityEngine.InputSystem;

public class MoverJumper : MonoBehaviour
{
    [Header("Movimiento y salto")]
    [SerializeField] private float speed = 7f;
    [SerializeField] private float jumpForce = 6.5f;
    
    [Header("Disparo y casteo")]
    [SerializeField] private GameObject FireBall;
    [SerializeField] private Transform fireStartPos;
    [SerializeField] private LayerMask clickableLayers;
    [SerializeField] private float castTime = 0.5f;
    [SerializeField] private GameObject chargeEffect; 
    private Rigidbody rb;
    private CapsuleCollider bodyCollider;
    private int groundLayerMask;
    private Vector2 move;
    private bool isGrounded;
    private float lastJumpTime = -1f;
    private float maxAirborneYVelocity;
    private const float JumpCooldown = 0.15f;
    private bool isAttack;
    private bool hasFiredThisClick;
    private float castTimer;
    private Vector2 mousePos;
    private Vector3 lookAtPos;
    private Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        groundLayerMask = LayerMask.GetMask("Ground");
        foreach (CapsuleCollider col in GetComponents<CapsuleCollider>())
        {
            if (!col.isTrigger)
            {
                bodyCollider = col;
                break;
            }
        }
    }
    void Start()
    {
        lookAtPos = transform.position + transform.forward;
    }

    public void OnMove(InputValue value)
    {
        move = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed && isGrounded && Time.time - lastJumpTime >= JumpCooldown)
        {
            float jumpVy = jumpForce / Mathf.Max(0.0001f, rb.mass);
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpVy, rb.linearVelocity.z);
            isGrounded = false;
            lastJumpTime = Time.time;
            maxAirborneYVelocity = jumpVy;
        }
    }

    public void OnAttack(InputValue value)
    {
        isAttack = value.isPressed;
        if (isAttack)
        {
            castTimer = 0f;
            hasFiredThisClick = false;
        }
        else
        {
            // Si suelta el clic, cancela el casteo
            castTimer = 0f;
            hasFiredThisClick = false;
            if (chargeEffect != null) chargeEffect.SetActive(false);
        }
    }

    public void OnMouseMove(InputValue value)
    {
        mousePos = value.Get<Vector2>();
        UpdateMousePos();
    }

    private void UpdateMousePos()
    {
        if (Camera.main == null) return;

        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, clickableLayers))
        {
            lookAtPos = new Vector3(hit.point.x, transform.position.y, hit.point.z);
        }
    }

    private void Fire()
    {
        FireBallMover newProjectile = Instantiate(FireBall, fireStartPos.position, transform.rotation)
            .GetComponent<FireBallMover>();
        newProjectile.transform.LookAt(new Vector3(lookAtPos.x, fireStartPos.position.y, lookAtPos.z));
        if (anim != null) anim.SetTrigger("Attack");
    }

    private void UpdateGrounded()
    {
        if (Time.time - lastJumpTime < JumpCooldown || rb.linearVelocity.y > 0.5f)
        {
            isGrounded = false;
            return;
        }

        Vector3 center = bodyCollider != null ? bodyCollider.bounds.center : transform.position;
        float bottomY = bodyCollider != null ? bodyCollider.bounds.min.y : transform.position.y - 1f;
        float castRadius = bodyCollider != null ? Mathf.Max(0.1f, bodyCollider.radius * 0.8f) : 0.35f;

        Vector3 origin = new Vector3(center.x, bottomY + castRadius + 0.05f, center.z);
        RaycastHit[] hits = Physics.SphereCastAll(
            origin,
            castRadius,
            Vector3.down,
            0.12f,
            groundLayerMask,
            QueryTriggerInteraction.Ignore
        );

        isGrounded = false;
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].normal.y > 0.65f)
            {
                isGrounded = true;
                break;
            }
        }
    }

    void FixedUpdate()
    {
        UpdateGrounded();

        float vy = rb.linearVelocity.y;
        if (!isGrounded)
        {
            float allowedMaxY = Mathf.Max(0f, maxAirborneYVelocity);
            if (vy > allowedMaxY)
            {
                vy = allowedMaxY;
            }
            maxAirborneYVelocity = Mathf.Min(maxAirborneYVelocity, vy);
        }
        else
        {
            maxAirborneYVelocity = 0f;
        }

        rb.linearVelocity = new Vector3(move.x * speed, vy, move.y * speed);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        if (fireStartPos != null)
        {
            Gizmos.DrawLine(fireStartPos.position, lookAtPos);
        }
        Gizmos.DrawWireSphere(lookAtPos, 0.15f);
    }
    // Update is called once per frame
    void Update()
    {
        UpdateMousePos();
        Vector3 flatLookAt = new Vector3(lookAtPos.x, transform.position.y, lookAtPos.z);
        if ((flatLookAt - transform.position).sqrMagnitude > 0.001f)
        {
            transform.LookAt(flatLookAt);
        }
        if (isAttack && !hasFiredThisClick)
        {
            if (chargeEffect != null && !chargeEffect.activeSelf)
            {
                chargeEffect.SetActive(true);
            }
            castTimer += Time.deltaTime;
            if (castTimer >= castTime)
            {
                Fire();
                hasFiredThisClick = true; // Solo una bola por pulsación
                if (chargeEffect != null) chargeEffect.SetActive(false);
            }
        }
        if (anim != null)
{
    anim.SetBool("IsMoving", move.sqrMagnitude > 0.01f);
    anim.SetBool("IsGrounded", isGrounded);
    anim.SetBool("IsCasting", isAttack && !hasFiredThisClick);
}
    }

   

}
