using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleSceneTrans : MonoBehaviour
{
    public void TransScene()
    {
        SceneManager.LoadScene("Chat"); 
    }
}
