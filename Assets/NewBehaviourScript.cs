using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public float minSpeed = 1f; // 最小速度
    public float maxSpeed = 5f; // 最大速度
    public float intervalTime = 2f; // 时间间隔

    private float timer; // 计时器
    public float currentSpeed; // 当前速度
    private bool isIncreasing; // 是否正在增加速度

    private void Start()
    {
        currentSpeed = minSpeed; // 初始速度为最小速度
        isIncreasing = true;
        timer = 0f;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (isIncreasing)
        {
            currentSpeed = Mathf.Lerp(minSpeed, maxSpeed, timer / intervalTime);
            if (timer >= intervalTime)
            {
                isIncreasing = false;
                timer = 0f;
            }
        }
        else
        {
            currentSpeed = Mathf.Lerp(maxSpeed, minSpeed, timer / intervalTime);
            if (timer >= intervalTime)
            {
                isIncreasing = true;
                timer = 0f;
            }
        }

        // 根据当前速度更新物体位置
        transform.position += transform.forward * currentSpeed * Time.deltaTime;
    }
}
