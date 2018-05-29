﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(MicrogameCollection))]
public class MicrogameCollectionEditor : Editor
{
    MicrogameCollection collection;

    private List<MicrogameList> microgames;
    private class MicrogameList
    {
        public string milestoneName;
        public bool show = false;
        public List<string> idList;

        public MicrogameList()
        {
            idList = new List<string>();
        }
    }
    
    private void OnEnable()
    {
        collection = (MicrogameCollection)target;
        setMicrogames();
    }

    void setMicrogames()
    {
        microgames = new List<MicrogameList>();
        var collectionMicrogames = collection.microgames;
        var milestoneNames = Enum.GetNames(typeof(MicrogameTraits.Milestone));
        for (int i = milestoneNames.Length - 1; i >= 0; i--)
        {
            var milestoneMicrogames = collectionMicrogames.Where(a => (int)a.difficultyTraits[0].milestone == i);
            var newList = new MicrogameList();
            newList.milestoneName = milestoneNames[i];
            foreach (var milestoneMicrogame in milestoneMicrogames)
            {
                newList.idList.Add(milestoneMicrogame.microgameId);
            }
            microgames.Add(newList);
        }
    }

    public override void OnInspectorGUI()
	{
		if (GUILayout.Button("Update Microgames"))
		{
			collection.updateMicrogames();
            setMicrogames();
            EditorUtility.SetDirty(collection);
        }
        if (GUILayout.Button("Update Build Path"))
        {
            collection.updateBuildPath();
            EditorUtility.SetDirty(collection);
        }

        GUILayout.Label("Indexed Microgames:");
        var names = Enum.GetNames(typeof(MicrogameTraits.Milestone));
        foreach (var microgameList in microgames)
        {
            microgameList.show = EditorGUILayout.Foldout(microgameList.show, microgameList.milestoneName);
            if (microgameList.show)
            {
                foreach (var microgameId in microgameList.idList)
                {
                    EditorGUILayout.LabelField("    " + microgameId);
                }
            }
        }
	}

}
