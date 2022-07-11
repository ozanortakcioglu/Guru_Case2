using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Chibi : MonoBehaviour
{
    public GameObject model;
    private Animator animator;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void StartToRun(float timeFor2unit)
    {
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
        model.transform.DOMove(new Vector3(0, -5, 2), 1).SetRelative(true).SetEase(Ease.InSine).OnComplete(() => 
        {
            //fail
        });
    }

    public void Dance()
    {
        animator.SetTrigger("Dance");
        transform.DOKill();
    }
}
