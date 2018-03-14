﻿using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace KoganeEditorLib
{
	public sealed class CustomizableToolbar : EditorWindow
	{
		private const string TITLE = "Toolbar";
		private const float WINDOW_HEIGHT = 24;
		private const float BUTTON_HEIGHT = 20;

		private CustomizableToolbarSettings m_settings;

		[MenuItem( "Window/Project Settings Toolbar" )]
		private static void Init()
		{
			var win = GetWindow<CustomizableToolbar>( TITLE );

			var pos = win.position;
			pos.height = WINDOW_HEIGHT;
			win.position = pos;

			var minSize = win.minSize;
			minSize.y = WINDOW_HEIGHT;
			win.minSize = minSize;

			var maxSize = win.maxSize;
			maxSize.y = WINDOW_HEIGHT;
			win.maxSize = maxSize;
		}

		private void OnGUI()
		{
			EditorGUILayout.BeginHorizontal();

			var list = m_settings.List.Where( c => c.IsValid );

			foreach ( var n in list )
			{
				var commandName = n.CommandName;
				var buttonName = n.ButtonName;
				var image = n.Image;
				var width = n.Width;
				var content = image != null ? new GUIContent( image ) : new GUIContent( buttonName );
				var options = width != -1 ? new [] { GUILayout.Width( width ), GUILayout.Height( BUTTON_HEIGHT ) } : new [] { GUILayout.Height( BUTTON_HEIGHT ) };
				if ( GUILayout.Button( content, options ) )
				{
					EditorApplication.ExecuteMenuItem( commandName );
				}
			}

			EditorGUILayout.EndHorizontal();
		}

		private void OnEnable()
		{
			var mono = MonoScript.FromScriptableObject( this );
			var scriptPath = AssetDatabase.GetAssetPath( mono );
			var dir = Path.GetDirectoryName( scriptPath );
			var path = string.Format( "{0}/Settings.asset", dir );

			m_settings = AssetDatabase.LoadAssetAtPath<CustomizableToolbarSettings>( path );
		}
	}
}