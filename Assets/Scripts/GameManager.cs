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
    //缓存
    private GameObject _menuCanvas;
    private VRTK_SimplePointer _leftSimplePointer;
    private VRTK_UIPointer _leftUIPointer;
    private VRTK_SimplePointer _rightSimplePointer;
    private VRTK_UIPointer _rightUIPointer;
    void Awake()
    {
        Instance = this;
    }
    // Use this for initialization
    void Start ()
    {
        //初始化
        GameOverText.SetActive(false);
        GameClearText.SetActive(false);
        isPause = false;
        //绑定手柄按键事件
        RightControllerEvents.ButtonOnePressed += RightControllerEvents_ButtonOnePressed;
        LeftControllerEvents.ButtonOnePressed += LeftControllerEvents_ButtonOnePressed;

        _leftSimplePointer = LeftControllerEvents.GetComponent<VRTK_SimplePointer>();
        _leftUIPointer = LeftControllerEvents.GetComponent<VRTK_UIPointer>();
        _rightSimplePointer = RightControllerEvents.GetComponent<VRTK_SimplePointer>();
        _rightUIPointer = RightControllerEvents.GetComponent<VRTK_UIPointer>();

        _leftSimplePointer.enabled = false;
        _leftUIPointer.enabled = false;
        _rightSimplePointer.enabled = false;
        _rightUIPointer.enabled = false;
        //初始化拼图
        InitPuzzle(QuestionCount);
    }
    /// <summary>
    /// 左手柄暂停按键回掉函数
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void LeftControllerEvents_ButtonOnePressed(object sender, ControllerInteractionEventArgs e)
    {
        isPause = !isPause;
        isLeftHand = true;
    }
    /// <summary>
    /// 右手柄暂停按键回掉函数
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void RightControllerEvents_ButtonOnePressed(object sender, ControllerInteractionEventArgs e)
    {
        isPause = !isPause;
        isLeftHand = false;
    }

    void Update()
    {
        //暂停情况的处理
        if (isPause)
        {
            LeftControllerModel.SetActive(true);
            RightControllerModel.SetActive(true);
            LeftModel.SetActive(false);
            RightModel.SetActive(false);
            if (isLeftHand)
            {
                if (null == _menuCanvas)
                {
                    //显示UI
                    _menuCanvas = Instantiate(MenuCanvasPrefab, LeftControllerModel.transform.parent) as GameObject;
                    //给另一只手柄添加UI交互
                    _rightSimplePointer.enabled = true;
                    _rightUIPointer.enabled = true;
                    _menuCanvas.transform.localPosition = MenuCanvasPrefab.transform.position;
                    _menuCanvas.transform.localRotation = MenuCanvasPrefab.transform.rotation;
                    _menuCanvas.transform.localScale = MenuCanvasPrefab.transform.localScale;
                }
            }
            else
            {
                if (null == _menuCanvas)
                {
                    //显示UI
                    _menuCanvas = Instantiate(MenuCanvasPrefab, RightControllerModel.transform.parent) as GameObject;
                    //给另一只手柄添加UI交互
                    _leftSimplePointer.enabled = true;
                    _leftUIPointer.enabled = true;
                    _menuCanvas.transform.localPosition = MenuCanvasPrefab.transform.position;
                    _menuCanvas.transform.localRotation = MenuCanvasPrefab.transform.rotation;
                    _menuCanvas.transform.localScale = MenuCanvasPrefab.transform.localScale;
                }
            }
            Time.timeScale = 0;
            return;
        }

            //非暂停情况的处理
            LeftControllerModel.SetActive(false);
            RightControllerModel.SetActive(false);
            _leftSimplePointer.enabled = false;
            _leftUIPointer.enabled = false;
            _rightSimplePointer.enabled = false;
            _rightUIPointer.enabled = false;
            LeftModel.SetActive(true);
            RightModel.SetActive(true);
            if (_menuCanvas != null)
                Destroy(_menuCanvas);
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

            TimeText.text = GameInfo.Instance.Minute.ToString().PadLeft(2, '0') + "：" +
                            GameInfo.Instance.Second.ToString("00");

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

    /// <summary>
    /// 初始化拼图
    /// </summary>
    /// <param name="count"></param>
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

    /// <summary>
    /// 创建碰撞特效
    /// </summary>
    /// <param name="index"></param>
    /// <param name="pos"></param>
    public void CreateHitEffect(int index ,Vector3 pos)
    {
        GameObject go = Instantiate(HitEffect[index]);
        go.transform.position = pos;
        go.AddComponent<HitEffect>();
    }

    /// <summary>
    /// 打错时惩罚处理
    /// </summary>
    public void Penalty()
    {
        CameraRig.DOShakePosition(1.0f);
        Audio.PlayOneShot(SE_Clips[0]);
        if (StageNumber != 1)
        {
            GameInfo.Instance.HP -= 1;
        }
    }
    /// <summary>
    /// 通关的处理
    /// </summary>
    /// <param name="StageNumber"></param>
    /// <returns></returns>
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

    /// <summary>
    /// 游戏结束的处理
    /// </summary>
    /// <returns></returns>
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
