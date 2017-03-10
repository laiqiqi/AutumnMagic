using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VRTK;
using VRTK.GrabAttachMechanics;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;

    public Transform[] PuzzlePositons;
    public GameObject PuzzlePrefab;
    public Material[] PuzzleMaterials;
    public Transform[] InitPos;
    public GameObject[] Explode;
    public GameObject[] MovingEffects;

    public List<GameObject> Puzzles;
    public List<Transform> PeacePositions;

    public GameObject[] HitEffect;

    public Transform CameraRig;

    public int StageNumber;
    public int QuestionCount;

    public VRTK_HeadsetFade CameraFade;


    public AudioSource Audio;

    public AudioClip[] SE_Clips;
    public AudioClip[] BGM_Clips;

    public GameObject Qustions;
    public GameObject[] Wakus;

    public Text HPText;
    public Text TimeText;
    public GameObject GameOverText;
    public GameObject GameClearText;

    private bool isPause;
    public VRTK_ControllerEvents RightControllerEvents;
    public VRTK_ControllerEvents LeftControllerEvents;
    public GameObject LeftModel;
    public GameObject RightModel;
    public GameObject LeftControllerModel;
    public GameObject RightControllerModel;
    public GameObject MenuCanvasPrefab;

    private bool isLeftHand;
    private GameObject MenuCanvas;

    void Awake()
    {
        Instance = this;
    }
    // Use this for initialization
    void Start ()
    {
        GameOverText.SetActive(false);
        GameClearText.SetActive(false);
        isPause = false;
        RightControllerEvents.ButtonOnePressed += RightControllerEvents_ButtonOnePressed;
        LeftControllerEvents.ButtonOnePressed += LeftControllerEvents_ButtonOnePressed;
        InitPuzzle(QuestionCount);
    }

    private void LeftControllerEvents_ButtonOnePressed(object sender, ControllerInteractionEventArgs e)
    {
        isPause = !isPause;
        isLeftHand = true;
    }

    private void RightControllerEvents_ButtonOnePressed(object sender, ControllerInteractionEventArgs e)
    {
        isPause = !isPause;
        isLeftHand = false;
    }

    void Update()
    {
        if (isPause)
        {
            LeftControllerModel.SetActive(true);
            RightControllerModel.SetActive(true);
            LeftModel.SetActive(false);
            RightModel.SetActive(false);
            if (isLeftHand)
            {
                if (null == MenuCanvas)
                {
                    MenuCanvas = Instantiate(MenuCanvasPrefab, LeftControllerModel.transform.parent) as GameObject;
                    MenuCanvas.transform.localPosition = new Vector3(0, 0, 0.15f);
                    MenuCanvas.transform.localRotation = Quaternion.identity;
                    MenuCanvas.transform.localScale = new Vector3(0.003f, 0.003f, 0.003f);
                }
            }
            else
            {
                if (null == MenuCanvas)
                {
                    MenuCanvas = Instantiate(MenuCanvasPrefab, RightControllerModel.transform.parent) as GameObject;
                    MenuCanvas.transform.localPosition = new Vector3(0, 0, 0.15f);
                    MenuCanvas.transform.localRotation = Quaternion.identity;
                    MenuCanvas.transform.localScale = new Vector3(0.003f, 0.003f, 0.003f);
                }
            }
            Time.timeScale = 0;
            return;
        }
        else
        {
            LeftControllerModel.SetActive(false);
            RightControllerModel.SetActive(false);
            LeftModel.SetActive(true);
            RightModel.SetActive(true);
            if (MenuCanvas != null)
                Destroy(MenuCanvas);
            Time.timeScale = 1;

            if (GameInfo.Instance.isGameOver || GameInfo.Instance.Minute >= 60)
            {
                GameInfo.Instance.isGameOver = false;
                StartCoroutine(GameOver());
            }
            if (StageNumber != 0)
            {
                GameInfo.Instance.isTimeCount = true;
            }
            else
            {
                GameInfo.Instance.isTimeCount = false;
            }

            TimeText.text = GameInfo.Instance.Minute + "：" + (int)GameInfo.Instance.Second;

            if (StageNumber != 0)
            {
                if (GameInfo.Instance.HP < 0)
                {
                    HPText.text = "0";
                }
                else
                {
                    HPText.text = GameInfo.Instance.HP.ToString();
                }
            }
            else
            {
                HPText.text = "∞";
            }

            if (GameInfo.Instance.HP <= 5 && Audio.clip != BGM_Clips[1])
            {
                Audio.clip = BGM_Clips[1];
                Audio.Play();
            }

            if (QuestionCount == 0)
            {
                QuestionCount = 1;
                StartCoroutine(StageClear(StageNumber));
            }
        }
    }

    private void InitPuzzle(int count)
    {
        for (int i = 0; i < PuzzlePositons.Length; i++)
        {
            GameObject go = Instantiate(PuzzlePrefab);
            go.GetComponent<MeshRenderer>().material = PuzzleMaterials[i];
            go.transform.SetParent(PuzzlePositons[i]);
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;
            Puzzles.Add(go);
        }
        if (Puzzles.Count == PuzzlePositons.Length)
        {
            for (int i = 0; i < count; i++)
            {
                int index = Random.Range(0, Puzzles.Count);
                GameObject go = Instantiate(Explode[i]);
                go.transform.SetParent(Puzzles[index].transform.parent);
                go.transform.localPosition = Vector3.zero;
                go.transform.localScale = Vector3.one;
                PeacePositions.Add(Puzzles[index].transform.parent);
                Puzzles[index].transform.SetParent(null);
                Puzzles[index].transform.position = InitPos[i].position;
                Puzzles[index].transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                Puzzles[index].transform.rotation = InitPos[i].rotation;
                for (int j = 0; j < MovingEffects.Length; j++)
                {
                    GameObject effect = Instantiate(MovingEffects[j]);
                    effect.transform.SetParent(Puzzles[index].transform);
                    effect.transform.localPosition = Vector3.zero;
                    effect.transform.localRotation = Quaternion.identity;
                    effect.transform.localScale = Vector3.one;
                    if (j == 0)
                    {
                        effect.SetActive(false);
                    }
                }
                Puzzles[index].AddComponent<Rigidbody>().Sleep();
                Puzzles[index].AddComponent<VRTK_InteractableObject>().isGrabbable = true;
                Puzzles[index].AddComponent<VRTK_FixedJointGrabAttach>().throwMultiplier = 4;
                Puzzles[index].tag = "Peace";
                Puzzles[index].AddComponent<Peace>().Index = i;
                Puzzles.RemoveAt(index);
            }
        }
    }

    public void CreateHitEffect(int index ,Vector3 pos)
    {
        GameObject go = Instantiate(HitEffect[index]);
        go.transform.position = pos;
        go.AddComponent<HitEffect>();
    }

    public void Penalty()
    {
        CameraRig.DOShakePosition(1.0f);
        Audio.PlayOneShot(SE_Clips[0]);
        if (GameManager.Instance.StageNumber != 1)
        {
            GameInfo.Instance.HP -= 1;
        }
    }
    IEnumerator StageClear(int StageNumber)
    {
        yield return new WaitForSeconds(8.0f);
        Audio.PlayOneShot(SE_Clips[2]);
        GameClearText.SetActive(true);
        GameClearText.transform.DOScale(0.5f, 0.3f);
        yield return new WaitForSeconds(1.0f);
        CameraFade.Fade(Color.white, 2.0f);
        yield return new WaitForSeconds(3.0f);
        if (StageNumber != 0)
            GameInfo.Instance.HP += 10;
        SceneManager.LoadSceneAsync(StageNumber+2);
    }

    IEnumerator GameOver()
    {
        Destroy(Qustions);
        for (int i = 0; i < PuzzlePositons.Length; i++)
        {
            if (PuzzlePositons[i].GetComponent<Rigidbody>() != null)
            {
                PuzzlePositons[i].GetComponent<Rigidbody>().useGravity = true;
            }
            else
            {
                PuzzlePositons[i].gameObject.AddComponent<Rigidbody>().useGravity = true;
            }
        }
        for (int i = 0; i < Wakus.Length; i++)
        {
            if (Wakus[i].GetComponent<Rigidbody>() != null)
            {
                Wakus[i].GetComponent<Rigidbody>().useGravity = true;
            }
            else
            {
                Wakus[i].AddComponent<Rigidbody>().useGravity = true;
            }
        }
        Audio.PlayOneShot(SE_Clips[3]);
        GameOverText.SetActive(true);
        GameOverText.transform.DOScale(0.5f, 0.3f);
        yield return new WaitForSeconds(8.0f);
        CameraFade.Fade(Color.black, 2.0f);
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadSceneAsync(0);
    }

}
