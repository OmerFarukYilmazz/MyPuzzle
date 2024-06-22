using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ShakeObject : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            shakeObje();
        }
    }
    void shakeObje()
    {
        //Vector3 temp = transform.position
        const float duration = 0.3f;
        const float strength = 1f;
        transform.DOShakePosition(duration, strength);
    }

}
