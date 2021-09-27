using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    // Targets Variables // 

    public GameObject Player1;
    // Other players

    // Stats Variables //
    public float SmoothSpeed;
    public Vector3 offset;
    private Vector3 target;
    private Vector3 velocity = Vector3.zero;

    public bool isShaking = false;
    public float ShakePower;
    public float ShakeDuration;
    private float ShakeElapse;
    private Vector3 originalPosShake;

    // Camera functions // 

    void LateUpdate()
    {


        Vector3 DesiredPosition = Player1.gameObject.transform.position + offset;
        Vector3 SmoothedPosition = Vector3.SmoothDamp(transform.position, DesiredPosition, ref velocity, SmoothSpeed);
        transform.position = SmoothedPosition;
        //transform.LookAt(Player1.gameObject.transform.position);


        if (isShaking == true)
        {
            originalPosShake = gameObject.transform.position;
            CamShake();
        }

    }

    private void CamShake()
    {
        ShakeElapse += Time.deltaTime;

        float x = Random.Range(-1f, 1f) * ShakePower * (ShakeDuration - ShakeElapse);
        float y = Random.Range(-1f, 1f) * ShakePower * (ShakeDuration - ShakeElapse);

        transform.localPosition = new Vector3(originalPosShake.x + x, originalPosShake.y + y, originalPosShake.z);

        if (ShakeElapse >= ShakeDuration)
        {
            transform.localPosition = originalPosShake;
            isShaking = false;
            ShakeElapse = 0f;
        }

    }

    public void CamGoShake()
    {

        isShaking = true;
    }


}
