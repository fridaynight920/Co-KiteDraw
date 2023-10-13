using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public float minSpeed = 1f; // ��С�ٶ�
    public float maxSpeed = 5f; // ����ٶ�
    public float intervalTime = 2f; // ʱ����

    private float timer; // ��ʱ��
    public float currentSpeed; // ��ǰ�ٶ�
    private bool isIncreasing; // �Ƿ����������ٶ�

    private void Start()
    {
        currentSpeed = minSpeed; // ��ʼ�ٶ�Ϊ��С�ٶ�
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

        // ���ݵ�ǰ�ٶȸ�������λ��
        transform.position += transform.forward * currentSpeed * Time.deltaTime;
    }
}
