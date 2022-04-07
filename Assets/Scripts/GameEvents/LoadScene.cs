using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
   public void Load_Scene(string name)
    {
        SceneManager.LoadSceneAsync("Loading");
        SceneManager.LoadScene(name);
        SoundManager.Instance.Play(Sounds.ButtonClick);
    }
}
