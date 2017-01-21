using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
public class ClearStage : MonoBehaviour {
    public Text TimeText;
    public GameObject[] ImagePrefabs;
    public Transform[] ImagePos;
    public GameObject[] TitlePrefabs;
    public Transform TitlePos;
    public Transform InitPos;

	// Use this for initialization
	void Start () {
        GameInfo.Instance.isTimeCount = false;
        TimeText.text = GameInfo.Instance.Minute + "：" + (int)GameInfo.Instance.Second;
        TimeText.transform.parent.gameObject.SetActive(false);
        StartCoroutine(ShowClear());
    }

    IEnumerator ShowClear()
    {
        for (int i = 0; i < ImagePrefabs.Length; i++)
        {
            GameObject go = Instantiate(ImagePrefabs[i]);
            go.transform.position = InitPos.position;
            go.transform.localScale = Vector3.zero;
            go.transform.DOScale(InitPos.localScale, 0.3f).OnComplete(() => {
                go.transform.DOMove(ImagePos[i].position, 3.0f);
                go.transform.DOScale(ImagePos[i].localScale, 3.0f);
                go.transform.DORotate(ImagePos[i].rotation.eulerAngles, 3.0f);
            });
            yield return new WaitForSeconds(3.0f);
            if (i == ImagePrefabs.Length-1)
            {
                GameObject title = null;
                if (GameInfo.Instance.Minute <= 5)
                {
                    title = Instantiate(TitlePrefabs[0]);
                }else if (GameInfo.Instance.Minute <= 8)
                {
                    title = Instantiate(TitlePrefabs[1]);
                }
                else if (GameInfo.Instance.Minute <= 10)
                {
                    title = Instantiate(TitlePrefabs[2]);
                }
                else if (GameInfo.Instance.Minute <= 15)
                {
                    title = Instantiate(TitlePrefabs[3]);
                }
                else
                {
                    title = Instantiate(TitlePrefabs[4]);
                }
                title.transform.position = InitPos.position;
                title.transform.localScale = Vector3.zero;
                title.transform.DOScale(InitPos.localScale, 0.3f).OnComplete(() => {
                    title.transform.DOMove(TitlePos.position, 3.0f);
                    title.transform.DOScale(TitlePos.localScale, 3.0f);
                    title.transform.DORotate(TitlePos.rotation.eulerAngles, 3.0f);
                });
            }
        }
        TimeText.transform.parent.gameObject.SetActive(true);
    }
}
