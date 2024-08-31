using UnityEngine;

public class VFXController : MonoBehaviour
{
    public GameObject buff, debuff;
    private float timeCounter; //计时器

    private void Update()
    {
        if (buff.activeInHierarchy) //如果buff处于激活状态
        {
            timeCounter += Time.deltaTime;
            if (timeCounter >= 1.2f)
            {
                timeCounter = 0f;
                buff.SetActive(false);
            }
        }

        if (debuff.activeInHierarchy) //如果debuff处于激活状态
        {
            timeCounter += Time.deltaTime;
            if (timeCounter >= 1.2f)
            {
                timeCounter = 0f;
                debuff.SetActive(false);
            }
        }
    }
}
