using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX_Manager : MonoBehaviour
{
    [Header("VFX from Player")]
    public GameObject VFX_Shoot;
    public GameObject VFX_Walk;
    public ParticleSystem VFX_TakeDoor;

    [Header("VFX from Bullet")]
    public ParticleSystem VFX_ImpactWall;
    public ParticleSystem VFX_ImpactEnemy;

    [Header("VFX from Enemy")]
    public ParticleSystem VFX_E1_Shoot;
    //public ParticleSystem VFX_IdleEnemy;

    bool bFactor;

    /*public void PlayWalk(bool bFactor)
    {
        if (bFactor)
        {
            VFX_Walk.Play();
        }
        else
        {
            VFX_Walk.Stop();
        }
    }*/

    public IEnumerator PlayWalk()
    {
        VFX_Walk.SetActive(true);
        yield return new WaitForSeconds(.5f);
        VFX_Walk.SetActive(false);
    }

    public IEnumerator PlayShoot(Quaternion _RotationToVFX)
    {
        //VFX_Shoot.SetActive(true);
        VFX_Shoot.transform.rotation = _RotationToVFX;
        VFX_Shoot.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(.5f);
        //VFX_Shoot.SetActive(false);
    }

    /*public void StopAllParticles()
    {
        if(pWalk != null)
        {
            pWalk.Stop();
        }
    }*/

    private void Start()
    {
        VFX_Shoot.GetComponent<ParticleSystem>().Pause();
    }
}
