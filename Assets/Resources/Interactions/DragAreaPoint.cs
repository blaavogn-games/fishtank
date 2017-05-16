using UnityEngine;
using UnityEngine.SceneManagement;

public class DragAreaPoint : MonoBehaviour {
    DragArea dragArea;
    public Level2End level2End;
    public GameObject winEnemy;

    void Start()
    {
        dragArea = transform.parent.GetComponent<DragArea>();
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.transform.tag == "Player")
        {
            col.transform.GetComponent<Player>().Kill(Player.DeathCause.SUCKED);
            return;
        }
        if (col.gameObject == winEnemy && SceneManager.GetActiveScene() == SceneManager.GetSceneByName("2nd-Level"))
        {
            level2End.Run();
        }
        if(col.transform.tag == "Enemy")
        {
            dragArea.StopDrag();
            col.transform.GetComponent<Enemy>().SetFrozen();
        }
    }
}
