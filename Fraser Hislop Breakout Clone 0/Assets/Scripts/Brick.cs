using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void SetMaterial(Material material)
    {
        meshRenderer.material = material;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Disable();

        GameController.Instance.IncreaseScore();
        BricksController.Instance.DecrementBricksActive();
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    public void Enable()
    {
        gameObject.SetActive(true);
    }
}
