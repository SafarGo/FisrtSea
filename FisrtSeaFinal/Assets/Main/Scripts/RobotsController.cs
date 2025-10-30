using UnityEngine;

public class RobotsController : MonoBehaviour
{
    public Transform goal;
    private bool check;
    public void Go()
    {
        if(goal != null)
        gameObject.transform.position = Vector3.MoveTowards(transform.position, goal.position, Time.deltaTime);
    }

    void Update()
    {
        Go();
    }

    public void SetGoal(Transform tmp)
    {
        goal = tmp;
    }
}
