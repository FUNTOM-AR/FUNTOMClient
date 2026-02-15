using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HandFistController : MonoBehaviour
{
    [Header("Axes")]
    public Vector3 fingerAxis = Vector3.right;
    public Vector3 thumbAxis = Vector3.right;

    [Header("Finger Bones (assign in order 01,02,03)")]
    public Transform[] index = new Transform[3];
    public Transform[] middle = new Transform[3];
    public Transform[] ring = new Transform[3];
    public Transform[] pinky = new Transform[3];
    public Transform[] thumb = new Transform[3];

    [Header("Curl Degrees (01,02,03)")]
    public float[] fingerDegrees = new float[3] { 70f, 85f, 65f };
    public float[] thumbDegrees = new float[3] { 55f, 65f, 55f };

    Quaternion[][] openRotations;

    void Awake()
    {
        CacheOpenRotations();
    }
float currentGrip = 0f;      // 0 = open, 1 = closed
public float gripSpeed = 1.5f; // how fast it moves

void Update()
{
    if (Input.GetKey(KeyCode.W))
    {
        currentGrip += gripSpeed * Time.deltaTime;
    }

    if (Input.GetKey(KeyCode.A))
    {
        currentGrip -= gripSpeed * Time.deltaTime;
    }

    currentGrip = Mathf.Clamp01(currentGrip);

    SetGrip(currentGrip);
}

    void CacheOpenRotations()
    {
        Transform[][] fingers = { index, middle, ring, pinky, thumb };
        openRotations = new Quaternion[fingers.Length][];

        for (int i = 0; i < fingers.Length; i++)
        {
            openRotations[i] = new Quaternion[3];
            for (int j = 0; j < 3; j++)
            {
                if (fingers[i][j])
                    openRotations[i][j] = fingers[i][j].localRotation;
            }
        }
    }

    public void CloseFist() => ApplyGrip(1f);
    public void OpenHand() => ApplyGrip(0f);
    public void SetGrip(float value) => ApplyGrip(Mathf.Clamp01(value));

    void ApplyGrip(float grip)
    {
        ApplyFinger(index, 0, fingerAxis, fingerDegrees, grip);
        ApplyFinger(middle, 1, fingerAxis, fingerDegrees, grip);
        ApplyFinger(ring,   2, fingerAxis, fingerDegrees, grip);
        ApplyFinger(pinky,  3, fingerAxis, fingerDegrees, grip);
        ApplyFinger(thumb,  4, thumbAxis,  thumbDegrees,  grip);
    }

    void ApplyFinger(Transform[] bones, int fingerIndex, Vector3 axis, float[] degrees, float grip)
    {
        for (int i = 0; i < 3; i++)
        {
            if (bones[i] == null) continue;

            Quaternion open = openRotations[fingerIndex][i];
            bones[i].localRotation =
                open * Quaternion.AngleAxis(grip * degrees[i], axis.normalized);
        }
    }

}
