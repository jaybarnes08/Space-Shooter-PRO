using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackLaser : MonoBehaviour
{
    [SerializeField] float _speed = 3f;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.up * _speed * Time.deltaTime);

        if(transform.position.y > 6.5f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();

            if(player != null)
            {
                player.Damage();
                Destroy(this.gameObject);
            }
        }
    }
}
