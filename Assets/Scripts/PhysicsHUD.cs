using UnityEngine;

// Ekranda fizik kattaliklar: tezlik, impuls, kinetik energiya (ilgarilanma va aylanma).
// H tugmasi yoki panel tepasidagi tugma bilan yashiriladi / ko'rsatiladi.
public class PhysicsHUD : MonoBehaviour
{
    public CarController car;
    public bool visible = true;
    public KeyCode toggleKey = KeyCode.H;

    GUIStyle style, title, btn;

    void Update()
    {
        if (Input.GetKeyDown(toggleKey)) visible = !visible;
    }

    void OnGUI()
    {
        if (!car || car.Body == null) return;
        if (style == null)
        {
            style = new GUIStyle(GUI.skin.label) { fontSize = 12 };
            style.normal.textColor = Color.white;
            title = new GUIStyle(style) { fontSize = 13, fontStyle = FontStyle.Bold };
            title.normal.textColor = Color.yellow;
            btn = new GUIStyle(GUI.skin.button) { fontSize = 11 };
        }

        if (!visible)
        {
            if (GUI.Button(new Rect(10, 10, 130, 22), "Fizika paneli (H)", btn)) visible = true;
            GUI.Label(new Rect(10, Screen.height - 24, 400, 20), "W/S gaz  A/D burilish  Space tormoz  R qayta  H panel", style);
            return;
        }

        Rigidbody rb = car.Body;
        float m = rb.mass;
        Vector3 v = rb.velocity;
        Vector3 w = rb.angularVelocity;
        Vector3 I = rb.inertiaTensor;
        Vector3 wl = Quaternion.Inverse(rb.rotation * rb.inertiaTensorRotation) * w;
        float eRot = 0.5f * (I.x * wl.x * wl.x + I.y * wl.y * wl.y + I.z * wl.z * wl.z);
        float eTr = 0.5f * m * v.sqrMagnitude;

        const float W = 270f, lh = 16f;
        GUI.Box(new Rect(10, 10, W, 196), "");
        if (GUI.Button(new Rect(10 + W - 26, 13, 22, 18), "x", btn)) visible = false;

        float x = 16, y = 13;
        GUI.Label(new Rect(x, y, W, 20), "QATTIQ JISM DINAMIKASI (MI21)", title); y += 20;
        GUI.Label(new Rect(x, y, W, 20), $"m = {m:F0} kg", style); y += lh;
        GUI.Label(new Rect(x, y, W, 20), $"|v| = {v.magnitude:F1} m/s  ({v.magnitude * 3.6f:F0} km/soat)", style); y += lh;
        GUI.Label(new Rect(x, y, W, 20), $"p = m·v = {m * v.magnitude:F0} kg·m/s", style); y += lh;
        GUI.Label(new Rect(x, y, W, 20), $"|ω| = {w.magnitude:F2} rad/s", style); y += lh;
        GUI.Label(new Rect(x, y, W, 20), $"E_ilg = mv²/2 = {eTr / 1000f:F1} kJ", style); y += lh;
        GUI.Label(new Rect(x, y, W, 20), $"E_ayl = Iω²/2 = {eRot / 1000f:F2} kJ", style); y += lh;
        GUI.Label(new Rect(x, y, W, 20), car.Grounded ? "Holat: yerda" : "Holat: HAVODA (inersiya)", style); y += lh;
        GUI.Label(new Rect(x, y, W, 20), $"To'qnashuvlar: {CollisionReporter.Count}", style); y += lh;
        GUI.Label(new Rect(x, y, W - 10, 34), CollisionReporter.LastInfo, style);

        GUI.Label(new Rect(10, Screen.height - 24, 400, 20), "W/S gaz  A/D burilish  Space tormoz  R qayta  H panel", style);
    }
}
