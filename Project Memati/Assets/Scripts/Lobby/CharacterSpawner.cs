using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSpawner : MonoBehaviour
{
    public GameObject[] characters; // Prefab karakterler
    public Transform[] spawnPoints; // Karakterlerin spawn olacaðý noktalar (borularýn üstü)
    public Transform[] dropPoints;  // Karakterlerin düþeceði noktalar (borularýn içi)
    public Button addButton;        // Oyuncu Ekle butonu
    public Button removeButton;     // Oyuncu Çýkar butonu
    public Button[] nextButtons;    // Sonraki karakter butonlarý
    public Button[] previousButtons; // Önceki karakter butonlarý

    private GameObject[] currentCharacters; // Mevcut karakterler
    private int[] currentCharacterIndices;  // Mevcut karakter indeksleri
    private bool[] isChangingCharacter; // Her boru için karakter deðiþimi durumu
    private int currentIndex = 0; // Þu anki karakterin indeksi

    void Start()
    {
        currentCharacters = new GameObject[dropPoints.Length];
        currentCharacterIndices = new int[dropPoints.Length];
        isChangingCharacter = new bool[dropPoints.Length]; // Her boru için bayrak baþlat

        // Baþlangýçta sadece ilk karakteri ekle
        for (int i = 0; i < currentCharacters.Length; i++)
        {
            if (i == 0)
            {
                currentCharacterIndices[i] = 0;
                SpawnCharacterAt(i, currentCharacterIndices[i]);
            }
            else
            {
                currentCharacters[i] = null;
                currentCharacterIndices[i] = -1; // Baþlangýçta indeksler geçersiz
            }
        }

        // Buton olaylarýný baðla
        addButton.onClick.AddListener(AddPlayer);
        removeButton.onClick.AddListener(RemovePlayer);

        // Karakter deðiþtirme butonlarý için olaylarý baðla
        for (int i = 0; i < nextButtons.Length; i++)
        {
            int index = i; // Kapatma problemi için yerel deðiþken kullan
            nextButtons[i].onClick.AddListener(() => ChangeCharacter(index, 1));
            previousButtons[i].onClick.AddListener(() => ChangeCharacter(index, -1));
        }
    }

    // Yeni bir oyuncu ekle
    void AddPlayer()
    {
        if (currentIndex < currentCharacters.Length - 1)
        {
            currentIndex++;
            currentCharacterIndices[currentIndex] = 0; // Ýlk karakterle baþla
            SpawnCharacterAt(currentIndex, currentCharacterIndices[currentIndex]);
        }
    }

    // Mevcut oyuncuyu kaldýr
    void RemovePlayer()
    {
        if (currentIndex > 0)
        {
            Destroy(currentCharacters[currentIndex]);
            currentCharacters[currentIndex] = null;
            currentCharacterIndices[currentIndex] = -1; // Ýndeksi geçersiz yap
            currentIndex--;
        }
    }

    // Karakteri deðiþtir
    void ChangeCharacter(int index, int direction)
    {
        if (isChangingCharacter[index] || currentCharacterIndices[index] == -1)
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
        currentCharacterIndices[index] = (currentCharacterIndices[index] + direction + characters.Length) % characters.Length;

        // Yeni karakteri instantiate et ve yerine düþür
        SpawnCharacterAt(index, currentCharacterIndices[index]);
        yield return StartCoroutine(DropCharacter(currentCharacters[index], dropPoints[index].position));

        isChangingCharacter[index] = false; // Karakter deðiþimi iþlemi tamamlandý
    }

    // Belirli bir indekste karakteri spawnla ve yerine düþür
    void SpawnCharacterAt(int index, int characterIndex)
    {
        currentCharacters[index] = Instantiate(characters[characterIndex], spawnPoints[index].position, Quaternion.identity);
        currentCharacters[index].transform.Rotate(0, 180, 0);
        StartCoroutine(DropCharacter(currentCharacters[index], dropPoints[index].position));
    }

    // Karakteri belirli bir pozisyona düþür
    System.Collections.IEnumerator DropCharacter(GameObject character, Vector3 targetPosition)
    {
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
    }
}