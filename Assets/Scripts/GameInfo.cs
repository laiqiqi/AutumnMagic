using UnityEngine;
using System.Collections;

public class GameInfo : MonoBehaviour {
    private static GameInfo _instance;
    public static GameInfo Instance
    {
        get { return _instance; }
    }

    public int HP
    {
        get { return _hp; }
        set { _hp = value; }
    }

    private int _hp = 20;

    public bool isTimeCount = false;
    public bool isGameOver = false;

    public float Second = 0;
    public int Minute = 0;

    void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

	void FixedUpdate()
    {
        if (isTimeCount)
        {
            Second += Time.deltaTime;
            if (Second >= 60)
            {
                Second = 0;
                Minute += 1;
            }
        }
        if (_hp <= 0)
        {
            isGameOver = true;
        }
    }
}
