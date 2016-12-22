using UnityEngine;
using System.Collections;
using DG.Tweening;
public class HitCheck : MonoBehaviour {
    public int Index;

    public Vector3[] WayPoints;

    private GameManager gm;

    void Start()
    {
        gm = GameManager.Instance;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Peace")
        {
            int temp = other.GetComponent<Peace>().Index;
            if(Index == temp)
            {
                other.GetComponent<Rigidbody>().velocity = Vector3.zero;
                Destroy(other.GetComponent<Rigidbody>());
                gm.PeacePositions[Index].GetChild(0).gameObject.SetActive(false);
                other.transform.GetChild(0).gameObject.SetActive(true);
                //other.transform.SetParent(gm.PeacePositions[Index]);
                //other.transform.localRotation = Quaternion.Euler(Vector3.zero);
                //other.transform.DOLocalMove(Vector3.zero, 3.0f);
                gm.Audio.PlayOneShot(gm.Clips[9]);
                other.transform.DOPath(WayPoints, 3.0f).OnComplete(() => MyCallback(other)); 
                Debug.Log("正解");
                gm.QuestionCount--;
            }
            else
            {
                gm.Audio.PlayOneShot(gm.Clips[5]);
                gm.CreateHitEffect(0, other.transform.position);
                other.GetComponent<Rigidbody>().velocity = Vector3.zero;
                Destroy(other.GetComponent<Rigidbody>());
                other.gameObject.SetActive(false);
                StartCoroutine(ResetPeace(other.gameObject));
                Debug.Log("xx");
                gm.Penalty();
            }
        }
    }

    IEnumerator ResetPeace(GameObject go)
    {
        yield return new WaitForSeconds(1.0f);
        gm.CreateHitEffect(1, gm.InitPos[go.GetComponent<Peace>().Index].position);
        yield return new WaitForSeconds(0.5f);
        go.transform.position = gm.InitPos[go.GetComponent<Peace>().Index].position;
        go.transform.rotation = gm.InitPos[go.GetComponent<Peace>().Index].rotation;   
        go.SetActive(true);
        go.AddComponent<Rigidbody>().Sleep();
        gm.Audio.PlayOneShot(gm.Clips[4]);
        
    }

    private void MyCallback(Collider other)
    {
        other.transform.SetParent(gm.PeacePositions[Index]);
        other.transform.localRotation = Quaternion.Euler(Vector3.zero);
        other.transform.DOLocalMove(Vector3.zero, 3.0f).OnComplete(()=> {
            other.transform.GetChild(0).gameObject.SetActive(false);
            other.transform.GetChild(1).gameObject.SetActive(false);
            gm.Audio.PlayOneShot(gm.Clips[1]);
        });
        other.transform.DOScale(1.0f, 2.0f);
    }
}
