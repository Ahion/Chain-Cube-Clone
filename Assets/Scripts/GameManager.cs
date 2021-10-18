using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public GameObject[] cubePrefabs;
    public List<GameObject> allCubes;
    public GameObject linePrefab;
    public TextMeshProUGUI scoreText;
    public GameObject popupTextPrefab;
    private AudioSource soundSource;
    public AudioClip sendSound;

    public float controlSpeed = 10.0f;
    public float launchForce = 15.0f;
    public float moveLimitX = 1.96f;
    public float spawnDelay = 0.5f;

    private Vector3 startPos = new Vector3(0, 0.5f, -3);
    private GameObject cubeToSend;
    private GameObject cubeAimLine;
    private bool onControl;
    private bool controlBonus = false;
    private int lastCubeIndex;
    private float score = 0;

    // Start is called before the first frame update
    void Start()
    {
        soundSource = GetComponent<AudioSource>();
        SpawnCube(startPos, index: RandomCubeIndex());
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Mouse X");
        if (Input.GetMouseButton(0) && onControl && cubeToSend)
        {
            cubeToSend.transform.Translate(Vector3.right * horizontalInput * Time.deltaTime * controlSpeed);
            if (cubeToSend.transform.position.x > moveLimitX)
            {
                cubeToSend.transform.position = new Vector3(moveLimitX, cubeToSend.transform.position.y, cubeToSend.transform.position.z);
            }
            if (cubeToSend.transform.position.x < -moveLimitX)
            {
                cubeToSend.transform.position = new Vector3(-moveLimitX, cubeToSend.transform.position.y, cubeToSend.transform.position.z);
            }
        }
        if (Input.GetMouseButtonUp(0) && onControl)
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                //Debug.Log("Clicked on the UI");
                return;
            }
            SendCube();
            StartCoroutine(CubeSpawnDelay(spawnDelay));
        }
    }
    //index - Cube prefab index in cubePrefabs array.
    public GameObject SpawnCube(Vector3 position, bool control = true, int index = 0)
    {
        GameObject newCube = Instantiate(cubePrefabs[index], position, cubePrefabs[index].transform.rotation);
        Cube cubeScript = newCube.GetComponent<Cube>();
        cubeScript.cubeIndex = index;
        cubeScript.gameManagerScript = this;
        if (control)
        {
            Animation cubeAnim = newCube.GetComponent<Animation>();
            cubeAnim.Play();
            AddAimLine(newCube);
            cubeToSend = newCube;
            onControl = true;
        }
        return newCube;
    }
    public void UpdateScore(int cubeIndex)
    {
        float cubeNumber = Mathf.Pow(2, cubeIndex);
        score += cubeNumber;
        ShowPopUpText(cubeNumber);
        scoreText.text = "Score: " + score;
    }
    private void ShowPopUpText(float number)
    {
        GameObject popup = Instantiate(popupTextPrefab, scoreText.transform);
        TextMeshProUGUI popupText = popup.GetComponent<TextMeshProUGUI>();
        popupText.text = $"+{number}";
    }
    private void SendCube()
    {
        Rigidbody cubeRb = cubeToSend.GetComponent<Rigidbody>();
        cubeRb.AddForce(Vector3.forward * launchForce, ForceMode.Impulse);
        soundSource.PlayOneShot(sendSound, 1.0f);
        if (!controlBonus)
        {
            allCubes.Add(cubeToSend);
        }
        else
        {
            controlBonus = false;
        }
        onControl = false;
        Destroy(cubeAimLine);
        cubeToSend = null;
    }
    private int RandomCubeIndex()
    {
        int index = Random.Range(0, 6);
        if (index == lastCubeIndex)
        {
            index = RandomCubeIndex();
        }
        lastCubeIndex = index;
        return index;
    }
    IEnumerator CubeSpawnDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnCube(startPos, index: RandomCubeIndex());
    }
    private void AddAimLine(GameObject cube)
    {
        GameObject aimLine = Instantiate(linePrefab, cube.transform.position, linePrefab.transform.rotation);
        aimLine.transform.SetParent(cube.transform);
        cubeAimLine = aimLine;
    }
    public void SpawnBonus(GameObject bonus)
    {
        if (cubeToSend)
        {
            Destroy(cubeToSend);
        }
        StopCoroutine(CubeSpawnDelay(spawnDelay));
        GameObject newBonus = Instantiate(bonus, startPos, bonus.transform.rotation);
        AddAimLine(newBonus);
        cubeToSend = newBonus;
        onControl = true;
        controlBonus = true;
    }
}
