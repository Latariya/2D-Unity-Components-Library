using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipSprite : MonoBehaviour
{

    private bool isFaceRight;

    void Start()
    {
        isFaceRight = transform.localScale.x > 0;
    }

    void Update()
    {
        float xInput = Input.GetAxisRaw("Horizontal");

        if (isFaceRight && xInput < 0 || !isFaceRight && xInput > 0)
        {
            FlipSpriteProcess();
            isFaceRight = !isFaceRight;
        }
        
    }

    private void FlipSpriteProcess()
    {
        Vector2 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
