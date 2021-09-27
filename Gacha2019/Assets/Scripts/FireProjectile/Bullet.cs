using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float m_Speed;

    [SerializeField]
    private EEntityColor m_Color = EEntityColor.Red;

    [SerializeField]
    private int m_Damage = 1;

    private Entity m_Shooter = null;

    [SerializeField]
    private float m_LifeTime = 2.0f;

    #region Accessor
    public float Speed { get => m_Speed; }

    public EEntityColor Color { get => m_Color; }

    public int Damage { get => m_Damage; }

    public Entity Shooter { get => m_Shooter; set => m_Shooter = value; }

    #endregion

    public void Shoot(Vector3 _NormalizedDirection, Entity _Shooter)
    {
        Rigidbody rigidBody = GetComponent<Rigidbody>();

        if (rigidBody == null)
        {
            Debug.LogError("Bullet doesn't have a rigid body and cannot be shot");
            return;
        }

        rigidBody.velocity = _NormalizedDirection * m_Speed;
        m_Shooter = _Shooter;
    }

    private void Start()
    {
        Destroy(gameObject, m_LifeTime);
        AkSoundEngine.PostEvent("Play_Impact_Laser", gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
            AkSoundEngine.PostEvent("Play_Impact_Laser", gameObject);
        }
    }
}
