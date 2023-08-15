using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
   
    Vector3 CenterPosition = Vector3.zero; //圆心的位置，我设定在（0，0）点
    Vector3 r; //圆半径，也就是要旋转的向量。
    private GameObject[] tar;
    private Transform m_transform;
    
    
    
    

    public void Start()
    {
        
       // tar = GameObject.FindGameObjectsWithTag("input");
       

    }
   
    public void Update()
    {
        tar = DynamicPrefabLoader.Target;
        int sum = tar.Length;
        Debug.Log(sum);

        for (int i = 0; i < tar.Length; i++)
        {
            m_transform = GameObject.FindGameObjectsWithTag("input")[i].GetComponent<Transform>();
            //Debug.Log(i+1);
            //加入列表，打印序列号
            r = m_transform.position - CenterPosition;
            r = Quaternion.AngleAxis(10 * Time.deltaTime, Vector3.up) * r;
            m_transform.position = CenterPosition + r;

            //圆周运动
            // //以每秒180度的速度旋转“半径”向量，因为我做2D游戏，所以旋转轴是Z轴。
            // //圆心位置 + 半径 = 圆上的点


        }

    }
    
    
    
}
