using UnityEngine;

public class CharacterSelectArrows : MonoBehaviour
{
    // Set in inspector
    [SerializeField]
    private Transform topLeft, topRight, bottomLeft, bottomRight;
    [SerializeField]
    private Vector2 startSize, endSize;
    [SerializeField]
    private float moveFrequency;

    // Set at start
    private float timer;

    void Start()
    {
        SetPositions(startSize);
        timer = 0f;
    }

    void FixedUpdate()
    {
        if(GameManager.instance.CurrentMenuState == MenuState.CharacterSelect) {
            timer += Time.deltaTime;

            if(timer >= moveFrequency) {
                if((Vector2)topRight.localPosition == startSize) {
                    SetPositions(endSize);
                } else {
                    SetPositions(startSize);
                }
                timer = 0f;
            }
        }
    }

    private void SetPositions(Vector2 position) {
        topLeft.transform.localPosition = new Vector3(-position.x, position.y, 0);
        topRight.transform.localPosition = new Vector3(position.x, position.y, 0);
        bottomLeft.transform.localPosition = new Vector3(-position.x, -position.y, 0);
        bottomRight.transform.localPosition = new Vector3(position.x, -position.y, 0);
    }
}
