using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeModel : MonoBehaviour
{
    /*---制御点---*/
    //胴体の制御点
    private Vector3[] controllPointsBody = {
        new Vector3(0f, 0f, 0.01f),
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
        new Vector3(0f,0f,0.01f)
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

    }

    // Update is called once per frame
    void Update()
    {

    }
}
