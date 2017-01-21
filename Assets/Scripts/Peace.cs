using UnityEngine;
using System.Collections;
using DG.Tweening;
public class Peace : MonoBehaviour
{

    public int Index;


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Terrain")
        {
            if (GameManager.Instance.StageNumber != 0)
            {
                GameInfo.Instance.HP -= 1;
            }
            GameManager.Instance.Audio.PlayOneShot(GameManager.Instance.SE_Clips[8]);
            GameManager.Instance.CreateHitEffect(2, transform.position);
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            Destroy(GetComponent<Rigidbody>());
            Tween tw1 = transform.DOMove(GameManager.Instance.InitPos[Index].position, 1.0f);
            Tween tw2 = transform.DORotate(GameManager.Instance.InitPos[Index].rotation.eulerAngles, 1.0f);
            if (tw1.IsComplete() && tw2.IsComplete())
            {
                gameObject.AddComponent<Rigidbody>().Sleep();
            }
        }
    }
}

