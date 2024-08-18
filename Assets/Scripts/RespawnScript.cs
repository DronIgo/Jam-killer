using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class RespawnScript : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.instance.SaveStateInSail();
        SceneManager.LoadScene(0);
    }
}