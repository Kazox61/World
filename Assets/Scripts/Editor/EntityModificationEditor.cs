using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;
using System.Reflection;
using GameNS.Entity;

namespace EditorNS {
    [CustomEditor(typeof(EntityModification))]
    public class EntityModificationEditor : Editor {
	    
	    private const string XICONSTRING = "iVBORw0KGgoAAAANSUhEUgAAAA8AAAAPCAYAAAA71pVKAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAYdEVYdFNvZnR3YXJlAHBhaW50Lm5ldCA0LjAuNWWFMmUAAABoSURBVDhPnY3BDcAgDAOZhS14dP1O0x2C/LBEgiNSHvfwyZabmV0jZRUpq2zi6f0DJwdcQOEdwwDLypF0zHLMa9+NQRxkQ+ACOT2STVw/q8eY1346ZlE54sYAhVhSDrjwFymrSFnD2gTZpls2OvFUHAAAAABJRU5ErkJggg==";
		private const string ARROW0 = "iVBORw0KGgoAAAANSUhEUgAAAA8AAAAPCAYAAAA71pVKAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAYdEVYdFNvZnR3YXJlAHBhaW50Lm5ldCA0LjAuNWWFMmUAAACYSURBVDhPzZExDoQwDATzE4oU4QXXcgUFj+YxtETwgpMwXuFcwMFSRMVKKwzZcWzhiMg91jtg34XIntkre5EaT7yjjhI9pOD5Mw5k2X/DdUwFr3cQ7Pu23E/BiwXyWSOxrNqx+ewnsayam5OLBtbOGPUM/r93YZL4/dhpR/amwByGFBz170gNChA6w5bQQMqramBTgJ+Z3A58WuWejPCaHQAAAABJRU5ErkJggg==";
		private const string ARROW1 = "iVBORw0KGgoAAAANSUhEUgAAAA8AAAAPCAYAAAA71pVKAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAYdEVYdFNvZnR3YXJlAHBhaW50Lm5ldCA0LjAuNWWFMmUAAABqSURBVDhPxYzBDYAgEATpxYcd+PVr0fZ2siZrjmMhFz6STIiDs8XMlpEyi5RkO/d66TcgJUB43JfNBqRkSEYDnYjhbKD5GIUkDqRDwoH3+NgTAw+bL/aoOP4DOgH+iwECEt+IlFmkzGHlAYKAWF9R8zUnAAAAAElFTkSuQmCC";
		private const string ARROW2 = "iVBORw0KGgoAAAANSUhEUgAAAA8AAAAPCAYAAAA71pVKAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAYdEVYdFNvZnR3YXJlAHBhaW50Lm5ldCA0LjAuNWWFMmUAAAC0SURBVDhPjVE5EsIwDMxPKFKYF9CagoJH8xhaMskLmEGsjOSRkBzYmU2s9a58TUQUmCH1BWEHweuKP+D8tphrWcAHuIGrjPnPNY8X2+DzEWE+FzrdrkNyg2YGNNfRGlyOaZDJOxBrDhgOowaYW8UW0Vau5ZkFmXbbDr+CzOHKmLinAXMEePyZ9dZkZR+s5QX2O8DY3zZ/sgYcdDqeEVp8516o0QQV1qeMwg6C91toYoLoo+kNt/tpKQEVvFQAAAAASUVORK5CYII=";
		private const string ARROW3 = "iVBORw0KGgoAAAANSUhEUgAAAA8AAAAPCAYAAAA71pVKAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAYdEVYdFNvZnR3YXJlAHBhaW50Lm5ldCA0LjAuNWWFMmUAAAB2SURBVDhPzY1LCoAwEEPnLi48gW5d6p31bH5SMhp0Cq0g+CCLxrzRPqMZ2pRqKG4IqzJc7JepTlbRZXYpWTg4RZE1XAso8VHFKNhQuTjKtZvHUNCEMogO4K3BhvMn9wP4EzoPZ3n0AGTW5fiBVzLAAYTP32C2Ay3agtu9V/9PAAAAAElFTkSuQmCC";
		private const string ARROW5 = "iVBORw0KGgoAAAANSUhEUgAAAA8AAAAPCAYAAAA71pVKAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAYdEVYdFNvZnR3YXJlAHBhaW50Lm5ldCA0LjAuNWWFMmUAAABqSURBVDhPnY3BCYBADASvFx924NevRdvbyoLBmNuDJQMDGjNxAFhK1DyUQ9fvobCdO+j7+sOKj/uSB+xYHZAxl7IR1wNTXJeVcaAVU+614uWfCT9mVUhknMlxDokd15BYsQrJFHeUQ0+MB5ErsPi/6hO1AAAAAElFTkSuQmCC";
		private const string ARROW6 = "iVBORw0KGgoAAAANSUhEUgAAAA8AAAAPCAYAAAA71pVKAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAYdEVYdFNvZnR3YXJlAHBhaW50Lm5ldCA0LjAuNWWFMmUAAACaSURBVDhPxZExEkAwEEVzE4UiTqClUDi0w2hlOIEZsV82xCZmQuPPfFn8t1mirLWf7S5flQOXjd64vCuEKWTKVt+6AayH3tIa7yLg6Qh2FcKFB72jBgJeziA1CMHzeaNHjkfwnAK86f3KUafU2ClHIJSzs/8HHLv09M3SaMCxS7ljw/IYJWzQABOQZ66x4h614ahTCL/WT7BSO51b5Z5hSx88AAAAAElFTkSuQmCC";
		private const string ARROW7 = "iVBORw0KGgoAAAANSUhEUgAAAA8AAAAPCAYAAAA71pVKAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAYdEVYdFNvZnR3YXJlAHBhaW50Lm5ldCA0LjAuNWWFMmUAAABQSURBVDhPYxh8QNle/T8U/4MKEQdAmsz2eICx6W530gygr2aQBmSMphkZYxqErAEXxusKfAYQ7XyyNMIAsgEkaYQBkAFkaYQBsjXSGDAwAAD193z4luKPrAAAAABJRU5ErkJggg==";
		private const string ARROW8 = "iVBORw0KGgoAAAANSUhEUgAAAA8AAAAPCAYAAAA71pVKAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAYdEVYdFNvZnR3YXJlAHBhaW50Lm5ldCA0LjAuNWWFMmUAAACYSURBVDhPxZE9DoAwCIW9iUOHegJXHRw8tIdx1egJTMSHAeMPaHSR5KVQ+KCkCRF91mdz4VDEWVzXTBgg5U1N5wahjHzXS3iFFVRxAygNVaZxJ6VHGIl2D6oUXP0ijlJuTp724FnID1Lq7uw2QM5+thoKth0N+GGyA7IA3+yM77Ag1e2zkey5gCdAg/h8csy+/89v7E+YkgUntOWeVt2SfAAAAABJRU5ErkJggg==";
		private const string MIRRORX = "iVBORw0KGgoAAAANSUhEUgAAAA8AAAAPCAYAAAA71pVKAAAABGdBTUEAALGPC/xhBQAAAAlwSFlzAAAOwQAADsEBuJFr7QAAABh0RVh0U29mdHdhcmUAcGFpbnQubmV0IDQuMC41ZYUyZQAAAG1JREFUOE+lj9ENwCAIRB2IFdyRfRiuDSaXAF4MrR9P5eRhHGb2Gxp2oaEjIovTXSrAnPNx6hlgyCZ7o6omOdYOldGIZhAziEmOTSfigLV0RYAB9y9f/7kO8L3WUaQyhCgz0dmCL9CwCw172HgBeyG6oloC8fAAAAAASUVORK5CYII=";
		private const string MIRRORY = "iVBORw0KGgoAAAANSUhEUgAAAA8AAAAPCAYAAAA71pVKAAAABGdBTUEAALGPC/xhBQAAAAlwSFlzAAAOwgAADsIBFShKgAAAABh0RVh0U29mdHdhcmUAcGFpbnQubmV0IDQuMC41ZYUyZQAAAG9JREFUOE+djckNACEMAykoLdAjHbPyw1IOJ0L7mAejjFlm9hspyd77Kk+kBAjPOXcakJIh6QaKyOE0EB5dSPJAiUmOiL8PMVGxugsP/0OOib8vsY8yYwy6gRyC8CB5QIWgCMKBLgRSkikEUr5h6wOPWfMoCYILdgAAAABJRU5ErkJggg==";
		private const string ROTATED = "iVBORw0KGgoAAAANSUhEUgAAAA8AAAAPCAYAAAA71pVKAAAABGdBTUEAALGPC/xhBQAAAAlwSFlzAAAOwQAADsEBuJFr7QAAABh0RVh0U29mdHdhcmUAcGFpbnQubmV0IDQuMC41ZYUyZQAAAHdJREFUOE+djssNwCAMQxmIFdgx+2S4Vj4YxWlQgcOT8nuG5u5C732Sd3lfLlmPMR4QhXgrTQaimUlA3EtD+CJlBuQ7aUAUMjEAv9gWCQNEPhHJUkYfZ1kEpcxDzioRzGIlr0Qwi0r+Q5rTgM+AAVcygHgt7+HtBZs/2QVWP8ahAAAAAElFTkSuQmCC";
		

        private static Texture2D[] arrows;

        public static Texture2D[] Arrows {
            get {
                if (arrows == null) {
                    arrows = new Texture2D[10];
                    arrows[0] = Base64ToTexture(ARROW0);
                    arrows[1] = Base64ToTexture(ARROW1);
                    arrows[2] = Base64ToTexture(ARROW2);
                    arrows[3] = Base64ToTexture(ARROW3);
                    arrows[5] = Base64ToTexture(ARROW5);
                    arrows[6] = Base64ToTexture(ARROW6);
                    arrows[7] = Base64ToTexture(ARROW7);
                    arrows[8] = Base64ToTexture(ARROW8);
                    arrows[9] = Base64ToTexture(XICONSTRING);
                }
                return arrows;
            }
        }

        private ReorderableList reorderableList;
        public  EntityModification Modification => (target as EntityModification);

        private Rect listRect;

        const float DEFAULT_ELEMENT_HEIGHT = 48f;
        const float PADDING_BETWEEN_RULES = 13f;
        const float SINGLE_LINE_HEIGHT = 16f;
        const float LABEL_WIDTH = 53f;

        public void OnEnable() {
            if (Modification.rules == null) {
                Modification.rules = new List<Rule>();
            }
            reorderableList = new ReorderableList(Modification.rules, typeof(Rule), true, true, true, true);
            reorderableList.drawHeaderCallback = OnDrawHeader;
            reorderableList.drawElementCallback = OnDrawElement;
            reorderableList.elementHeightCallback = GetElementHeight;
            reorderableList.onReorderCallback = ListUpdated;
        }

        private void ListUpdated(ReorderableList list) {
	        Debug.Log("ListUpdated");
	        Debug.Log(Modification.rules[0].neighbors[0]);
            Save();
        }

        private float GetElementHeight(int index) {
            if (Modification.rules != null && Modification.rules.Count > 0) {
                switch (Modification.rules[index].output) {
                    case Rule.OutputSprite.Random:
                        return DEFAULT_ELEMENT_HEIGHT + SINGLE_LINE_HEIGHT * (Modification.rules[index].sprites.Length + 3) +
                               PADDING_BETWEEN_RULES;
                    case Rule.OutputSprite.Animation:
                        return DEFAULT_ELEMENT_HEIGHT + SINGLE_LINE_HEIGHT * (Modification.rules[index].sprites.Length + 2) +
                               PADDING_BETWEEN_RULES;
                }
            }

            return DEFAULT_ELEMENT_HEIGHT + PADDING_BETWEEN_RULES;
        }

        private void OnDrawElement(Rect rect, int index, bool isActive, bool isFocused) {
            Rule rule = Modification.rules[index];

            float yPos = rect.yMin + 2f;
            float height = rect.height - PADDING_BETWEEN_RULES;
            float matrixWidth = DEFAULT_ELEMENT_HEIGHT;
			
            Rect inspectorRect = new Rect(rect.xMin, yPos, rect.width - matrixWidth * 2f - 20f, height);
            Rect matrixRect = new Rect(rect.xMax - matrixWidth * 2f - 10f, yPos, matrixWidth, DEFAULT_ELEMENT_HEIGHT);
            Rect spriteRect = new Rect(rect.xMax - matrixWidth - 5f, yPos, matrixWidth, DEFAULT_ELEMENT_HEIGHT);

            EditorGUI.BeginChangeCheck();
            RuleInspectorOnGUI(inspectorRect, rule);
            RuleMatrixOnGUI(matrixRect, rule);
            SpriteOnGUI(spriteRect, rule);
            if (EditorGUI.EndChangeCheck())
                Save();
        }

        private void Save() {
	        Debug.Log("Save");
            EditorUtility.SetDirty(target);
            SceneView.RepaintAll();
        }
        
        private void OnDrawHeader(Rect rect) {
            GUI.Label(rect, "Rules");
        }

        public override void OnInspectorGUI() {
            Modification.defaultSprite = EditorGUILayout.ObjectField("Default Sprite", Modification.defaultSprite, typeof(Sprite), false) as Sprite;
            //tile.m_DefaultColliderType = (Tile.ColliderType)EditorGUILayout.EnumPopup("Default Collider", tile.m_DefaultColliderType);
            EditorGUILayout.Space();

            if (reorderableList != null && Modification.rules != null) {
	            reorderableList.DoLayoutList();
            }
                
        }
        
        private void RuleMatrixOnGUI(Rect rect, Rule rule) {
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
						switch (rule.neighbors[index])
						{
							case Rule.Neighbor.This:
								GUI.DrawTexture(r, Arrows[y*3 + x]);
								break;
							case Rule.Neighbor.NotThis:
								GUI.DrawTexture(r, Arrows[9]);
								break;
						}
						if (Event.current.type == EventType.MouseDown && r.Contains(Event.current.mousePosition))
						{
							rule.neighbors[index] = (Rule.Neighbor) (((int)rule.neighbors[index] + 1) % 3);
							GUI.changed = true;
							Event.current.Use();
						}

						index++;
					}
					else
					{
						switch (rule.ruleTransform)
						{
							case Rule.Transform.Rotated:
								//GUI.DrawTexture(r, autoTransforms[0]);
								break;
							case Rule.Transform.MirrorX:
								//GUI.DrawTexture(r, autoTransforms[1]);
								break;
							case Rule.Transform.MirrorY:
								//GUI.DrawTexture(r, autoTransforms[2]);
								break;
						}

						if (Event.current.type == EventType.MouseDown && r.Contains(Event.current.mousePosition))
						{
							rule.ruleTransform = (Rule.Transform)(((int)rule.ruleTransform + 1) % 4);
							GUI.changed = true;
							Event.current.Use();
						}
					}
				}
			}
        }

        public static void OnSelect(object userdata) {
	        var data = (MenuItemData) userdata;
	        data.rule.ruleTransform = data.newValue;
        }

        private class MenuItemData {
	        public Rule rule;
	        public Rule.Transform newValue;

	        public MenuItemData(Rule mRule, Rule.Transform mNewValue)
	        {
		        this.rule = mRule;
		        this.newValue = mNewValue;
	        }
        }

        private void SpriteOnGUI(Rect rect, Rule rule) {
	        rule.sprites[0] = EditorGUI.ObjectField(new Rect(rect.xMax - rect.height, rect.yMin, rect.height, rect.height), rule.sprites[0], typeof (Sprite), false) as Sprite;
        }
        
        private void RuleInspectorOnGUI(Rect rect, Rule rule) {
            float y = rect.yMin;
			EditorGUI.BeginChangeCheck();
			
			GUI.Label(new Rect(rect.xMin, y, LABEL_WIDTH, SINGLE_LINE_HEIGHT), "offsetX");
			rule.offsetX = EditorGUI.FloatField(new Rect(rect.xMin + LABEL_WIDTH, y, rect.width - LABEL_WIDTH, SINGLE_LINE_HEIGHT), rule.offsetX);
			y += SINGLE_LINE_HEIGHT;
			
			GUI.Label(new Rect(rect.xMin, y, LABEL_WIDTH, SINGLE_LINE_HEIGHT), "offsetY");
			rule.offsetY = EditorGUI.FloatField(new Rect(rect.xMin + LABEL_WIDTH, y, rect.width - LABEL_WIDTH, SINGLE_LINE_HEIGHT), rule.offsetY);
			y += SINGLE_LINE_HEIGHT;
			
			
			/*
			GUI.Label(new Rect(rect.xMin, y, LABEL_WIDTH, SINGLE_LINE_HEIGHT), "Rule");
			rule.ruleTransform = (Rule.Transform)EditorGUI.EnumPopup(new Rect(rect.xMin + LABEL_WIDTH, y, rect.width - LABEL_WIDTH, SINGLE_LINE_HEIGHT), rule.ruleTransform);
			y += SINGLE_LINE_HEIGHT;
			GUI.Label(new Rect(rect.xMin, y, LABEL_WIDTH, SINGLE_LINE_HEIGHT), "Collider");
			rule.colliderType = (Rule.ColliderType)EditorGUI.EnumPopup(new Rect(rect.xMin + LABEL_WIDTH, y, rect.width - LABEL_WIDTH, SINGLE_LINE_HEIGHT), rule.colliderType);
			y += SINGLE_LINE_HEIGHT;
			GUI.Label(new Rect(rect.xMin, y, LABEL_WIDTH, SINGLE_LINE_HEIGHT), "Output");
			rule.output = (Rule.OutputSprite)EditorGUI.EnumPopup(new Rect(rect.xMin + LABEL_WIDTH, y, rect.width - LABEL_WIDTH, SINGLE_LINE_HEIGHT), rule.output);
			y += SINGLE_LINE_HEIGHT;

			if (rule.output == Rule.OutputSprite.Animation)
			{
				GUI.Label(new Rect(rect.xMin, y, LABEL_WIDTH, SINGLE_LINE_HEIGHT), "Speed");
				rule.animationSpeed = EditorGUI.FloatField(new Rect(rect.xMin + LABEL_WIDTH, y, rect.width - LABEL_WIDTH, SINGLE_LINE_HEIGHT), rule.animationSpeed);
				y += SINGLE_LINE_HEIGHT;
			}
			if (rule.output == Rule.OutputSprite.Random)
			{
				GUI.Label(new Rect(rect.xMin, y, LABEL_WIDTH, SINGLE_LINE_HEIGHT), "Noise");
				rule.perlinScale = EditorGUI.Slider(new Rect(rect.xMin + LABEL_WIDTH, y, rect.width - LABEL_WIDTH, SINGLE_LINE_HEIGHT), rule.perlinScale, 0.001f, 0.999f);
				y += SINGLE_LINE_HEIGHT;

				GUI.Label(new Rect(rect.xMin, y, LABEL_WIDTH, SINGLE_LINE_HEIGHT), "Shuffle");
				rule.randomTransform = (Rule.Transform)EditorGUI.EnumPopup(new Rect(rect.xMin + LABEL_WIDTH, y, rect.width - LABEL_WIDTH, SINGLE_LINE_HEIGHT), rule.randomTransform);
				y += SINGLE_LINE_HEIGHT;
			}

			if (rule.output != Rule.OutputSprite.Single)
			{
				GUI.Label(new Rect(rect.xMin, y, LABEL_WIDTH, SINGLE_LINE_HEIGHT), "Size");
				EditorGUI.BeginChangeCheck();
				int newLength = EditorGUI.IntField(new Rect(rect.xMin + LABEL_WIDTH, y, rect.width - LABEL_WIDTH, SINGLE_LINE_HEIGHT), rule.sprites.Length);
				if (EditorGUI.EndChangeCheck())
					Array.Resize(ref rule.sprites, Math.Max(newLength, 1));
				y += SINGLE_LINE_HEIGHT;

				for (int i = 0; i < rule.sprites.Length; i++)
				{
					rule.sprites[i] = EditorGUI.ObjectField(new Rect(rect.xMin + LABEL_WIDTH, y, rect.width - LABEL_WIDTH, SINGLE_LINE_HEIGHT), rule.sprites[i], typeof(Sprite), false) as Sprite;
					y += SINGLE_LINE_HEIGHT;
				}
			}
			*/
        }
        
        public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
        {
	        if (Modification.defaultSprite != null)
	        {
		        Type t = GetType("UnityEditor.SpriteUtility");
		        if (t != null)
		        {
			        MethodInfo method = t.GetMethod("RenderStaticPreview", new Type[] {typeof (Sprite), typeof (Color), typeof (int), typeof (int)});
			        if (method != null)
			        {
				        object ret = method.Invoke("RenderStaticPreview", new object[] {Modification.defaultSprite, Color.white, width, height});
				        if (ret is Texture2D)
					        return ret as Texture2D;
			        }
		        }
	        }
	        return base.RenderStaticPreview(assetPath, subAssets, width, height);
        }

        private static Type GetType(string typeName)
        {
	        var type = Type.GetType(typeName);
	        if (type != null)
		        return type;

	        if (typeName.Contains("."))
	        {
		        var assemblyName = typeName.Substring(0, typeName.IndexOf('.'));
		        var assembly = Assembly.Load(assemblyName);
		        if (assembly == null)
			        return null;
		        type = assembly.GetType(typeName);
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
			        type = assembly.GetType(typeName);
			        if (type != null)
				        return type;
		        }
	        }
	        return null;
        }

        private static Texture2D Base64ToTexture(string base64)
        {
	        Texture2D t = new Texture2D(1, 1);
	        t.hideFlags = HideFlags.HideAndDontSave;
	        t.LoadImage(System.Convert.FromBase64String(base64));
	        return t;
        }
    }
}