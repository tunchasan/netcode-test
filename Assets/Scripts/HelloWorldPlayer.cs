using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class HelloWorldPlayer : NetworkBehaviour
{
    private NetworkVariable<Vector3> _position = new();
    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            Move();
        }
    }

    public void Move()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            var randomPosition = GetRandomPositionOnPlane();
            transform.position = randomPosition;
            _position.Value = randomPosition;
        }

        else
        {
            SubmitPositionRequestServerRpc();
        }
    }

    private Vector3 GetRandomPositionOnPlane()
    {
        return new Vector3(Random.Range(-3f, 3f), 1F, Random.Range(-3F, 3F));
    }

    [ServerRpc]
    private void SubmitPositionRequestServerRpc(ServerRpcParams rpcParams = default)
    {
        _position.Value = GetRandomPositionOnPlane();
    }

    private void Update()
    {
        transform.position = _position.Value;
    }
}