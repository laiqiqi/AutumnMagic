using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VRTK;
using DG.Tweening;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour {

    public static GameManager Instance;

    public Transform[] PuzzlePositons;
    public GameObject[] PuzzlePrefabs;
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

    public AudioClip[] Clips;

    //public float power = 20;

    //public Transform cameraRigPos;

    //public bool isGameStart = false;
    //public int Count = 0;

    //public GameObject Sphere;

    void Awake()
    {
        Instance = this;
    }
    // Use this for initialization
    void Start ()
    {
        //for (int i = 0; i < PuzzlePrefabs.Length; i++)
        //{
        //    GameObject go = Instantiate(PuzzlePrefabs[i]);
        //    go.transform.SetParent(PuzzlePositons[i]);
        //    go.transform.localPosition = Vector3.zero;
        //    go.transform.localRotation = Quaternion.identity;
        //    go.transform.localScale = Vector3.one;

        //    //go.transform.GetChild(0).gameObject.SetActive(false);
        //    //go.AddComponent<Puzzle>().PuzzleNumber = i;
        //    Puzzles.Add(go);
        //}
        //Count = 0;
        //Sphere.SetActive(false);
        InitPuzzle(QuestionCount);
    }


    void Update()
    {
        //if (StageNumber > 0)
        //{
        //    Timer -= Time.deltaTime;
        //    if (Timer > 30.0f)
        //    {
        //        light01.spotAngle -= Time.deltaTime;
        //    }
        //    else
        //    {
        //        light01.spotAngle -= Time.deltaTime * 2;
        //    }
        //    if (Timer <= 0)
        //    {
        //        light01.enabled = false;
        //        light02.enabled = false;
        //        Audio.PlayOneShot(Clips[3]);
        //        SceneManager.LoadSceneAsync(0);
        //    }
        //}

        //Debug.Log(Timer);

        //if(StageNumber == 0 && Timer > 0)
        //{
        //    StartCoroutine(StageClear());
        //}
        if (QuestionCount == 0)
        {
            QuestionCount = 1;
            StartCoroutine(StageClear(StageNumber));
        }
        //if (cameraRigPos.position.z > 110.0f &&!isGameStart)
        //{
        //    OnGameStart();
        //    isGameStart = true;
        //}

        //if (Count >= 16 && Sphere != null)
        //{
        //    Sphere.SetActive(true);
        //}
    }
    //private void OnGameStart()
    //{
    //    for (int i = 0; i < Puzzles.Count; i++)
    //    {
    //        Puzzles[i].transform.SetParent(null);
    //        Puzzles[i].GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(1, 2), Random.Range(100, 200), Random.Range(1, 2)) * 20);
            
    //    }
    //    for (int i = 0; i < PuzzlePositons.Length; i++)
    //    {
    //        PuzzlePositons[i].gameObject.AddComponent<PositionCheck>().PosNumber = i;
    //    }
    //}


    //public void StageClear()
    //{
    //    for (int i = 0; i < PuzzlePositons.Length; i++)
    //    {
    //        Destroy(PuzzlePositons[i].gameObject);
    //    }
    //    CorrectAnswer.SetActive(true);
    //    CorrectAnswer.transform.DOMoveY(6.0f, 7.0f);
    //    CorrectAnswer.transform.DORotate(new Vector3(110.0f, 180.0f, 0.0f), 10.0f).SetDelay(7.0f);
    //}

    private void InitPuzzle(int count)
    {
        for (int i = 0; i < PuzzlePrefabs.Length; i++)
        {
            GameObject go = Instantiate(PuzzlePrefabs[i]);
            go.GetComponent<MeshRenderer>().material = PuzzleMaterials[i];
            go.transform.SetParent(PuzzlePositons[i]);
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;
            Puzzles.Add(go);
        }
        if (Puzzles.Count == PuzzlePrefabs.Length)
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
                Puzzles[index].GetComponent<VRTK_InteractableObject>().throwMultiplier = 4;
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
        Audio.PlayOneShot(Clips[0]);
    }
    IEnumerator StageClear(int StageNumber)
    {
        yield return new WaitForSeconds(8.0f);
        Audio.PlayOneShot(Clips[2]);
        yield return new WaitForSeconds(2.0f);
        CameraFade.Fade(Color.white, 2.0f);
        SceneManager.LoadSceneAsync(StageNumber+1);
    }

}
