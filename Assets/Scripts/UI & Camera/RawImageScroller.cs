using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

[RequireComponent(typeof(RawImage))]
public class RawImageScroller : MonoBehaviour
{
    /// <summary>
    /// The raw image
    /// </summary>
    private RawImage image;

    /// <summary>
    /// Direction to move image
    /// </summary>
    [SerializeField] private Vector2 direction;

    /// <summary>
    /// Speed to move image
    /// </summary>
    [SerializeField] private float magnitude;

    /// <summary>
    /// Vector has direction and magnitude, wow
    /// </summary>
    private Vector2 vector => -direction.normalized * magnitude; 

    private void Start()
    {
        image = GetComponent<RawImage>(); 
    }

    /// <summary>
    /// Offsets raw image ui in direction with magnitude, wow
    /// </summary>
    private void Update()
    {
        image.uvRect = new Rect(image.uvRect.x + vector.x, image.uvRect.y + vector.y, image.uvRect.width, image.uvRect.height);
    }
}
