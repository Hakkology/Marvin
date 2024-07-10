using TMPro;
using UnityEngine;

public class BlackHoleHotkeyController : MonoBehaviour {
    
    private SpriteRenderer sr;
    private KeyCode hotkey;
    private TextMeshProUGUI hotkeyText;

    private Transform enemy;
    private BlackHoleController blackHoleController;

    public void SetupHotKey(KeyCode _hotkey, Transform _enemy, BlackHoleController _blackHoleController) {

        sr = GetComponent<SpriteRenderer>();
        hotkeyText = GetComponentInChildren<TextMeshProUGUI>();
        enemy = _enemy;
        blackHoleController = _blackHoleController;
        hotkey = _hotkey;
        hotkeyText.text = hotkey.ToString();
    }

    void Update() {
        if (Input.GetKeyDown(hotkey)) {
            blackHoleController.AddEnemyToList(enemy);
            hotkeyText.color = Color.clear;
            sr.color = Color.clear;
        }

    }
}