/* This wizard will replace a selection with an object or prefab.
 * Scene objects will be cloned (destroying their prefab links).
 * Original coding by 'yesfish', nabbed from Unity Forums
 * 'keep parent' added by Dave A (also removed 'rotation' option, using localRotation
 */
using UnityEngine;
using UnityEditor;
using System.Collections;

public class ReplaceSelection : ScriptableWizard
{
    static GameObject replacement = null;
    static bool keepOriginals = false;
    static bool keepNames = false;

    public GameObject ReplacementObject = null;
    public bool KeepOriginals = false;
    public bool KeepNames = true;

    [MenuItem("GameObject/- Replace Selection -")]
    static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard(
            "Replace Selection", typeof(ReplaceSelection), "Replace");
    }

    public ReplaceSelection()
    {
        ReplacementObject = replacement;
        KeepOriginals = keepOriginals;
		KeepNames = keepNames;
		
    }

    void OnWizardUpdate()
    {
        replacement = ReplacementObject;
        keepOriginals = KeepOriginals;
		keepNames = KeepNames;
    }

    void OnWizardCreate()
    {
        if (replacement == null)
            return;

        Transform[] transforms = Selection.GetTransforms(
            SelectionMode.TopLevel | SelectionMode.OnlyUserModifiable);

        foreach (Transform t in transforms)
        {
            GameObject g;

			PrefabType pref = PrefabUtility.GetPrefabType(replacement);

            if (pref == PrefabType.Prefab || pref == PrefabType.ModelPrefab)
            {
				g =  (GameObject)PrefabUtility.InstantiatePrefab(replacement);
            }
            else
            {
                g = (GameObject)Editor.Instantiate(replacement);
            }

            g.transform.parent = t.parent;
            g.name = keepNames ? t.gameObject.name : replacement.name;
            g.transform.localPosition = t.localPosition;
            g.transform.localScale = t.localScale;
            g.transform.localRotation = t.localRotation;
        }

        if (!keepOriginals)
        {
            foreach (GameObject g in Selection.gameObjects)
			{
                GameObject.DestroyImmediate(g);
            }
        }
    }
}