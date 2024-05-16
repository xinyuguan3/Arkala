using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MemoryPrefab : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]private Text nameText;
    [SerializeField]private Text contentText;

    public void SetNameText(string _msg){
        nameText.text=_msg;
    }

    public void SetContentText(string _msg){
        contentText.text=_msg;
    }
}
