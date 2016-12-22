using UnityEngine;
using System.Collections;
using VRTK;
public class PositionCheck : MonoBehaviour {

    public int PosNumber;

    private bool isCorrect = false;
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Peace")
        {
            other.transform.position = this.transform.position;
            other.transform.rotation = this.transform.rotation;
            if (other.GetComponent<Puzzle>().PuzzleNumber == PosNumber && !isCorrect)
            {
                other.GetComponent<VRTK_InteractableObject>().isGrabbable = false;
                other.GetComponent<Rigidbody>().isKinematic = true;
                other.transform.GetChild(0).gameObject.SetActive(true);
                other.transform.SetParent(this.transform);
                other.GetComponent<AudioSource>().PlayOneShot(other.GetComponent<AudioSource>().clip);
                //GameManager.Instance.Count++;
                isCorrect = true;
            }
        }
    }
}
