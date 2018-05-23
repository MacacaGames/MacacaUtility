using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A console to display Unity's debug logs in-game.
/// </summary>
public class UI_Console : MonoBehaviour
{
    public float baseWidth = 720;
    public float baseHeight = 1280;
	public bool showFPS = true;
	float deltaTime = 0.0f;
    private float baseAspect;
    private float targetAspect;
    private float m03;
    private float m13;
    private float m33;

    void Awake()
    {
		DontDestroyOnLoad(transform.gameObject);
        float targetWidth = (float)Screen.width;
        float targetHeight = (float)Screen.height;

        this.baseAspect = this.baseWidth / this.baseHeight;
        this.targetAspect = targetWidth / targetHeight;

        float factor = this.targetAspect > this.baseAspect ? targetHeight / this.baseHeight : targetWidth / this.baseWidth;

        this.m33 = 1 / factor;
        this.m03 = (targetWidth - this.baseWidth * factor) / 2 * this.m33;
        this.m13 = (targetHeight - this.baseHeight * factor) / 2 * this.m33;
    }
    struct Log
	{
		public string message;
		public string stackTrace;
		public LogType type;
	}

	/// <summary>
	/// The hotkey to show and hide the console window.
	/// </summary>
	public KeyCode toggleKey = KeyCode.BackQuote;

	List<Log> logs = new List<Log>();
	Vector2 scrollPosition;
	bool show;
	bool showTrack = false;
	bool collapse;

	// Visual elements:

	static readonly Dictionary<LogType, Color> logTypeColors = new Dictionary<LogType, Color>()
	{
		{ LogType.Assert, Color.white },
		{ LogType.Error, Color.red },
		{ LogType.Exception, Color.red },
		{ LogType.Log, Color.white },
		{ LogType.Warning, Color.yellow },
	};

	const int margin = 30;

	Rect windowRect;
	Rect titleBarRect = new Rect(0, 0, 1000, 20);
	GUIContent clearLabel = new GUIContent("Clear", "Clear the contents of the console.");
	GUIContent collapseLabel = new GUIContent("Collapse", "Hide repeated messages.");

	void OnEnable ()
	{
		windowRect = new Rect(margin, margin, (baseWidth - (margin * 2)), (baseHeight - (margin * 2)));
        Application.RegisterLogCallback(HandleLog);
	}

	void OnDisable ()
	{
		Application.RegisterLogCallback(null);
	}

	void Update ()
	{
		if(showFPS){
			deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
		}
		if (Input.GetKeyDown(toggleKey)) {
			show = !show;
		}
	}
	//public Rect buttonRect;
	void OnGUI ()
	{
		if(showFPS){
			int w = Screen.width, h = Screen.height;
 
			GUIStyle style = new GUIStyle();
	
			Rect rect = new Rect(0, 0, w, h * 2 / 100);
			style.alignment = TextAnchor.UpperLeft;
			style.fontSize = h * 2 / 100;
			style.normal.textColor = new Color (0.0f, 0.0f, 0.5f, 1.0f);
			float msec = deltaTime * 1000.0f;
			float fps = 1.0f / deltaTime;
			string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
			GUI.Label(rect, text, style);
		}
        Matrix4x4 _matrix = GUI.matrix;

        _matrix.m33 = this.m33;

        if (this.targetAspect > this.baseAspect) _matrix.m03 = this.m03;
        else _matrix.m13 = this.m13;

        GUI.matrix = _matrix;


        if (GUI.Button (new Rect (450, 1, 62, 21), "Debug")) {
			show = !show;
		}
			

		if (!show) {
			return;
		}

		windowRect = GUILayout.Window(12345, windowRect, ConsoleWindow, "Console");
	}

	/// <summary>
	/// A window that displayss the recorded logs.
	/// </summary>
	/// <param name="windowID">Window ID.</param>
	void ConsoleWindow (int windowID)
	{
		scrollPosition = GUILayout.BeginScrollView(scrollPosition);

			// Iterate through the recorded logs.
			for (int i = 0; i < logs.Count; i++) {
				var log = logs[i];

				// Combine identical messages if collapse option is chosen.
				if (collapse) {
					var messageSameAsPrevious = i > 0 && log.message == logs[i - 1].message;

					if (messageSameAsPrevious) {
						continue;
					}
				}

				GUI.contentColor = logTypeColors[log.type];
				GUILayout.Label(log.message);
				if(showTrack)GUILayout.Label(log.stackTrace);
			}

		GUILayout.EndScrollView();

		GUI.contentColor = Color.white;

		GUILayout.BeginHorizontal();

			if (GUILayout.Button(clearLabel)) {
				logs.Clear();
			}
			if (GUILayout.Button("Show Trace")) {
				showTrack = !showTrack;
			}
			collapse = GUILayout.Toggle(collapse, collapseLabel, GUILayout.ExpandWidth(false));

		GUILayout.EndHorizontal();

		// Allow the window to be dragged by its title bar.
		GUI.DragWindow(titleBarRect);
	}

	/// <summary>
	/// Records a log from the log callback.
	/// </summary>
	/// <param name="message">Message.</param>
	/// <param name="stackTrace">Trace of where the message came from.</param>
	/// <param name="type">Type of message (error, exception, warning, assert).</param>
	void HandleLog (string message, string stackTrace, LogType type)
	{
		logs.Add(new Log() {
			message = message,
			stackTrace = stackTrace,
			type = type,
		});
	}
}