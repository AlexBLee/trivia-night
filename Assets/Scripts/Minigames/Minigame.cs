using UnityEngine;

public abstract class Minigame : MonoBehaviour
{
    public virtual void Initialize()
    {
        gameObject.SetActive(true);
    }

    protected virtual void Play()
    {

    }

    protected virtual void FinishGame()
    {
        gameObject.SetActive(false);
    }
}
