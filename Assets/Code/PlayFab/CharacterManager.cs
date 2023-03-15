using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    public void SetCharacterName(string newName)
    {
        _characterName = newName;
    }


    [SerializeField]
    private GameObject _enterName_Panel;
    [SerializeField]
    private Button _createNewCharacter_Button;
    private string _characterName;

    [Space(200)]

    [SerializeField]
    private Button[] _addCharacters;

    [SerializeField]
    private Button[] _useCharactetrs;

    private void Start()
    {
        GetCharacters();
       
        _createNewCharacter_Button.onClick.AddListener(CreateNewCharacter);
    }

    private void CreateNewCharacter()
    {
        //_enterName_Panel.SetActive(false);

        //PlayFabClientAPI.PurchaseItem(new PurchaseItemRequest
        //{
        //    CatalogVersion  ="0.1",
        //    ItemId = _item.ItemId,
        //    Price = (int) _price.Value,
        //    VirtualCurrency = _price.Key
        //}, result=> 
        //{

        //}, Debug.LogError);
    }


    public void GetCharacters()
    {
        PlayFabClientAPI.GetAllUsersCharacters(new ListUsersCharactersRequest(), 
            res=>
            {
                Debug.Log($"Characters owned: {res.Characters.Count}");

                foreach (var character in _addCharacters)
                    character.onClick.AddListener(() => _enterName_Panel.SetActive(true));

                foreach (var character in _useCharactetrs)
                    character.onClick.AddListener(() => _enterName_Panel.SetActive(false));

                for (int i = 0; i < res.Characters.Count; ++i)
                {
                    _addCharacters[i].gameObject.SetActive(false);
                    _useCharactetrs[i].gameObject.SetActive(true);
                }

                foreach (var character in _addCharacters)
                    character.onClick.AddListener(()=> _enterName_Panel.SetActive(true));

            }, Debug.LogError);
    }

    public void CreateCharacterWithItemId(string itemId)
    {
        PlayFabClientAPI.GrantCharacterToUser(new GrantCharacterToUserRequest
        {
            CharacterName = _characterName,
            ItemId = itemId
        }, result =>
        {
            Debug.Log($"Character type: {result.CharacterType}");
            UpdateCharacterStatistics(result.CharacterId);
        }, Debug.LogError);
    }

    private void UpdateCharacterStatistics(string characterId)
    {
        PlayFabClientAPI.UpdateCharacterStatistics(new UpdateCharacterStatisticsRequest
        {
            CharacterId = characterId,
            CharacterStatistics = new Dictionary<string, int>
            {
                {"Level", 1},
                {"XP", 0},
                {"Gold", 0}
            }
        }, result =>
            {
                Debug.Log($"Initial stats set, telling client to update character list");
                
            },
        Debug.LogError);
    }

}
