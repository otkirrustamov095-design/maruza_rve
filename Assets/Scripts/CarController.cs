using UnityEngine;

// Mashina: Rigidbody ga KUCH (ilgarilanma harakat) va MOMENT (aylanma harakat) beriladi.
[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    [Header("Ilgarilanma harakat (F = m*a)")]
    public float motorForce = 9000f;      // N
    public float maxSpeed = 25f;          // m/s
    public float brakeForce = 12000f;

    [Header("Aylanma harakat (M = I*e)")]
    public float steerTorque = 6000f;     // N*m
    public float sideGrip = 6f;           // yon sirpanishni kamaytirish

    [Header("Vizual g'ildiraklar")]
    public Transform[] wheels;
    public float wheelRadius = 0.4f;

    Rigidbody rb;
    Vector3 startPos;
    Quaternion startRot;
    float wheelAngle;

    public bool Grounded { get; private set; }
    public Rigidbody Body => rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0f, -0.4f, 0f); // past og'irlik markazi -> barqarorlik
        startPos = transform.position;
        startRot = transform.rotation;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) ResetCar();

        // G'ildiraklar aylanishi: omega = v / r
        float forwardSpeed = Vector3.Dot(rb.velocity, transform.forward);
        wheelAngle += forwardSpeed / wheelRadius * Mathf.Rad2Deg * Time.deltaTime;
        if (wheels != null)
            foreach (var w in wheels)
                if (w) w.localRotation = Quaternion.Euler(wheelAngle, 0f, 90f);
    }

    void FixedUpdate()
    {
        Grounded = Physics.Raycast(transform.position + Vector3.up * 0.2f, -transform.up, 1.0f, ~0, QueryTriggerInteraction.Ignore);

        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");
        bool brake = Input.GetKey(KeyCode.Space);

        if (!Grounded) return; // havoda boshqarib bo'lmaydi: faqat inersiya va gravitatsiya

        float forwardSpeed = Vector3.Dot(rb.velocity, transform.forward);

        // 1) Ilgarilanma harakat: tortish kuchi
        if (Mathf.Abs(forwardSpeed) < maxSpeed || Mathf.Sign(v) != Mathf.Sign(forwardSpeed))
            rb.AddForce(transform.forward * v * motorForce, ForceMode.Force);

        // Tormoz: harakatga qarama-qarshi kuch
        if (brake)
            rb.AddForce(-rb.velocity.normalized * brakeForce * Mathf.Clamp01(rb.velocity.magnitude), ForceMode.Force);

        // 2) Aylanma harakat: vertikal o'q atrofida burovchi moment
        float steerFactor = Mathf.Clamp(forwardSpeed / 5f, -1f, 1f); // joyida burilmaydi
        rb.AddTorque(transform.up * h * steerTorque * steerFactor, ForceMode.Force);

        // Yon ishqalanish (shina ilashishi): yon tezlikni so'ndirish
        Vector3 sideVel = Vector3.Project(rb.velocity, transform.right);
        rb.AddForce(-sideVel * sideGrip * rb.mass, ForceMode.Force);
    }

    public void ResetCar()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.SetPositionAndRotation(startPos, startRot);
    }
}
