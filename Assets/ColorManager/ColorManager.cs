
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;


public class ColorManager : MonoBehaviour, IPointerDownHandler, IDragHandler
{
#if UNITY_EDITOR || UNITY_STANDALONE_OSX
    [SerializeField] Image circlePalette;
    [SerializeField] Image picker;
    [SerializeField] Color selectedColor;
    [SerializeField] bool isSkyBox = true;
    [SerializeField] GameObject linkedObject;

    Vector2 sizeOfPalette;
    BoxCollider2D paletteCollider;
    GameObject colorCanvas;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            colorCanvas.SetActive(colorCanvas.activeSelf ? false : true);
        }
    }


    void Start()
    {
        colorCanvas = transform.GetChild(0).gameObject;
        colorCanvas.SetActive(false);

        paletteCollider = circlePalette.GetComponent<BoxCollider2D>();

        sizeOfPalette = new Vector2(
            circlePalette.GetComponent<RectTransform>().rect.width,
            circlePalette.GetComponent<RectTransform>().rect.height);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        selectColor();
    }
    public void OnDrag(PointerEventData eventData)
    {
        selectColor();
    }

    private Color getColor()
    {
        Vector2 circlePalettePosition = circlePalette.transform.position;
        Vector2 pickerPosition = picker.transform.position;

        Vector2 position = pickerPosition - circlePalettePosition + sizeOfPalette * 0.5f;
        Vector2 normalized = new Vector2(
            (position.x / (circlePalette.GetComponent<RectTransform>().rect.width)),
            (position.y / (circlePalette.GetComponent<RectTransform>().rect.height)));

        Texture2D texture = circlePalette.mainTexture as Texture2D;
        Color circularSelectedColor = texture.GetPixelBilinear(normalized.x, normalized.y);

        return circularSelectedColor;
    }

    private void selectColor()
    {
        Vector3 offset = Input.mousePosition - transform.position;
        Vector3 diff = Vector3.ClampMagnitude(offset, paletteCollider.size.x);

        picker.transform.position = transform.position + diff;

        selectedColor = getColor();

        if (isSkyBox) Camera.main.backgroundColor = selectedColor;
        else if (linkedObject != null) linkedObject.GetComponent<MeshRenderer>().materials[0].color = selectedColor;

    }
    public Color GetCurrentColor()
    {
        return selectedColor;
    }
#endif
}

#if UNITY_EDITOR

public class ColorEditor
{
    [MenuItem("MonLibrary/Woody Color")]
    public static void MakeCursor()
    {
        GameObject go = GameObject.Instantiate(Resources.Load("ColorManager")) as GameObject;
        go.name = "Woody_Color";
    }
}
#endif