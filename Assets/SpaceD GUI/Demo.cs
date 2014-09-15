using UnityEngine;
using System.Collections;

public class Demo : MonoBehaviour {

	public GUISkin Skin;

	private static float GUIScale = 1.0f;
	private static Matrix4x4 oMatrix;
	private static int CurrentDesk = 0;
	private static float LoadingBarPercent = 0.0f;

	private static UIWindow Window1;
	private static UIWindow Window2;
	private static UIWindow Window3;
	private static UIWindow Window4;
	private static UIWindow Window5;
	private static UIWindow Window6;
	private static UIWindow Window7;

	private static UIAnimation Animation1;
	public Texture2D AnimationTile;

	// Variables used trough the elements
	private static bool rememberme = false;
	private static string textFieldStr = "Click to edit this text input field";
	private static string textFieldStr2 = "Please enter username";
	private static string textFieldStr3 = "Please enter password";
	private static Vector2 scrollPosition = Vector2.zero;
	private static string scrollViewText = "Fusce ac justo ornare, tempor purus eu, sagittis diam. Donec eu erat eget odio ullamcorper iaculis. Proin placerat tincidunt velit, id pharetra dolor tempor vitae.Donec eu erat eget odio ullamcorper iaculis. Fusce ac justo ornare, tempor purus eu, sagittis diam. Donec eu erat eget odio ullamcorper iaculis.\n\nFusce ac justo ornare, tempor purus eu, sagittis diam. Donec eu erat eget odio ullamcorper iaculis. Proin placerat tincidunt velit, id pharetra dolor tempor vitae.Donec eu erat eget odio ullamcorper iaculis.";
	private static int radioSelected = 0;

	// Variable to hold the cursor textures
	private static Texture2D CursorNormal;
	private static Texture2D CursorActive;

	public void Start()
	{
		// Prepare the windows
		Window1 = UIWindow.Create(new Rect(0, 0, 484, 711), 0, DrawWindow1);
		Window1.Title = "Window Title";
		Window1.Draggable = true;

		Window2 = UIWindow.Create(new Rect(484, 0, 484, 711), 1, DrawWindow2);
		Window2.Title = "Second Window";
		Window2.Draggable = true;

		Window3 = UIWindow.Create(new Rect(968, 0, 484, 711), 2, DrawWindow3);
		Window3.Title = "Third Window";
		Window3.Draggable = true;

		Window4 = UIWindow.Create(new Rect(0, 0, 484, 473), 3, DrawLoginWindow);
		Window4.verticalPivot = UIWindow.VerticalPivot.Middle;
		Window4.horizontalPivot = UIWindow.HorizontalPivot.Middle;
		Window4.SetAnchor(UIWindow.Anchor.Middle);

		// A window containing the demo controls
		Window5 = UIWindow.Create(new Rect(30, -90, (Screen.width - 60), 60), 4, DrawDemoControls);
		Window5.SetAnchor(UIWindow.Anchor.BottomLeft);
		Window5.AutoDraw = false;
		Window5.Background = false;
		Window5.Show();

		Window6 = UIWindow.Create(new Rect(0f, 0f, 924f, 714f), 6, DrawTableWindow);
		Window6.Title = "Table Window";
		Window6.Draggable = true;

		Window7 = UIWindow.Create(new Rect(0f, 0f, 931.0f, 113.0f), 7, DrawLoadingBar);
		Window7.verticalPivot = UIWindow.VerticalPivot.Middle;
		Window7.horizontalPivot = UIWindow.HorizontalPivot.Middle;
		Window7.SetAnchor(UIWindow.Anchor.Middle);
		Window7.Background = false;
		
		// Show the current desk windows
		SwitchToDesk(CurrentDesk);

		// Prepare the animation
		Animation1 = new UIAnimation(AnimationTile, 23, 1);

		// Start the example loading bar progress coroutine
		this.StartCoroutine("LoadingProgress");

		// Try loading the cursors
		CursorNormal = Resources.Load("Cursor/normal") as Texture2D;
		CursorActive = Resources.Load("Cursor/active") as Texture2D;
	}

	public void Update()
	{
		// Feed our animation update event
		Animation1.Update();

		// Set the custom cursor
		if (Input.GetMouseButton(0))
		{
			if (CursorActive)
				Cursor.SetCursor(CursorActive, new Vector2(0f, 1.0f), CursorMode.Auto);
		}
		else
		{
			if (CursorNormal)
				Cursor.SetCursor(CursorNormal, Vector2.zero, CursorMode.Auto);
		}
	}

	private static void ApplyScaling()
	{
		// save current matrix
		oMatrix = GUI.matrix;

		// substitute matrix - only scale is altered from standard
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(GUIScale, GUIScale, 1.0f));
	}

	public void OnGUI()
	{
		// Set the skin
		GUI.skin = Skin;

		// The scaling control window has the auto draw disabled
		// that means we decide where and when to draw it
		// so draw it outside the scaling
		Window5.Draw();

		// Apply GUI scaling before drawing the windows
		ApplyScaling();

		// Feed the UIWindow class OnGUI event
		UIWindow.OnGUI();

		// restore the original matrix before the scaling was done
		GUI.matrix = oMatrix;
	}

	private static void DrawDemoControls(int WindowId)
	{
		GUILayout.BeginHorizontal();

			// Scale box
			UIElements.BeginBox(GUILayout.Width(305.0f));
				GUILayout.BeginHorizontal();
				
					UIElements.Label("GUI Scale:");

					GUILayout.Space(15.0f);
				
					GUILayout.BeginVertical();
						GUILayout.Space(3.0f);
						// Make a slider for the GUI scaling value
						GUIScale = GUILayout.HorizontalSlider(GUIScale, 0.75f, 1.0f);
					GUILayout.EndVertical();
			
				GUILayout.EndHorizontal();
			UIElements.EndBox();
			
			GUILayout.Space(10f);
			
			// Toggle windows
			UIElements.BeginBox(GUILayout.Width(670.0f));
				GUILayout.Space(-1f);
				GUILayout.BeginHorizontal();

					UIElements.Label("Show:");

					GUILayout.BeginVertical();
						GUILayout.Space(-7.0f);
						GUILayout.BeginHorizontal();
			
							// Toggle between the diferrent windows, since there's not enough space for all of them
							int selectedDesk = UIElements.ToggleList(CurrentDesk, new string[4] {"Standard windows", "Login window", "Table window", "Loading bar"});
							
							// Detect a change in the toggles
							if (selectedDesk != CurrentDesk)
								SwitchToDesk(selectedDesk);
			
						GUILayout.EndHorizontal();
						GUILayout.Space(-8.0f);
					GUILayout.EndVertical();

				GUILayout.EndHorizontal();
				GUILayout.Space(1f);
			UIElements.EndBox();
		
		GUILayout.EndHorizontal();
	}

	private static void SwitchToDesk(int desk)
	{
		// Hide the current desk windows
		switch (CurrentDesk)
		{
			case 0:
				Window1.Hide();
				Window2.Hide();
				Window3.Hide();
				break;
			case 1:
				Window4.Hide();
				break;
			case 2:
				Window6.Hide();
				break;
			case 3:
				Window7.Hide();
				break;
		}
		
		// Show the new desk
		switch (desk)
		{
			case 0:
				Window1.Show();
				Window2.Show();
				Window3.Show();
				break;
			case 1:
				Window4.Show();
				break;
			case 2:
				Window6.Show();
				break;
			case 3:
				Window7.Show();
				break;
		}
		
		// Update the current desk variable
		CurrentDesk = desk;
	}

	private static void DrawWindow1(int WindowId)
	{
		GUILayout.BeginVertical();

		// Do a text with the first style
		UIElements.Text("Suspendisse potenti. Cras eleifend nisi sit amet molestie pellentesque. Fusce vehicula eros neque, a suscipit tortor tristique id. Nam ullamcorper luctus tempus. Sed posuere volutpat dolor. ");

		// Add space
		GUILayout.Space(27.0f);

		// Do an image
		UIElements.ImageBox("image");

		// Add space
		GUILayout.Space(21.0f);

		// Do a separator
		UIElements.Separator();

		// Add space
		GUILayout.Space(15.0f);

		// Do some toggles
		Window4.ShowWindow = UIElements.Toggle(Window4.ShowWindow, "Display the login window?");
		GUILayout.Space(8.0f);
		Window2.ShowWindow = UIElements.Toggle(Window2.ShowWindow, "Display the second window?");
		GUILayout.Space(8.0f);
		Window3.ShowWindow = UIElements.Toggle(Window3.ShowWindow, "Display the thrid window?");

		// Add space
		GUILayout.Space(14.0f);

		// Do a separator
		UIElements.Separator();

		// Add space
		GUILayout.Space(23.0f);

		// Do a text with the second style
		UIElements.Text("Suspendisse potenti. Cras eleifend nisi sit amet molestie pellentesque. Fusce vehicula eros neque, a suscipit tortor tristique id. Nam ullamcorper luctus tempus. Sed posuere volutpat dolor.", UIElements.TextStyle.Two);

		GUILayout.EndVertical();
	}

	private static void DrawWindow2(int WindowId)
	{
		GUILayout.BeginVertical();
		GUILayout.Space(1.0f);

		// Do a button
		UIElements.Button("BUTTON", UIElements.Align.Center);

		// Do a text area
		//textAreaStr = GUI.TextArea(new Rect(68, 226, 350, 182), textAreaStr);

		// Add space
		GUILayout.Space(20.0f);

		// Do a box
		UIElements.BeginBox();
		UIElements.BoxTitle("Text Container");
		UIElements.BoxText("Suspendisse potenti. Cras eleifend nisi sit amet molestie pellentesque. Fusce vehicula eros neque, a suscipit tortor tristique id. Nam ullamcorper luctus tempus. Sed posuere volutpat dolor. Nunc nibh lacus, congue eu scelerisque non, euismod non libero.");
		UIElements.EndBox();

		// Add space
		GUILayout.Space(22.0f);

		// Do a text field
		textFieldStr = GUILayout.TextField(textFieldStr);

		// Add space
		GUILayout.Space(17.0f);

		// Do a group of radio style toggles
		radioSelected = UIElements.ToggleList(radioSelected, new string[3] {"Semper facilisis tellus ?", "Phasellus eu sodales leo!", "Aliquam semper facilisis tellus..."});

		GUILayout.EndVertical();
	}

	private static void DrawWindow3(int WindowId)
	{
		GUILayout.BeginVertical();

		// Do a scroll view
		scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Height(148));

		// Do a text with the first style
		UIElements.Text(scrollViewText);

		// End the scroll view
		GUILayout.EndScrollView();

		// Add space
		GUILayout.Space(20.0f);

		// Do a horizontal slider
		Animation1.PercentageUpdatePeriod = GUILayout.HorizontalSlider(Animation1.PercentageUpdatePeriod, 0.0f, 0.2f);

		// Add space
		GUILayout.Space(21.0f);

		// Begin a horizontal group
		GUILayout.BeginHorizontal();

		// Do a vertical slider
		Animation1.FPS = GUILayout.VerticalSlider(Animation1.FPS, 20.0f, 40.0f, GUILayout.Height(160.0f));

		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();

		// Do the loading animation, begin a group for it
		GUI.BeginGroup(new Rect(179, 388, 128, 106));
		
		// Draw the animation background
		GUI.Label(new Rect(0, 0, 128, 106), "", "animationBackground");
		
		// Draw the tiled animation
		Animation1.Draw(new Rect(24, 14, 80, 78));

		// Draw the animation percentage
		GUIStyle PercentageStyle = GUI.skin.GetStyle("animationPercentage");
		GUIStyle TextStyle = GUI.skin.GetStyle("animationText");
		GUIStyle TextShadowStyle = GUI.skin.GetStyle("animationTextShadow");

		UIElements.TextWithShadow(new Rect(35, 33, 59, 25), Animation1.Percentage.ToString() + "%", PercentageStyle, PercentageStyle.normal.textColor, TextShadowStyle.normal.textColor, new Vector2(0.0f, 1.0f));
		UIElements.TextWithShadow(new Rect(35, 56, 59, 15), "loading", TextStyle, TextStyle.normal.textColor, TextShadowStyle.normal.textColor, new Vector2(0.0f, 1.0f));

		// End the animation group
		GUI.EndGroup();

		GUILayout.Space(24f);
		
		GUILayout.BeginHorizontal();
		GUILayout.Space(5.0f);
		
		// Do the tiny icon buttons
		UIElements.TinyButton(UIElements.TinyButtons.Accept);
		GUILayout.Space(18.0f);
		UIElements.TinyButton(UIElements.TinyButtons.Decline);
		GUILayout.Space(18.0f);
		UIElements.TinyButton(UIElements.TinyButtons.Social);
		GUILayout.Space(18.0f);
		UIElements.TinyButton(UIElements.TinyButtons.Mail);
		
		// Add horizonta space
		GUILayout.Space(28.0f);
		GUILayout.BeginVertical();
		GUILayout.Space(4.0f);
		GUILayout.BeginHorizontal();
		
		// Do pagination arrows
		UIElements.ArrowButton(UIElements.ArrowButtons.Left);
		GUILayout.Space(1.0f);
		UIElements.ArrowButton(UIElements.ArrowButtons.Middle);
		GUILayout.Space(1.0f);
		UIElements.ArrowButton(UIElements.ArrowButtons.Right);
		
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();

		// Add space
		GUILayout.Space(25.0f);
		
		GUILayout.BeginHorizontal();
		GUILayout.Space(6.0f);
		
		// Do a small button
		UIElements.SmallButton("Button");
		
		GUILayout.EndHorizontal();
		
		// Do a positioned tooltip box
		// there is a non-positioned but i dont see why anybody would need it... : ]
		UIElements.TooltipBox(new Vector2(230.0f, 588.0f), "Integer pulvinar orci eu lorem...");

		GUILayout.EndVertical();
	}

	private static void DrawLoginWindow(int WindowId)
	{
		GUILayout.BeginVertical();
		GUILayout.Space(23.0f);

		// Do a label
		UIElements.InputLabel("Username / Email");
		
		// Do a text field
		textFieldStr2 = GUILayout.TextField(textFieldStr2);

		// Add space
		GUILayout.Space(11.0f);

		// Do a label
		UIElements.InputLabel("Password");
		
		// Do a text field
		textFieldStr3 = GUILayout.TextField(textFieldStr3);

		GUILayout.Space(15.0f);
		GUILayout.BeginHorizontal();
		GUILayout.Space(100.0f);

		// Do a toggle
		rememberme = UIElements.Toggle(rememberme, "Remember me?");

		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.Space(13.0f);

		// Do a button
		UIElements.Button("LOGIN", UIElements.Align.Center);

		GUILayout.EndVertical();
	}

	private static void DrawTableWindow(int WindowId)
	{
		GUILayout.BeginVertical();
		GUILayout.Space(-32.0f);
		GUILayout.BeginHorizontal();
		GUILayout.Space(-35.0f);
		
		// Do a table, make sure you give unique ID's to every diferrent table
		UIElements.BeginTable(1);
		
		// Do the table header
		UIElements.BeginTableHeader();
		UIElements.TableColumn("Etiam eu", 150.0f);
		UIElements.TableColumn("Ipsum diam", 140.0f);
		UIElements.TableColumn("Nunc", 88.0f);
		UIElements.TableColumn("Convallis", 120.0f);
		UIElements.TableColumn("Blandit", 147.0f);
		UIElements.TableColumn("");
		UIElements.EndTableHeader();

		// Add vertical space
		GUILayout.Space(11f);

		// Do the table body
		UIElements.BeginTableBody(506.0f);

		// Do some rows
		for (int i = 0; i < 20; i++)
		{
			RectInteraction rowInteraction = UIElements.BeginTableRow();

				UIElements.TableColumn((rowInteraction.IsHovered ? "<color=#8ab6bcff>" : "<color=#8ab6bcff>") + "A title goes here</color>");
				UIElements.TableColumn("<color=#667175ff>255 / 255</color>   <color=#aa3330ff><b>FULL</b></color>");
				UIElements.TableColumn("<color=#425934ff>12 ms</color>", GUI.skin.GetStyle("tableTextLarge"));
				UIElements.TableColumn("<i>None</i>");
				UIElements.TableColumn("<i>None</i>");

				// Begin a column
				UIElements.BeginTableColumn(104.0f);
				
					GUILayout.BeginVertical();
					GUILayout.Space(10.0f);
					GUILayout.BeginHorizontal();
					GUILayout.Space(4.0f);
					
					// Do some buttons
					UIElements.IconButton(UIElements.IconButtons.Heart);
					GUILayout.Space(14.0f);
					UIElements.IconButton(UIElements.IconButtons.Plus);
					GUILayout.Space(14.0f);
					UIElements.IconButton(UIElements.IconButtons.Decline);
					GUILayout.Space(14.0f);
					UIElements.IconButton(UIElements.IconButtons.Star);
					
					GUILayout.EndHorizontal();
					GUILayout.EndVertical();
				
				UIElements.EndTableColumn();

			UIElements.EndTableRow();
		}

		UIElements.EndTableBody();
		UIElements.EndTable();
		
		GUILayout.Space(-24.0f);
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
	}

	private static void DrawLoadingBar(int WindowId)
	{
		// Draw the bar
		UIElements.LoadingBar(new Vector2(0.0f, 0.0f), LoadingBarPercent);
	}
	
	private IEnumerator LoadingProgress()
	{
		float Duration = 4.0f;
		float ResetDelay = 1.0f;
		
		// Reset to 0%
		LoadingBarPercent = 0.0f;
		
		// Get the timestamp
		float startTime = Time.time;
		
		while (Time.time < (startTime + Duration))
		{
			float RemainingTime = (startTime + Duration) - Time.time;
			float ElapsedTime = Duration - RemainingTime;
			
			// update the percent value
			LoadingBarPercent = (ElapsedTime / Duration);
			
			yield return 0;
		}
		
		// Round to 100%
		LoadingBarPercent = 1.0f;
		
		// Duration of the display of the notification
		yield return new WaitForSeconds(ResetDelay);
		
		// Get the timestamp
		startTime = Time.time;
		
		while (Time.time < (startTime + Duration))
		{
			float RemainingTime = (startTime + Duration) - Time.time;
			
			// update the percent value
			LoadingBarPercent = (RemainingTime / Duration);
			
			yield return 0;
		}
		
		// Reset to 0%
		LoadingBarPercent = 0.0f;
		
		// Duration of the display of the notification
		yield return new WaitForSeconds(ResetDelay);
		
		// Start it again
		this.StartCoroutine("LoadingProgress");
	}
}

