using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    public void LoadLevelByIndex(int sceneIndex)
    {        
        SceneManager.LoadScene(sceneIndex);
    }
}
