using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using UnityEngine;

public class CatalogReader : MonoBehaviour
{
    private readonly Dictionary<string, CatalogItem> _catalog = 
        new Dictionary<string, CatalogItem>();

    private void OnGetCatalogSuccess(
        PlayFab.ClientModels.GetCatalogItemsResult result)
    {
        foreach (var item in result.Catalog)
        {
            _catalog.Add(item.ItemId, item);
            Debug.Log($"{item.ItemId}");
        }
        Debug.Log($"Catalog was loaded successfully!");
    }

    internal void Read()
    {
        PlayFabClientAPI.GetCatalogItems(
            new PlayFab.ClientModels.GetCatalogItemsRequest(),
            OnGetCatalogSuccess,
            OnFailure);
    }

    private void OnFailure(PlayFabError error)
    {
        var errorMessage = error.GenerateErrorReport();
        Debug.LogError($"Something went wrong: {errorMessage}");
    }

}
