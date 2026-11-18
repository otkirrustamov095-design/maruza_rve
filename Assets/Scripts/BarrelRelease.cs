using UnityEngine;

// Mashina yaqinlashganda bochka qo'yib yuboriladi va qiyalikdan dumalab tushadi
// (ilgarilanma + aylanma harakat birgalikda).
[RequireComponent(typeof(Rigidbody))]
public class BarrelRelease : MonoBehaviour
{
    public float triggerDistance = 30f;
    Rigidbody rb;
    Transform car;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        var c = FindObjectOfType<CarController>();
        if (c) car = c.transform;
    }

    void Update()
    {
        if (!rb.isKinematic || !car) return;
        if (Vector3.Distance(car.position, transform.position) < triggerDistance)
        {
            rb.isKinematic = false;
            rb.WakeUp();
        }
    }
}
