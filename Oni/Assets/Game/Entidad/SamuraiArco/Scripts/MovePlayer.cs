using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public Controls controls;

    public Vector2 direccion;

    public Rigidbody2D rb2D;
    public Animator anim;
    public float velocidadMovimiento;

    public bool mirandoDerecha = true;
    public bool attack;

    public int saltosMaximos = 2;
    private int saltosRestantes;

    public float fuerzaSalto;
    public LayerMask groundlayer;
    public Transform groundCheck;
    public float groundCheckRadius;
    public bool inGround;

    [SerializeField] private DisparoJugador disparoJugador;

    private void Awake()
    {
        controls = new Controls();
    }

    private void OnEnable()
    {
        controls.Enable();
        controls.Moviment.Jump.started += OnJumpPerformed;
    }

    private void OnDisable()
    {
        controls.Moviment.Jump.started -= OnJumpPerformed;
        controls.Disable();
    }

    private void Update()
    {
        direccion = controls.Moviment.Move.ReadValue<Vector2>();

        AjustarRotacion(direccion.x);

        inGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundlayer);

        if (inGround)
        {
            saltosRestantes = saltosMaximos;
        }

        if (controls.Moviment.Attack.triggered && !attack)
        {
            Atacando();
        }

        HandleAnimations();
    }

    private void HandleAnimations()
    {
        anim.SetBool("isIdle", Mathf.Abs(direccion.x) < .1f && inGround && !attack);
        anim.SetBool("isRuning", Mathf.Abs(direccion.x) > .1f && inGround && !attack);
        anim.SetBool("isJumping", !inGround && !attack);
        anim.SetBool("isAttack", attack);
    }

    private void FixedUpdate()
    {
        rb2D.linearVelocity = new Vector2(direccion.x * velocidadMovimiento, rb2D.linearVelocity.y);
    }

    private void AjustarRotacion(float direccionX)
    {
        if (direccionX > 0 && !mirandoDerecha)
        {
            Girar();
        }
        else if (direccionX < 0 && mirandoDerecha)
        {
            Girar();
        }
    }

    private void Girar()
    {
        mirandoDerecha = !mirandoDerecha;
        transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y + 180f, 0f);
    }

    private void OnJumpPerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Saltar();
    }

    private void Saltar()
    {
        if (saltosRestantes > 0)
        {
            rb2D.linearVelocity = new Vector2(rb2D.linearVelocity.x, 0);
            rb2D.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
            saltosRestantes--;
        }
    }

    private void Atacando()
    {
        attack = true;

        if (disparoJugador != null)
        {
            disparoJugador.Disparar();
        }

        Invoke(nameof(DesactivaAtaque), 0.4f);

        Debug.Log("Se ejecutó Atacando");
    }

    private void DesactivaAtaque()
    {
        attack = false;
        Debug.Log("Ataque terminado");
    }

    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}