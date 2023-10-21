using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System.IO;

public class move : MonoBehaviour
{
  //  public string folderPath; // 指定文件夹路径

    private float[] frequency = new float[50]; 
    private float[] amplitude = new float[50]; 
    private float[] speed = new float[50]; 
    // 随机选择的数值
    
    Vector3 CenterPosition = new Vector3(0,2,24); 
    //圆心的位置，我设定在（0，0）点
    Vector3 r; 
    //圆半径，也就是要旋转的向量。
    private GameObject[] tar;
    private Transform m_transform;

    /// <summary>
    //速度变更
    /// </summary>
    /// 
    //private float minSpeed= 10f;
    //private float maxSpeed= 50f;
    //public float currentSpeed;
    private float intervalTime = 0.1f; // 时间间隔
    private float timer; // 计时器
    private bool isIncreasing; // 是否正在增加速度

    private float[] minSpeed = new float[50];
    private float[] maxSpeed = new float[50];
    public float[] currentSpeed = new float[50];
    /// <summary>
    /// /////
    /// </summary>
    public void Start()
    {

       
        GenerateRandomValues();  // 初始化随机数数组
     // currentSpeed = minSpeed; // 初始速度为最小速度
        isIncreasing = true;
        timer = 0f;

    }

    public void Update()
    {
        for (int i = 0; i < 50; i++)
        {
            timer += Time.deltaTime;
            if (isIncreasing)
            {
                currentSpeed[i] = Mathf.Lerp(minSpeed[i], maxSpeed[i], timer / intervalTime);
                if (timer >= intervalTime)
                {
                    isIncreasing = false;
                    timer = 0f;
                }
            }
            else
            {
                currentSpeed[i] = Mathf.Lerp(maxSpeed[i], minSpeed[i], timer / intervalTime);
                if (timer >= intervalTime)
                {
                    isIncreasing = true;
                    timer = 0f;
                }
            }
        }
        /////////////////////////////////////////////////速度变更
        tar = DynamicPrefabLoader.Target;

       

        for (int i = 0; i < tar.Length; i++)
        {
           
            


            m_transform = GameObject.FindGameObjectsWithTag("input")[i].GetComponent<Transform>();

            // 计算切线方向并设置 y 轴旋转角度
            Vector3 tangent = m_transform.position - CenterPosition;
            tangent.y = 0f;
            float distance = Vector3.Distance(m_transform.position, CenterPosition);
            Quaternion targetRotation = Quaternion.LookRotation(tangent) * Quaternion.Euler(70f, CalculateRotationAngle(currentSpeed[i], distance)+90f, 0f);

            // 绕中心圆周运动
            Vector3 r = m_transform.position - CenterPosition;
            r = Quaternion.AngleAxis(currentSpeed[i]* Time.deltaTime, Vector3.up) * r;
            m_transform.position = CenterPosition + r;

            // y轴变化
         
            float newY = Mathf.Sin(Time.time * frequency[i]) * amplitude[i];
            m_transform.position = new Vector3(m_transform.position.x, newY, m_transform.position.z);

            // 更新 rotation
            m_transform.rotation = targetRotation;

            Debug.Log(frequency[i]);
            Debug.Log(amplitude[i]);
        }

        // 根据速度和半径计算旋转角度
         float CalculateRotationAngle(float speed, float radius)
        {
            float circumference = 2f * Mathf.PI * radius;  // 圆周长
            float distance = speed * Time.deltaTime;  // 物体在当前帧行进的距离
            float rotationAngle = -360f * distance / circumference;  // 根据行进距离计算旋转角度
            return rotationAngle;
        }


    }

    
    void GenerateRandomValues()
    {
        for (int i = 0; i < 50;  i++)
        {
            frequency[i] = Random.Range(0.6f, 1.5f);
            amplitude[i] = Random.Range(1f, 3f);

            minSpeed[i] = Random.Range(0f, 30f);
            maxSpeed[i] = Random.Range(45f, 60f);
            currentSpeed[i] = minSpeed[i];

           

        }
    }


    /// <summary>
    /// /////
    /// </summary>
    //private void OnApplicationQuit()
    //{
    //    DestroyFilesInFolder(folderPath);
    //}

    //private void DestroyFilesInFolder(string folderPath)
    //{
    //    if (Directory.Exists(folderPath))
    //    {
    //        DirectoryInfo directory = new DirectoryInfo(folderPath);
    //        FileInfo[] files = directory.GetFiles();

    //        foreach (var file in files)
    //        {
    //            file.Delete();

    //        }

    //        Debug.Log("文件夹下的所有文件已销毁：" + folderPath);
    //    }
    //    else
    //    {
    //        Debug.LogWarning("文件夹不存在：" + folderPath);
    //    }
    //}


}
