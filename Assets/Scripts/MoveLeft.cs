using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    private float speed = 10;


    // Update is called once per frame
    void Update()
    {
        MoveToTheLeft();
        DeleteOutOfScene();
    }

    private void MoveToTheLeft()
    {
        if(!GameManager.gameOver)
        {
            transform.Translate(Vector3.left * Time.deltaTime * speed);
        }
    }

    private void DeleteOutOfScene()
    {
        if (gameObject.tag == "Obstacle" && transform.position.y < 0)
        {
            Destroy(gameObject);
        }
    }
}
