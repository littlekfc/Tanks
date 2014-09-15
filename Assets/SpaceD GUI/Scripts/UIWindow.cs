using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIWindow {

	public enum Anchor
	{
		TopLeft,
		TopMiddle,
		TopRight,
		Left,
		Middle,
		Right,
		BottomLeft,
		BottomMiddle,
		BottomRight
	}

	public enum VerticalPivot
	{
		Top,
		Middle,
		Bottom,
	}

	public enum HorizontalPivot
	{
		Left,
		Middle,
		Right,
	}

	public delegate void OnWindowOpen();
	public delegate void OnWindowClose();

	// STATIC
	private static Dictionary<int, UIWindow> mWindows = new Dictionary<int, UIWindow>();

	// Store the currently drawing window, the variable is nulled once done drawing
	private static UIWindow mCurrent;

	/// <summary>
	/// Gets the currently being drawn <see cref="UIWindow"/> if any at all.
	/// </summary>
	/// <value>The currently drawn UIWindow.</value>
	public static UIWindow Current { get { return mCurrent; } }

	// This contains the window id of the lastly hovered window
	private static int MouseOverWindowId = -1;

	// We need an object to call coroutines from
	private static GameObject mCoroutines;

	/// <summary>
	/// Create <see cref="UIWindow"/> with the given rect, ID and Draw callback.
	/// </summary>
	/// <param name="rect">Window rect.</param>
	/// <param name="WindowId">Window identifier.</param>
	/// <param name="callback">Draw callback.</param>
	public static UIWindow Create(Rect rect, int WindowId, GUI.WindowFunction callback)
	{
		// Initialize an object
		return new UIWindow(rect, WindowId, callback);
	}

	/// <summary>
	/// Get the specified <see cref="UIWindow"/> by the given ID.
	/// </summary>
	/// <param name="WindowId">Window identifier.</param>
	public static UIWindow Get(int WindowId)
	{
		if (mWindows.ContainsKey(WindowId))
			return mWindows[WindowId];

		return null;
	}

	public static void OnGUI()
	{
		// Call the draw for all the windows
		foreach (KeyValuePair<int, UIWindow> window in mWindows)
		{
			// If we have the auto draw set to true
			if (window.Value.AutoDraw)
				window.Value.Draw();
		}

		// After all the windows have been drawn
		// set which one of them has the mouse over
		// this is important to be done after all the windows have been drawn
		foreach (KeyValuePair<int, UIWindow> window in mWindows)
			window.Value.mIsMouseOver = (window.Value.WindowId == MouseOverWindowId);

	}

	private static void OnShowWindow(int WindowId)
	{
		UIWindow window = mWindows[WindowId];

		// Check for delegates
		if (window.OnOpen != null)
			window.OnOpen();
	}

	private static void OnHideWindow(int WindowId)
	{
		UIWindow window = mWindows[WindowId];

		// Check for delegates
		if (window.OnClose != null)
			window.OnClose();
	}

	/// <summary>
	/// Gets anchor offset based on the current screen size.
	/// </summary>
	/// <returns>The anchor position.</returns>
	/// <param name="anch">Anch.</param>
	public static Vector2 GetAnchorPosition(Anchor anch)
	{
		Vector2 position = Vector2.zero;

		// Prepare the position for this anchor
		switch (anch)
		{
			case Anchor.TopLeft: 		position = Vector2.zero; 											break;
			case Anchor.TopMiddle: 		position = new Vector2(Screen.width / 2.0f, 0.0f); 					break;
			case Anchor.TopRight: 		position = new Vector2(Screen.width, 0.0f); 						break;
			case Anchor.Left: 			position = new Vector2(0.0f, Screen.height / 2.0f); 				break;
			case Anchor.Middle: 		position = new Vector2(Screen.width / 2.0f, Screen.height / 2.0f); 	break;
			case Anchor.Right: 			position = new Vector2(Screen.width, Screen.height / 2.0f); 		break;
			case Anchor.BottomLeft: 	position = new Vector2(0.0f, Screen.height); 						break;
			case Anchor.BottomMiddle: 	position = new Vector2(Screen.width / 2.0f, Screen.height); 		break;
			case Anchor.BottomRight: 	position = new Vector2(Screen.width, Screen.height); 				break;
		}

		return position;
	}

	/// <summary>
	/// Gets the coroutines object.
	/// </summary>
	/// <returns>The coroutines object.</returns>
	public static UIWindowCoroutines GetCoroutinesObject()
	{
		if (mCoroutines == null)
			mCoroutines = GameObject.Find("_UIWindowCoroutines");

		if (mCoroutines == null)
		{
			mCoroutines = new GameObject("_UIWindowCoroutines");
			mCoroutines.AddComponent("UIWindowCoroutines");
		}
		
		return mCoroutines.GetComponent("UIWindowCoroutines") as UIWindowCoroutines;
	}

	private static void DrawTitle(Rect rect, string text)
	{
		text = text.ToUpper();

		Vector2 bgOffset = new Vector2(36, 15);
		Rect windowTitleBGRect = new Rect(bgOffset.x, bgOffset.y, (rect.width - (2.0f * bgOffset.x)), 91.0f);
		
		// Lay the title background
		GUI.Label(windowTitleBGRect, "", "windowTitleBackground");
		
		Vector2 titleOffset0 = new Vector2(0, 56);
		Vector2 titleOffset1 = new Vector2(2, 53);
		Vector2 titleOffset2 = new Vector2(-3, 57);
		Vector2 titleOffset3 = new Vector2(1, 55);
		Vector2 titleOffset4 = new Vector2(4, 53);
		Vector2 titleSize = new Vector2(rect.width, 29);
		
		// Draw the title
		GUI.Label(new Rect(titleOffset0.x, titleOffset0.y, titleSize.x, titleSize.y), text, "windowTitle");
		GUI.Label(new Rect(titleOffset1.x, titleOffset1.y, titleSize.x, titleSize.y), text, "windowTitleShadow");
		GUI.Label(new Rect(titleOffset2.x, titleOffset2.y, titleSize.x, titleSize.y), text, "windowTitleShadow");
		GUI.Label(new Rect(titleOffset3.x, titleOffset3.y, titleSize.x, titleSize.y), text, "windowTitleShadow");
		GUI.Label(new Rect(titleOffset4.x, titleOffset4.y, titleSize.x, titleSize.y), text, "windowTitleShadow");
		
		Vector2 olOffset = new Vector2(37, 41);
		Rect windowTitleOLRect = new Rect(olOffset.x, olOffset.y, (rect.width - (2.0f * olOffset.x)), 55.0f);
		
		// Lay the title overlay
		GUI.Label(windowTitleOLRect, "", "windowTitleOverlay");
	}

	// OOP
	private bool mShowWindow = false;
	private bool mShowWindowInternal = false;

	/// <summary>
	/// The on open delegate.
	/// </summary>
	public OnWindowOpen OnOpen;

	/// <summary>
	/// The on close delegate.
	/// </summary>
	public OnWindowClose OnClose;

	/// <summary>
	/// Gets or sets a value indicating whether this <see cref="UIWindow"/> should be shown.
	/// </summary>
	/// <value><c>true</c> if show window; otherwise, <c>false</c>.</value>
	public bool ShowWindow
	{
		get { return this.mShowWindow; }
		set {
			if (this.UseFadeAnimation)
			{
				FadeMethods method = (value ? FadeMethods.In : FadeMethods.Out);
				
				if (this.animationCurrentMethod != method && this.mFadeCoroutine != null)
					this.mFadeCoroutine.Stop();
				
				// Start the new animation
				if (this.animationCurrentMethod != method)
					this.mFadeCoroutine = new UICoroutine(GetCoroutinesObject(), this.FadeAnimation(method));
			}
			
			// Important to be set after the coroutine has been started
			this.mShowWindow = value;
		}
	}

	/// <summary>
	/// Determines whether the window is shown.
	/// </summary>
	/// <returns><c>true</c> if this instance is shown; otherwise, <c>false</c>.</returns>
	public bool IsShown() { return this.mShowWindow; }

	/// <summary>
	/// Show the window.
	/// </summary>
	public void Show() { this.Toggle(true); }

	/// <summary>
	/// Hide the window.
	/// </summary>
	public void Hide() { this.Toggle(false); }

	// This is called internally
	private void Toggle(bool value)
	{
		this.ShowWindow = value;
	}

	private float WindowAlpha = 0.0f;
	private bool mAutoDraw = true;

	/// <summary>
	/// Gets or sets a value indicating whether this <see cref="UIWindow"/> should be drawn automatically.
	/// </summary>
	/// <value><c>true</c> if auto draw; otherwise, <c>false</c>.</value>
	public bool AutoDraw
	{
		get { return this.mAutoDraw; }
		set { this.mAutoDraw = value; }
	}

	// This is the rect used for the current draw
	private Rect mCurrentRect;
	private Vector2 appliedPivotOffset = Vector2.zero;

	// This is the original window rect
	private Rect mWindowRect;

	/// <summary>
	/// Gets or sets the window rect.
	/// </summary>
	/// <value>The window rect.</value>
	public Rect WindowRect
	{
		get { return this.mWindowRect; }
		set { this.mWindowRect = value; }
	}

	/// <summary>
	/// Sets the position of the window.
	/// </summary>
	/// <param name="pos">Position.</param>
	public void SetPosition(Vector2 pos)
	{
		this.mWindowRect.x = pos.x;
		this.mWindowRect.y = pos.y;
	}

	/// <summary>
	/// Sets the size of the window.
	/// </summary>
	/// <param name="size">Size.</param>
	public void SetSize(Vector2 size)
	{
		this.mWindowRect.width = size.x;
		this.mWindowRect.height = size.y;
	}

	private bool mAnchored = false;
	private UIWindow.Anchor mAnchor;

	/// <summary>
	/// Sets the anchor.
	/// </summary>
	/// <param name="anch">Anch.</param>
	public void SetAnchor(UIWindow.Anchor anch)
	{
		this.mAnchor = anch;
		this.mAnchored = true;
	}

	/// <summary>
	/// The vertical pivot of the window.
	/// </summary>
	public VerticalPivot verticalPivot = VerticalPivot.Top;

	/// <summary>
	/// The horizontal pivot of the window.
	/// </summary>
	public HorizontalPivot horizontalPivot = HorizontalPivot.Left;

	/// <summary>
	/// Gets the window ID.
	/// </summary>
	/// <value>The I.</value>
	public int ID { get { return this.WindowId; } }
	protected int WindowId;

	/// <summary>
	/// The draw callback.
	/// </summary>
	protected GUI.WindowFunction callback;

	/// <summary>
	/// Gets or sets the window title.
	/// </summary>
	/// <value>The title.</value>
	public string Title { get { return this.mTitle; } set { this.mTitle = value; } }
	protected string mTitle = "";

	/// <summary>
	/// Gets or sets a value indicating whether this <see cref="UIWindow"/> should be drawn with background.
	/// </summary>
	/// <value><c>true</c> if background; otherwise, <c>false</c>.</value>
	public bool Background { get { return this.mWithBackground; } set { this.mWithBackground = value; } }
	protected bool mWithBackground = true;

	/// <summary>
	/// Gets or sets a value indicating whether this <see cref="UIWindow"/> is draggable.
	/// </summary>
	/// <value><c>true</c> if draggable; otherwise, <c>false</c>.</value>
	public bool Draggable { get { return this.mDraggable; } set { this.mDraggable = value; } }
	protected bool mDraggable = false;

	/// <summary>
	/// Determines whether the mouse is over this window.
	/// </summary>
	/// <returns><c>true</c> if this instance is mouse over; otherwise, <c>false</c>.</returns>
	public bool IsMouseOver() { return mIsMouseOver; }
	protected bool mIsMouseOver = false;

	/// <summary>
	/// Initializes a new instance of the <see cref="UIWindow"/> class.
	/// </summary>
	/// <param name="rect">Rect.</param>
	/// <param name="WindowId">Window identifier.</param>
	/// <param name="callback">Callback.</param>
	public UIWindow(Rect rect, int WindowId, GUI.WindowFunction callback)
	{
		this.mWindowRect = rect;
		this.WindowId = WindowId;
		this.callback = callback;

		// Save it
		mWindows.Add(WindowId, this);
	}

	/// <summary>
	/// Gets the pivot offset.
	/// </summary>
	/// <returns>The pivot offset.</returns>
	public Vector2 GetPivotOffset()
	{
		Vector2 offset = Vector2.zero;

		switch (this.horizontalPivot)
		{
			case HorizontalPivot.Middle:
				offset.x = (0f - (this.mWindowRect.width / 2f));
				break;
			case HorizontalPivot.Right:
				offset.x = (0f - this.mWindowRect.width);
				break;
		}

		switch (this.verticalPivot)
		{
			case VerticalPivot.Middle:
				offset.y = (0f - (this.mWindowRect.height / 2f));
				break;
			case VerticalPivot.Bottom:
				offset.y = (0f - this.mWindowRect.height);
				break;
		}

		return offset;
	}

	/// <summary>
	/// Makes internal position calculation to include anchor and pivot offsets.
	/// </summary>
	private void PreDrawCalculations()
	{
		// Apply original rect
		this.mCurrentRect = this.mWindowRect;

		// Check for anchoring and apply anchor position
		if (this.mAnchored)
		{
			this.mCurrentRect.x = this.mCurrentRect.x + GetAnchorPosition(this.mAnchor).x;
			this.mCurrentRect.y = this.mCurrentRect.y + GetAnchorPosition(this.mAnchor).y;
		}

		// Add the pivot
		this.appliedPivotOffset = this.GetPivotOffset();
		this.mCurrentRect.x = this.mCurrentRect.x + this.appliedPivotOffset.x;
		this.mCurrentRect.y = this.mCurrentRect.y + this.appliedPivotOffset.y;
	}

	/// <summary>
	/// Extracts position changes after the draw call, in case the window was dragged.
	/// </summary>
	private void ExtractPositionChanges()
	{
		// Remove the pivot offset
		this.mCurrentRect.x = this.mCurrentRect.x - this.appliedPivotOffset.x;
		this.mCurrentRect.y = this.mCurrentRect.y - this.appliedPivotOffset.y;

		// Reset the variable
		this.appliedPivotOffset = Vector2.zero;

		// Check if we had anchoring
		if (this.mAnchored)
		{
			this.mCurrentRect.x = this.mCurrentRect.x - GetAnchorPosition(this.mAnchor).x;
			this.mCurrentRect.y = this.mCurrentRect.y - GetAnchorPosition(this.mAnchor).y;
		}

		// Apply the extracted position
		this.mWindowRect.x = this.mCurrentRect.x;
		this.mWindowRect.y = this.mCurrentRect.y;
	}

	private float mOriginalAlpha = 1.0f;
	private void ApplyAlpha(float newAlpha)
	{
		// Check if we need to change the alpha at all
		if (GUI.color.a == newAlpha)
			return;

		// Keep the original alpha so we can restore it later
		this.mOriginalAlpha = GUI.color.a;

		// Set the alpha
		GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, newAlpha);
	}

	private void RestoreAlpha()
	{
		// Check if we need restore it
		if (GUI.color.a == this.mOriginalAlpha)
			return;

		// Restore the alpha
		GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, this.mOriginalAlpha);
	}

	/// <summary>
	/// Draws the window, this should be done automatically if not set otherwise.
	/// </summary>
	public void Draw()
	{
		// Make some changes to the current rect
		this.PreDrawCalculations();

		// Apply alpha outside the window
		this.ApplyAlpha(this.WindowAlpha);

		// Get the window style
		GUIStyle style = new GUIStyle(GUI.skin.GetStyle("window"));

		// If we have a header we need to increase the padding
		if (!string.IsNullOrEmpty(this.mTitle))
			style.padding = new RectOffset(style.padding.left, style.padding.right, 130, style.padding.bottom);

		// Check if we dont wanna have background
		if (!this.mWithBackground)
		{
			style.padding = new RectOffset(0, 0, 0, 0);
			style.normal.background = null;
		}

		bool display = (this.animationCurrentMethod != FadeMethods.None) ? this.mShowWindowInternal : this.mShowWindow;

		// Draw the window
		if (display)
			this.mCurrentRect = GUI.Window(this.WindowId, this.mCurrentRect, this.OnCallback, "", style);


		// Restore the original alpha
		this.RestoreAlpha();

		// After the window has been drawn
		// extract any position changes
		// made to the current rect
		this.ExtractPositionChanges();
	}

	private Rect GetBackgroundRect()
	{
		return new Rect(0, 0, this.mWindowRect.width, this.mWindowRect.height);
	}

	public void OnCallback(int WindowId)
	{
		// Set this window as the current one
		mCurrent = this;

		// Set the window alpha
		if (GUI.color.a != this.WindowAlpha)
			GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, this.WindowAlpha);

		// Draw the title
		if (!string.IsNullOrEmpty(this.mTitle))
			DrawTitle(this.GetBackgroundRect(), this.mTitle);

		// Call the window draw callback
		this.callback(this.WindowId);

		// Check for draggable
		if (this.mDraggable)
		{
			// Check if we are using a title
			if (!string.IsNullOrEmpty(this.mTitle))
				GUI.DragWindow(new Rect(30.0f, 30.0f, (this.GetBackgroundRect().width - 60.0f), 70.0f));
			else
				GUI.DragWindow(new Rect(0, 0, this.GetBackgroundRect().width, this.GetBackgroundRect().height));
		}

		// Detect when the mouse is over this window
		DetectMouseOverWindow();

		// Null the current window variable
		mCurrent = null;
	}

	private void DetectMouseOverWindow()
	{
		GUI.Box(new Rect(0, 0, this.mCurrentRect.width, this.mCurrentRect.height), new GUIContent("", "mouseOverWindow:" + this.WindowId), new GUIStyle());
		
		if (Event.current.type == EventType.Repaint && GUI.tooltip == "mouseOverWindow:" + this.WindowId)
			MouseOverWindowId = this.WindowId;
	}

	/*
	 * Show / Hide Fade Animation
	 */
	
	private enum FadeMethods
	{
		None,
		In,
		Out
	}
	
	private bool mUseFadeAnimation = true;

	/// <summary>
	/// Gets or sets a value indicating whether this <see cref="UIWindow"/> should use fade animations.
	/// </summary>
	/// <value><c>true</c> if use fade animation; otherwise, <c>false</c>.</value>
	public bool UseFadeAnimation {
		get { return this.mUseFadeAnimation; }
		set {
			if (!value)
				this.WindowAlpha = 1.0f;
			else
			{
				if (this.mShowWindow)
					this.WindowAlpha = 1.0f;
				else
					this.WindowAlpha = 0.0f;
			}
			
			this.mUseFadeAnimation = value;
		}
	}
	
	private FadeMethods animationCurrentMethod = FadeMethods.None;
	private UICoroutine mFadeCoroutine;
	private float FadeDuration = 0.5f;

	/// <summary>
	/// Sets the duration of the fade animations.
	/// </summary>
	/// <param name="v">V.</param>
	public void SetFadeDuration(float v) { this.FadeDuration = v; }
	
	// Show / Hide fade animation coroutine
	private IEnumerator FadeAnimation(FadeMethods method)
	{
		// Check if we are trying to fade in and the window is already shown
		if (method == FadeMethods.In && this.IsShown())
			yield break;
		else if (method == FadeMethods.Out && !this.IsShown())
			yield break;
		
		// Define that animation is in progress
		this.animationCurrentMethod = method;
		
		// Get the timestamp
		float startTime = Time.time;
		
		// Determine Fade in or Fade out
		if (method == FadeMethods.In)
		{
			// Show the window
			this.mShowWindowInternal = true;
			
			// Call the on show
			OnShowWindow(this.WindowId);
			
			// Calculate the time we need to fade in from the current alpha
			float internalDuration = (this.FadeDuration - (this.FadeDuration * this.WindowAlpha));
			
			// Update the start time
			startTime -= (this.FadeDuration - internalDuration);
			
			// Fade In
			while (Time.time < (startTime + internalDuration))
			{
				float RemainingTime = (startTime + this.FadeDuration) - Time.time;
				float ElapsedTime = this.FadeDuration - RemainingTime;
				
				// Update the alpha by the percentage of the time elapsed
				this.WindowAlpha = ElapsedTime / this.FadeDuration;
				
				yield return 0;
			}
			
			// Make sure it's 1
			this.WindowAlpha = 1.0f;
		}
		else if (method == FadeMethods.Out)
		{
			// Calculate the time we need to fade in from the current alpha
			float internalDuration = (this.FadeDuration * this.WindowAlpha);
			
			// Update the start time
			startTime -= (this.FadeDuration - internalDuration);
			
			// Fade Out
			while (Time.time < (startTime + internalDuration))
			{
				float RemainingTime = (startTime + this.FadeDuration) - Time.time;
				
				// Update the alpha by the percentage of the remaing time
				this.WindowAlpha = RemainingTime / this.FadeDuration;
				
				yield return 0;
			}
			
			// Make sure it's 0
			this.WindowAlpha = 0.0f;
			
			// Hide the window
			this.mShowWindowInternal = false;
			
			// Call the on hide
			OnHideWindow(this.WindowId);
		}
		
		// No longer animating
		this.animationCurrentMethod = FadeMethods.None;
	}
}