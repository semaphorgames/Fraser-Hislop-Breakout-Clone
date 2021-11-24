using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Brick : NetworkBehaviour
{
    public MeshRenderer meshRenderer;

    [Server]
    public void SetMaterial(Material material)
    {
        meshRenderer.material = material;
    }

    [ServerCallback]
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Disable();

        GameController.Instance.IncreaseScore();
        BricksController.Instance.DecrementBricksActive();
    }

    // Disable for everyone
    [Server]
    public void Disable()
    {
        gameObject.SetActive(false);
    }

    // Enable for everyone
    [Server]
    public void Enable()
    {
        gameObject.SetActive(true);
    }
}
