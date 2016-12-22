using UnityEngine;
using System.Collections;
using VRTK;
using DG.Tweening;
public class Puzzle : MonoBehaviour
{
    public int PuzzleNumber;

    private VRTK_InteractableObject interactableObjectScript;

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Terrain")
        {
            transform.position = new Vector3(90, 16, 120);
        }
        if(collision.gameObject.name == "Sphere")
        {
            Destroy(collision.gameObject);
            //GameManager.Instance.StageClear();
        }
    }

    void Update()
    {
        if(interactableObjectScript == null)
        {
            interactableObjectScript = GetComponent<VRTK_InteractableObject>();
        }
        else
        {
            if (interactableObjectScript.IsGrabbed())
            {
                transform.DOScale(Vector3.one * 0.5f, 0.3f);
            }
            else
            {
                transform.DOScale(Vector3.one, 0.3f);
            }
        }

    }
}
