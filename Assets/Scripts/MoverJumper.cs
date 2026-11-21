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
    private Vector2 move;
    private bool isGrounded;
    private bool isAttack;
    private bool hasFiredThisClick;
    private float castTimer;
    private Vector2 mousePos;
    private Vector3 lookAtPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
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
        if (value.isPressed && isGrounded)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = false;
        }
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
        rb.linearVelocity = new Vector3(move.x * speed, rb.linearVelocity.y, move.y * speed);
        UpdateMousePos();
        Vector3 flatLookAt = new Vector3(lookAtPos.x, transform.position.y, lookAtPos.z);
        if ((flatLookAt - transform.position).sqrMagnitude > 0.001f)
        {
            transform.LookAt(flatLookAt);
        }
        transform.LookAt(lookAtPos);
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
    }

   

}
