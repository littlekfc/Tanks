using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIElements {

	public enum Align
	{
		Left,
		Right,
		Center
	}

	public enum TextStyle
	{
		One,
		Two
	}

	// In case we are using positioned tooltip max width is always needed
	private static float defaultTooltipMaxWidth = 300.0f;

	// GUILayout Elements
	public static void Text(string text)
	{
		Text(text, TextStyle.One);
	}

	public static void Text(string text, TextStyle mStyle)
	{
		string styleStr = "textStyle1";

		switch (mStyle)
		{
			case TextStyle.One: styleStr = "textStyle1"; break;
			case TextStyle.Two: styleStr = "textStyle2"; break;
		}

		GUIStyle style = GUI.skin.GetStyle(styleStr);
		GUIStyle styleShadow = GUI.skin.GetStyle(styleStr + "Shadow");
		
		TextWithShadow(text, style, style.normal.textColor, styleShadow.normal.textColor, new Vector2(1.0f, 2.0f));
	}

	private static string StripColorTag(string source)
	{
		return System.Text.RegularExpressions.Regex.Replace(source, "<(color=#(.*?)|/color)>", string.Empty);
	}
	
	public static void TextWithShadow(string content, GUIStyle style, Color txtColor, Color shadowColor, Vector2 direction)
	{
		TextWithShadow(new GUIContent(content), style, txtColor, shadowColor, direction);
	}
	
	public static void TextWithShadow(GUIContent content, GUIStyle style, Color txtColor, Color shadowColor, Vector2 direction)
	{
		GUIStyle newStyle = new GUIStyle(style);
		
		// Get the rect where the text should be placed
		Rect rect = GUILayoutUtility.GetRect(content, style);
		
		newStyle.normal.textColor = shadowColor;
		GUI.Label(new Rect((direction.x + rect.x), (direction.y + rect.y), rect.width, rect.height), new GUIContent(StripColorTag(content.text)), newStyle);
		
		newStyle.normal.textColor = txtColor;
		GUI.Label(rect, content, newStyle);
	}

	public static void ImageBox(string textureResource)
	{
		Texture2D tex = Resources.Load(textureResource) as Texture2D;
		
		if (tex)
			ImageBox(tex);
	}
	
	public static void ImageBox(Texture2D imageTexture)
	{
		GUIStyle background = GUI.skin.GetStyle("imageBox");
		GUIStyle overlay = GUI.skin.GetStyle("imageFrame");
		
		// Draw the image with the background
		GUILayout.Box(imageTexture, background);
		
		// Get the last rect so we can draw the frame
		Rect rect = GUILayoutUtility.GetLastRect();
		
		// Draw the frame & overlay
		GUI.Box(new Rect((rect.x - 4.0f), (rect.y - 4.0f), (rect.width + 8.0f), (rect.height + 8.0f)), new GUIContent(""), overlay);
	}

	public static void Separator(Align align)
	{
		GUILayout.BeginHorizontal();
		
		if (align != Align.Left)
			GUILayout.FlexibleSpace();
		
		// Draw the sperator
		Separator();
		
		if (align != Align.Right)
			GUILayout.FlexibleSpace();
		
		GUILayout.EndHorizontal();
	}
	
	public static void Separator()
	{
		GUILayout.Box("", GUI.skin.GetStyle("separator"));
	}

	public static bool Toggle(bool toggle, string text)
	{
		GUIStyle ToggleTextStyle = GUI.skin.GetStyle("toggleText");
		GUIStyle ToggleTextShadowStyle = GUI.skin.GetStyle("toggleTextShadow");

		GUILayout.BeginHorizontal();

		// Get the rect where the toggle should be
		Rect toggleRect = GUILayoutUtility.GetRect(new GUIContent(""), "Toggle");

		// Calculate the toggle text size
		Vector2 textSize = ToggleTextStyle.CalcSize(new GUIContent(text));

		// Now put together a little interaction rect
		Rect interRect = new Rect(toggleRect.x, toggleRect.y, (toggleRect.width + 7.0f + textSize.x), toggleRect.height);

		// Get the interaction
		RectInteraction interaction = RectInteraction.Get(interRect);

		// Draw the toggle
		if (Event.current.type == EventType.Repaint)
			GUI.skin.GetStyle("Toggle").Draw(toggleRect, interaction.IsHovered, interaction.IsPressed, toggle, false);

		// Draw the toggle text
		TextWithShadow(new Rect((toggleRect.x + toggleRect.width + 7.0f), toggleRect.y, textSize.x, textSize.y), text, ToggleTextStyle, ToggleTextStyle.normal.textColor, ToggleTextShadowStyle.normal.textColor, new Vector2(1.0f, 2.0f));

		// Click
		if (interaction.Click)
			toggle = !toggle;

		GUILayout.EndHorizontal();
		
		return toggle;
	}

	public static bool Button(Rect rect, string text)
	{
		GUILayout.BeginArea(rect);
		bool result = Button(text, Align.Left);
		GUILayout.EndArea();

		return result;
	}

	public static bool Button(string text)
	{
		return Button(text, Align.Left);
	}
	
	public static bool Button(string text, Align align)
	{
		GUIStyle btnStyle = GUI.skin.GetStyle("button");
		GUIStyle textStyle = GUI.skin.GetStyle("buttonText");
		GUIStyle shadowStyle = GUI.skin.GetStyle("buttonTextShadow");

		// Upper case text
		text = text.ToUpper();

		GUILayout.BeginHorizontal();
		
		if (align != Align.Left)
			GUILayout.FlexibleSpace();
		
		// Get the button rect
		Rect rect = GUILayoutUtility.GetRect(new GUIContent(text), btnStyle);
		
		// Get interaction for that button
		RectInteraction interaction = RectInteraction.Get(new Rect((rect.x + 13.0f), (rect.y + 13.0f), (rect.width - 26.0f), (rect.height - 26f)));
		
		// Draw the button background
		if (Event.current.type == EventType.Repaint)
			btnStyle.Draw(rect, text, interaction.IsHovered, interaction.IsPressed, false, false);
		
		// Draw the text
		TextWithShadow(
			rect,
			text,
			textStyle,
			(interaction.IsPressed ? textStyle.active.textColor : (interaction.IsHovered ? textStyle.hover.textColor : textStyle.normal.textColor)),
			(interaction.IsPressed ? shadowStyle.active.textColor : (interaction.IsHovered ? shadowStyle.hover.textColor : shadowStyle.normal.textColor)),
			new Vector2(1.0f, 1.0f)
		);

		if (align != Align.Right)
			GUILayout.FlexibleSpace();
		
		GUILayout.EndHorizontal();
		
		return interaction.Click;
	}

	public enum TinyButtons
	{
		Accept,
		Decline,
		Social,
		Mail
	}
	
	public static bool TinyButton(TinyButtons btn)
	{
		return TinyButton(btn, Align.Left);
	}
	
	public static bool TinyButton(TinyButtons btn, Align align)
	{
		GUIStyle btnStyle = GUI.skin.GetStyle("tinyButton");
		GUIStyle texStyle = new GUIStyle();
		
		// Get the texture based on the button type
		switch (btn)
		{
		case TinyButtons.Accept: texStyle = GUI.skin.GetStyle("tinyButtonAccept"); break;
		case TinyButtons.Decline: texStyle = GUI.skin.GetStyle("tinyButtonDecline"); break;
		case TinyButtons.Social: texStyle = GUI.skin.GetStyle("tinyButtonSocial"); break;
		case TinyButtons.Mail: texStyle = GUI.skin.GetStyle("tinyButtonMail"); break;
		}
		
		// Get the button texture
		Texture2D tex = texStyle.normal.background;
		
		if (align != Align.Left)
			GUILayout.BeginHorizontal();
		
		if (align != Align.Left)
			GUILayout.FlexibleSpace();
		
		// Get the button rect
		Rect rect = GUILayoutUtility.GetRect(new GUIContent(tex), btnStyle);
		
		// Get interaction for that button
		RectInteraction interaction = RectInteraction.Get(new Rect((rect.x + 4.0f), (rect.y + 4.0f), (rect.width - 8.0f), (rect.height - 8.0f)));
		
		// Draw the button background
		if (Event.current.type == EventType.Repaint)
			btnStyle.Draw(rect, new GUIContent(""), interaction.IsHovered, interaction.IsPressed, false, false);
		
		// Now draw the icon in the middle of the button
		GUI.DrawTexture(new Rect((texStyle.contentOffset.x + (rect.x + ((rect.width - tex.width) / 2))), (texStyle.contentOffset.y + (rect.y + ((rect.height - tex.height) / 2))), tex.width, tex.height), tex);
		
		if (align == Align.Center)
			GUILayout.FlexibleSpace();
		
		if (align != Align.Left)
			GUILayout.EndHorizontal();
		
		return interaction.Click;
	}
	
	public enum IconButtons
	{
		Heart,
		Plus,
		Decline,
		Star
	}
	
	public static bool IconButton(IconButtons btn)
	{
		return IconButton(btn, Align.Left);
	}
	
	public static bool IconButton(IconButtons btn, Align align)
	{
		string styleStr = "";
		
		// Get the button style string
		switch (btn)
		{
			case IconButtons.Star: styleStr = "StarButton"; break;
			case IconButtons.Plus: styleStr = "PlusButton"; break;
			case IconButtons.Decline: styleStr = "DeclineButton"; break;
			case IconButtons.Heart: styleStr = "HeartButton"; break;
		}
		
		// Convert the string style to GUIStyle
		GUIStyle style = GUI.skin.GetStyle(styleStr);
		
		if (align != Align.Left)
			GUILayout.BeginHorizontal();
		
		if (align != Align.Left)
			GUILayout.FlexibleSpace();
		
		bool result = GUILayout.Button("", style);
		
		if (align == Align.Center)
			GUILayout.FlexibleSpace();
		
		if (align != Align.Left)
			GUILayout.EndHorizontal();
		
		return result;
	}
	
	public enum ArrowButtons
	{
		Left,
		Middle,
		Right
	}
	
	public static bool ArrowButton(ArrowButtons type)
	{
		return ArrowButton(type, Align.Left);
	}
	
	public static bool ArrowButton(ArrowButtons type, Align align)
	{
		string style = "";
		
		switch (type)
		{
		case ArrowButtons.Left: style = "arrowButtonLeft"; break;
		case ArrowButtons.Middle: style = "arrowButtonMiddle"; break;
		case ArrowButtons.Right: style = "arrowButtonRight"; break;
		}
		
		if (align != Align.Left)
			GUILayout.BeginHorizontal();
		
		if (align != Align.Left)
			GUILayout.FlexibleSpace();
		
		bool result = GUILayout.Button("", style);
		
		if (align == Align.Center)
			GUILayout.FlexibleSpace();
		
		if (align != Align.Left)
			GUILayout.EndHorizontal();
		
		return result;
	}
	
	public static bool SmallButton(string text)
	{
		return SmallButton(text, Align.Left);
	}
	
	public static bool SmallButton(string text, Align align)
	{
		GUIStyle btnStyle = GUI.skin.GetStyle("smallButton");
		GUIStyle textStyle = GUI.skin.GetStyle("smallButtonText");
		
		// Upper case text
		text = text.ToUpper();
		
		if (align != Align.Left)
			GUILayout.BeginHorizontal();
		
		if (align != Align.Left)
			GUILayout.FlexibleSpace();
		
		// Get the button rect
		Rect rect = GUILayoutUtility.GetRect(new GUIContent(text), btnStyle);
		
		// Get interaction for that button
		RectInteraction interaction = RectInteraction.Get(new Rect((rect.x + 4.0f), (rect.y + 4.0f), (rect.width - 8.0f), (rect.height - 8.0f)));
		
		// Draw the button background
		if (Event.current.type == EventType.Repaint)
			btnStyle.Draw(rect, text, interaction.IsHovered, interaction.IsPressed, false, false);
		
		// Draw the text
		if (!string.IsNullOrEmpty(text))
		{
			TextWithShadow(
				rect,
				text,
				textStyle,
				(interaction.IsPressed ? textStyle.active.textColor : (interaction.IsHovered ? textStyle.hover.textColor : textStyle.normal.textColor)),
				(interaction.IsPressed ? textStyle.onActive.textColor : (interaction.IsHovered ? textStyle.onHover.textColor : textStyle.onNormal.textColor)),
				new Vector2(0.0f, 1.0f)
			);
		}
		
		if (align == Align.Center)
			GUILayout.FlexibleSpace();
		
		if (align != Align.Left)
			GUILayout.EndHorizontal();
		
		return interaction.Click;
	}

	public static void BeginBox(params GUILayoutOption[] options)
	{
		GUILayout.BeginHorizontal(GUI.skin.GetStyle("textContainer"), options);
		GUILayout.BeginVertical();
	}

	public static void EndBox()
	{
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
	}

	public static void BoxTitle(string text)
	{
		GUIStyle TextStyle = GUI.skin.GetStyle("textContainerTitle");
		
		TextWithShadow(text, TextStyle, TextStyle.normal.textColor, TextStyle.hover.textColor, new Vector2(2.0f, 1.0f));
	}
	
	public static void BoxText(string text)
	{
		GUIStyle TextStyle = GUI.skin.GetStyle("textContainerText");
		
		TextWithShadow(text, TextStyle, TextStyle.normal.textColor, TextStyle.hover.textColor, new Vector2(2.0f, 1.0f));
	}

	public static void TooltipBox(Vector2 position, string text)
	{
		TooltipBox(position, text, 0.0f);
	}

	public static void TooltipBox(Vector2 position, string text, float maxWidth)
	{
		GUIStyle boxStyle = GUI.skin.GetStyle("tooltipBox");
		float internalMaxWidth = (maxWidth > 0.0f ? maxWidth : defaultTooltipMaxWidth);
		float height = boxStyle.CalcHeight(new GUIContent(text), internalMaxWidth);

		GUILayout.BeginArea(new Rect(position.x, position.y, internalMaxWidth, height));
		GUILayout.BeginHorizontal();
		TooltipBox(text, maxWidth);
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}
	
	public static void TooltipBox(string text)
	{
		TooltipBox(text, 0.0f);
	}
	
	public static void TooltipBox(string text, float maxWidth)
	{
		GUIStyle boxStyle = GUI.skin.GetStyle("tooltipBox");
		GUIStyle textStyle = GUI.skin.GetStyle("tooltipBoxText");
		
		// Draw the box
		if (maxWidth > 0.0f)
			GUILayout.Box(text, boxStyle, GUILayout.MaxWidth(maxWidth));
		else
			GUILayout.Box(text, boxStyle);
		
		// Get the tooltip rect
		Rect rect = GUILayoutUtility.GetLastRect();
		
		// Draw the box text
		TextWithShadow(
			new Rect((rect.x + boxStyle.padding.left), (rect.y + boxStyle.padding.top), (rect.width - boxStyle.padding.left - boxStyle.padding.right), (rect.height - boxStyle.padding.top - boxStyle.padding.bottom)),
			text,
			textStyle,
			textStyle.normal.textColor,
			textStyle.onNormal.textColor,
			new Vector2(0.0f, 1.0f)
		);
	}

	// Displays a vertical list of toggles and returns the index of the selected item.
	public static int ToggleList(int selected, string[] items)
	{
		return ToggleList(selected, items, 10.0f);
	}
	
	public static int ToggleList(int selected, string[] items, float spacing)
	{
		// Keep the selected index within the bounds of the items array
		selected = ((selected < 0) ? 0 : (selected >= items.Length ? (items.Length - 1) : selected));
		
		// Get the radio toggles style
		GUIStyle radioStyle = GUI.skin.GetStyle("radioToggle");
		GUIStyle ToggleTextStyle = GUI.skin.GetStyle("toggleText");
		GUIStyle ToggleTextShadowStyle = GUI.skin.GetStyle("toggleTextShadow");

		for (int i = 0; i < items.Length; i++)
		{
			// Add spacing
			if (i > 0)
				GUILayout.Space(spacing);
			
			GUILayout.BeginHorizontal();
			GUILayout.Space(14.0f);
			
			// Get the rect where the toggle should be
			Rect toggleRect = GUILayoutUtility.GetRect(new GUIContent(""), radioStyle);
			
			// Get the rect for the text
			Rect textRect = GUILayoutUtility.GetRect(new GUIContent(items[i]), ToggleTextStyle);
			
			// Now put together a little interaction rect
			Rect interRect = new Rect(toggleRect.x, toggleRect.y, (toggleRect.width + textRect.width), toggleRect.height);
			
			// Get the interaction
			RectInteraction interaction = RectInteraction.Get(interRect);
			
			// Draw the toggle
			if (Event.current.type == EventType.Repaint)
				radioStyle.Draw(toggleRect, interaction.IsHovered, interaction.IsPressed, (selected == i), false);
			
			// Draw the toggle text
			TextWithShadow(new Rect((toggleRect.x + toggleRect.width + 7.0f), textRect.y, textRect.width, textRect.height), items[i], ToggleTextStyle, ToggleTextStyle.normal.textColor, ToggleTextShadowStyle.normal.textColor, new Vector2(1.0f, 2.0f));
			
			// Click
			if (interaction.Click)
				selected = i;
			
			GUILayout.EndHorizontal();
		}
		
		// Return the currently selected item's index
		return selected;
	}

	public static void Label(string text)
	{
		GUIStyle LabelStyle = GUI.skin.GetStyle("label");
		GUIStyle LabelShadowStyle = GUI.skin.GetStyle("labelTextShadow");

		TextWithShadow(text, LabelStyle, LabelStyle.normal.textColor, LabelShadowStyle.normal.textColor, new Vector2(1.0f, 2.0f));
	}

	public static void InputLabel(string text)
	{
		GUIStyle LabelStyle = GUI.skin.GetStyle("inputLabelText");
		GUIStyle LabelShadowStyle = GUI.skin.GetStyle("labelTextShadow");
		Texture2D LabelBackground = GUI.skin.GetStyle("inputLabelBackground").normal.background;

		// Get the rect we're gonna be using
		Rect rect = GUILayoutUtility.GetRect(new GUIContent(text), LabelStyle);

		// Get the label closer to the input that's gonna be drawn next
		rect.y += 4.0f;

		// Draw the background
		GUI.DrawTexture(new Rect((rect.x + ((rect.width / 2) - (LabelBackground.width / 2))), (rect.y - 24.0f), LabelBackground.width, LabelBackground.height), LabelBackground);

		// Draw the text
		TextWithShadow(rect, text, LabelStyle, LabelStyle.normal.textColor, LabelShadowStyle.normal.textColor, new Vector2(1.0f, 2.0f));
	}

	public static void LoadingBar(Vector2 position, float percent)
	{
		Texture2D background = GUI.skin.GetStyle("LoadingBarBackground").normal.background;
		Texture2D fill = GUI.skin.GetStyle("LoadingBarFill").normal.background;
		GUIStyle textStyle = GUI.skin.GetStyle("LoadingBarText");
		
		// Begin a group to hold everything
		GUI.BeginGroup(new Rect(position.x, position.y, background.width, background.height));
		
		// Draw the background
		GUI.DrawTexture(new Rect(0, 0, background.width, background.height), background);
		
		// Draw the fill
		GUI.BeginGroup(new Rect(36.0f, 38.0f, (percent * fill.width), fill.height));
		GUI.DrawTexture(new Rect(0, 0, fill.width, fill.height), fill);
		GUI.EndGroup();
		
		// Draw the text
		TextWithShadow(
			new Rect(0, 0, background.width, background.height),
			"Loading...",
			textStyle,
			textStyle.normal.textColor,
			textStyle.onNormal.textColor,
			new Vector2(0f, 1.0f)
		);
		
		// End the group
		GUI.EndGroup();
	}

	/*
	 * TABLES
	 */
	
	private class TableData
	{
		public bool IsHeader = false;
		public int CurrentRowIndex = 0;
		public int CurrentColumnIndex = 0;
		public Dictionary<int, Rect> SavedRowRects = new Dictionary<int, Rect>();
		public Dictionary<int, float> SavedColumnWidth = new Dictionary<int, float>();
		
		public float BodyHeight = 0.0f;
		
		// Scroll view stuff
		public Vector2 ScrollPosition = Vector2.zero;
		public float ScrollViewContentHeight = 0.0f;
		
		public float CurrentColumnWidth()
		{
			if (this.SavedColumnWidth.ContainsKey(this.CurrentColumnIndex))
				return this.SavedColumnWidth[this.CurrentColumnIndex];
			
			return 0.0f;
		}
		
		public void SaveColumnWidth(float width)
		{
			this.SavedColumnWidth[this.CurrentColumnIndex] = width;
		}
		
		public void SaveRowRect(Rect r)
		{
			if (r.x == 0.0f && r.y == 0.0f && r.width == 1.0f && r.height == 1.0f)
				return;
			
			if (this.SavedRowRects.ContainsKey(this.CurrentRowIndex))
				this.SavedRowRects[this.CurrentRowIndex] = r;
			else
				this.SavedRowRects.Add(this.CurrentRowIndex, r);
		}
		
		public Rect GetCurrentRowRect()
		{
			if (this.SavedRowRects.ContainsKey(this.CurrentRowIndex))
				return this.SavedRowRects[this.CurrentRowIndex];
			
			return new Rect(0, 0, 0, 0);
		}
	}
	
	private static int TableCurrentControlID = 0;
	private static Dictionary<int, TableData> TablesData = new Dictionary<int, TableData>();
	
	private static TableData GetCurrentTableData()
	{
		if (TablesData.ContainsKey(TableCurrentControlID))
			return TablesData[TableCurrentControlID];
		else
			TablesData.Add(TableCurrentControlID, new TableData());
		
		return TablesData[TableCurrentControlID];
	}

	/// <summary>
	/// Begins a table.
	/// </summary>
	/// <param name="ControlID">Control ID.</param>
	public static void BeginTable(int ControlID)
	{
		// Get the table control id
		TableCurrentControlID = ControlID;
		
		// Begin the table
		GUILayout.BeginVertical("table");
		
		// Reset the rows indexer
		GetCurrentTableData().CurrentRowIndex = 0;
	}

	/// <summary>
	/// Ends a table.
	/// </summary>
	public static void EndTable()
	{
		GUILayout.EndVertical();
	}

	/// <summary>
	/// Begins the table header row.
	/// </summary>
	public static void BeginTableHeader()
	{
		GUILayout.BeginHorizontal("tableHeader");
		
		GetCurrentTableData().IsHeader = true;
		// Reset the column indexer
		GetCurrentTableData().CurrentColumnIndex = 0;
	}

	/// <summary>
	/// Ends the table header row.
	/// </summary>
	public static void EndTableHeader()
	{
		GUILayout.EndHorizontal();
		
		GetCurrentTableData().IsHeader = false;
	}

	/// <summary>
	/// Begins the table body, it's a scroll view.
	/// </summary>
	/// <param name="height">Height.</param>
	public static void BeginTableBody(float height)
	{
		// Save the height
		GetCurrentTableData().BodyHeight = height;
		
		GUILayout.BeginHorizontal();
		
		// Do the scroll view
		GetCurrentTableData().ScrollPosition = GUILayout.BeginScrollView(GetCurrentTableData().ScrollPosition, "tableScrollView", GUILayout.Height(height));
		
		GUILayout.BeginVertical();
	}

	/// <summary>
	/// Ends the table body.
	/// </summary>
	public static void EndTableBody()
	{
		// Check if we need to make some corrections on the scrollview padding
		// only in case we have a scroll bar, so we need to determine if we have one
		GUILayout.EndVertical();
		
		// Save the scroll view content height
		if (Event.current.type == EventType.Repaint)
			GetCurrentTableData().ScrollViewContentHeight = GUILayoutUtility.GetLastRect().height;
		
		GUILayout.EndScrollView();
		
		// Check if the content is greater than the scroll view height
		if (Event.current.type != EventType.Repaint && GetCurrentTableData().ScrollViewContentHeight > GetCurrentTableData().BodyHeight)
			GUILayout.Space(-1f);
		
		GUILayout.EndHorizontal();
	}

	/// <summary>
	/// Begins a table row.
	/// </summary>
	/// <returns>The row mouse interactions.</returns>
	public static RectInteraction BeginTableRow()
	{
		// Prepare the row style
		GUIStyle style = new GUIStyle("tableRow");
		
		// Get the row rect
		Rect rect = GetCurrentTableData().GetCurrentRowRect();
		
		// Get the row interaction
		RectInteraction interaction = RectInteraction.Get(rect, false);
		
		// Check if the row is hovered
		if (interaction.IsHovered)
			style.normal.background = style.hover.background;
		
		// remove the hover texture from the style
		style.hover.background = null;
		
		// Do the row
		GUILayout.BeginHorizontal(style);
		
		// Reset the column indexer
		GetCurrentTableData().CurrentColumnIndex = 0;
		
		// Return the row interaction
		return interaction;
	}

	/// <summary>
	/// Ends the table row.
	/// </summary>
	public static void EndTableRow()
	{
		GUILayout.EndHorizontal();

		// Save the rect
		if (Event.current.type == EventType.Repaint)
			GetCurrentTableData().SaveRowRect(GUILayoutUtility.GetLastRect());
		
		// Increase the rows indexer
		GetCurrentTableData().CurrentRowIndex++;
	}

	/// <summary>
	/// Draws a column.
	/// </summary>
	/// <param name="text">Text.</param>
	public static void TableColumn(string text)
	{
		TableColumn(new GUIContent(text), 0.0f);
	}

	/// <summary>
	/// Draws a column with a custom text style.
	/// </summary>
	/// <param name="text">Text.</param>
	/// <param name="textStyle">Text style.</param>
	public static void TableColumn(string text, GUIStyle textStyle)
	{
		TableColumn(new GUIContent(text), 0.0f, textStyle);
	}

	/// <summary>
	/// Draws a column.
	/// </summary>
	/// <param name="text">Text.</param>
	public static void TableColumn(GUIContent text)
	{
		TableColumn(text, 0.0f);
	}

	/// <summary>
	/// Draws a column with fixed width.
	/// </summary>
	/// <param name="text">Text.</param>
	/// <param name="width">Width.</param>
	public static void TableColumn(string text, float width)
	{
		TableColumn(new GUIContent(text), width);
	}

	/// <summary>
	/// Draws a column with fixed width.
	/// </summary>
	/// <param name="Text">Text.</param>
	/// <param name="width">Width.</param>
	public static void TableColumn(GUIContent Text, float width)
	{
		string textStyleStr = (GetCurrentTableData().IsHeader ? "tableHeaderText" : "tableText");

		TableColumn(Text, width, GUI.skin.GetStyle(textStyleStr));
	}

	/// <summary>
	/// Draws a column with fixed width and custom text style
	/// </summary>
	/// <param name="text">Text.</param>
	/// <param name="width">Width.</param>
	/// <param name="textStyle">Text style.</param>
	public static void TableColumn(GUIContent text, float width, GUIStyle textStyle)
	{
		GUIStyle separatorStyle = GUI.skin.GetStyle(GetCurrentTableData().IsHeader ? "tableHeaderSeparator" : "tableSeparator");
		
		// Check if we need fixed width
		if (width > 0.0f)
		{
			textStyle = new GUIStyle(textStyle);
			textStyle.fixedWidth = width;
		}
		else if (!GetCurrentTableData().IsHeader)
		{
			// Try using a saved size
			if (GetCurrentTableData().CurrentColumnWidth() > 0.0f)
			{
				textStyle = new GUIStyle(textStyle);
				textStyle.fixedWidth = GetCurrentTableData().CurrentColumnWidth();
			}
		}
		
		// Check if we need place separator
		if (GetCurrentTableData().CurrentColumnIndex > 0)
			GUILayout.Box("", separatorStyle);
		
		// Draw the text
		TextWithShadow(
			text,
			textStyle,
			textStyle.normal.textColor,
			textStyle.onNormal.textColor,
			new Vector2(0.0f, 1.0f)
		);
		
		// If this is a header column, save it's width to the list
		if (GetCurrentTableData().IsHeader)
		{
			if (width > 0.0f)
			{
				GetCurrentTableData().SaveColumnWidth(width);
			}
			else if (Event.current.type == EventType.Repaint)
			{
				GetCurrentTableData().SaveColumnWidth(GUILayoutUtility.GetLastRect().width);
			}
		}
		
		// Increase the index
		GetCurrentTableData().CurrentColumnIndex++;
	}

	/// <summary>
	/// Begins a table column.
	/// </summary>
	public static void BeginTableColumn()
	{
		BeginTableColumn(0.0f);
	}

	/// <summary>
	/// Begins a table column with fixed width.
	/// </summary>
	/// <param name="width">Width.</param>
	public static void BeginTableColumn(float width)
	{
		GUIStyle separatorStyle = GUI.skin.GetStyle(GetCurrentTableData().IsHeader ? "tableHeaderSeparator" : "tableSeparator");
		
		// Check if this is the header row and we can save the user defined width
		if (GetCurrentTableData().IsHeader && width > 0.0f)
			GetCurrentTableData().SaveColumnWidth(width);
		
		// Check if there is no user defined width and we should try using a saved width
		if (!GetCurrentTableData().IsHeader && width == 0.0f && GetCurrentTableData().CurrentColumnWidth() > 0.0f)
			width = GetCurrentTableData().CurrentColumnWidth();
		
		// Check if we need place separator
		if (GetCurrentTableData().CurrentColumnIndex > 0)
			GUILayout.Box("", separatorStyle);
		
		// Check if we have fixed width
		if (width > 0.0f)
			GUILayout.BeginVertical(GUILayout.Width(width));
		else
			GUILayout.BeginVertical();
		
		GUILayout.BeginHorizontal("tableColumn");
	}

	/// <summary>
	/// Ends a table column.
	/// </summary>
	public static void EndTableColumn()
	{
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
		
		// If this is a header column, save it's width to the list if no width was saved already
		if (GetCurrentTableData().IsHeader && Event.current.type == EventType.Repaint && GetCurrentTableData().CurrentColumnWidth() == 0.0f)
			GetCurrentTableData().SaveColumnWidth(GUILayoutUtility.GetLastRect().width);
		
		// Increase the index
		GetCurrentTableData().CurrentColumnIndex++;
	}

	// GUI Elements
	public static void TextWithShadow(Rect rect, string text, GUIStyle style, Color txtColor, Color shadowColor)
	{
		TextWithShadow(rect, new GUIContent(text), style, txtColor, shadowColor, new Vector2(1.0f, 1.0f));
	}
	
	public static void TextWithShadow(Rect rect, string text, GUIStyle style, Color txtColor, Color shadowColor, Vector2 direction)
	{
		TextWithShadow(rect, new GUIContent(text), style, txtColor, shadowColor, direction);
	}
	
	public static void TextWithShadow(Rect rect, GUIContent content, GUIStyle style, Color txtColor, Color shadowColor, Vector2 direction)
	{
		GUIStyle newStyle = new GUIStyle(style);
		
		newStyle.normal.textColor = shadowColor;
		rect.x += direction.x;
		rect.y += direction.y;
		GUI.Label(rect, new GUIContent(StripColorTag(content.text)), newStyle);
		
		newStyle.normal.textColor = txtColor;
		rect.x -= direction.x;
		rect.y -= direction.y;
		GUI.Label(rect, content, newStyle);
	}
	
	public static void Separator(Vector2 offset)
	{
		GUI.Label(new Rect(offset.x, offset.y, 340, 16), "", "separator");
	}
	
	public static bool Toggle(Vector2 offset, bool toggle, string text)
	{
		GUIStyle ToggleTextStyle = GUI.skin.GetStyle("toggleText");
		GUIStyle ToggleTextShadowStyle = GUI.skin.GetStyle("toggleTextShadow");
		
		GUI.BeginGroup(new Rect(offset.x, offset.y, 349, 146));
		toggle = GUI.Toggle(new Rect(0, 0, 32, 32), toggle, "");
		TextWithShadow(new Rect(39, 0, 278, 32), text, ToggleTextStyle, ToggleTextStyle.normal.textColor, ToggleTextShadowStyle.normal.textColor, new Vector2(2.0f, 1.0f));
		GUI.EndGroup();
		
		return toggle;
	}
	
	// Displays a vertical list of toggles and returns the index of the selected item.
	public static int ToggleList(Rect offset, int selected, string[] items)
	{
		// Keep the selected index within the bounds of the items array
		selected = ((selected < 0) ? 0 : (selected >= items.Length ? (items.Length - 1) : selected));
		
		// Get the radio toggles style
		GUIStyle radioStyle = GUI.skin.GetStyle("radioToggle");
		GUIStyle ToggleTextStyle = GUI.skin.GetStyle("toggleText");
		GUIStyle ToggleTextShadowStyle = GUI.skin.GetStyle("toggleTextShadow");
		
		// Get the toggles height
		float height = radioStyle.fixedHeight;
		float width = radioStyle.fixedWidth;
		
		GUI.BeginGroup(new Rect(offset.x, offset.y, offset.width, ((height * items.Length) + height)));
		GUILayout.BeginVertical();
		
		float offsetY = 0.0f;
		float textOffsetX = 37.0f;
		
		for (int i = 0; i < items.Length; i++)
		{
			// Display toggle. Get if toggle changed.
			bool change = GUI.Toggle(new Rect(0, offsetY, width, height), (selected == i), "", radioStyle);
			TextWithShadow(new Rect(textOffsetX, (offsetY + 1), (offset.width - textOffsetX), height), items[i], ToggleTextStyle, ToggleTextStyle.normal.textColor, ToggleTextShadowStyle.normal.textColor);
			
			// If changed, set selected to current index.
			if (change)
				selected = i;
			
			// Increase the offset for the next toggle
			offsetY = (offsetY + (height + 8.0f));
		}
		
		GUILayout.EndVertical();
		GUI.EndGroup();
		
		// Return the currently selected item's index
		return selected;
	}
}

