﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeModel : MonoBehaviour
{
    /*---スクリプト---*/
    Tools toolscripts;

    /*---制御点---*/
    //胴体の制御点
    private Vector3[] controllPointsBody = {
        new Vector3(0f, 0.01f, 0f),
        new Vector3(1f, 0f, 0f),
        new Vector3(2f, 0f, 0f),
        new Vector3(3f, 0f, 0f),
        new Vector3(4f, 0f, 0f)
    };
    //尾の制御点
    public List<Vector3> controllPointsTail = new List<Vector3>()
    {
        new Vector3(4f,0f,0f)
    };
    //首の制御点
    public List<Vector3> controllPointsNeck = new List<Vector3>()
    {
        new Vector3(0f,0.01f,0f)
    };
    //頭の制御点
    public List<Vector3> controllPointsHead = new List<Vector3>()
    {
        new Vector3(0f,0.01f,0f)
    };
    //顎の制御点
    public List<Vector3> controllPointsJaw = new List<Vector3>()
    {
        new Vector3(0f,0.01f,0f)
    };

    /*---尾の制御点の操作---*/
    //尾の制御点の個数
    public int tailPoints = 0;
    //尾の制御点を追加
    public void AddTail()
    {
        //制御点を追加
        float beforeCP_x = controllPointsTail[tailPoints].x;
        Vector3 newCP = new Vector3(beforeCP_x + 1f, 0f, 0f);
        controllPointsTail.Add(newCP);
        //制御点の個数を増やす
        tailPoints += 1;
    }
    //尾の制御点を減らす
    public void CutTail()
    {
        //制御点を削除
        //末尾の制御点を削除
        controllPointsTail.RemoveAt(tailPoints);
        //制御点の個数を減らす
        tailPoints = tailPoints - 1;
    }

    /*---首の制御点の操作---*/
    //首の制御点を追加
    public int neckPoints = 0;
    //首の制御点を増やす
    public void AddNeck()
    {
        //制御点を追加
        float beforeCP_x = controllPointsNeck[neckPoints].x;
        float beforeCP_y = controllPointsNeck[neckPoints].y;
        Vector3 newCP = new Vector3(beforeCP_x - 0.3f, beforeCP_y + 0.3f, 0f);
        controllPointsNeck.Add(newCP);
        //制御点の個数を増やす
        neckPoints += 1;
    }
    //首の制御点を減らす
    public void CutNeck()
    {
        //制御点を削除
        //末尾の制御点を削除
        controllPointsNeck.RemoveAt(neckPoints);
        //制御点の個数を減らす
        neckPoints = neckPoints - 1;
    }
    //首を曲げる
    public void BendNeck(float a)
    {
        //新しい制御点
        float[] newCPNeck = new float[controllPointsNeck.Count];
        //曲線の分割数
        int dv = 1;
        //曲線を作って更新
        newCPNeck = toolscripts.Sigmoid(controllPointsNeck, dv, a);
        for(int i = 0; i < controllPointsNeck.Count; i++)
        {
            Vector3 newPoint = controllPointsNeck[i];
            newPoint.y = newCPNeck[i];
            controllPointsNeck[i] = newPoint;
        }
    }

    /*---頭部の制御点の操作---*/
    /*--頭--*/
    //頭の制御点を追加
    public int headPoints = 0;
    //頭の制御点を増やす
    public void AddHead()
    {

    }
    //頭の制御点を減らす
    public void CutHead()
    {

    }
    /*--下顎--*/
    //下顎の制御点を追加
    public int jawPoint = 0;



    /*---頂点を確認---*/
    void OnDrawGizmos()
    {
        //中心線
        Gizmos.color = Color.white;
        foreach (var point in controllPointsBody)
        {
            Gizmos.DrawSphere(point, 0.1f);
        }
        Gizmos.color = Color.red;
        foreach (var point in controllPointsTail)
        {
            Gizmos.DrawSphere(point, 0.1f);
        }
        Gizmos.color = Color.blue;
        foreach (var point in controllPointsNeck)
        {
            Gizmos.DrawSphere(point, 0.1f);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //ツールスクリプトの取得
        toolscripts = GameObject.Find("Tools").GetComponent<Tools>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
