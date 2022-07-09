using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPiece : MonoBehaviour
{
    [SerializeField] private GameObject model;
    [SerializeField] private GameObject leftPos;
    [SerializeField] private GameObject rightPos;

    public void SetColor(Color color)
    {
        model.GetComponent<Renderer>().material.color = color;
    }

    public Color GetColor()
    {
        return model.GetComponent<Renderer>().material.color;
    }

    public GameObject GetModel()
    {
        return model;
    }

    public Vector3 GetLeftPos()
    {
        return leftPos.transform.position;
    }

    public Vector3 GetRigtPos()
    {
        return rightPos.transform.position;
    }
}
