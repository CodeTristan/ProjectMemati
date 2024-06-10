using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class CharacterSpawner : MonoBehaviour
{
    public static CharacterSpawner instance;

    public GameObject[] characters; // Prefab karakterler
    public Transform[] spawnPoints; // Karakterlerin spawn olaca�� noktalar (borular�n �st�)
    public Transform[] dropPoints;  // Karakterlerin d��ece�i noktalar (borular�n i�i)

    public Button[] nextButtons;    // Sonraki karakter butonlar�
    public Button[] previousButtons; // �nceki karakter butonlar�
    public Button startButton;      // Start butonu

    private GameObject[] currentCharacters; // Mevcut karakterler
    private int[] currentCharacterIndexes;  // Mevcut karakter indeksleri
    private bool[] isChangingCharacter; // Her boru i�in karakter de�i�imi durumu
    private bool isDropping = false;    // D��me i�lemi durumu
    private int currentIndex = 0; // �u anki karakterin indeksi

    [Header("MinigameMenu")]
    [SerializeField] private TextMeshProUGUI MinigameTexts;

    void Start()
    {
        instance = this;
        currentCharacters = new GameObject[dropPoints.Length];
        currentCharacterIndexes = new int[dropPoints.Length];
        isChangingCharacter = new bool[dropPoints.Length]; // Her boru i�in bayrak ba�lat

        // Karakter de�i�tirme butonlar� i�in olaylar� ba�la
        for (int i = 0; i < nextButtons.Length; i++)
        {
            int index = i; // Kapatma problemi i�in yerel de�i�ken kullan
            nextButtons[i].onClick.AddListener(() => ChangeCharacter(index, 1));
            previousButtons[i].onClick.AddListener(() => ChangeCharacter(index, -1));
        }

        // Ba�lang��ta start butonunu etkinle�tir
        startButton.interactable = true;

        //Minigameleri se�
        MinigameManager.instance.SelectMinigames();
    }

    //_ demek buton taraf�ndan kullan�lacak demek
    public void _StartGame()
    {
        List<Player> players = PlayerManager.instance.players;
        for (int i = 0; i < players.Count; i++)
        {
            players[i].CharacterPrefab = characters[currentCharacterIndexes[i]];
        }
    }

    public void _SelectMinigames()
    {
        MinigameManager.instance.SelectMinigames();
        MinigameTexts.text = "";
        foreach (var item in MinigameManager.instance.selectedMinigames)
        {
            MinigameTexts.text += "1- " + item + "\n";
        }
    }

    // Yeni bir oyuncu ekle
    void AddPlayer()
    {
        if (currentIndex < currentCharacters.Length - 1)
        {
            currentIndex++;
            currentCharacterIndexes[currentIndex] = 0; // �lk karakterle ba�la
            SpawnCharacterAt(currentIndex, currentCharacterIndexes[currentIndex]);
        }
    }

    // Mevcut oyuncuyu kald�r
    void RemovePlayer()
    {
        if (currentIndex > 0)
        {
            Destroy(currentCharacters[currentIndex]);
            currentCharacters[currentIndex] = null;
            currentCharacterIndexes[currentIndex] = -1; // �ndeksi ge�ersiz yap
            currentIndex--;
        }
    }

    // Karakteri de�i�tir
    void ChangeCharacter(int index, int direction)
    {
        if (isChangingCharacter[index] || currentCharacterIndexes[index] == -1)
        {
            return; // Halen karakter de�i�imi yap�l�yorsa veya ge�ersiz indeksse i�lemi durdur
        }

        StartCoroutine(ChangeCharacterCoroutine(index, direction));
    }

    // Karakter de�i�tirme i�lemini coroutine olarak yap
    System.Collections.IEnumerator ChangeCharacterCoroutine(int index, int direction)
    {
        isChangingCharacter[index] = true; // Karakter de�i�imi i�lemini ba�lat

        // Mevcut karakteri silmeden �nce null kontrol� yap
        if (currentCharacters[index] != null)
        {
            Destroy(currentCharacters[index]);
        }

        // Bir sonraki veya �nceki karaktere ge�
        currentCharacterIndexes[index] = (currentCharacterIndexes[index] + direction + characters.Length) % characters.Length;

        // Yeni karakteri instantiate et ve yerine d���r
        SpawnCharacterAt(index, currentCharacterIndexes[index]);
        yield return StartCoroutine(DropCharacter(currentCharacters[index], dropPoints[index].position));

        isChangingCharacter[index] = false; // Karakter de�i�imi i�lemi tamamland�
    }

    // Belirli bir indekste karakteri spawnla ve yerine d���r
    public void SpawnCharacterAt(int index, int characterIndex)
    {
        currentCharacters[index] = Instantiate(characters[characterIndex], spawnPoints[index].position, Quaternion.identity);
        currentCharacters[index].transform.Rotate(0, 180, 0);
        StartCoroutine(DropCharacter(currentCharacters[index], dropPoints[index].position));
    }

    // Karakteri belirli bir pozisyona d���r
    System.Collections.IEnumerator DropCharacter(GameObject character, Vector3 targetPosition)
    {
        isDropping = true; // D��me i�lemi ba�lad�
        startButton.interactable = false; // Start butonunu devre d��� b�rak

        float duration = 1.0f; // D���� s�resi
        float elapsedTime = 0;

        Vector3 startPosition = character.transform.position;

        while (elapsedTime < duration)
        {
            character.transform.position = Vector3.Lerp(startPosition, targetPosition, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        character.transform.position = targetPosition;

        isDropping = false; // D��me i�lemi tamamland�
        startButton.interactable = true; // Start butonunu tekrar etkinle�tir
    }

    void Update()
    {
        // D��me i�lemi devam ediyorsa "Start" butonuna bas�lmas�n� engelle
        startButton.interactable = !isDropping;

        // Enter tu�una bas�ld���nda s�radaki karakteri spawnla ve d���r
        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            SpawnNextCharacter();
        }
    }

    // Enter tu�una bas�ld���nda karakterlerin spawn edilmesini ve d��mesini sa�lar
    void SpawnNextCharacter()
    {
        if (currentIndex < currentCharacters.Length)
        {
            currentCharacterIndexes[currentIndex] = 0; // �lk karakterle ba�la
            SpawnCharacterAt(currentIndex, currentCharacterIndexes[currentIndex]);
            currentIndex++;
        }
    }
}