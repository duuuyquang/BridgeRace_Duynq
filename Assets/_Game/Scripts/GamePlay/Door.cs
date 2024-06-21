using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Door : MonoBehaviour
{
    [SerializeField] Transform doorAvt;
    [SerializeField] Renderer doorRenderer;
    [SerializeField] protected ColorDataSO colorDataSO;
    private bool isOpen = false;

    private void OnTriggerEnter(Collider collider)
    {
        //TODO: fix - done
        Character character = Cache.GenCharacters(collider);
        if (character is Player && character.IsMovingBack)
        {
            return;
        }

        ChangeColor(character.ColorType);
        StartCoroutine(IEOpen());
    }

    private void ChangeColor(ColorType type)
    {
        //doorRenderer.material.color = DataByType.Colors[(int)type];
        doorRenderer.material = colorDataSO.GetMat(type);
    }

    private IEnumerator IEOpen()
    {
        yield return new WaitForSeconds(.5f);

        while (doorAvt.localPosition.y > -4f)
        {
            doorAvt.localPosition += Vector3.down * 0.1f;
            yield return new WaitForEndOfFrame();
        }

        isOpen = true;
    }
}
