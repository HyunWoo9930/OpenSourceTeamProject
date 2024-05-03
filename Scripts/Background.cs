using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
<<<<<<< Updated upstream
    // Start is called before the first frame update
    void Start()
    {
        debug.log("sdaa");
    }

    // Update is called once per frame
=======
    private float moveSpeed = 4f;
>>>>>>> Stashed changes
    void Update()
    {
        transform.position += Vector3.down * moveSpeed * Time.deltaTime;
        if (transform.position.y < -10)
        {
            transform.position += new Vector3(0, 10 * 2f, 0);
        }
    }
}
