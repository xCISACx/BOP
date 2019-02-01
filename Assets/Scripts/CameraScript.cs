using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    public GameObject Target;
    public float DollyDistance = -10.0f;
    public AnimationCurve DeltaXDistance;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Target == null)
        {
            return;
        }

        //A ideia � termos uma camera que vai tentar reduzir a dist�ncia  entre o Mario e ela de uma forma vari�vel consoante a curva de movimento definida.
        var delta = Target.transform.position - transform.position;
        var distance = Vector3.Distance(Target.transform.position, transform.position);
        transform.position = Vector3.MoveTowards(transform.position, transform.position + delta * DeltaXDistance.Evaluate(distance), distance);
        transform.position = new Vector3(transform.position.x, transform.position.y, DollyDistance);
    }
}
