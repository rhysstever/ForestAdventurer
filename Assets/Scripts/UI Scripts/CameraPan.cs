using System.Collections;
using UnityEngine;

public class CameraPan : MonoBehaviour
{
    [SerializeField]
    private Transform startingPos, endingPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetCameraPosition()
    {
        Camera.main.transform.position = new Vector3(
            startingPos.position.x,
            startingPos.position.y,
            Camera.main.transform.position.z);
    }

    public void PanCameraDown() {
        StartCoroutine(PanDown());
    }

    IEnumerator PanDown() {
        while(transform.position.y > endingPos.position.y)
        {
            // If the user clicks while panning, the panning is skipped
            if(Input.GetMouseButton(0))
            {
                break;
            }

            transform.position = new Vector3(
                transform.position.x,
                transform.position.y - 0.01f,
                transform.position.z
            );

            yield return new WaitForSeconds(0.01f);
        }

        // Readjust y position to be exact
        transform.position = new Vector3(
            transform.position.x,
            endingPos.position.y,
            transform.position.z
        );

        GameManager.instance.ChangeMenuState(MenuState.CharacterSelect);
        yield return null;
    }
}
