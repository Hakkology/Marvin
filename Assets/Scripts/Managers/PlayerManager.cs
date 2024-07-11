using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    public Player player;

    void Awake() {
        if (Instance != null)
            Destroy(Instance.gameObject);
        else
            Instance = this;
    }
}
