using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public float bulletSpeed;
    public bool isRotating;

    public Transform ProjectileSpawnPoint;
    public GameObject ProjectileObj;

    PlayerCharacter PlayerCharacterScr;

    private void Awake()
    {
        PlayerCharacterScr = GetComponent<PlayerCharacter>();
    }

    public void FireShuriken()
    {
        var bullet = Instantiate(ProjectileObj, ProjectileSpawnPoint.position, transform.rotation);
        bullet.GetComponent<Rigidbody2D>().velocity = transform.right * bulletSpeed;
    }
}