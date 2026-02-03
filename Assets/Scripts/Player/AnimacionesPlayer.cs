using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimacionesPlayer : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody rb;

    /*
     * En el componente Animator, desmarca:
        ✔ Apply Root Motion
        Eso permitirá que:
        El Rigidbody controle el movimiento
        La velocidad real aparezca en rb.linearVelocity
     */


    void Awake()
    {
        if (!animator) animator = GetComponent<Animator>();
        if (!rb) rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 vWorld = rb.linearVelocity;
        Vector3 vLocal = transform.InverseTransformDirection(vWorld);

        // vLocal.x = derecha/izquierda (strafe)
        // vLocal.z = adelante/atrás
        animator.SetFloat("X", vLocal.x);
        animator.SetFloat("Y", vLocal.z);
        animator.SetFloat("VelVertical", vLocal.y);
    }

    public void AnimacionSaltar01()
    {
        animator.SetTrigger("Saltar");
    }

    public void AnimacionSaltar02()
    {
        animator.SetTrigger("Saltar2");
    }

    public void Ensuelo(bool ensuelo)
    {
        animator.SetBool("EnSuelo", ensuelo );
    }
}