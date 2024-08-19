using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishFactory : MonoBehaviour
{
    public GameObject fish1;
    public GameObject fish2;
    public GameObject fish3;
    public GameObject fish4;

    private GameObject fishInScene;

    // Start is called before the first frame update
    void Start()
    {
        fishInScene = Instantiate(fish1);
        fishInScene.transform.position = this.gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        fishInScene.transform.position += new Vector3(Time.deltaTime, 0, 0);
    }
}
