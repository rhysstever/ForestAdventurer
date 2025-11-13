using System.Collections;
using UnityEngine;

public class CameraPan : MonoBehaviour
{
    float moveAmount = 1.87f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PanCameraDown() {
        StartCoroutine(PanDown());
    }

    IEnumerator PanDown() {

        for(int i = 0; i < moveAmount * 100; i++) {
            transform.position = new Vector3(
                transform.position.x,
                transform.position.y - 0.01f,
                transform.position.z
            );
            yield return new WaitForSeconds(0.01f);
        }

        GameManager.instance.ChangeMenuState(MenuState.CharacterSelect);
        yield return null;
    }
}
