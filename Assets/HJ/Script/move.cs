using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class move : MonoBehaviour
{
    private float[] frequency = new float[50]; 
    private float[] amplitude = new float[50]; 
    private float[] speed = new float[50]; 
 // 随机选择的数值
    
    Vector3 CenterPosition = Vector3.zero; //圆心的位置，我设定在（0，0）点
    Vector3 r; //圆半径，也就是要旋转的向量。
    private GameObject[] tar;
    private Transform m_transform;
    
   
   // public float frequency = 1f; // 频率
  
    public void Start()
    {
        GenerateRandomValues();  // 初始化随机数数组
        
    }

    public void Update()
    {

      
        
      tar = DynamicPrefabLoader.Target;
       
        for (int i = 0; i < tar.Length; i++)
        {
            m_transform = GameObject.FindGameObjectsWithTag("input")[i].GetComponent<Transform>();
            r = m_transform.position - CenterPosition;
            r = Quaternion.AngleAxis(speed[i] * Time.deltaTime, Vector3.up) * r;
            m_transform.position = CenterPosition + r;

            float newY = Mathf.Sin(Time.time * frequency[i]) * amplitude[i];
            m_transform.position = new Vector3(m_transform.position.x, newY, m_transform.position.z);
           
           Debug.Log(frequency[i]);
           Debug.Log(amplitude[i]);
            //圆周运动
            // //以每秒180度的速度旋转“半径”向量，因为我做2D游戏，所以旋转轴是Z轴。
            // //圆心位置 + 半径 = 圆上的点


        }

    }
    void GenerateRandomValues()
    {
        for (int i = 0; i < 50;  i++)
        {
            frequency[i] = Random.Range(0.5f, 2f);
            amplitude[i] = Random.Range(0.7f, 3f);
            speed[i] = Random.Range(5f, 20f);
            
         //   Debug.Log("Random value for i = " + i + ": " + randomValues[i]);
          
        }
    }    
    
   
}
