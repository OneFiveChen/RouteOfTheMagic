using System.Collections;  
using System.Collections.Generic;  
using UnityEngine;  
using UnityEngine.UI;  
using UnityEngine.Events;  
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
#if UNITY_EDITOR  
using UnityEditor;
#else
using System.ComponentModel.Design;
#endif

/// <summary>
/// 扩展的BUtton，主要为了后期实现鼠标移动到Button,图标放大，目前未实现该动画。
/// TODO:代码可以进一步精简
/// </summary>
public class ButtonEx : UnityEngine.UI.Selectable,
    IPointerClickHandler,
    ISubmitHandler,
    IPointerDownHandler,
    IPointerUpHandler,
    IPointerEnterHandler,
    IPointerExitHandler
{
    /// <summary>
    /// 是否响应Click事件
    /// </summary>
    public bool fatherIsPass = false;

    /// <summary>  
    /// 长按等待时间  
    /// </summary>  
    public float onLongWaitTime = 1.5f;
    /// <summary>  
    /// 不断的产生长按事件  
    /// </summary>  
    public bool onLongContinue = false;

    [System.Serializable]
    public class ButtonClickedEvent : UnityEvent { }

    [FormerlySerializedAs("onClick")]
    [SerializeField]
    private ButtonClickedEvent m_OnClick = new ButtonClickedEvent();
    [FormerlySerializedAs("onLongClick")]
    [SerializeField]
    private ButtonClickedEvent m_OnLongClick = new ButtonClickedEvent();
    [FormerlySerializedAs("onDown")]
    [SerializeField]
    private ButtonClickedEvent m_OnDown = new ButtonClickedEvent();
    [FormerlySerializedAs("onUp")]
    [SerializeField]
    private ButtonClickedEvent m_OnUp = new ButtonClickedEvent();
    [FormerlySerializedAs("onEnter")]
    [SerializeField]
    private ButtonClickedEvent m_OnEnter = new ButtonClickedEvent();
    [FormerlySerializedAs("onExit")]
    [SerializeField]
    private ButtonClickedEvent m_OnExit = new ButtonClickedEvent();

    //private System.Reflection.PropertyInfo pro_isDown;  
    //private System.Reflection.PropertyInfo pro_isEnter;  

    protected ButtonEx() : base() { }

    /// <summary>  
    /// 文本值  
    /// </summary>  
    public string text
    {
        get
        {
            Text v = getText();
            if (v != null)
                return v.text;
            return null;
        }
        set
        {
            Text v = getText();
            if (v != null)
                v.text = value;
        }
    }

    private bool isPointerDown = false;
    private bool isPointerInside = false;

    /// <summary>  
    /// 是否被按下  
    /// </summary>  
    public bool isDown
    {
        get
        {
            return isPointerDown;
            /**  
            if (pro_isDown == null) {  
                pro_isDown = typeof(Selectable).GetProperty ("isPointerDown", getRBFlag());  
            }  
            if (pro_isDown == null)  
                return false;  
            bool b = (bool) pro_isDown.GetValue (this, null);  
            return b;  
            */
        }
    }

    /// <summary>  
    /// 是否进入  
    /// </summary>  
    public bool isEnter
    {
        get
        {
            return isPointerInside;
            /**  
            if (pro_isEnter == null) {  
                pro_isEnter = typeof(Selectable).GetProperty ("isPointerInside", getRBFlag());  
            }  
            if (pro_isEnter == null)  
                return false;  
            bool b = (bool) pro_isEnter.GetValue (this, null);  
            return b;  
            */
        }
    }

    /// <summary>  
    /// 点击事件  
    /// </summary>  
    public ButtonClickedEvent onClick
    {
        get { return m_OnClick; }
        set { m_OnClick = value; }
    }

    /// <summary>  
    /// 长按事件  
    /// </summary>  
    public ButtonClickedEvent onLongClick
    {
        get { return m_OnLongClick; }
        set { m_OnLongClick = value; }
    }

    /// <summary>  
    /// 按下事件  
    /// </summary>  
    public ButtonClickedEvent onDown
    {
        get { return m_OnDown; }
        set { m_OnDown = value; }
    }

    /// <summary>  
    /// 松开事件  
    /// </summary>  
    public ButtonClickedEvent onUp
    {
        get { return m_OnUp; }
        set { m_OnUp = value; }
    }

    /// <summary>  
    /// 进入事件  
    /// </summary>  
    public ButtonClickedEvent onEnter
    {
        get { return m_OnEnter; }
        set { m_OnEnter = value; }
    }

    /// <summary>  
    /// 离开事件  
    /// </summary>  
    public ButtonClickedEvent onExit
    {
        get { return m_OnExit; }
        set { m_OnExit = value; }
    }

    private System.Reflection.BindingFlags getRBFlag()
    {
        return System.Reflection.BindingFlags.Static |
            System.Reflection.BindingFlags.NonPublic |
            System.Reflection.BindingFlags.GetProperty |
            System.Reflection.BindingFlags.Instance |
            System.Reflection.BindingFlags.GetField |
            System.Reflection.BindingFlags.ExactBinding;
    }

    private UnityEngine.UI.Text getText()
    {
        Transform f = transform.Find("Text");
        Text v = null;
        if (f != null)
        {
            v = f.gameObject.GetComponent<Text>();
        }
        if (v == null && transform.childCount > 0)
        {
            GameObject obj = transform.GetChild(0).gameObject;
            v = obj.GetComponent<Text>();
        }
        return v;
    }

    private void Press()
    {
        if (!IsActive() || !IsInteractable()||!fatherIsPass)
            return;
        m_OnClick.Invoke();
    }

    private void Down()
    {
        if (!IsActive() || !IsInteractable())
            return;
        m_OnDown.Invoke();
        StartCoroutine(grow());
    }

    private void Up()
    {
        if (!IsActive() || !IsInteractable() || !isDown)
            return;
        m_OnUp.Invoke();
    }

    private void Enter()
    {
        if (!IsActive())
            return;
        this.gameObject.GetComponent<RectTransform>().sizeDelta *=3.0f/2;
        m_OnEnter.Invoke();
    }

    public void Exit()
    {
        if (!IsActive() || !isEnter)
            return;
        this.gameObject.GetComponent<RectTransform>().sizeDelta *=2.0f/3;
        m_OnExit.Invoke();
    }

    private void LongClick()
    {
        if (!IsActive() || !isDown)
            return;
        m_OnLongClick.Invoke();
    }

    private float downTime = 0f;
    private IEnumerator grow()
    {
        downTime = Time.time;
        while (isDown)
        {
            if (Time.time - downTime > onLongWaitTime)
            {
                LongClick();
                if (onLongContinue)
                    downTime = Time.time;
                else
                    break;
            }
            else
                yield return null;
        }
    }

    protected override void OnDisable()
    {
        isPointerDown = false;
        isPointerInside = false;
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;
        Press();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;
        isPointerDown = true;
        Down();
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;
        Up();
        isPointerDown = false;
        base.OnPointerUp(eventData);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        isPointerInside = true;
        Enter();
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        Exit();
        isPointerInside = false;
        base.OnPointerExit(eventData);
    }

    public virtual void OnSubmit(BaseEventData eventData)
    {
        Press();

        // if we get set disabled during the press  
        // don't run the coroutine.  
        if (!IsActive() || !IsInteractable())
            return;

        DoStateTransition(SelectionState.Pressed, false);
        StartCoroutine(OnFinishSubmit());
    }

    private IEnumerator OnFinishSubmit()
    {
        var fadeTime = colors.fadeDuration;
        var elapsedTime = 0f;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        DoStateTransition(currentSelectionState, false);
    }

    public static GameObject CreateUIElementRoot(string name, Vector2 size)
    {
        GameObject go = new GameObject(name);
        RectTransform rect = go.AddComponent<RectTransform>();
        rect.sizeDelta = size;
        return go;
    }
    public static GameObject CreateUIElementRoot(string name, float w, float h)
    {
        return CreateUIElementRoot(name, new Vector2(w, h));
    }

    public static GameObject CreateUIText(string name, string text, GameObject parent)
    {
        GameObject childText = CreateUIObject(name, parent);
        Text v = childText.AddComponent<Text>();
        v.text = text;
        v.alignment = TextAnchor.MiddleCenter;
        SetDefaultTextValues(v);

        RectTransform r = childText.GetComponent<RectTransform>();
        r.anchorMin = Vector2.zero;
        r.anchorMax = Vector2.one;
        r.sizeDelta = Vector2.zero;

        return childText;
    }

    public static GameObject CreateUIObject(string name, GameObject parent)
    {
        GameObject go = new GameObject(name);
        go.AddComponent<RectTransform>();
        SetParentAndAlign(go, parent);
        return go;
    }

    public static void SetDefaultTextValues(Text lbl)
    {
        lbl.color = new Color(50f / 255f, 50f / 255f, 50f / 255f, 1f);
    }

    public static void SetParentAndAlign(GameObject child, GameObject parent)
    {
        if (parent == null)
            return;

        child.transform.SetParent(parent.transform, false);
        SetLayerRecursively(child, parent.layer);
    }
    public static void SetLayerRecursively(GameObject go, int layer)
    {
        go.layer = layer;
        Transform t = go.transform;
        for (int i = 0; i < t.childCount; i++)
            SetLayerRecursively(t.GetChild(i).gameObject, layer);
    }

    public static T findRes<T>(string name) where T : Object
    {
        T[] objs = Resources.FindObjectsOfTypeAll<T>();
        if (objs != null && objs.Length > 0)
        {
            foreach (Object obj in objs)
            {
                if (obj.name == name)
                    return obj as T;
            }
        }
        objs = AssetBundle.FindObjectsOfType<T>();
        if (objs != null && objs.Length > 0)
        {
            foreach (Object obj in objs)
            {
                if (obj.name == name)
                    return obj as T;
            }
        }
        return default(T);
    }

    public static void PlaceUIElementRoot(GameObject element, MenuCommand menuCommand)
    {
#if UNITY_EDITOR
        GameObject parent = menuCommand.context as GameObject;
        if (parent == null || parent.GetComponentInParent<Canvas>() == null)
        {
            parent = GetOrCreateCanvasGameObject();
        }

        string uniqueName = GameObjectUtility.GetUniqueNameForSibling(parent.transform, element.name);
        element.name = uniqueName;
        Undo.RegisterCreatedObjectUndo(element, "Create " + element.name);
        Undo.SetTransformParent(element.transform, parent.transform, "Parent " + element.name);
        GameObjectUtility.SetParentAndAlign(element, parent);
        if (parent != menuCommand.context) // not a context click, so center in sceneview  
            SetPositionVisibleinSceneView(parent.GetComponent<RectTransform>(), element.GetComponent<RectTransform>());

        Selection.activeGameObject = element;
#endif
    }

    public static GameObject GetOrCreateCanvasGameObject()
    {
#if UNITY_EDITOR
        GameObject selectedGo = Selection.activeGameObject;

        // Try to find a gameobject that is the selected GO or one if its parents.  
        Canvas canvas = (selectedGo != null) ? selectedGo.GetComponentInParent<Canvas>() : null;
        if (canvas != null && canvas.gameObject.activeInHierarchy)
            return canvas.gameObject;

        // No canvas in selection or its parents? Then use just any canvas..  
        canvas = Object.FindObjectOfType(typeof(Canvas)) as Canvas;
        if (canvas != null && canvas.gameObject.activeInHierarchy)
            return canvas.gameObject;

        // No canvas in the scene at all? Then create a new one.  
        return CreateNewUI();
#else
            return null;  
#endif
    }

#if UNITY_EDITOR
    private static void SetPositionVisibleinSceneView(RectTransform canvasRTransform, RectTransform itemTransform)
    {
        // Find the best scene view  
        SceneView sceneView = SceneView.lastActiveSceneView;
        if (sceneView == null && SceneView.sceneViews.Count > 0)
            sceneView = SceneView.sceneViews[0] as SceneView;

        // Couldn't find a SceneView. Don't set position.  
        if (sceneView == null || sceneView.camera == null)
            return;

        // Create world space Plane from canvas position.  
        Vector2 localPlanePosition;
        Camera camera = sceneView.camera;
        Vector3 position = Vector3.zero;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRTransform, new Vector2(camera.pixelWidth / 2, camera.pixelHeight / 2), camera, out localPlanePosition))
        {
            // Adjust for canvas pivot  
            localPlanePosition.x = localPlanePosition.x + canvasRTransform.sizeDelta.x * canvasRTransform.pivot.x;
            localPlanePosition.y = localPlanePosition.y + canvasRTransform.sizeDelta.y * canvasRTransform.pivot.y;

            localPlanePosition.x = Mathf.Clamp(localPlanePosition.x, 0, canvasRTransform.sizeDelta.x);
            localPlanePosition.y = Mathf.Clamp(localPlanePosition.y, 0, canvasRTransform.sizeDelta.y);

            // Adjust for anchoring  
            position.x = localPlanePosition.x - canvasRTransform.sizeDelta.x * itemTransform.anchorMin.x;
            position.y = localPlanePosition.y - canvasRTransform.sizeDelta.y * itemTransform.anchorMin.y;

            Vector3 minLocalPosition;
            minLocalPosition.x = canvasRTransform.sizeDelta.x * (0 - canvasRTransform.pivot.x) + itemTransform.sizeDelta.x * itemTransform.pivot.x;
            minLocalPosition.y = canvasRTransform.sizeDelta.y * (0 - canvasRTransform.pivot.y) + itemTransform.sizeDelta.y * itemTransform.pivot.y;

            Vector3 maxLocalPosition;
            maxLocalPosition.x = canvasRTransform.sizeDelta.x * (1 - canvasRTransform.pivot.x) - itemTransform.sizeDelta.x * itemTransform.pivot.x;
            maxLocalPosition.y = canvasRTransform.sizeDelta.y * (1 - canvasRTransform.pivot.y) - itemTransform.sizeDelta.y * itemTransform.pivot.y;

            position.x = Mathf.Clamp(position.x, minLocalPosition.x, maxLocalPosition.x);
            position.y = Mathf.Clamp(position.y, minLocalPosition.y, maxLocalPosition.y);
        }

        itemTransform.anchoredPosition = position;
        itemTransform.localRotation = Quaternion.identity;
        itemTransform.localScale = Vector3.one;
    }
#endif

    public static GameObject CreateNewUI()
    {
        // Root for the UI  
        var root = new GameObject("Canvas");
        root.layer = LayerMask.NameToLayer("UI");
        Canvas canvas = root.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        root.AddComponent<CanvasScaler>();
        root.AddComponent<GraphicRaycaster>();
#if UNITY_EDITOR
        Undo.RegisterCreatedObjectUndo(root, "Create " + root.name);
#endif
        // if there is no event system add one...  
        CreateEventSystem(false, null);
        return root;
    }

    public static void CreateEventSystem(bool select, GameObject parent)
    {
#if UNITY_EDITOR
        var esys = Object.FindObjectOfType<EventSystem>();
        if (esys == null)
        {
            var eventSystem = new GameObject("EventSystem");
            GameObjectUtility.SetParentAndAlign(eventSystem, parent);
            esys = eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();

            Undo.RegisterCreatedObjectUndo(eventSystem, "Create " + eventSystem.name);
        }

        if (select && esys != null)
        {
            Selection.activeGameObject = esys.gameObject;
        }
#endif
    }


#if UNITY_EDITOR
    [MenuItem("GameObject/UI/ButtonEx")]
    static void CreateButtonEx(MenuCommand menuCmd)
    {
        // 创建游戏对象  
        float w = 160f;
        float h = 30f;
        GameObject btnRoot = CreateUIElementRoot("ButtonEx", w, h);

        // 创建Text对象  
        CreateUIText("Text", "Button", btnRoot);

        // 添加脚本  
        btnRoot.AddComponent<CanvasRenderer>();
        Image img = btnRoot.AddComponent<Image>();
        img.color = Color.white;
        img.fillCenter = true;
        img.raycastTarget = true;
        img.sprite = findRes<Sprite>("UISprite");
        if (img.sprite != null)
            img.type = Image.Type.Sliced;

        btnRoot.AddComponent<ButtonEx>();
        btnRoot.GetComponent<Selectable>().image = img;

        // 放入到UI Canvas中  
        PlaceUIElementRoot(btnRoot, menuCmd);
    }
#endif


}


