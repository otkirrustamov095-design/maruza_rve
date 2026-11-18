using UnityEngine;

// Mayatnik arqonini chizadi: osilish nuqtasidan sharga.
[RequireComponent(typeof(LineRenderer), typeof(HingeJoint))]
public class PendulumRope : MonoBehaviour
{
    LineRenderer lr;
    HingeJoint hj;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        hj = GetComponent<HingeJoint>();
    }

    void LateUpdate()
    {
        lr.SetPosition(0, hj.connectedAnchor);
        lr.SetPosition(1, transform.position);
    }
}
