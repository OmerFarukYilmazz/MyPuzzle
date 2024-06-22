using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameManager _gameManager;   

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(_gameManager._boardCenter.x, _gameManager._boardCenter.y, transform.position.z);
    }
    private void Awake()
    {
        _gameManager = Object.FindObjectOfType<GameManager>();
        cameraSet();
    }
    public void cameraSet()
    {        
        float orthoSize = ((float)_gameManager._width * (float)Screen.height / (float)Screen.width * 0.503f);
        Camera.main.orthographicSize = orthoSize;
    }
    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 orignalPosition = transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = transform.position.x + Random.Range(-0.5f, 0.5f) * magnitude;
            float y = transform.position.y + Random.Range(-0.5f, 0.5f) * magnitude;

            transform.position = new Vector3(x, y, -10f);
            elapsed += Time.deltaTime;
            yield return 0;
        }
        transform.position = orignalPosition;
    }
}
