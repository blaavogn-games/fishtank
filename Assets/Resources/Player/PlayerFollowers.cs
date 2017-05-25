using UnityEngine;

public class PlayerFollowers : MonoBehaviour {
    public Player player;
    private GameObject[] followers = new GameObject[5];
    private int target = 0;

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

    public void RemoveFollower(GameObject g)
    {
        for(int i = 0; i < followers.Length; i++) {
            if (followers[i] == g)
                followers[i] = null;
        }
    }

    public void Update()
    {
        target = 0;
    }

    public Transform GetTarget()
    {
        if (player.state == Player.State.DYING)
            return null;
        for (int i = 0; i < followers.Length; i++)
            if (followers[(i + target)%followers.Length] != null) 
                return followers[(i + target++) % followers.Length].transform;
        return transform;
    }
}
