using UnityEngine;

public class DragAreaPoint : MonoBehaviour {
    DragArea dragArea;
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
        
        if(col.transform.tag == "Enemy")
        {
            dragArea.StopDrag();
            col.transform.GetComponent<Enemy>().SetFrozen();
        }
    }
}
