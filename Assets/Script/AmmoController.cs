using UnityEngine;

public class AmmoController : MonoBehaviour
{
    public AudioClip collectedClip;
    public int LövedékDB;

    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController rubyController = other.GetComponent<RubyController>();

        if (rubyController != null)
        {
            if (rubyController.Lövedék < rubyController.maxLövedék)
            {
                rubyController.ChangeAmmo(LövedékDB);
                Destroy(gameObject);
                rubyController.PlaySound(collectedClip);
            }
        }
    }

}
