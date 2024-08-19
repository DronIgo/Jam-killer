using UnityEngine;

public class TimerObjectDestroyer : MonoBehaviour
{
    public float timeToDestroy = 10f; // in seconds

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeToDestroy -= Time.deltaTime;
        if (timeToDestroy < 0)
        {
            Destroy(this.gameObject);
        }
    }
}
