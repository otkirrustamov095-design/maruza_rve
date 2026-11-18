using UnityEngine;

// Har bir to'qnashuvda impuls (J = F*dt = dp) va nisbiy tezlikni qayd qiladi,
// urilgan Rigidbody qisqa vaqt qizil rangga bo'yaladi.
public class CollisionReporter : MonoBehaviour
{
    public static string LastInfo = "Hali to'qnashuv yo'q";
    public static int Count;

    void OnCollisionEnter(Collision c)
    {
        if (c.collider.name == "Ground") return;
        float impulse = c.impulse.magnitude;
        if (impulse < 50f) return;

        Count++;
        LastInfo = $"{c.collider.name}: J = {impulse:F0} N*s, v_nisbiy = {c.relativeVelocity.magnitude:F1} m/s";
        Debug.Log("[To'qnashuv] " + LastInfo);

        var r = c.collider.GetComponent<Renderer>();
        if (r && c.rigidbody) StartCoroutine(Flash(r));
    }

    System.Collections.IEnumerator Flash(Renderer r)
    {
        var mat = r.material;
        Color orig = mat.color;
        mat.color = Color.red;
        yield return new WaitForSeconds(0.25f);
        if (r) mat.color = orig;
    }
}
