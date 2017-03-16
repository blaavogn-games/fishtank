using UnityEngine;

public class PlayerFollowers : MonoBehaviour {
    private GameObject[] followers = new GameObject[10];

    public int AddFollower(GameObject g)
    {
        for(int i = 0; i < followers.Length; i++) {
            if(followers[i] == null)
            {
                followers[i] = g;
                return i;
            }
        }
        return -1;
    }

    public void RemoveFollow(GameObject g)
    {
        for(int i = 0; i < followers.Length; i++)
            if(followers[i] == g)
                followers[i] = null;
    }
}
