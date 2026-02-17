using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class GroundProximity : MonoBehaviour
{
    [Header("Ground Check")]
    public LayerMask groundLayer;
    public float castExtraDistance = 0.25f;   // Distancia extra por debajo del collider
    public float groundedDistance = 0.08f;    // <= esto: grounded
    public float nearGroundMin = 0.12f;       // > groundedDistance y >= esto: nearGround
    public QueryTriggerInteraction triggerInteraction = QueryTriggerInteraction.Ignore;

    [Header("Animator Params")]
    public Animator animator;
    public string groundedBool = "groundedBool";
    public string nearGroundBool = "nearGroundBool";
    public string groundDistFloat = "groundDistFloat";

    [Header("Debug")]
    public bool debugDraw = false;

    private Rigidbody rb;
    private CapsuleCollider capsule;

    private bool grounded;
    private bool nearGround;
    private float distance;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();

        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    void FixedUpdate()
    {
        EvaluateGround(out grounded, out nearGround, out distance);

        if (animator != null)
        {
            animator.SetBool(groundedBool, grounded);
            animator.SetBool(nearGroundBool, nearGround);
            animator.SetFloat(groundDistFloat, distance); // nunca Infinity
        }
    }

    public void EvaluateGround(out bool grounded, out bool nearGround, out float distance)
    {
        grounded = false;
        nearGround = false;

        // Valor finito por defecto
        distance = castExtraDistance + 1f;

        Bounds b = capsule.bounds;

        // Origen: ligeramente por encima de la base del capsule
        float upOffset = Mathf.Max(0.02f, capsule.radius * 0.25f);
        Vector3 origin = new Vector3(
            b.center.x,
            b.min.y + upOffset,
            b.center.z
        );

        // Radio ligeramente menor que el collider real (evita detectar paredes)
        float radius = Mathf.Max(0.01f, capsule.radius * 0.95f);

        float maxDistance = castExtraDistance + groundedDistance + 0.2f;

        bool hit = Physics.SphereCast(
            origin,
            radius,
            Vector3.down,
            out RaycastHit h,
            maxDistance,
            groundLayer,
            triggerInteraction
        );

        if (hit)
        {
            distance = h.distance;

            grounded = distance <= groundedDistance;
            nearGround = !grounded && distance >= nearGroundMin;
        }

        if (debugDraw)
        {
            Debug.DrawLine(origin, origin + Vector3.down * maxDistance,
                hit ? Color.green : Color.red);

            if (hit)
                Debug.DrawRay(h.point, Vector3.up * 0.2f, Color.cyan);
        }
    }

    // Propiedades públicas opcionales si PlayerMovement quiere consultarlo
    public bool IsGrounded => grounded;
    public bool IsNearGround => nearGround;
    public float GroundDistance => distance;
}
