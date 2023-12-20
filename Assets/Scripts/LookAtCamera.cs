using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private enum Mode
    {
        LookAt,
        LookAtInverted,
        LookAtForward,
        LookAtForwardInverted,
    }

    [SerializeField] private Mode mode;

    private void LateUpdate()
    {
        switch (mode)
        {
            case Mode.LookAt:
                transform.LookAt(Camera.main.transform.position);
                break;
            case Mode.LookAtInverted:
                Vector3 mirrorOfCameraPosition = 2 * transform.position - Camera.main.transform.position;
                transform.LookAt(mirrorOfCameraPosition);
                break;
            case Mode.LookAtForward:
                transform.forward = Camera.main.transform.forward;
                break;
            case Mode.LookAtForwardInverted:
                transform.forward = -Camera.main.transform.forward;
                break;
        }
    }
}
