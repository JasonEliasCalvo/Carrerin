using UnityEngine;

public class CharacterModelSelector : MonoBehaviour
{
    public GameObject rabbitCar;
    public GameObject catCar;
    public GameObject rabbitCharacter;
    public GameObject catCharacter;

    public void SetCharacterModel(int playerIndex)
    {
        bool isRabbit = playerIndex == 0;

        rabbitCar.SetActive(isRabbit);
        catCar.SetActive(!isRabbit);

        rabbitCharacter.SetActive(isRabbit);
        catCharacter.SetActive(!isRabbit);
    }

}
