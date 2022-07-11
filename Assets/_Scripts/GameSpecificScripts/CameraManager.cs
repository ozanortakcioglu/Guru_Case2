using UnityEngine;
using DG.Tweening;

public class CameraManager : MonoBehaviour
{
    public Transform target;

    private Vector3 offset;

    private void Start()
    {
        offset = transform.position - target.position;
    }

    void Update()
    {
        transform.DOKill();
        var pos = target.transform.position + offset;
        pos.y = transform.position.y;
        transform.DOMove(pos, 0.2f).SetEase(Ease.Linear);
    }
}
