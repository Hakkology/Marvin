using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlackHoleSkillController : MonoBehaviour {
    [SerializeField] private GameObject hotkeyPrefab;
    [SerializeField] private List<KeyCode> KeyCodeList;

    [Header("Black hole info")]
    private float maxSize = 15;
    private float growSpeed = 1;
    private float shrinkSpeed;
    private float blackHoleTimer;

    private bool canGrow = true;
    private bool canShrink;
    private bool canCreateHotkeys = true;

    [Header("Clone info")]
    private int amountOfAttacks = 4;
    private float cloneAttackCooldown = 0.3f;
    private float cloneAttackTimer;
    private bool canAttack;

    private List<Transform> targets = new List<Transform>();
    private List<GameObject> hotkeyContainer = new List<GameObject>();
    private Player player;

    public bool playerCanExitState {get; private set;}
    private bool playerDisappearence = true;

    void Start() => player = PlayerManager.Instance.player;
    void Update()
    {
        cloneAttackTimer -= Time.deltaTime;
        blackHoleTimer -= Time.deltaTime;

        if(blackHoleTimer < 0){

            blackHoleTimer = Mathf.Infinity;

            if(targets.Count >0)
                ReleaseCloneAttack();
            else
                ShrinkHole();
        }

        if (Input.GetKeyDown(KeyCode.R))
            ReleaseCloneAttack();
        
        ApplyClones();

        if (canGrow && !canShrink)
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector3(maxSize, maxSize), growSpeed * Time.deltaTime);

        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector3(-1, -1), shrinkSpeed * Time.deltaTime);
            if (transform.localScale.x < 0)
                Destroy(gameObject);
        }
    }

    public void SetupBlackHole(float _maxSize, float _growSpeed, float _shrinkSpeed, int _amountOfAttacks, float _cloneAttackCooldown, float _blackHoleDuration){
        maxSize = _maxSize;
        growSpeed = _growSpeed;
        shrinkSpeed = _shrinkSpeed;
        amountOfAttacks = _amountOfAttacks;
        cloneAttackCooldown = _cloneAttackCooldown;
        blackHoleTimer = _blackHoleDuration;
    }

    void OnTriggerEnter2D(Collider2D other) {

        Enemy enemy = other.GetComponent<Enemy>();

        if(enemy != null)
        {
            enemy.FreezeTime(true);
            CreateHotkey(other, enemy);
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        
        Enemy enemy = other.GetComponent<Enemy>();

        if (enemy != null)
            enemy.FreezeTime(false);
    }

    #region Clones
    private void ReleaseCloneAttack()
    {
        if(targets.Count <= 0)
            return;

        DestroyHotkeys();
        canAttack = true;
        canCreateHotkeys = false;

        if(playerDisappearence){
            playerDisappearence = false;
            player.fx.TurnInvisible(true);  
        }

    }
    public void AddEnemyToList(Transform transform) => targets.Add(transform);

    private void ApplyClones()
    {
        if (cloneAttackTimer < 0 && canAttack && amountOfAttacks > 0)
        {
            cloneAttackTimer = cloneAttackCooldown;
            int randomIndex = Random.Range(0, targets.Count);

            float xOffset;
            if (Random.Range(0, 100) > 50)
                xOffset = 1.5f;
            else
                xOffset = -1.5f;

            SkillManager.Instance.cloneSkill.CreateClone(targets[randomIndex], new Vector2(xOffset, 0));
            amountOfAttacks--;

            if (amountOfAttacks <= 0)
                Invoke("ShrinkHole", .8f);
        }
    }

    private void ShrinkHole()
    {
        DestroyHotkeys();
        playerCanExitState = true;
        //player.ExitTrance();
        canShrink = true;
        canAttack = false;
    }
    #endregion

    #region Hotkeys
    private void CreateHotkey(Collider2D other, Enemy enemy)
    {
        if (KeyCodeList.Count <= 0)
            return;
        
        if(!canCreateHotkeys)
            return;

        GameObject newHotKey = Instantiate(hotkeyPrefab, other.transform.position + new Vector3(0, 1), Quaternion.identity);
        hotkeyContainer.Add(newHotKey);

        KeyCode chosenKey = KeyCodeList[Random.Range(0, KeyCodeList.Count)];
        KeyCodeList.Remove(chosenKey);

        BlackHoleHotkeyController controller = newHotKey.GetComponent<BlackHoleHotkeyController>();
        controller.SetupHotKey(chosenKey, enemy.transform, this);
    }

    private void DestroyHotkeys(){
        if (hotkeyContainer.Count <= 0)
            return;
        
        for (int i = 0; i < hotkeyContainer.Count; i++)
            Destroy(hotkeyContainer[i]);
    }
    #endregion
}