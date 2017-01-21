using UnityEngine;
using System.Collections;
using VRTK;
using UnityEngine.SceneManagement;

public class TitleReturn : MonoBehaviour
{
    private VRTK_ControllerEvents controllerEvents;
	// Use this for initialization
	void Start () {
        controllerEvents = GetComponent<VRTK_ControllerEvents>();
        controllerEvents.TouchpadPressed += ControllerEvents_TouchpadPressed;
    }

    private void ControllerEvents_TouchpadPressed(object sender, ControllerInteractionEventArgs e)
    {
        SceneManager.LoadSceneAsync(0);
    }
}
