using UnityEngine;
using Photon.Pun;

public class FlipCar : MonoBehaviourPunCallbacks
{
    public KeyCode flipKey = KeyCode.F; // Key to trigger the flip

    void Update()
    {
        if (photonView.IsMine && Input.GetKeyDown(flipKey))
        {
            Flip();
            photonView.RPC("FlipRPC", RpcTarget.OthersBuffered);
        }
    }

    void Flip()
    {
        // Reset rotation to zero on the x and z axes, maintaining current y rotation
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);

        // Optionally, you can slightly adjust the position to avoid ground collision issues
        transform.position += Vector3.up * 0.5f; // Adjust the value as needed
    }

    [PunRPC]
    void FlipRPC()
    {
        Flip();
    }
}