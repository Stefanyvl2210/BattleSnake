using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Botones : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [HideInInspector]
    public bool presionado = false;
    public bool usarComoJoystick = false;
    public float horizontal = 0f;
    public Sprite estadoNeutral;
    public Sprite estadoIzquierda;
    public Sprite estadoDerecha;
    private RectTransform rectTransform;
    private Image joystickImage;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        PrepararVisualJoystick();
        EnsureMenuExtras();
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void PrepareMenuOnSceneLoad()
    {
        EnsureMenuExtras();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        presionado = true;
        UpdateJoystick(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        UpdateJoystick(eventData);
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }
    
    public void Exit()
    {
        Application.Quit();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        presionado = false;
        horizontal = 0f;
        ActualizarVisualJoystick();
    }

    private void UpdateJoystick(PointerEventData eventData)
    {
        if (!usarComoJoystick || rectTransform == null)
        {
            return;
        }

        float halfWidth = rectTransform.rect.width * 0.5f;
        if (halfWidth > 0f)
        {
            Vector2 center = RectTransformUtility.WorldToScreenPoint(eventData.pressEventCamera, rectTransform.position);
            horizontal = Mathf.Clamp((eventData.position.x - center.x) / halfWidth, -1f, 1f);
            ActualizarVisualJoystick();
        }
    }

    private void PrepararVisualJoystick()
    {
        if (!usarComoJoystick || rectTransform == null)
        {
            return;
        }

        joystickImage = GetComponent<Image>();
        if (joystickImage != null && estadoNeutral == null)
        {
            estadoNeutral = joystickImage.sprite;
        }
        ActualizarVisualJoystick();
    }

    private void ActualizarVisualJoystick()
    {
        if (joystickImage == null)
        {
            return;
        }

        if (horizontal < -0.2f && estadoIzquierda != null)
        {
            joystickImage.sprite = estadoIzquierda;
        }
        else if (horizontal > 0.2f && estadoDerecha != null)
        {
            joystickImage.sprite = estadoDerecha;
        }
        else if (estadoNeutral != null)
        {
            joystickImage.sprite = estadoNeutral;
        }
    }

    private static void EnsureMenuExtras()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            return;
        }

        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            return;
        }

        StyleExistingMenuButton("Play", new Vector2(0f, -100f));
        StyleExistingMenuButton("Exit", new Vector2(0f, -214f));

        GameObject highScoresPanel = GetOrCreatePanel(canvas.transform, "HighScoresPanel");
        GameObject instructionsPanel = GetOrCreatePanel(canvas.transform, "InstructionsPanel");

        Text highScoresText = GetOrCreatePanelText(highScoresPanel.transform, "HIGH SCORES\n\nBest Score: " + PlayerPrefs.GetInt("BestScore", 0));
        GetOrCreatePanelText(instructionsPanel.transform, "INSTRUCTIONS\n\nMove: drag the joystick left or right.\nShoot: tap the venom button.\nGoal: destroy all enemies to level up.\nAvoid enemies and borders.");

        Button instructionsButton = GetOrCreateMenuButton(canvas.transform, "InstructionsButton", "INSTRUCTIONS", new Vector2(0f, -138f));
        instructionsButton.onClick.RemoveAllListeners();
        instructionsButton.onClick.AddListener(delegate {
            instructionsPanel.transform.SetAsLastSibling();
            instructionsPanel.SetActive(true);
        });

        Button highScoresButton = GetOrCreateMenuButton(canvas.transform, "HighScoresButton", "HIGH SCORES", new Vector2(0f, -176f));
        highScoresButton.onClick.RemoveAllListeners();
        highScoresButton.onClick.AddListener(delegate {
            highScoresText.text = "HIGH SCORES\n\nBest Score: " + PlayerPrefs.GetInt("BestScore", 0);
            highScoresPanel.transform.SetAsLastSibling();
            highScoresPanel.SetActive(true);
        });

        Button closeHighScoresButton = GetOrCreateMenuButton(highScoresPanel.transform, "CloseHighScores", "CLOSE", new Vector2(0f, -120f));
        closeHighScoresButton.onClick.RemoveAllListeners();
        closeHighScoresButton.onClick.AddListener(delegate {
            highScoresPanel.SetActive(false);
        });

        Button closeInstructionsButton = GetOrCreateMenuButton(instructionsPanel.transform, "CloseInstructions", "CLOSE", new Vector2(0f, -120f));
        closeInstructionsButton.onClick.RemoveAllListeners();
        closeInstructionsButton.onClick.AddListener(delegate {
            instructionsPanel.SetActive(false);
        });

        highScoresPanel.SetActive(false);
        instructionsPanel.SetActive(false);
    }

    private static GameObject GetOrCreatePanel(Transform parent, string name)
    {
        Transform panelTransform = parent.Find(name);
        if (panelTransform != null)
        {
            return panelTransform.gameObject;
        }

        return CreatePanel(parent, name);
    }

    private static GameObject CreatePanel(Transform parent, string name)
    {
        GameObject panel = new GameObject(name);
        panel.transform.SetParent(parent, false);

        RectTransform rectTransform = panel.AddComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.sizeDelta = new Vector2(430f, 300f);

        Image image = panel.AddComponent<Image>();
        image.color = new Color(0f, 0f, 0f, 1f);

        return panel;
    }

    private static Text GetOrCreatePanelText(Transform parent, string text)
    {
        Transform textTransform = parent.Find("PanelText");
        if (textTransform != null)
        {
            Text existingText = textTransform.GetComponent<Text>();
            if (existingText != null)
            {
                existingText.text = text;
                return existingText;
            }
        }

        return CreatePanelText(parent, text);
    }

    private static Text CreatePanelText(Transform parent, string text)
    {
        GameObject textObject = new GameObject("PanelText");
        textObject.transform.SetParent(parent, false);

        RectTransform rectTransform = textObject.AddComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = new Vector2(0f, 45f);
        rectTransform.sizeDelta = new Vector2(380f, 165f);

        Text panelText = textObject.AddComponent<Text>();
        panelText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        panelText.fontSize = 24;
        panelText.color = Color.white;
        panelText.alignment = TextAnchor.MiddleCenter;
        panelText.text = text;

        return panelText;
    }

    private static Button GetOrCreateMenuButton(Transform parent, string name, string label, Vector2 position)
    {
        Transform buttonTransform = parent.Find(name);
        if (buttonTransform == null)
        {
            return CreateMenuButton(parent, name, label, position);
        }

        Text text = buttonTransform.GetComponentInChildren<Text>();
        if (text != null)
        {
            text.text = label;
        }

        StyleExistingMenuButton(name, position);

        Button button = buttonTransform.GetComponent<Button>();
        if (button == null)
        {
            button = buttonTransform.gameObject.AddComponent<Button>();
        }

        Image image = buttonTransform.GetComponent<Image>();
        if (image != null)
        {
            button.targetGraphic = image;
        }

        return button;
    }

    private static Button CreateMenuButton(Transform parent, string name, string label, Vector2 position)
    {
        GameObject buttonObject = new GameObject(name);
        buttonObject.transform.SetParent(parent, false);

        RectTransform rectTransform = buttonObject.AddComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);

        Image image = buttonObject.AddComponent<Image>();

        Button button = buttonObject.AddComponent<Button>();
        button.targetGraphic = image;

        GameObject textObject = new GameObject("Text");
        textObject.transform.SetParent(buttonObject.transform, false);

        RectTransform textTransform = textObject.AddComponent<RectTransform>();
        textTransform.anchorMin = Vector2.zero;
        textTransform.anchorMax = Vector2.one;
        textTransform.offsetMin = Vector2.zero;
        textTransform.offsetMax = Vector2.zero;

        Text text = textObject.AddComponent<Text>();
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.text = label;

        StyleExistingMenuButton(name, position);
        return button;
    }

    private static void StyleExistingMenuButton(string name, Vector2 position)
    {
        GameObject buttonObject = GameObject.Find(name);
        if (buttonObject == null)
        {
            return;
        }

        RectTransform rectTransform = buttonObject.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = position;
            rectTransform.sizeDelta = new Vector2(190f, 34f);
        }

        Image image = buttonObject.GetComponent<Image>();
        if (image != null)
        {
            CopyMenuButtonImageStyle(image, name);
            image.color = new Color(0.1f, 0.35f, 0.1f, 0.95f);
        }

        Button button = buttonObject.GetComponent<Button>();
        Button templateButton = GetMenuButtonTemplateButton(name);
        if (button != null && templateButton != null && button != templateButton)
        {
            button.transition = templateButton.transition;
            button.colors = templateButton.colors;
            button.spriteState = templateButton.spriteState;
            button.animationTriggers = templateButton.animationTriggers;
        }

        Text text = buttonObject.GetComponentInChildren<Text>();
        if (text != null)
        {
            text.fontSize = 20;
            text.fontStyle = FontStyle.Bold;
            text.color = Color.white;
            text.alignment = TextAnchor.MiddleCenter;
        }
    }

    private static void CopyMenuButtonImageStyle(Image image, string currentName)
    {
        Image template = GetMenuButtonTemplateImage(currentName);
        if (template == null || template == image)
        {
            return;
        }

        image.sprite = template.sprite;
        image.type = template.type;
        image.fillCenter = template.fillCenter;
        image.preserveAspect = template.preserveAspect;
        image.material = template.material;
        image.raycastTarget = template.raycastTarget;
    }

    private static Image GetMenuButtonTemplateImage(string currentName)
    {
        GameObject templateObject = GetMenuButtonTemplateObject(currentName);
        return templateObject != null ? templateObject.GetComponent<Image>() : null;
    }

    private static Button GetMenuButtonTemplateButton(string currentName)
    {
        GameObject templateObject = GetMenuButtonTemplateObject(currentName);
        return templateObject != null ? templateObject.GetComponent<Button>() : null;
    }

    private static GameObject GetMenuButtonTemplateObject(string currentName)
    {
        if (currentName != "Play")
        {
            GameObject play = GameObject.Find("Play");
            if (play != null)
            {
                return play;
            }
        }

        if (currentName != "Exit")
        {
            return GameObject.Find("Exit");
        }

        return null;
    }
}
