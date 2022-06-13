using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using UnityEngine.Video;

public enum ESlideType
{
    Slide,
    VideoSlide,
    TimedSlide
}

public enum EFontStyle
{
    Normal,
    Bold,
    Italic
}

public enum ERectPosition
{
    Middle,
    MiddleLeft,
    MiddleRight,
    Top,
    TopLeft,
    TopRight,
    Bot,
    BotLeft,
    BotRight
}

public class SlideEditorUI
{
    const int Size = 10;

    static Vector2 scrollPosition = Vector2.zero;
    static ESlideType _selectedSlideTypeEnumVal;
    static string _slideName = "New Slide";
    static SlideController _slideController;

    // Canvas options
    static bool _addCanvas = false;
    static RenderMode _canvasRenderMode = RenderMode.ScreenSpaceOverlay;
    static apperture.editor.DeviceType _deviceType;
    static Vector2 _canvasSizeIfWorldSpace = new Vector2(1024f, 768f);
    static float _canvasPlaneDistance;
    static Camera _eventCamera;

    //Images
    static bool _addImg = false;
    static int _imgNb = 1;
    static string[] _imgNames = Enumerable.Repeat("Image", Size).ToArray();
    static bool[] _imgAreFullScreen = Enumerable.Repeat(true, Size).ToArray();
    static bool[] _imgSetNativeSizes = Enumerable.Repeat(false, Size).ToArray();
    static ERectPosition[] _imgRectPositions = new ERectPosition[Size];
    static Vector2[] _imgOffSets = new Vector2[Size];
    static Sprite[] _imgSprites = new Sprite[Size];

    //Buttons
    static bool _addBtn = false;
    static int _btnNb = 1;
    static string[] _btnNames = Enumerable.Repeat("Button", Size).ToArray();
    static string[] _btnTextNames = Enumerable.Repeat("Text", Size).ToArray();
    static string[] _btnTextText = Enumerable.Repeat("[...]", Size).ToArray();
    static ERectPosition[] _btnRectPositions = new ERectPosition[Size];
    static Vector2[] _btnOffSets = new Vector2[Size];
    static Sprite[] _btnSprites = new Sprite[Size];
    static bool[] _btnSpritesAreNatives = Enumerable.Repeat(false, Size).ToArray();
    static Vector2[] _btnSpriteSizes = Enumerable.Repeat(new Vector2(160, 30), Size).ToArray();
    static int[] _btnTextSizes = Enumerable.Repeat(14, Size).ToArray();
    static EFontStyle[] _btnTextFonts = new EFontStyle[Size];
    static Color[] _btnColors = Enumerable.Repeat(new Color(0, 0, 0, 1), Size).ToArray();

    //Texts
    static bool _addTxt = false;
    static int _txtNb = 1;
    static string[] _txtNames = Enumerable.Repeat("Text", Size).ToArray();
    static string[] _txtTexts = Enumerable.Repeat("[...]", Size).ToArray();
    static ERectPosition[] _txtRectPositions = new ERectPosition[Size];
    static Vector2[] _txtBoxSizes = Enumerable.Repeat(new Vector2(200, 50), Size).ToArray();
    static Vector2[] _txtOffSets = new Vector2[Size];
    static int[] _txtSizes = Enumerable.Repeat(14, Size).ToArray();
    static EFontStyle[] _txtFontStyles = new EFontStyle[Size];
    static Color[] _txtColors = Enumerable.Repeat(new Color(0, 0, 0, 1), Size).ToArray();

    //Prefabs
    static bool _addPrefab = false;
    static int _prefabNb = 1;
    static GameObject[] _prefabGameObjects = new GameObject[Size];


    // Slide fields
    static bool _autoTransition = true;

    // Timed slide fields
    static float _timeBeforeFadeOut = 5f;

    //Video slide
    static VideoClip _video;


    public static void OnGUI()
    {
        if (_slideController == null && GameObject.FindObjectOfType<SlideController>())
        {
            _slideController = GameObject.FindObjectOfType<SlideController>();
        }
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, true, true);
        GUIStyle guiStyle = new GUIStyle();
        guiStyle.alignment = TextAnchor.MiddleCenter;

        EditorGUILayout.Space();
        _selectedSlideTypeEnumVal = (ESlideType)EditorGUILayout.EnumPopup("Select slide type : ", _selectedSlideTypeEnumVal);
        _slideName = EditorGUILayout.TextField("Slide GameObject name", _slideName);

        DrawSpecificSlideOptions();


        //CANVAS


        GUILayoutExtension.DrawHorizontalLine("More options");

        _addCanvas = EditorGUILayout.Toggle("Canvas : ", _addCanvas);
        if (_addCanvas)
        {
            _canvasRenderMode = (RenderMode)EditorGUILayout.EnumPopup("Canvas type : ", _canvasRenderMode);
            if (_canvasRenderMode == RenderMode.WorldSpace)
            {
                _deviceType = (apperture.editor.DeviceType)EditorGUILayout.EnumPopup("Device type : ", _deviceType);
                _canvasSizeIfWorldSpace = apperture.editor.DeviceUtility.GetSize(_deviceType);
                EditorGUILayout.BeginHorizontal();
                var oldCol = GUI.contentColor;
                GUI.contentColor = Color.yellow;
                EditorGUILayout.LabelField("Width : " + _canvasSizeIfWorldSpace.x, GUILayout.MaxWidth(100f));
                EditorGUILayout.LabelField("Height : " + _canvasSizeIfWorldSpace.y, GUILayout.MaxWidth(100f));
                GUI.contentColor = oldCol;
                EditorGUILayout.EndHorizontal();
                _eventCamera = EditorGUILayout.ObjectField("Event Camera : ", _eventCamera, typeof(Camera), true) as Camera;
            }
            if (_canvasRenderMode == RenderMode.ScreenSpaceCamera)
            {
                _eventCamera = EditorGUILayout.ObjectField("Event Camera : ", _eventCamera, typeof(Camera), true) as Camera;
                _canvasPlaneDistance = EditorGUILayout.FloatField("Plane distance : ", _canvasPlaneDistance);
            }

            EditorGUILayout.Space();


            //IMAGES

            GUILayoutExtension.DrawHorizontalLine("Image(s)");
            _addImg = EditorGUILayout.Toggle("Image", _addImg);

            if (_addImg)
            {
                _imgNb = Mathf.Clamp(EditorGUILayout.IntField("Number :", _imgNb), 0, 10);
                GUILayoutExtension.DrawHorizontalLine();
                EditorGUILayout.Space();
                EditorGUILayout.Space();

                for (int i = 0; i < _imgNb; i++)
                {
                    EditorGUILayout.LabelField("Image " + (i + 1).ToString(), guiStyle, new GUILayoutOption[] { });
                    EditorGUILayout.Space();

                    _imgNames[i] = EditorGUILayout.TextField("Name :", _imgNames[i]);
                    _imgSprites[i] = EditorGUILayout.ObjectField("Sprite : ", _imgSprites[i], typeof(Sprite), false) as Sprite;
                    _imgSetNativeSizes[i] = EditorGUILayout.Toggle("Native Size :", _imgSetNativeSizes[i]);
                    _imgAreFullScreen[i] = EditorGUILayout.Toggle("FullScreen :", _imgAreFullScreen[i]);
                    if (_imgSetNativeSizes[i] == true)
                    {
                        _imgAreFullScreen[i] = false;
                    }
                    if (_imgAreFullScreen[i] != true)
                    {
                        _imgRectPositions[i] = (ERectPosition)EditorGUILayout.EnumPopup("Button Pos :", _imgRectPositions[i]);
                        _imgOffSets[i] = EditorGUILayout.Vector2Field("Offset :", _imgOffSets[i]);
                    }

                    GUILayoutExtension.DrawHorizontalLine();
                    EditorGUILayout.Space();
                }
            }


            //BUTTONS


            GUILayoutExtension.DrawHorizontalLine("Button(s)");
            _addBtn = EditorGUILayout.Toggle("Button", _addBtn);


            if (_addBtn)
            {
                _btnNb = Mathf.Clamp(EditorGUILayout.IntField("Number : ", _btnNb), 0, 10);
                GUILayoutExtension.DrawHorizontalLine();
                EditorGUILayout.Space();
                EditorGUILayout.Space();

                for (int i = 0; i < _btnNb; i++)
                {
                    EditorGUILayout.LabelField("Button " + (i + 1).ToString(), guiStyle, new GUILayoutOption[] { });
                    EditorGUILayout.Space();

                    _btnNames[i] = EditorGUILayout.TextField("Button Name :", _btnNames[i]);
                    _btnSprites[i] = EditorGUILayout.ObjectField("Button Sprite :", _btnSprites[i], typeof(Sprite), false) as Sprite;
                    _btnSpritesAreNatives[i] = EditorGUILayout.Toggle("Set native :", _btnSpritesAreNatives[i]);
                    if (_btnSpritesAreNatives[i] == false)
                    {
                        _btnSpriteSizes[i] = EditorGUILayout.Vector2Field("Size :", _btnSpriteSizes[i]);
                    }
                    _btnRectPositions[i] = (ERectPosition)EditorGUILayout.EnumPopup("Button Pos :", _btnRectPositions[i]);
                    _btnOffSets[i] = EditorGUILayout.Vector2Field("Offset :", _btnOffSets[i]);
                    _btnTextNames[i] = EditorGUILayout.TextField("Text Name :", _btnTextNames[i]);
                    _btnTextText[i] = EditorGUILayout.TextField("Text :", _btnTextText[i]);
                    _btnTextSizes[i] = EditorGUILayout.IntField("Size :", _btnTextSizes[i]);
                    _btnTextFonts[i] = (EFontStyle)EditorGUILayout.EnumPopup("Font Style :", _btnTextFonts[i]);
                    _btnColors[i] = EditorGUILayout.ColorField("Color :", _btnColors[i]);

                    GUILayoutExtension.DrawHorizontalLine();
                    EditorGUILayout.Space();
                }
            }


            //TEXTS


            GUILayoutExtension.DrawHorizontalLine("Text(s)");
            _addTxt = EditorGUILayout.Toggle("Text", _addTxt);


            if (_addTxt)
            {
                _txtNb = Mathf.Clamp(EditorGUILayout.IntField("Number : ", _txtNb), 0, 10);
                GUILayoutExtension.DrawHorizontalLine();
                EditorGUILayout.Space();
                EditorGUILayout.Space();

                for (int i = 0; i < _txtNb; i++)
                {
                    EditorGUILayout.LabelField("Text " + (i + 1).ToString(), guiStyle, new GUILayoutOption[] { });
                    EditorGUILayout.Space();

                    _txtNames[i] = EditorGUILayout.TextField("Text Name :", _txtNames[i]);
                    _txtTexts[i] = EditorGUILayout.TextField("Text :", _txtTexts[i]);
                    _txtSizes[i] = EditorGUILayout.IntField("Size :", _txtSizes[i]);
                    _txtFontStyles[i] = (EFontStyle)EditorGUILayout.EnumPopup("Font Style :", _txtFontStyles[i]);
                    _txtColors[i] = EditorGUILayout.ColorField("Color :", _txtColors[i]);
                    _txtBoxSizes[i] = EditorGUILayout.Vector2Field("Taille :", _txtBoxSizes[i]);
                    _txtRectPositions[i] = (ERectPosition)EditorGUILayout.EnumPopup("Text Pos :", _txtRectPositions[i]);
                    _txtOffSets[i] = EditorGUILayout.Vector2Field("Offset :", _txtOffSets[i]);

                    GUILayoutExtension.DrawHorizontalLine();
                    EditorGUILayout.Space();
                }
            }


            //PREFABS


            GUILayoutExtension.DrawHorizontalLine("Prefabs(s)");
            _addPrefab = EditorGUILayout.Toggle("Prefab", _addPrefab);

            if (_addPrefab)
            {
                _prefabNb = Mathf.Clamp(EditorGUILayout.IntField("Number : ", _prefabNb), 0, 10);
                GUILayoutExtension.DrawHorizontalLine();
                EditorGUILayout.Space();
                EditorGUILayout.Space();

                for (int i = 0; i < _prefabNb; i++)
                {
                    EditorGUILayout.LabelField("Prefab " + (i + 1).ToString(), guiStyle, new GUILayoutOption[] { });
                    EditorGUILayout.Space();

                    _prefabGameObjects[i] = EditorGUILayout.ObjectField("Prefab :", _prefabGameObjects[i], typeof(GameObject), false) as GameObject;

                    GUILayoutExtension.DrawHorizontalLine();
                    EditorGUILayout.Space();
                }
            }
        }

        EditorGUILayout.Space();
        GUILayoutExtension.DrawHorizontalLine();
        EditorGUILayout.Space();


        if (GUILayout.Button("Create step",
            new GUILayoutOption[]
            {
                GUILayout.Height(50),
            }
        ))
        {
            CreateStep(_selectedSlideTypeEnumVal);
        }


        GUILayout.EndScrollView();
    }

    static private void DrawSpecificSlideOptions()
    {
        switch (_selectedSlideTypeEnumVal)
        {
            case ESlideType.Slide:
                break;
            case ESlideType.VideoSlide:
                GUILayoutExtension.DrawHorizontalLine("Slide options");
                _video = EditorGUILayout.ObjectField("Video  : ", _video, typeof(VideoClip), false) as VideoClip;
                _autoTransition = EditorGUILayout.Toggle("Auto Transition : ", _autoTransition);
                break;
            case ESlideType.TimedSlide:
                GUILayoutExtension.DrawHorizontalLine("Slide options");
                _timeBeforeFadeOut = EditorGUILayout.FloatField("Step duration : ", _timeBeforeFadeOut);
                break;
            default:
                break;
        }
    }


    static void CreateStep(ESlideType stepType)
    {

        var selectedGO = UnityEditor.Selection.activeGameObject;
        var go = new GameObject();
        if (selectedGO != null)
        {
            go.transform.SetParent(selectedGO.transform, false);
        }
        go.name = _slideName;

        switch (stepType)
        {
            case ESlideType.Slide:
                var slide = go.AddComponent<Slide>();
                InitGlobalStepOptions(slide);
                break;
            case ESlideType.VideoSlide:
                var videoSlide = go.AddComponent<VideoSlide>();
                InitGlobalStepOptions(videoSlide);
                videoSlide.VideoClip = _video;
                break;
            case ESlideType.TimedSlide:
                var timedSlide = go.AddComponent<TimedSlide>();
                InitGlobalStepOptions(timedSlide);
                timedSlide.TimeBeforeFadeOut = _timeBeforeFadeOut;
                break;
            default:
                break;
        }

        
        if (_addCanvas)
        {
            var canvas = go.AddComponent<Canvas>();
            canvas.renderMode = _canvasRenderMode;
            canvas.planeDistance = _canvasPlaneDistance;
            if (_canvasRenderMode == RenderMode.ScreenSpaceCamera || _canvasRenderMode == RenderMode.WorldSpace)
            {
                canvas.worldCamera = _eventCamera;
            }
            if (_canvasRenderMode == RenderMode.WorldSpace)
            {
                var canvasRectTransform = canvas.GetComponent<RectTransform>();
                canvasRectTransform.anchorMin = Vector2.zero;
                canvasRectTransform.anchorMax = Vector2.one;
                canvasRectTransform.sizeDelta = _canvasSizeIfWorldSpace;
            }

            //Images
            if (_addImg)
            {
                for (int i = 0; i < _imgNb; i++)
                {
                    var image = new GameObject();
                    image.name = _imgNames[i];
                    var rectComp = image.AddComponent<RectTransform>();
                    var imgComp = image.AddComponent<Image>();
                    imgComp.sprite = _imgSprites[i];
                    imgComp.preserveAspect = true;
                    if (_imgAreFullScreen[i])
                    {
                        if (_imgSetNativeSizes[i] == true)
                        {
                            imgComp.SetNativeSize();
                        }
                        rectComp.anchorMin = Vector2.zero;
                        rectComp.anchorMax = Vector2.one;
                    }
                    else
                    {
                        if (_imgSetNativeSizes[i] == true)
                        {
                            imgComp.SetNativeSize();
                        }
                        SetRectTransform(rectComp, _imgRectPositions[i], _imgOffSets[i]);

                    }
                    image.transform.SetParent(go.transform, false);
                }
            }

            //Buttons
            if (_addBtn)
            {
                for (int i = 0; i < _btnNb; i++)
                {
                    var button = new GameObject();
                    button.name = _btnNames[i];
                    var rectComp = button.AddComponent<RectTransform>();
                    var imgComp = button.AddComponent<Image>();
                    var buttonComp = button.AddComponent<Button>();
                    imgComp.sprite = _btnSprites[i];
                    buttonComp.targetGraphic = imgComp;
                    SetRectTransform(rectComp, _btnRectPositions[i], _btnOffSets[i]);
                    if (_btnSpritesAreNatives[i] == true)
                    {
                        imgComp.SetNativeSize();
                    }
                    else
                    {
                        rectComp.sizeDelta = _btnSpriteSizes[i];
                    }
                    button.transform.SetParent(go.transform, false);

                    var text = new GameObject();
                    text.name = "Text";
                    var rectComp2 = text.AddComponent<RectTransform>();
                    var txtComp = text.AddComponent<TextMeshProUGUI>();

                    txtComp.text = _btnTextText[i];
                    txtComp.fontSize = _btnTextSizes[i];
                    txtComp.color = _btnColors[i];
                    txtComp.fontStyle = SelectFontStyle(_btnTextFonts[i]);
                    txtComp.alignment = TextAlignmentOptions.Center;

                    rectComp2.anchorMin = Vector2.zero;
                    rectComp2.anchorMax = Vector2.one;
                    rectComp2.sizeDelta = Vector2.zero;


                    text.transform.SetParent(button.transform, false);
                }
            }

            //Text
            if (_addTxt)
            {
                for (int i = 0; i < _btnNb; i++)
                {
                    var text = new GameObject();
                    var rectComp = text.AddComponent<RectTransform>();
                    var textComp = text.AddComponent<TextMeshProUGUI>();
                    text.name = _txtNames[i];
                    textComp.text = _txtTexts[i];
                    textComp.fontSize = _txtSizes[i];
                    SetRectTransform(rectComp, _txtRectPositions[i], _txtOffSets[i]);
                    textComp.fontSize = _txtSizes[i];
                    textComp.color = _txtColors[i];
                    textComp.fontStyle = SelectFontStyle(_txtFontStyles[i]);
                    textComp.alignment = TextAlignmentOptions.Center;
                    rectComp.sizeDelta = _txtBoxSizes[i];
                    text.transform.SetParent(go.transform, false);
                }
            }

            //Prefabs
            if (_addPrefab)
            {
                for (int i = 0; i < _prefabNb; i++)
                {
                    if (_prefabGameObjects[i] != null)
                    {
                        var prefab = GameObject.Instantiate(_prefabGameObjects[i], go.transform, false);
                        prefab.name = _prefabGameObjects[i].name;
                    }
                }
            }

            var canvasScaler = go.AddComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = new Vector2(1920f, 1080f);
            canvasScaler.matchWidthOrHeight = 0.5f;
        }

        
    }

    static void InitGlobalStepOptions(Slide theSlide)
    {
        theSlide.AutoTransition = _autoTransition;
    }

    static void SetRectTransform(RectTransform rectTransform, ERectPosition rectPos, Vector2 offset)
    {
        switch (rectPos)
        {
            case ERectPosition.Middle:
                rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                rectTransform.pivot = new Vector2(0.5f, 0.5f);

                break;
            case ERectPosition.MiddleLeft:
                rectTransform.anchorMin = new Vector2(0, 0.5f);
                rectTransform.anchorMax = new Vector2(0, 0.5f);
                rectTransform.pivot = new Vector2(0, 0.5f);

                break;
            case ERectPosition.MiddleRight:
                rectTransform.anchorMin = new Vector2(1, 0.5f);
                rectTransform.anchorMax = new Vector2(1, 0.5f);
                rectTransform.pivot = new Vector2(1, 0.5f);

                break;
            case ERectPosition.Top:
                rectTransform.anchorMin = new Vector2(0.5f, 1);
                rectTransform.anchorMax = new Vector2(0.5f, 1);
                rectTransform.pivot = new Vector2(0.5f, 1);

                break;
            case ERectPosition.TopLeft:
                rectTransform.anchorMin = new Vector2(0, 1);
                rectTransform.anchorMax = new Vector2(0, 1);
                rectTransform.pivot = new Vector2(0, 1);

                break;
            case ERectPosition.TopRight:
                rectTransform.anchorMin = new Vector2(1, 1);
                rectTransform.anchorMax = new Vector2(1, 1);
                rectTransform.pivot = new Vector2(1, 1);

                break;
            case ERectPosition.Bot:
                rectTransform.anchorMin = new Vector2(0.5f, 0);
                rectTransform.anchorMax = new Vector2(0.5f, 0);
                rectTransform.pivot = new Vector2(0.5f, 0);

                break;
            case ERectPosition.BotLeft:
                rectTransform.anchorMin = new Vector2(0, 0);
                rectTransform.anchorMax = new Vector2(0, 0);
                rectTransform.pivot = new Vector2(0, 0);

                break;
            case ERectPosition.BotRight:
                rectTransform.anchorMin = new Vector2(1, 0);
                rectTransform.anchorMax = new Vector2(1, 0);
                rectTransform.pivot = new Vector2(1, 0);

                break;
            default:
                break;
        }

        rectTransform.anchoredPosition = offset;
    }

    static FontStyles SelectFontStyle(EFontStyle fontStyle)
    {
        switch (fontStyle)
        {
            case EFontStyle.Normal:
                return FontStyles.Normal;
            case EFontStyle.Bold:
                return FontStyles.Bold;
            case EFontStyle.Italic:
                return FontStyles.Italic;
            default:
                return FontStyles.Normal;
        }
    }


    static void ResizeTab<T>(T[] tab, int nb)
    {
        Type elementType = tab.GetType().GetElementType();
        tab = new T[nb];
    }
}
