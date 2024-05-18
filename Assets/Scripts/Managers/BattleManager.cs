using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class BattleManager : MonoBehaviour
{
    public static System.Random random = new System.Random(); // number generator

    public static BattleManager Instance;

    public static List<Character> characterList = new List<Character>();

    //character and transform prefabs
    public Character playerFirstPrefab;
    public Character playerSecondPrefab;
    public Character enemyFirstPrefab;
    public Character enemySecondPrefab;

    private Character playerFirst;
    private Character playerSecond;
    private Character enemyFirst;
    private Character enemySecond;
    public Transform characterHolder;

    public Character activeCharacter;
    public Character targetCharacter;

    public CharacterHUD playerFirstHUD;
    public CharacterHUD playerSecondHUD;
    public CharacterHUD enemyFirstHUD;
    public CharacterHUD enemySecondHUD;

    public int enemyCount;
    public int playerCount;

    private void Awake()
    {
        Instance = this;
        enemyCount = playerCount = 0;
    }
    public void battleStart()
    {
        // add characters to list and instantiate them if they are available
        playerFirst = InitializePrefab(playerFirstPrefab, playerFirst, new Vector2(-7, 0));
        playerSecond = InitializePrefab(playerSecondPrefab, playerSecond, new Vector2(-5, 2.6f));
        enemyFirst = InitializePrefab(enemyFirstPrefab, enemyFirst, new Vector2(7, 0));
        enemySecond = InitializePrefab(enemySecondPrefab, enemySecond, new Vector2(5, 2.6f));

        // set the huds of the characters
        playerFirst.charHUD = playerFirstHUD;
        playerSecond.charHUD = playerSecondHUD;
        enemyFirst.charHUD = enemyFirstHUD;
        enemySecond.charHUD = enemySecondHUD;

        foreach (Character character in characterList)
            character.SetHUD();

        // get the character count for both sides
        for (int i = 0; i < characterList.Count; i++)
        {
            if (!characterList[i].isPlayable)
            {
                enemyCount++;
            }
        }
        for (int i = 0; i < characterList.Count; i++)
        {
            if (characterList[i].isPlayable)
            {
                playerCount++;
            }
        }

        setTurnOrder();
        activeCharacter = characterList[0]; // the first turn
        targetCharacter = new GameObject().AddComponent<Character>();
        GameManager.Instance.setGameState(GameState.Battle); // set gamestate to battle
    }

    public void setTurnOrder() // TODO make this visible
    {
        characterList = characterList.OrderByDescending(c => c.speed).ToList<Character>(); // re-order characters according to speed   
    }

    public IEnumerator EnemyTurn()
    {
        GameManager.Instance.setGameState(GameState.Busy);

        yield return new WaitForSeconds(0.5f);

        activeCharacter.abilityList[random.Next(activeCharacter.abilityList.Count)].Invoke(activeCharacter,null);

        StopCoroutine(EnemyTurn());
        yield return new WaitForSeconds(0.5f);
        activeCharacter = advanceTurn(characterList.IndexOf(activeCharacter) + 1);

        StateChecker();
    }

    public Character InitializePrefab(Character prefab, Character character, Vector2 vector) // initializes every character prefab and adds them to the list
    {
        if (prefab != null)
        {
            character = Instantiate(prefab, vector, Quaternion.identity, characterHolder);
            characterList.Add(character);
            Debug.Log(character.charName); // test
            return character;
        }
        return null;
    }

    public Character advanceTurn(int index) // turn changer
    {
        foreach (Character c in characterList) // mark every character to not take turns
            c.isTurn = false;
        Character character = characterList[index % characterList.Count];
        character.isTurn = true;
        if (character.isPlayable)
            GameManager.Instance.setGameState(GameState.PlayerTurn);
        else if (!character.isPlayable)
            GameManager.Instance.setGameState(GameState.EnemyTurn);
        return character;
    }
    public void StateChecker()
    {
        if (enemyCount == 0)
        {
            GameManager.Instance.setGameState(GameState.Won);
        }
        else if (playerCount == 0)
        {
            GameManager.Instance.setGameState(GameState.Lost);
        }
    }
}

