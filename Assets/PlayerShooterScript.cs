using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooterScript : MonoBehaviour
{
    public GameObject ammoBasicPrefab;
    public GameObject ammoPowerPrefab;
    
    [ColorUsage(true,true)]
    public Color powerEmissionColor;

    void Start()
    {
        UpdateEmissionFromPower(GetComponent<Player>().ammoPowerCapacity);
    }

    void Update()
    {
        Player playerComponent = GetComponent<Player>();
        
        Vector3 direction = transform.forward;
        if (Input.GetButtonDown("Fire1"))
        {
            Vector3 lFirePoint = transform.position - transform.right * 12.5f + transform.forward * 15;
            Vector3 rFirePoint = transform.position + transform.right * 12.5f + transform.forward * 15;
            lFirePoint.z = 0;
            rFirePoint.z = 0;
            
            InstantiateAmmo(ammoBasicPrefab, lFirePoint, direction);
            InstantiateAmmo(ammoBasicPrefab, rFirePoint, direction);
        }
        
        if (Input.GetButtonDown("Fire2") && playerComponent.ammoPowerCapacity > 0)
        {
            Vector3 firePoint = transform.position + direction * 25f;
            firePoint.z = 0;
            InstantiateAmmo(ammoPowerPrefab, firePoint, direction);

            --playerComponent.ammoPowerCapacity;
        }
        
        UpdateEmissionFromPower(GetComponent<Player>().ammoPowerCapacity);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Powerup"))
        {
            Player playerComponent = GetComponent<Player>();
            playerComponent.ammoPowerCapacity++;
            
            Destroy(collision.collider.gameObject);
        }
    }

    void InstantiateAmmo(GameObject ammoPrefab, Vector3 firePoint, Vector3 direction)
    {
        GameObject ammo = Instantiate(ammoPrefab, firePoint, Quaternion.identity);
        ammo.GetComponent<Rigidbody>().velocity = direction * ammo.GetComponent<Ammo>().speed;
        ammo.GetComponent<Ammo>().shooter = gameObject;
    }

    void UpdateEmissionFromPower(uint ammoPowerCapacity)
    {
        if (ammoPowerCapacity > 0)
        {
            GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", powerEmissionColor);
        }
        else
        {
            GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.black);
        }
    }
}
