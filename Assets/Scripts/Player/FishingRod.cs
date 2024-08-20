using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingRod : MonoBehaviour
{
    public Transform tip;
    public Transform bait;
    public LineRenderer line;
    public float maxRodDistance = 1f;
    Vector3[] linePoses = new Vector3[2];
    public static bool rodActive
    {
        get;
        private set;
    }
    // Start is called before the first frame update
    void Start()
    {
        rodActive = false;
        tip = transform.Find("Tip");
        bait = transform.Find("Bait");
        UpdateComponents();
    }

    public void Throw(Vector2 dir)
    {
        SoundManager.instance.OnBaltDeploy();
        if (dir.sqrMagnitude > maxRodDistance)
            dir = dir.normalized * maxRodDistance;
        bait.localPosition = new Vector3(dir.x, dir.y, 0);
        SetActive(true);
    }

    public void SetActive(bool active)
    {
        rodActive = active;
        UpdateComponents();
    }

    public void UpdateComponents()
    {
        bait.gameObject.SetActive(rodActive);
        line.gameObject.SetActive(rodActive);
    }

    // Update is called once per frame
    void Update()
    {
        if (rodActive)
        {
            linePoses[0] = tip.localPosition;
            linePoses[1] = bait.localPosition;
            line.SetPositions(linePoses);
        }
    }
}
