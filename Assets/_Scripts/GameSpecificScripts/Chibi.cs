using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Chibi : MonoBehaviour
{
    [SerializeField] private GameObject model;
    [SerializeField] private GameObject cameraRotator;

    private Animator animator;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void StartToRun(float timeFor2unit)
    {
        cameraRotator.transform.DOKill();
        cameraRotator.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.8f).SetEase(Ease.InOutSine);


        animator.applyRootMotion = false;
        animator.SetTrigger("Run");
        model.transform.DOLocalRotate(Vector3.zero, 0.3f);
        transform.DOMoveZ(2, timeFor2unit).SetRelative(true).SetEase(Ease.Linear).OnComplete(() => 
        {
            StartToRun(timeFor2unit);
        });
    }

    public void Fall()
    {
        transform.DOKill();
        model.transform.DOScale(Vector3.zero, 1f).SetEase(Ease.InSine);
        model.transform.DOMove(new Vector3(0, -5, 2), 1).SetRelative(true).SetEase(Ease.InSine).OnComplete(() => 
        {
            SoundManager.Instance.PlaySound(SoundTrigger.Lose);
            UIManager.Instance.OpenPanel(PanelNames.LosePanel);
            
        });
    }

    public void Dance()
    {
        animator.applyRootMotion = true;
        animator.ResetTrigger("Run");
        animator.SetTrigger("Dance");
        transform.DOKill();
        cameraRotator.transform.DOKill();
        cameraRotator.transform.DOLocalRotate(new Vector3(0, 359, 0), 3f, RotateMode.FastBeyond360).SetEase(Ease.InOutSine);

    }
}
