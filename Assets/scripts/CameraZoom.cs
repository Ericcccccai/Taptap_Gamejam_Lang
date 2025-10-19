using UnityEngine;
using Unity.Cinemachine;

public class CameraZoom : MonoBehaviour
{
    public float normalSize = 7.26f;
    public float fightSize = 11.5f;
    public float zoomSpeed = 2f;

    private CinemachineCamera vcam;
    private float targetSize;

    void Start()
    {
        vcam = GetComponent<CinemachineCamera>();
        targetSize = normalSize;
        vcam.Lens.OrthographicSize = normalSize;
    }

    void Update()
    {
        float current = vcam.Lens.OrthographicSize;
        vcam.Lens.OrthographicSize = Mathf.Lerp(current, targetSize, Time.deltaTime * zoomSpeed);
    }

    public void SetFightingPhase(bool on)
    {
        targetSize = on ? fightSize : normalSize;
    }
}
