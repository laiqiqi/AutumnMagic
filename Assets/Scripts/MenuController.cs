using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    #region 菜单
    public void OnRestartButtonClick()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
    public void OnOptionButtonClick()
    {
        //TODO
    }
    public void OnExitButtonClick()
    {
        SceneManager.LoadScene(0);
    }
    #endregion
}
