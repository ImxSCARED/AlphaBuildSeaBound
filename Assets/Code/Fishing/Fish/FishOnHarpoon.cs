using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FishOnHarpoon : MonoBehaviour
{
    //Cannon cannonOrigin;
    //public Rigidbody rb;
    //bool didHitFish;
    //public bool returning;
    //Fish hitFish;

    //void Start()
    //{
    //    rb = GetComponent<Rigidbody>();
    //}
    //public void SetIdentifiers(Cannon cannon)
    //{
    //    cannonOrigin = cannon;
    //}
    //public bool IsHitFish()
    //{
    //    return hitFish;
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    //Makes sure fish cant be hit when returning to cannon
    //    if(!returning)
    //    {
    //        if (other.tag == "Fish")
    //        {
    //            other.gameObject.SetActive(false);
    //            rb.Sleep();
    //            rb.isKinematic = true;

    //            didHitFish = true;
    //            hitFish = other.GetComponent<Fish>();

    //            cannonOrigin.HarpoonHitFish(hitFish);

    //            GameObject hookedFish = Instantiate(hitFish.theFish.fishBody, Vector3.zero, Quaternion.identity);
    //            hookedFish.transform.SetParent(transform, true);
    //            hookedFish.transform.localPosition = Vector3.zero;
    //            hookedFish.transform.localRotation = Quaternion.identity;


    //        }
    //    }
    //    else
    //    {
    //        if(other.tag == "Cannon")
    //        {
    //            if (didHitFish)
    //            {
    //                cannonOrigin.HarpoonReturned(true);
    //                Destroy(hitFish.gameObject);
    //            }
    //            else
    //            {
    //                cannonOrigin.HarpoonReturned(false);
    //            }
    //            Destroy(transform.gameObject);
    //        }   
    //    }
        
        
    //}

}
