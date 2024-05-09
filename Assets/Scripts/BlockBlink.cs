using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBlink : MonoBehaviour
{
    [HideInInspector] public bool willBeDestroyed = false;

    private Color currColor;

    private Renderer renderer;
    // Start is called before the first frame update
    void Start()
    {
        renderer = transform.GetComponent<Renderer>();
        currColor = renderer.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (willBeDestroyed)
        {
            var newColor = new Color(currColor.r + 0.5f, currColor.b, currColor.g, 0.2f);
            renderer.material.color = Color.Lerp(currColor, newColor, Mathf.PingPong(Time.time * GameManager.Instance.blockBlinkSpeed, 1));
        }
        
    }
}
