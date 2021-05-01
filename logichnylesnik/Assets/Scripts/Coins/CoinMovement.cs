using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CoinMovement : MonoBehaviour
{
    [SerializeField] private float _moveToPlayerSpeed = 5f;

    private Action onCoinAdded;

    private bool IsCoinAdded = false;

    public void AddPower(Vector3 direction)
    {
        GetComponent<Rigidbody>().AddForce(direction, ForceMode.Impulse);
    }

    public void StartMoveToPlayer(Transform player, Action onCoinAdded)
    {
        this.onCoinAdded = onCoinAdded;

        StartCoroutine(MoveToPlayer(player));
    }

    private IEnumerator MoveToPlayer(Transform player)
    {
        yield return new WaitForSeconds(1f);

        GetComponent<Collider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;

        do
        {
            transform.position += (player.transform.position - transform.position).normalized * _moveToPlayerSpeed * Time.deltaTime;

            yield return null;
        } while ((transform.position - player.transform.position).magnitude > .1f);

        onCoinAdded?.Invoke();
        IsCoinAdded = true;

        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (!IsCoinAdded)
        {
            onCoinAdded?.Invoke();
        }
    }
}
