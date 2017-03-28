using UnityEngine;

public class PlayerFollowers : MonoBehaviour {
    private GameObject[] followers = new GameObject[5];
    private int targetIndex = -1;
    //Invariant:
    // followers[targetIndex] != null or (targetIndex == -1) -> followers[i] == null

    public int AddFollower(GameObject g)
    {
        for(int i = 0; i < followers.Length; i++) {
            if(followers[i] == null)
            {
                followers[i] = g;
                targetIndex = i;
                return i;
            }
        }
        return -1;
    }

    public void RemoveFollower(GameObject g)
    {
        targetIndex = -1;
        for(int i = 0; i < followers.Length; i++) {
            if (followers[i] == g)
                followers[i] = null;
            if(followers[i] != null)
                targetIndex = i;
        }
    }

    public Transform GetTarget()
    {
        return (targetIndex == -1) ? transform : followers[targetIndex].transform;
    }
}
