using UnityEngine;

public class HELP : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Circle"))
        {
            GameManager.instance.ReportVictimRescued();
            Destroy(gameObject);
        }
    }
}
