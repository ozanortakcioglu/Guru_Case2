using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlatformCreator : MonoBehaviour
{
    [SerializeField] private GameObject piecePrefab;
    [SerializeField] private List<GameObject> pieces;
    [SerializeField] private float successAssistance = 0.1f;

    private float zPosAddition = 2f;
    private float xStartPos = 3;
    private GameObject activePiece;
    private GameObject oldPiece;
    private bool isFail = false;

    private void Start()
    {
        SetInitialColors();
        CreateNewActive();
    }

    private void Update()
    {
        //controller taşı
        if (Input.GetMouseButtonDown(0))
        {
            PlaceActivePiece();
        }
    }

    public void PlaceActivePiece()
    {
        if (isFail)
            return;
        activePiece.transform.DOKill();

        float posDifference = activePiece.transform.position.x - oldPiece.transform.position.x;

        if(Mathf.Abs(posDifference) < successAssistance) //successfully placed
        {
            activePiece.transform.position = new Vector3(oldPiece.transform.position.x, activePiece.transform.position.y, activePiece.transform.position.z);
        }
        else //cut activePiece
        {
            if(posDifference / 2 < activePiece.transform.localScale.x)
                CutActivePiece(posDifference);
            else
            {
                isFail = true;
                activePiece.GetComponent<PlatformPiece>().GetModel().GetComponent<Renderer>().material.DOColor(Color.gray, 0.5f).SetEase(Ease.InSine);
                activePiece.AddComponent<Rigidbody>();
            }
        }

        if(!isFail)
            CreateNewActive();
    }

    private void CutActivePiece(float difference)
    {
        var scale = activePiece.transform.localScale;
        var oldX = scale.x;
        scale.x -= Mathf.Abs(difference / 2);
        activePiece.transform.localScale = scale;
        activePiece.transform.position += new Vector3(-difference / 2, 0, 0);
        

        var cuttedPiece = Instantiate(activePiece);
        float cuttedScale = oldX - scale.x;
        cuttedPiece.transform.localScale = new Vector3(cuttedScale, 1, 1);
        
        if(difference < 0)
            cuttedPiece.transform.position = activePiece.GetComponent<PlatformPiece>().GetLeftPos() + new Vector3(-cuttedScale, 0, 0);
        else
            cuttedPiece.transform.position = activePiece.GetComponent<PlatformPiece>().GetRigtPos() + new Vector3(cuttedScale, 0, 0);

        cuttedPiece.GetComponent<PlatformPiece>().GetModel().GetComponent<Renderer>().material.DOColor(Color.gray, 0.5f).SetEase(Ease.InSine);
        cuttedPiece.AddComponent<Rigidbody>();
        cuttedPiece.AddComponent<SelfDestruct>().lifetime = 4;
    }

    private void CreateNewActive()
    {
        var firstOne = pieces[0];
        pieces.Remove(firstOne);

        var rand = Random.Range(0f, 1f);
        float x = xStartPos;
        if (rand < 0.5f)
            x *= -1;

        firstOne.transform.position = new Vector3(x, activePiece.transform.position.y, activePiece.transform.position.z + zPosAddition);
        firstOne.transform.localScale = activePiece.transform.localScale;

        firstOne.GetComponent<PlatformPiece>().SetColor(GetNextColor(activePiece.GetComponent<PlatformPiece>().GetColor()));

        oldPiece = activePiece;
        activePiece = firstOne;
        pieces.Add(activePiece);

        if (rand < 0.5f)
            Start2MoveActivePiece(false);
        else
            Start2MoveActivePiece(true);
    }

    private void Start2MoveActivePiece(bool isGoingLeft)
    {
        if (isGoingLeft)
            activePiece.transform.DOMoveX(-10, 5).SetRelative(true).SetEase(Ease.Linear);
        else
            activePiece.transform.DOMoveX(10, 5).SetRelative(true).SetEase(Ease.Linear);
    }

    private void SetInitialColors()
    {
        activePiece = pieces[pieces.Count - 1];
        foreach (var item in pieces)
        {
            var tempColor = GetNextColor(activePiece.GetComponent<PlatformPiece>().GetColor());
            item.GetComponent<PlatformPiece>().SetColor(tempColor);
            activePiece = item;
        }
    }

    private Color GetNextColor(Color oldColor)
    {
        float h;
        float s;
        float v;

        Color.RGBToHSV(oldColor, out h, out s, out v);

        h += 20f / 360f;

        return Color.HSVToRGB(h, s, v);
    }
}
