using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

class BattleUIManager : MonoBehaviour
{
    public static BattleUIManager Instance;

    public Text battleInfo;
    public GameObject enemyMarkerPrefab;
    public List<GameObject> enemyMarkerList;
    public GameObject playerPanel;
    public List<Button> playerButtonList;
    public Button activeBtn;

    public bool anyHighlights = false;
    public int activeBtnIndex = 0;
    public bool btnSelected = false;
    public int activeCharacterIndex = 0;
    public List<Character> targetList;
    public bool listCreated = false;

    public bool enemyTurn = false;

    private void Start()
    {
        Instance = this;
        enemyMarkerList = new List<GameObject>();
        targetList = new List<Character>();
        for (int i = 0; i < playerPanel.transform.childCount; i++)
        {
            playerButtonList.Add(playerPanel.transform.GetChild(i).GetComponent<Button>());
        }
    }
    private void Update()
    {
        if (GameManager.Instance.gameState == GameState.PlayerTurn) // player menu disappears if it isn't player turn
        {
            if (!btnSelected) // button isn't selected
            {
                playerPanel.SetActive(true);
                if (!anyHighlights)
                {
                    changeActiveButton(activeBtnIndex);
                    anyHighlights = true;
                }
                if (Input.GetKeyDown(KeyCode.D))
                {
                    if (activeBtnIndex < playerButtonList.Count - 1)
                        changeActiveButton(++activeBtnIndex);
                }
                else if (Input.GetKeyDown(KeyCode.A))
                {
                    if (activeBtnIndex > 0)
                        changeActiveButton(--activeBtnIndex);
                }
                else if (Input.GetKeyDown(KeyCode.S))
                {
                    if (activeBtnIndex < playerButtonList.Count - 1)
                    {
                        activeBtnIndex += 2;
                        if (activeBtnIndex > playerButtonList.Count - 1)
                            activeBtnIndex = playerButtonList.Count - 1;
                        changeActiveButton(activeBtnIndex);
                    }
                }
                else if (Input.GetKeyDown(KeyCode.W))
                {
                    if (activeBtnIndex > 0)
                    {
                        activeBtnIndex -= 2;
                        if (activeBtnIndex < 0)
                            activeBtnIndex = 0;
                        changeActiveButton(activeBtnIndex);
                    }
                }
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    btnSelected = true;
                    anyHighlights = false;
                }
            }
            else // button is selected
            {
                if (!listCreated)
                {
                    if (activeBtn.GetComponent<BaseButton>().target == BaseButton.Target.Enemy)
                    {
                        foreach (Character character in BattleManager.characterList)
                        {
                            if (character != null && !character.isPlayable)
                                targetList.Add(character);
                        }
                        listCreated = true;
                    }
                    else if (activeBtn.GetComponent<BaseButton>().target == BaseButton.Target.Ally)
                    {
                        foreach (Character character in BattleManager.characterList)
                        {
                            if (character != null && character.isPlayable)
                                targetList.Add(character);
                        }
                        listCreated = true;
                    }
                    else if (activeBtn.GetComponent<BaseButton>().target == BaseButton.Target.Self)
                    {
                        targetList.Add(BattleManager.Instance.activeCharacter);
                        listCreated = true;
                    }
                }
                if (!anyHighlights)
                {
                    markTarget(activeCharacterIndex);
                    anyHighlights = true;
                }
                if (Input.GetKeyDown(KeyCode.S) && !(targetList.Count==1)) // kinda reversed cause of placement in inspector
                {
                    activeCharacterIndex = (activeCharacterIndex + 1) % 2;
                    markTarget(activeCharacterIndex);
                }
                else if (Input.GetKeyDown(KeyCode.W)&& !(targetList.Count == 1))
                {
                    activeCharacterIndex = (activeCharacterIndex + 1) % 2;
                    markTarget(activeCharacterIndex);
                }
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    activeBtn.GetComponent<BaseButton>().Ability(BattleManager.Instance.activeCharacter, targetList[activeCharacterIndex]); // FINALLY ABILITY OCCURS
                    clearMarkers();
                    btnSelected = false;
                    BattleManager.Instance.activeCharacter = BattleManager.Instance.advanceTurn(BattleManager.characterList.IndexOf(BattleManager.Instance.activeCharacter) + 1);
                    listCreated = false;
                    targetList.Clear();
                    activeCharacterIndex = 0; // fixes a bug where if any character in index 1 was chosen before the wait action, the game crashed
                }
            }
        }
        else
        {
            playerPanel.SetActive(false);
            targetList.Clear();
            //activeBtn = null;

            if (GameManager.Instance.gameState == GameState.EnemyTurn)
            {
                StartCoroutine(BattleManager.Instance.EnemyTurn());
            }
            else if (GameManager.Instance.gameState == GameState.Busy)
                StopCoroutine(BattleManager.Instance.EnemyTurn());
        }
    }

    public void Highlight(int index)
    {
        if (index < playerButtonList.Count)
        {
            removeHighlights();
            playerButtonList[index].transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    public void removeHighlights()
    {
        foreach (Button button in playerButtonList)
            button.transform.GetChild(0).gameObject.SetActive(false);
    }

    public void changeActiveButton(int index)
    {
        Highlight(index);
        activeBtn = playerButtonList[index];
    }
    public void markTarget(int index) // TODO
    {
        clearMarkers();
        if (index < targetList.Count && targetList[index] != null)
        {
            enemyMarkerList.Add(Instantiate(enemyMarkerPrefab, targetList[index].transform.position + new Vector3(0, 1.3f), Quaternion.identity));
        }
    }
    public void markEnemyTargets()
    {
        clearMarkers();
        foreach (Character character in BattleManager.characterList)
        {
            if (character != null && !character.isPlayable)
            {
                enemyMarkerList.Add(Instantiate(enemyMarkerPrefab, character.transform.position + new Vector3(0, 1.3f), Quaternion.identity));
            }
        }
    }
    public void clearMarkers()
    {
        foreach (GameObject marker in enemyMarkerList)
        {
            Destroy(marker);
        }
    }
    public void SetBattleInfo(string info) // TODO
    {
        battleInfo.text = info;
    }
}
