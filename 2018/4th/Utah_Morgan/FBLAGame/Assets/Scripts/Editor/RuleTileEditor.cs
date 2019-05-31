#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Tilemaps;
using Object = UnityEngine.Object;

namespace UnityEditor
{
    [CustomEditor(typeof(RuleTile))]
	[CanEditMultipleObjects]
	internal class RuleTileEditor : Editor
	{
        //Strings for the icons
		private const string s_XIconString = "iVBORw0KGgoAAAANSUhEUgAAAA8AAAAPCAYAAAA71pVKAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAYdEVYdFNvZnR3YXJlAHBhaW50Lm5ldCA0LjAuNWWFMmUAAABoSURBVDhPnY3BDcAgDAOZhS14dP1O0x2C/LBEgiNSHvfwyZabmV0jZRUpq2zi6f0DJwdcQOEdwwDLypF0zHLMa9+NQRxkQ+ACOT2STVw/q8eY1346ZlE54sYAhVhSDrjwFymrSFnD2gTZpls2OvFUHAAAAABJRU5ErkJggg==";
		private const string s_Arrow0 = "iVBORw0KGgoAAAANSUhEUgAAAA8AAAAPCAYAAAA71pVKAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAYdEVYdFNvZnR3YXJlAHBhaW50Lm5ldCA0LjAuNWWFMmUAAACYSURBVDhPzZExDoQwDATzE4oU4QXXcgUFj+YxtETwgpMwXuFcwMFSRMVKKwzZcWzhiMg91jtg34XIntkre5EaT7yjjhI9pOD5Mw5k2X/DdUwFr3cQ7Pu23E/BiwXyWSOxrNqx+ewnsayam5OLBtbOGPUM/r93YZL4/dhpR/amwByGFBz170gNChA6w5bQQMqramBTgJ+Z3A58WuWejPCaHQAAAABJRU5ErkJggg==";
		private const string s_Arrow1 = "iVBORw0KGgoAAAANSUhEUgAAAA8AAAAPCAYAAAA71pVKAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAYdEVYdFNvZnR3YXJlAHBhaW50Lm5ldCA0LjAuNWWFMmUAAABqSURBVDhPxYzBDYAgEATpxYcd+PVr0fZ2siZrjmMhFz6STIiDs8XMlpEyi5RkO/d66TcgJUB43JfNBqRkSEYDnYjhbKD5GIUkDqRDwoH3+NgTAw+bL/aoOP4DOgH+iwECEt+IlFmkzGHlAYKAWF9R8zUnAAAAAElFTkSuQmCC";
		private const string s_Arrow2 = "iVBORw0KGgoAAAANSUhEUgAAAA8AAAAPCAYAAAA71pVKAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAYdEVYdFNvZnR3YXJlAHBhaW50Lm5ldCA0LjAuNWWFMmUAAAC0SURBVDhPjVE5EsIwDMxPKFKYF9CagoJH8xhaMskLmEGsjOSRkBzYmU2s9a58TUQUmCH1BWEHweuKP+D8tphrWcAHuIGrjPnPNY8X2+DzEWE+FzrdrkNyg2YGNNfRGlyOaZDJOxBrDhgOowaYW8UW0Vau5ZkFmXbbDr+CzOHKmLinAXMEePyZ9dZkZR+s5QX2O8DY3zZ/sgYcdDqeEVp8516o0QQV1qeMwg6C91toYoLoo+kNt/tpKQEVvFQAAAAASUVORK5CYII=";
		private const string s_Arrow3 = "iVBORw0KGgoAAAANSUhEUgAAAA8AAAAPCAYAAAA71pVKAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAYdEVYdFNvZnR3YXJlAHBhaW50Lm5ldCA0LjAuNWWFMmUAAAB2SURBVDhPzY1LCoAwEEPnLi48gW5d6p31bH5SMhp0Cq0g+CCLxrzRPqMZ2pRqKG4IqzJc7JepTlbRZXYpWTg4RZE1XAso8VHFKNhQuTjKtZvHUNCEMogO4K3BhvMn9wP4EzoPZ3n0AGTW5fiBVzLAAYTP32C2Ay3agtu9V/9PAAAAAElFTkSuQmCC";
		private const string s_Arrow5 = "iVBORw0KGgoAAAANSUhEUgAAAA8AAAAPCAYAAAA71pVKAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAYdEVYdFNvZnR3YXJlAHBhaW50Lm5ldCA0LjAuNWWFMmUAAABqSURBVDhPnY3BCYBADASvFx924NevRdvbyoLBmNuDJQMDGjNxAFhK1DyUQ9fvobCdO+j7+sOKj/uSB+xYHZAxl7IR1wNTXJeVcaAVU+614uWfCT9mVUhknMlxDokd15BYsQrJFHeUQ0+MB5ErsPi/6hO1AAAAAElFTkSuQmCC";
		private const string s_Arrow6 = "iVBORw0KGgoAAAANSUhEUgAAAA8AAAAPCAYAAAA71pVKAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAYdEVYdFNvZnR3YXJlAHBhaW50Lm5ldCA0LjAuNWWFMmUAAACaSURBVDhPxZExEkAwEEVzE4UiTqClUDi0w2hlOIEZsV82xCZmQuPPfFn8t1mirLWf7S5flQOXjd64vCuEKWTKVt+6AayH3tIa7yLg6Qh2FcKFB72jBgJeziA1CMHzeaNHjkfwnAK86f3KUafU2ClHIJSzs/8HHLv09M3SaMCxS7ljw/IYJWzQABOQZ66x4h614ahTCL/WT7BSO51b5Z5hSx88AAAAAElFTkSuQmCC";
		private const string s_Arrow7 = "iVBORw0KGgoAAAANSUhEUgAAAA8AAAAPCAYAAAA71pVKAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAYdEVYdFNvZnR3YXJlAHBhaW50Lm5ldCA0LjAuNWWFMmUAAABQSURBVDhPYxh8QNle/T8U/4MKEQdAmsz2eICx6W530gygr2aQBmSMphkZYxqErAEXxusKfAYQ7XyyNMIAsgEkaYQBkAFkaYQBsjXSGDAwAAD193z4luKPrAAAAABJRU5ErkJggg==";
		private const string s_Arrow8 = "iVBORw0KGgoAAAANSUhEUgAAAA8AAAAPCAYAAAA71pVKAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAYdEVYdFNvZnR3YXJlAHBhaW50Lm5ldCA0LjAuNWWFMmUAAACYSURBVDhPxZE9DoAwCIW9iUOHegJXHRw8tIdx1egJTMSHAeMPaHSR5KVQ+KCkCRF91mdz4VDEWVzXTBgg5U1N5wahjHzXS3iFFVRxAygNVaZxJ6VHGIl2D6oUXP0ijlJuTp724FnID1Lq7uw2QM5+thoKth0N+GGyA7IA3+yM77Ag1e2zkey5gCdAg/h8csy+/89v7E+YkgUntOWeVt2SfAAAAABJRU5ErkJggg==";
		private const string s_MirrorX = "iVBORw0KGgoAAAANSUhEUgAAAA8AAAAPCAYAAAA71pVKAAAABGdBTUEAALGPC/xhBQAAAAlwSFlzAAAOwQAADsEBuJFr7QAAABh0RVh0U29mdHdhcmUAcGFpbnQubmV0IDQuMC41ZYUyZQAAAG1JREFUOE+lj9ENwCAIRB2IFdyRfRiuDSaXAF4MrR9P5eRhHGb2Gxp2oaEjIovTXSrAnPNx6hlgyCZ7o6omOdYOldGIZhAziEmOTSfigLV0RYAB9y9f/7kO8L3WUaQyhCgz0dmCL9CwCw172HgBeyG6oloC8fAAAAAASUVORK5CYII=";
		private const string s_MirrorY = "iVBORw0KGgoAAAANSUhEUgAAAA8AAAAPCAYAAAA71pVKAAAABGdBTUEAALGPC/xhBQAAAAlwSFlzAAAOwgAADsIBFShKgAAAABh0RVh0U29mdHdhcmUAcGFpbnQubmV0IDQuMC41ZYUyZQAAAG9JREFUOE+djckNACEMAykoLdAjHbPyw1IOJ0L7mAejjFlm9hspyd77Kk+kBAjPOXcakJIh6QaKyOE0EB5dSPJAiUmOiL8PMVGxugsP/0OOib8vsY8yYwy6gRyC8CB5QIWgCMKBLgRSkikEUr5h6wOPWfMoCYILdgAAAABJRU5ErkJggg==";
		private const string s_Rotated = "iVBORw0KGgoAAAANSUhEUgAAAA8AAAAPCAYAAAA71pVKAAAABGdBTUEAALGPC/xhBQAAAAlwSFlzAAAOwQAADsEBuJFr7QAAABh0RVh0U29mdHdhcmUAcGFpbnQubmV0IDQuMC41ZYUyZQAAAHdJREFUOE+djssNwCAMQxmIFdgx+2S4Vj4YxWlQgcOT8nuG5u5C732Sd3lfLlmPMR4QhXgrTQaimUlA3EtD+CJlBuQ7aUAUMjEAv9gWCQNEPhHJUkYfZ1kEpcxDzioRzGIlr0Qwi0r+Q5rTgM+AAVcygHgt7+HtBZs/2QVWP8ahAAAAAElFTkSuQmCC";
		        
		private static Texture2D[] s_Arrows;

        /// <summary>
        /// The texture array for the arrows
        /// </summary>
        public static Texture2D[] arrows
		{
			get
			{
				if (s_Arrows == null)
				{
					s_Arrows = new Texture2D[10];
					s_Arrows[0] = Base64ToTexture(s_Arrow0);
					s_Arrows[1] = Base64ToTexture(s_Arrow1);
					s_Arrows[2] = Base64ToTexture(s_Arrow2);
					s_Arrows[3] = Base64ToTexture(s_Arrow3);
					s_Arrows[5] = Base64ToTexture(s_Arrow5);
					s_Arrows[6] = Base64ToTexture(s_Arrow6);
					s_Arrows[7] = Base64ToTexture(s_Arrow7);
					s_Arrows[8] = Base64ToTexture(s_Arrow8);
					s_Arrows[9] = Base64ToTexture(s_XIconString);
				}
				return s_Arrows;
			}
		}

		private static Texture2D[] s_AutoTransforms;

        /// <summary>
        /// Array for the transforms
        /// </summary>
        public static Texture2D[] autoTransforms
		{
			get
			{
				if (s_AutoTransforms == null)
				{
					s_AutoTransforms = new Texture2D[3];
					s_AutoTransforms[0] = Base64ToTexture(s_Rotated);
					s_AutoTransforms[1] = Base64ToTexture(s_MirrorX);
					s_AutoTransforms[2] = Base64ToTexture(s_MirrorY);
				}
				return s_AutoTransforms;
			}
		}

        /// <summary>
        /// A list of tilemaps
        /// </summary>
        private List<Tilemap> Tilemaps
        {
            get
            {
                List<Tilemap> list = new List<Tilemap>();
                Transform grid = GameObject.FindGameObjectWithTag("Grid").transform;
                for (int i = 0; i < grid.childCount; i++)
                {
                    list.Add(grid.GetChild(i).GetComponent<Tilemap>());
                }
                return list;
            }
        }
		
		private ReorderableList m_ReorderableList;

        /// <summary>
        /// The tile we are using
        /// </summary>
        public RuleTile tile { get { return (target as RuleTile); } }
		private Rect m_ListRect;

        const float k_DefaultElementHeight = 64f;
		const float k_PaddingBetweenRules = 13f;
		const float k_SingleLineHeight = 16f;
		const float k_LabelWidth = 53f;
			
        /// <summary>
        /// Called when enabled
        /// </summary>
		public void OnEnable()
		{
			if (tile.m_TilingRules == null)
				tile.m_TilingRules = new List<RuleTile.TilingRule>();

            m_ReorderableList = new ReorderableList(tile.m_TilingRules, typeof(RuleTile.TilingRule), true, true, true, true);
            m_ReorderableList.drawHeaderCallback = OnDrawHeader;
            m_ReorderableList.drawElementCallback = OnDrawElement;
            m_ReorderableList.elementHeightCallback = GetElementHeight;
            m_ReorderableList.onReorderCallback = ListUpdated;
            
        }

        /// <summary>
        /// When the list is updated
        /// </summary>
        /// <param name="list">The list updated</param>
		private void ListUpdated(ReorderableList list)
		{
			SaveTile();
		}

        /// <summary>
        /// Gets the height of the element
        /// </summary>
        /// <param name="index">The index of the element</param>
        /// <returns>The height</returns>
		private float GetElementHeight(int index)
		{
            float height = k_DefaultElementHeight + k_PaddingBetweenRules;
            if (tile.m_TilingRules != null && tile.m_TilingRules.Count > 0)
			{                			
                switch (tile.m_TilingRules[index].m_Output)
                {
                    case RuleTile.TilingRule.OutputSprite.Random:
                        height += k_SingleLineHeight * (tile.m_TilingRules[index].m_Sprites.Length + 3); break;
                    case RuleTile.TilingRule.OutputSprite.Animation:
                        height +=  k_SingleLineHeight * (tile.m_TilingRules[index].m_Sprites.Length + 2); break;
                }                
			}
            return height;
		}

        /// <summary>
        /// Draws the element
        /// </summary>
        /// <param name="rect">The rect to draw on</param>
        /// <param name="index">The index of the element</param>
        /// <param name="isactive">If the element is active</param>
        /// <param name="isfocused">If it is focused</param>
		private void OnDrawElement(Rect rect, int index, bool isactive, bool isfocused)
		{
			RuleTile.TilingRule rule = tile.m_TilingRules[index];

			float yPos = rect.yMin + 2f;
			float height = rect.height - k_PaddingBetweenRules;
			float matrixWidth = k_DefaultElementHeight;
			
			Rect inspectorRect = new Rect(rect.xMin, yPos, rect.width - matrixWidth * 2f - 20f, height);
			Rect matrixRect = new Rect(rect.xMax - matrixWidth * 2f - 10f, yPos, matrixWidth, k_DefaultElementHeight);
			Rect spriteRect = new Rect(rect.xMax - matrixWidth - 5f, yPos, matrixWidth, k_DefaultElementHeight);

			EditorGUI.BeginChangeCheck();
			RuleInspectorOnGUI(inspectorRect, rule);
			RuleMatrixOnGUI(matrixRect, rule);
			SpriteOnGUI(spriteRect, rule);
			if (EditorGUI.EndChangeCheck())
				SaveTile();
		}

        /// <summary>
        /// Saves the tile
        /// </summary>
		private void SaveTile()
		{
			EditorUtility.SetDirty(target);
			SceneView.RepaintAll();
		}

        /// <summary>
        /// Draws the header
        /// </summary>
        /// <param name="rect"></param>
		private void OnDrawHeader(Rect rect)
		{
			GUI.Label(rect, "Tiling Rules");
		}

        /// <summary>
        /// Called everytime the GUI is updated
        /// </summary>
		public override void OnInspectorGUI()
		{            
            
			tile.m_DefaultSprite = EditorGUILayout.ObjectField("Default Sprite", tile.m_DefaultSprite, typeof(Sprite), false) as Sprite;
			tile.m_DefaultColliderType = (Tile.ColliderType)EditorGUILayout.EnumPopup("Default Collider", tile.m_DefaultColliderType);           
            tile.m_Input = (RuleTile.TilingRule.InputSprite)EditorGUILayout.EnumPopup("Other Tile Refs", tile.m_Input);
            Rect rect = new Rect();
            float y = 0;
            if (tile.m_Input == RuleTile.TilingRule.InputSprite.Yes)
            {                
                EditorGUI.BeginChangeCheck();
                int newLength = EditorGUILayout.DelayedIntField("Size", tile.m_InputTiles.Length);                
                if (EditorGUI.EndChangeCheck())
                {                    
                    Array.Resize(ref tile.m_InputTiles, Math.Max(newLength, 1));
                    for (int i = 0; i < tile.m_InputTiles.Length; i++)
                    {
                        if (tile.m_InputTiles[i] == null)
                        {
                            tile.m_InputTiles[i] = new RuleTile.InputTile(null, (Tilemaps.Count == 0) ? null : Tilemaps[0]);
                        }
                    }                                        
                }
                           
                for (int i = 0; i < tile.m_InputTiles.Length; i++)
                {                                        
                    if (y==0)
                        y = rect.yMin;                    
                    y += k_SingleLineHeight;                    
                    Tilemap map = this.tile.m_InputTiles[i].Tilemap;
                    TileBase tile = this.tile.m_InputTiles[i].Tile;
                    if (map == null)
                    {
                        map = GameObject.Find("Ground").GetComponent<Tilemap>();
                        SaveTile();
                    }
                    map = EditorGUILayout.ObjectField((i + 1).ToString(), map, typeof(Tilemap), true) as Tilemap;                    
                    tile = EditorGUILayout.ObjectField(" ",tile, typeof(TileBase), true) as TileBase;                    
                    this.tile.m_InputTiles[i] = new RuleTile.InputTile(tile, map);
                    y += k_SingleLineHeight;                      
                }
            }
            
            EditorGUILayout.Space();

            if (m_ReorderableList != null && tile.m_TilingRules != null)
            {
                if (tile.m_Input == RuleTile.TilingRule.InputSprite.Yes)
                {                    
                    m_ReorderableList.DoLayoutList();
                }
                else
                {
                    m_ReorderableList.DoLayoutList();
                }
            }
		}

        //This used to be static
        /// <summary>
        /// The On GUI for the rule matrix
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="tilingRule"></param>
		private void RuleMatrixOnGUI(Rect rect, RuleTile.TilingRule tilingRule)
		{
			Handles.color = EditorGUIUtility.isProSkin ? new Color(1f, 1f, 1f, 0.2f) : new Color(0f, 0f, 0f, 0.2f);
			int index = 0;
			float w = rect.width / 3f;
			float h = rect.height / 3f;

			for (int y = 0; y <= 3; y++)
			{
				float top = rect.yMin + y * h;
				Handles.DrawLine(new Vector3(rect.xMin, top), new Vector3(rect.xMax, top));
			}
			for (int x = 0; x <= 3; x++)
			{
				float left = rect.xMin + x * w;
				Handles.DrawLine(new Vector3(left, rect.yMin), new Vector3(left, rect.yMax));
			}
			Handles.color = Color.white;

			for (int y = 0; y <= 2; y++)
			{
				for (int x = 0; x <= 2; x++)
				{
					Rect r = new Rect(rect.xMin + x * w, rect.yMin + y * h, w - 1, h - 1);
					if (x != 1 || y != 1)
					{
						switch (tilingRule.m_Neighbors[index])
						{
							case RuleTile.TilingRule.Neighbor.This:
								GUI.DrawTexture(r, arrows[y*3 + x]);
								break;
							case RuleTile.TilingRule.Neighbor.NotThis:
								GUI.DrawTexture(r, arrows[9]);
								break;
                            case RuleTile.TilingRule.Neighbor.DontCare: break;
                            default: GUI.Label(new Rect(r.xMin + 5f, r.yMin + 3f, 20, 20), ((int)tilingRule.m_Neighbors[index] - 2 ).ToString()); break;
						}
						if (Event.current.type == EventType.MouseDown && r.Contains(Event.current.mousePosition) && Event.current.type != EventType.ContextClick)
						{
                            //If it is single, cycle through normally. Else cycle through based on the amount of inputtiles                            
                            int mod = tile.m_Input == RuleTile.TilingRule.InputSprite.No ? 3 : 3 + tile.m_InputTiles.Length;
                            tilingRule.m_Neighbors[index] = (RuleTile.TilingRule.Neighbor) (((int)tilingRule.m_Neighbors[index] + 1) % mod);
							GUI.changed = true;
							Event.current.Use();
						}
                        if (Event.current.type == EventType.ContextClick && r.Contains(Event.current.mousePosition))
                        {
                            tilingRule.m_Neighbors[index] = RuleTile.TilingRule.Neighbor.DontCare;
                            GUI.changed = true;
                            Event.current.Use();
                        }
                        index++;
					}
					else
					{
						switch (tilingRule.m_RuleTransform)
						{
							case RuleTile.TilingRule.Transform.Rotated:
								GUI.DrawTexture(r, autoTransforms[0]);
								break;
							case RuleTile.TilingRule.Transform.MirrorX:
								GUI.DrawTexture(r, autoTransforms[1]);
								break;
							case RuleTile.TilingRule.Transform.MirrorY:
								GUI.DrawTexture(r, autoTransforms[2]);
								break;
						}

						if (Event.current.type == EventType.MouseDown && r.Contains(Event.current.mousePosition))
						{
							tilingRule.m_RuleTransform = (RuleTile.TilingRule.Transform)(((int)tilingRule.m_RuleTransform + 1) % 4);
							GUI.changed = true;
							Event.current.Use();
						}
					}
				}
			}
		}

        /// <summary>
        /// When the tile is selected
        /// </summary>
        /// <param name="userdata"></param>
		private static void OnSelect(object userdata)
		{
			MenuItemData data = (MenuItemData) userdata;
			data.m_Rule.m_RuleTransform = data.m_NewValue;
		}

        /// <summary>
        /// Container for the menu data
        /// </summary>
		private class MenuItemData
		{
			public RuleTile.TilingRule m_Rule;
			public RuleTile.TilingRule.Transform m_NewValue;

			public MenuItemData(RuleTile.TilingRule mRule, RuleTile.TilingRule.Transform mNewValue)
			{
				this.m_Rule = mRule;
				this.m_NewValue = mNewValue;
			}
		}

        /// <summary>
        /// When the sprite's gui is changed
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="tilingRule"></param>
		private void SpriteOnGUI(Rect rect, RuleTile.TilingRule tilingRule)
		{
			tilingRule.m_Sprites[0] = EditorGUI.ObjectField(new Rect(rect.xMax - rect.height, rect.yMin, rect.height, rect.height), tilingRule.m_Sprites[0], typeof (Sprite), false) as Sprite;
		}


        //this used to be static
        /// <summary>
        /// When the rule inspector's gui is changed
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="tilingRule"></param>
		private void RuleInspectorOnGUI(Rect rect, RuleTile.TilingRule tilingRule)
		{
			float y = rect.yMin;
			EditorGUI.BeginChangeCheck();
			GUI.Label(new Rect(rect.xMin, y, k_LabelWidth, k_SingleLineHeight), "Rule");
			tilingRule.m_RuleTransform = (RuleTile.TilingRule.Transform)EditorGUI.EnumPopup(new Rect(rect.xMin + k_LabelWidth, y, rect.width - k_LabelWidth, k_SingleLineHeight), tilingRule.m_RuleTransform);
			y += k_SingleLineHeight;            
            GUI.Label(new Rect(rect.xMin, y, k_LabelWidth, k_SingleLineHeight), "Collider");
			tilingRule.m_ColliderType = (Tile.ColliderType)EditorGUI.EnumPopup(new Rect(rect.xMin + k_LabelWidth, y, rect.width - k_LabelWidth, k_SingleLineHeight), tilingRule.m_ColliderType);
			y += k_SingleLineHeight;            
            GUI.Label(new Rect(rect.xMin, y, k_LabelWidth, k_SingleLineHeight), "Output");
			tilingRule.m_Output = (RuleTile.TilingRule.OutputSprite)EditorGUI.EnumPopup(new Rect(rect.xMin + k_LabelWidth, y, rect.width - k_LabelWidth, k_SingleLineHeight), tilingRule.m_Output);
			y += k_SingleLineHeight;

			if (tilingRule.m_Output == RuleTile.TilingRule.OutputSprite.Animation)
			{
				GUI.Label(new Rect(rect.xMin, y, k_LabelWidth, k_SingleLineHeight), "Speed");
				tilingRule.m_AnimationSpeed = EditorGUI.FloatField(new Rect(rect.xMin + k_LabelWidth, y, rect.width - k_LabelWidth, k_SingleLineHeight), tilingRule.m_AnimationSpeed);
				y += k_SingleLineHeight;
			}
			if (tilingRule.m_Output == RuleTile.TilingRule.OutputSprite.Random)
			{
				GUI.Label(new Rect(rect.xMin, y, k_LabelWidth, k_SingleLineHeight), "Noise");
				tilingRule.m_PerlinScale = EditorGUI.Slider(new Rect(rect.xMin + k_LabelWidth, y, rect.width - k_LabelWidth, k_SingleLineHeight), tilingRule.m_PerlinScale, 0.001f, 0.999f);
				y += k_SingleLineHeight;

				GUI.Label(new Rect(rect.xMin, y, k_LabelWidth, k_SingleLineHeight), "Shuffle");
				tilingRule.m_RandomTransform = (RuleTile.TilingRule.Transform)EditorGUI.EnumPopup(new Rect(rect.xMin + k_LabelWidth, y, rect.width - k_LabelWidth, k_SingleLineHeight), tilingRule.m_RandomTransform);
				y += k_SingleLineHeight;
			}

			if (tilingRule.m_Output != RuleTile.TilingRule.OutputSprite.Single)
			{
				GUI.Label(new Rect(rect.xMin, y, k_LabelWidth, k_SingleLineHeight), "Size");
				EditorGUI.BeginChangeCheck();
				int newLength = EditorGUI.DelayedIntField(new Rect(rect.xMin + k_LabelWidth, y, rect.width - k_LabelWidth, k_SingleLineHeight), tilingRule.m_Sprites.Length);
				if (EditorGUI.EndChangeCheck())
					Array.Resize(ref tilingRule.m_Sprites, Math.Max(newLength, 1));
				y += k_SingleLineHeight;

				for (int i = 0; i < tilingRule.m_Sprites.Length; i++)
				{
					tilingRule.m_Sprites[i] = EditorGUI.ObjectField(new Rect(rect.xMin + k_LabelWidth, y, rect.width - k_LabelWidth, k_SingleLineHeight), tilingRule.m_Sprites[i], typeof(Sprite), false) as Sprite;
					y += k_SingleLineHeight;
				}
			}
		}
                
        /// <summary>
        /// When the preview is moved
        /// </summary>
        /// <param name="assetPath"></param>
        /// <param name="subAssets"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
		public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
		{
			if (tile.m_DefaultSprite != null)
			{
				Type t = GetType("UnityEditor.SpriteUtility");               
				if (t != null)
				{
					MethodInfo method = t.GetMethod("RenderStaticPreview", new Type[] {typeof (Sprite), typeof (Color), typeof (int), typeof (int)});
					if (method != null)
					{
						object ret = method.Invoke("RenderStaticPreview", new object[] {tile.m_DefaultSprite, Color.white, width, height});
						if (ret is Texture2D)
							return ret as Texture2D;
					}
				}
			}
			return base.RenderStaticPreview(assetPath, subAssets, width, height);
		}

        /// <summary>
        /// Gets a type given the type name
        /// </summary>
        /// <param name="TypeName">The name of the type</param>
        /// <returns>The type</returns>
		private static Type GetType(string TypeName)
		{
			var type = Type.GetType(TypeName);
			if (type != null)
				return type;

			if (TypeName.Contains("."))
			{
				var assemblyName = TypeName.Substring(0, TypeName.IndexOf('.'));
				var assembly = Assembly.Load(assemblyName);
				if (assembly == null)
					return null;
				type = assembly.GetType(TypeName);
				if (type != null)
					return type;
			}

			var currentAssembly = Assembly.GetExecutingAssembly();
			var referencedAssemblies = currentAssembly.GetReferencedAssemblies();
			foreach (var assemblyName in referencedAssemblies)
			{
				var assembly = Assembly.Load(assemblyName);
				if (assembly != null)
				{
					type = assembly.GetType(TypeName);
					if (type != null)
						return type;
				}
			}
			return null;
		}

        /// <summary>
        /// Converts the base 64 string to a texture
        /// </summary>
        /// <param name="base64">The string to convert</param>
        /// <returns>The texture associated with the string</returns>
		private static Texture2D Base64ToTexture(string base64)
		{
			Texture2D t = new Texture2D(1, 1);
			t.hideFlags = HideFlags.HideAndDontSave;
			t.LoadImage(System.Convert.FromBase64String(base64));
			return t;
		}
	}
}
#endif
