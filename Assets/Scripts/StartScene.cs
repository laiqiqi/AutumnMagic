using UnityEngine;
using System.Collections;
using VRTK;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour {

    private VRTK_ControllerEvents controllerEvents;

	// Use this for initialization
	void Start () {
        controllerEvents = GetComponent<VRTK_ControllerEvents>();
        controllerEvents.TriggerClicked += ControllerEvents_TriggerClicked;
    }

    private void ControllerEvents_TriggerClicked(object sender, ControllerInteractionEventArgs e)
    {
        SceneManager.LoadSceneAsync(1);
    }
}
