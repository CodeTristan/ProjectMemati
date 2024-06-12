using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class CharacterSpawner : MonoBehaviour
{
    public static CharacterSpawner instance;

    public GameObject[] characters; // Prefab karakterler
    public Transform[] spawnPoints; // Karakterlerin spawn olacaðý noktalar (borularýn üstü)
    public Transform[] dropPoints;  // Karakterlerin düþeceði noktalar (borularýn içi)

    public Button[] nextButtons;    // Sonraki karakter butonlarý
    public Button[] previousButtons; // Önceki karakter butonlarý
    public Button startButton;      // Start butonu

    private GameObject[] currentCharacters; // Mevcut karakterler
    private int[] currentCharacterIndexes;  // Mevcut karakter indeksleri
    private bool[] isChangingCharacter; // Her boru için karakter deðiþimi durumu
    private bool isDropping = false;    // Düþme iþlemi durumu
    private int currentIndex = 0; // Þu anki karakterin indeksi

    [Header("MinigameMenu")]
    [SerializeField] private TextMeshProUGUI MinigameTexts;

    void Start()
    {
        instance = this;
        currentCharacters = new GameObject[dropPoints.Length];
        currentCharacterIndexes = new int[dropPoints.Length];
        isChangingCharacter = new bool[dropPoints.Length]; // Her boru için bayrak baþlat

        // Karakter deðiþtirme butonlarý için olaylarý baðla
        for (int i = 0; i < nextButtons.Length; i++)
        {
            int index = i; // Kapatma problemi için yerel deðiþken kullan
            nextButtons[i].onClick.AddListener(() => ChangeCharacter(index, 1));
            previousButtons[i].onClick.AddListener(() => ChangeCharacter(index, -1));
        }

        // Baþlangýçta start butonunu etkinleþtir
        startButton.interactable = true;
    }

    //_ demek buton tarafýndan kullanýlacak demek
    public void _StartGame()
    {
        List<Player> players = PlayerManager.instance.players;
        for (int i = 0; i < players.Count; i++)
        {
            players[i].CharacterPrefab = characters[currentCharacterIndexes[i]];
            players[i].playerControl = players[i].CharacterPrefab.GetComponent<PlayerControl>();
            int index = i;
            players[i].playerControl.player = players[index];
        }

        string minigame = MinigameManager.instance.selectedMinigames[0];
        MinigameManager.instance.selectedMinigames.RemoveAt(0);
        if(minigame == "Obezman")
        {
            SahneManager.instance.LoadScene("ObezmanScene");
        }
        else if(minigame == "Cliff Run")
        {
            SahneManager.instance.LoadScene("TempleRun");
        }
        else if (minigame == "Bombacı Mülayim")
        {
            SahneManager.instance.LoadScene("Bomberman");
        }
    }

    public void _StartMinigame(){
        string minigame = MinigameManager.instance.selectedMinigames[0];
        MinigameManager.instance.selectedMinigames.RemoveAt(0);
        if(minigame == "Obezman")
        {
            SahneManager.instance.LoadScene("ObezmanScene");
        }
        else if(minigame == "Cliff Run")
        {
            SahneManager.instance.LoadScene("TempleRun");
        }
        else if (minigame == "Bombacı Mülayim")
        {
            SahneManager.instance.LoadScene("Bomberman");
        }
    }
    public void _ShowMinigames()
    {
        MinigameTexts.text = "";
        int count = 1;
        foreach (var item in MinigameManager.instance.selectedMinigames)
        {
            MinigameTexts.text += count + "- " + item + "\n";
            count++;
        }
    }

    public void _SelectMinigames()
    {
        MinigameManager.instance.SelectMinigames();
        MinigameTexts.text = "";
        int count = 1;
        foreach (var item in MinigameManager.instance.selectedMinigames)
        {
            MinigameTexts.text += count + "- " + item + "\n";
            count++;
        }
    }

    // Yeni bir oyuncu ekle
    void AddPlayer()
    {
        if (currentIndex < currentCharacters.Length - 1)
        {
            currentIndex++;
            currentCharacterIndexes[currentIndex] = 0; // Ýlk karakterle baþla
            SpawnCharacterAt(currentIndex, currentCharacterIndexes[currentIndex]);
        }
    }

    // Mevcut oyuncuyu kaldýr
    void RemovePlayer()
    {
        if (currentIndex > 0)
        {
            Destroy(currentCharacters[currentIndex]);
            currentCharacters[currentIndex] = null;
            currentCharacterIndexes[currentIndex] = -1; // Ýndeksi geçersiz yap
            currentIndex--;
        }
    }

    // Karakteri deðiþtir
    void ChangeCharacter(int index, int direction)
    {
        if (isChangingCharacter[index] || currentCharacterIndexes[index] == -1)
        {
            return; // Halen karakter deðiþimi yapýlýyorsa veya geçersiz indeksse iþlemi durdur
        }

        StartCoroutine(ChangeCharacterCoroutine(index, direction));
    }

    // Karakter deðiþtirme iþlemini coroutine olarak yap
    System.Collections.IEnumerator ChangeCharacterCoroutine(int index, int direction)
    {
        isChangingCharacter[index] = true; // Karakter deðiþimi iþlemini baþlat

        // Mevcut karakteri silmeden önce null kontrolü yap
        if (currentCharacters[index] != null)
        {
            Destroy(currentCharacters[index]);
        }

        // Bir sonraki veya önceki karaktere geç
        currentCharacterIndexes[index] = (currentCharacterIndexes[index] + direction + characters.Length) % characters.Length;

        // Yeni karakteri instantiate et ve yerine düþür
        SpawnCharacterAt(index, currentCharacterIndexes[index]);
        yield return StartCoroutine(DropCharacter(currentCharacters[index], dropPoints[index].position));

        isChangingCharacter[index] = false; // Karakter deðiþimi iþlemi tamamlandý
    }

    // Belirli bir indekste karakteri spawnla ve yerine düþür
    public void SpawnCharacterAt(int index, int characterIndex)
    {
        currentCharacters[index] = Instantiate(characters[characterIndex], spawnPoints[index].position, Quaternion.identity);
        currentCharacters[index].transform.Rotate(0, 180, 0);
        StartCoroutine(DropCharacter(currentCharacters[index], dropPoints[index].position));
    }

    // Karakteri belirli bir pozisyona düþür
    System.Collections.IEnumerator DropCharacter(GameObject character, Vector3 targetPosition)
    {
        isDropping = true; // Düþme iþlemi baþladý
        startButton.interactable = false; // Start butonunu devre dýþý býrak

        float duration = 1.0f; // Düþüþ süresi
        float elapsedTime = 0;

        Vector3 startPosition = character.transform.position;

        while (elapsedTime < duration)
        {
            character.transform.position = Vector3.Lerp(startPosition, targetPosition, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        character.transform.position = targetPosition;

        isDropping = false; // Düþme iþlemi tamamlandý
        startButton.interactable = true; // Start butonunu tekrar etkinleþtir
    }

    void Update()
    {
        // Düþme iþlemi devam ediyorsa "Start" butonuna basýlmasýný engelle
        startButton.interactable = !isDropping;
    }

    // Enter tuþuna basýldýðýnda karakterlerin spawn edilmesini ve düþmesini saðlar
    void SpawnNextCharacter()
    {
        if (currentIndex < currentCharacters.Length)
        {
            currentCharacterIndexes[currentIndex] = 0; // Ýlk karakterle baþla
            SpawnCharacterAt(currentIndex, currentCharacterIndexes[currentIndex]);
            currentIndex++;
        }
    }
}