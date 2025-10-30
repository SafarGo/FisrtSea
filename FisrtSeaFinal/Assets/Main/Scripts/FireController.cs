using System.Collections;
using UnityEngine;

public class FireController : MonoBehaviour
{
    public float Damage;

    private void Update()
    {
        GameManager.instance.ShipHP -=  Damage;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Robot"))
        {
            Debug.Log("!");
            StartCoroutine(Fixing());
        }
    }


    IEnumerator Fixing()
    {
        yield return new WaitForSeconds(4);
        GameManager.instance.disastersResolved++;
        Destroy(this.gameObject);
    }
}
