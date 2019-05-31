using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


[Serializable]
	[CreateAssetMenu]
	public class RuleTile : TileBase
	{
		public Sprite m_DefaultSprite;
		public Tile.ColliderType m_DefaultColliderType = Tile.ColliderType.Sprite;
        public InputTile[] m_InputTiles = new InputTile[1] { new InputTile() };
        public TilingRule.InputSprite m_Input = TilingRule.InputSprite.No;


        [Serializable]
		public class TilingRule
		{
			public Neighbor[] m_Neighbors;
			public Sprite[] m_Sprites;
            //public InputTile[] m_InputTiles;
			public float m_AnimationSpeed;
			public float m_PerlinScale;
			public Transform m_RuleTransform;
            //public InputSprite m_Input;
			public OutputSprite m_Output;
			public Tile.ColliderType m_ColliderType;
			public Transform m_RandomTransform;
			
			public TilingRule()
			{
                //m_Input = InputSprite.Single;
				m_Output = OutputSprite.Single;
				m_Neighbors = new Neighbor[8];
				m_Sprites = new Sprite[1];
                //m_InputTiles = new InputTile[1];
				m_AnimationSpeed = 1f;
				m_PerlinScale = 0.5f;
				m_ColliderType = Tile.ColliderType.Sprite;

				for(int i=0; i<m_Neighbors.Length; i++)
					m_Neighbors[i] = Neighbor.DontCare;
			}

			public enum Transform { Fixed, Rotated, MirrorX, MirrorY }
			public enum Neighbor { DontCare, This, NotThis }
			public enum OutputSprite { Single, Random, Animation }
            public enum InputSprite { No, Yes }
		}

        [Serializable]
        public class InputTile
        {
            public TileBase Tile;
            public Tilemap Tilemap;
            public InputTile(TileBase Tile, Tilemap Tilemap)
            {
                this.Tile = Tile;
                this.Tilemap = Tilemap;
            }
            public InputTile()
            {
            Tile = null;
            Tilemap = null;
            }
        }

		[HideInInspector] public List<TilingRule> m_TilingRules;

		public override void GetTileData(Vector3Int position, ITilemap tileMap, ref TileData tileData)
		{
            if (m_InputTiles.Length > 0 && m_InputTiles[0].Tilemap == null)
            {
                m_InputTiles[0].Tilemap = GameObject.Find("Ground").GetComponent<Tilemap>();
            }
            tileData.sprite = m_DefaultSprite;
			tileData.colliderType = m_DefaultColliderType;
			tileData.flags = TileFlags.LockTransform;
			tileData.transform = Matrix4x4.identity;
			
			foreach (TilingRule rule in m_TilingRules)
			{
				Matrix4x4 transform = Matrix4x4.identity;
				if (RuleMatches(rule, position, tileMap, ref transform))
				{
					switch (rule.m_Output)
					{
							case TilingRule.OutputSprite.Single:
							case TilingRule.OutputSprite.Animation:
								tileData.sprite = rule.m_Sprites[0];
							break;
							case TilingRule.OutputSprite.Random:
								int index = Mathf.Clamp(Mathf.FloorToInt(GetPerlinValue(position, rule.m_PerlinScale, 100000f) * rule.m_Sprites.Length), 0, rule.m_Sprites.Length - 1);
								tileData.sprite = rule.m_Sprites[index];
								if (rule.m_RandomTransform != TilingRule.Transform.Fixed)
									transform = ApplyRandomTransform(rule.m_RandomTransform, transform, rule.m_PerlinScale, position);
							break;
					}
					tileData.transform = transform;
					tileData.colliderType = rule.m_ColliderType;
					break;
				}
			}
		}

		private static float GetPerlinValue(Vector3Int position, float scale, float offset)
		{
			return Mathf.PerlinNoise((position.x + offset) * scale, (position.y + offset) * scale);
		}

		public override bool GetTileAnimationData(Vector3Int position, ITilemap tilemap, ref TileAnimationData tileAnimationData)
		{
			foreach (TilingRule rule in m_TilingRules)
			{
				Matrix4x4 transform = Matrix4x4.identity;
				if (RuleMatches(rule, position, tilemap, ref transform) && rule.m_Output == TilingRule.OutputSprite.Animation)
				{
					tileAnimationData.animatedSprites = rule.m_Sprites;
					tileAnimationData.animationSpeed = rule.m_AnimationSpeed;
					return true;
				}
			}
			return false;
		}
		
		public override void RefreshTile(Vector3Int location, ITilemap tileMap)
		{
            
        if (m_TilingRules != null && m_TilingRules.Count > 0)
			{
				for (int y = -1; y <= 1; y++)
				{
					for (int x = -1; x <= 1; x++)
					{
						base.RefreshTile(location + new Vector3Int(x, y, 0), tileMap);
					}
				}
			}
			else
			{
				base.RefreshTile(location, tileMap);
			}
		}

		public bool RuleMatches(TilingRule rule, Vector3Int position, ITilemap tilemap, ref Matrix4x4 transform)
		{
			// Check rule against rotations of 0, 90, 180, 270
			for (int angle = 0; angle <= (rule.m_RuleTransform == TilingRule.Transform.Rotated ? 270 : 0); angle += 90)
			{
				if (RuleMatches(rule, position, tilemap, angle))
				{
					transform = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, -angle), Vector3.one);
					return true;
				}
			}

			// Check rule against x-axis mirror
			if ((rule.m_RuleTransform == TilingRule.Transform.MirrorX) && RuleMatches(rule, position, tilemap, true, false))
			{
				transform = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(-1f, 1f, 1f));
				return true;
			}

			// Check rule against y-axis mirror
			if ((rule.m_RuleTransform == TilingRule.Transform.MirrorY) && RuleMatches(rule, position, tilemap, false, true))
			{
				transform = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1f, -1f, 1f));
				return true;
			}

			return false;
		}

		private static Matrix4x4 ApplyRandomTransform(TilingRule.Transform type, Matrix4x4 original, float perlinScale, Vector3Int position)
		{
			float perlin = GetPerlinValue(position, perlinScale, 200000f);
			switch (type)
			{
				case TilingRule.Transform.MirrorX:
					return original * Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(perlin < 0.5 ? 1f : -1f, 1f, 1f));
				case TilingRule.Transform.MirrorY:
					return original * Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1f, perlin < 0.5 ? 1f : -1f, 1f));
				case TilingRule.Transform.Rotated:
					int angle = Mathf.Clamp(Mathf.FloorToInt(perlin * 4), 0, 3) * 90;
					return Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, -angle), Vector3.one);		
			}
			return original;
		}

		public bool RuleMatches(TilingRule rule, Vector3Int position, ITilemap tilemap, int angle)
		{
			for (int y = -1; y <= 1; y++)
			{
				for (int x = -1; x <= 1; x++)
				{
					if (x != 0 || y != 0)
					{
						Vector3Int offset = new Vector3Int(x, y, 0);
						Vector3Int rotated = GetRotatedPos(offset, angle);
						int index = GetIndexOfOffset(rotated);
                        //TileBase tile = tilemap.GetTile(position + offset);
                        TileBase tile = GetTile(position + offset, rule, index, tilemap);
                        if (!CheckTile(index, tile, rule))
                        {
                            return false;
                        }
					}
				}
				
			}
			return true;
		}

		public bool RuleMatches(TilingRule rule, Vector3Int position, ITilemap tilemap, bool mirrorX, bool mirrorY)
		{
			for (int y = -1; y <= 1; y++)
			{
				for (int x = -1; x <= 1; x++)
				{
					if (x != 0 || y != 0)
					{
						Vector3Int offset = new Vector3Int(x, y, 0);
						Vector3Int mirrored = GetMirroredPos(offset, mirrorX, mirrorY);
						int index = GetIndexOfOffset(mirrored);
                        //TileBase tile = tilemap.GetTile(position + offset);
                        TileBase tile = GetTile(position + offset, rule, index, tilemap);
						if (!CheckTile(index, tile, rule))
						{
							return false;
						}
					}
				}
			}
			
			return true;
		}

        private TileBase GetTile(Vector3Int pos, TilingRule rule, int index, ITilemap tilemap)
        {
            
            if ((int)rule.m_Neighbors[index] > 2 && m_Input == TilingRule.InputSprite.Yes)
            {                
                //return rule.m_InputTiles[(int)rule.m_Neighbors[index] - 3].Tilemap.GetTile(pos);
                return (m_InputTiles[(int)rule.m_Neighbors[index] - 3] == null || m_InputTiles[(int)rule.m_Neighbors[index] - 3].Tilemap == null) ? null : m_InputTiles[(int)rule.m_Neighbors[index] - 3].Tilemap.GetTile(pos);
            }
            return tilemap.GetTile(pos);
        }

        private bool CheckTile(int index, TileBase tile, TilingRule rule)
        {           
            if (rule.m_Neighbors[index] == TilingRule.Neighbor.This && tile != this || rule.m_Neighbors[index] == TilingRule.Neighbor.NotThis && tile == this)
            {
                return false;
            }
            if (m_Input == TilingRule.InputSprite.Yes)
            {                
                int nIndex = (int)rule.m_Neighbors[index] - 3;
                //if (tile.GetType() != rule.m_InputTiles[nIndex].Tile.GetType())
                //{
                //    return false;
                //}
                if ((tile == null && nIndex >= 0) || (nIndex >= 0 && m_InputTiles[nIndex].Tile != null && tile.GetType() != m_InputTiles[nIndex].Tile.GetType()))
                {
                    return false;
                }
            }
            return true;
        }

		private int GetIndexOfOffset(Vector3Int offset)
		{
			int result = offset.x + 1 + (-offset.y + 1) * 3;
			if (result >= 4)
				result--;
			return result;
		}

		public Vector3Int GetRotatedPos(Vector3Int original, int rotation)
		{
			switch (rotation)
			{
				case 0:
					return original;
				case 90:
					return new Vector3Int(-original.y, original.x, original.z);
				case 180:
					return new Vector3Int(-original.x, -original.y, original.z);
				case 270:
					return new Vector3Int(original.y, -original.x, original.z);
			}
			return original;
		}

		public Vector3Int GetMirroredPos(Vector3Int original, bool mirrorX, bool mirrorY)
		{
			return new Vector3Int(original.x * (mirrorX ? -1 : 1), original.y * (mirrorY ? -1 : 1), original.z);
		}
	}

