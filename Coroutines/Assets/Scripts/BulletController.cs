using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public BridgeController bridgeController;

    private void Update()
    {
        Vector3 mpos = Input.mousePosition;
        mpos.z = 40;
        Vector3 mvel = Camera.main.ScreenToWorldPoint(mpos) - Camera.main.transform.position;
        Debug.DrawRay(Camera.main.transform.position, mvel);
        
        if (Input.GetMouseButtonDown(0) && bulletPrefab != null && !bridgeController.reversing)
        {
            var bullet = Instantiate(bulletPrefab);
            bullet.transform.position = Camera.main.transform.position;
            bullet.GetComponent<Rigidbody>().AddForce(mvel, ForceMode.VelocityChange);

            bridgeController.recording = true;
        }
    }
}
