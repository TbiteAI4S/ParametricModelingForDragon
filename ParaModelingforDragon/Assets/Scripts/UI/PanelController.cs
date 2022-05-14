using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelController : MonoBehaviour
{
    //パネル
    public GameObject bodyPanel;
    public GameObject neckPanel;
    public GameObject tailPanel;



    void Start()
    {
        //初めは全てのパネルは見えないようにする
        bodyPanel.SetActive(false);
        neckPanel.SetActive(false);
        tailPanel.SetActive(false);
    }

    /*---ボタンを押したら目的のパネルを表示し他のパネルは消す*/
    public void BodyView()
    {
        bodyPanel.SetActive(true);
        neckPanel.SetActive(false);
        tailPanel.SetActive(false);
    }
    public void NeckView()
    {
        bodyPanel.SetActive(false);
        neckPanel.SetActive(true);
        tailPanel.SetActive(false);
    }public void TailView()
    {
        bodyPanel.SetActive(false);
        neckPanel.SetActive(false);
        tailPanel.SetActive(true);
    }


}
