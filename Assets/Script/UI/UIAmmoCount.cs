using UnityEngine;
using UnityEngine.UI;

public class UIAmmoCount : MonoBehaviour
{
    public static UIAmmoCount Instance { get; private set; }
    public Text countText;

    // Start is called before the first frame update
    void Awake() => Instance = this;

    public void SetAmmo(int count, int max) => countText.text = "X " + count + " / " + max;
}
