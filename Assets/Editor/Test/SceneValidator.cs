using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class SceneValidator {
    //To do: test that scene is valid
    // Player is tagged as Player
    // Navigator is tagged as Navigator
    // Possibly that Navigator works
    [Test]
    public void PlayerExists()
    {
        GameObject go = GameObject.FindGameObjectWithTag("Player");
        Assert.IsNotNull(go);
    }

    [Test]
    public void NaviagtorExists()
    {
        GameObject go = GameObject.FindGameObjectWithTag("Navigator");
        Assert.IsNotNull(go);
    }

    [Test]
    public void CameraKnowsPlayer()
    {
        CameraController c = Camera.main.GetComponent<CameraController>();
        Assert.IsNotNull(c);
        Assert.IsNotNull(c.Target);
    }
}
