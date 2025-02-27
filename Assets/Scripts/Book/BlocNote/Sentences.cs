﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sentences : MonoBehaviour
{
    public string phrase;

    private Text text;
    private int wordIndex;

    private GameObject[] holeFull;

    private int[] indexChar;

    private void Start()
    {
        text = GetComponent<Text>();
        holeFull = new GameObject[transform.childCount];
    }

    //public void FillWord(string mot)
    //{
    //    wordIndex = FindWord(mot);

    //    if (wordIndex > -1)
    //    {
    //        Debug.Log(text.text.Substring(0, wordIndex) + mot);
    //        text.text = text.text.Substring(0, wordIndex) + mot + text.text.Substring(wordIndex + 5, phrase.Length - wordIndex + 5);
    //    }
    //}

    public void FillHole(int holeIndex, GameObject mot)
    {
        holeFull[holeIndex] = mot;

        CalculateSentence();
        VerifySentence();
    }

    public void EmptyHole(int holeIndex)
    {
        holeFull[holeIndex] = null;

        CalculateSentence();
    }

    public GameObject[] GetFilledHoles()
    {
        return holeFull;
    }

    private void CalculateSentence()
    {
        indexChar = BlocNoteManager.instance.FindChar(phrase, '_').ToArray();

        if (indexChar.Length > 0)
        {
            gameObject.GetComponent<Text>().text = "";
            gameObject.GetComponent<Text>().text += phrase.Substring(0, indexChar[0]);

            Vector3 tempVector;
            int indexOffset = 0;
            GameObject temp = transform.GetChild(0).gameObject;

            for (int i = 0; i < indexChar.Length - 2; i += 2)
            {
                temp = transform.GetChild(i / 2).gameObject;
                tempVector = BlocNoteManager.instance.GetCharPos(gameObject.GetComponent<Text>(), gameObject.GetComponent<Text>().text + "\'", gameObject.GetComponent<Text>().text.Length, 0);
                //tempVector -= new Vector3(0, 5, 0); // ROUSTINE

                temp.transform.localPosition = new Vector2(-temp.transform.parent.GetComponent<RectTransform>().rect.width / 2, 0);
                temp.transform.localPosition += tempVector;

                if (holeFull[i / 2] == null)
                {
                    //temp = Instantiate(holePrefab, gameObject.transform);
                    //Debug.Log(tempVector);
                    gameObject.GetComponent<Text>().text += "_____";
                    gameObject.GetComponent<Text>().text += phrase.Substring(indexChar[i + 1] + 1, indexChar[i + 2] - indexChar[i + 1] - 1);
                    //temp.GetComponent<Hole>().wordIndex = indexChar[i] + indexOffset;
                    indexOffset -= indexChar[i + 1] - indexChar[i] + 1;
                    indexOffset += 5;
                    //temp.name = phrase.Substring(indexChar[i] + 1, indexChar[i + 1] - indexChar[i] - 1);
                }
                else
                {
                    gameObject.GetComponent<Text>().text += holeFull[i / 2].name + phrase.Substring(indexChar[i + 1] + 1, indexChar[i + 2] - indexChar[i + 1] - 1);
                }
            }

            temp = transform.GetChild(indexChar.Length / 2 - 1).gameObject;
            tempVector = BlocNoteManager.instance.GetCharPos(gameObject.GetComponent<Text>(), gameObject.GetComponent<Text>().text + "\'", gameObject.GetComponent<Text>().text.Length, 0);
            //tempVector -= new Vector3(0, 5, 0); // ROUSTINE

            temp.transform.localPosition = new Vector2(-temp.transform.parent.GetComponent<RectTransform>().rect.width / 2, 0);
            temp.transform.localPosition += tempVector;

            if (holeFull[indexChar.Length / 2 - 1] == null)
            {
                //Debug.Log(tempVector);
                gameObject.GetComponent<Text>().text += "_____";
                gameObject.GetComponent<Text>().text += phrase.Substring(indexChar[indexChar.Length - 1] + 1, phrase.Length - indexChar[indexChar.Length - 1] - 1);
                //temp.GetComponent<Hole>().wordIndex = indexChar[indexChar.Length - 2] + indexOffset;
                //temp.name = phrase.Substring(indexChar[indexChar.Length - 2] + 1, indexChar[indexChar.Length - 1] - indexChar[indexChar.Length - 2] - 1);
            }
            else
            {
                gameObject.GetComponent<Text>().text += holeFull[holeFull.Length - 1].name + phrase.Substring(indexChar[indexChar.Length - 1] + 1, phrase.Length - indexChar[indexChar.Length - 1] - 1);
            }
        }
        else
            gameObject.GetComponent<Text>().text = phrase;

        BlocNoteManager.instance.PlaceWords();
    }

    private void VerifySentence()
    {
        for (int i = 0; i < holeFull.Length; i++)
            if (holeFull[i] == null)
                return;

        for (int i = 0; i < holeFull.Length; i++)
            if (holeFull[i].name.ToLower() != transform.GetChild(i).name.ToLower())
            {
                for (int j = 0; j < holeFull.Length; j++)
                    BlocNoteManager.instance.VerifyWords(holeFull[j].GetComponent<Words>());

                return;
            }

        //Debug.LogError("WIIIIIIIN !!!");

        for (int i = 0; i < holeFull.Length; i++)
            BookManager.instance.Bin(holeFull[i]);

        BookManager.instance.blocNote.CalculWords();

        StartCoroutine(Grow(0.2f, 6));

        //text.text = phrase;
    }

    private int FindWord(string mot)
    {
        for (int i = 0; i < phrase.Length; i++)
            if (phrase[i] == mot[0] && phrase.Length - i > mot.Length)
                for (int j = 1; j < mot.Length; j++)
                    if (phrase[i + j] == mot[j])
                        return i;

        return -1;
    }

    IEnumerator Grow(float time, int frame)
    {
        for (int i = 0; i < frame; i++)
        {
            if(i < frame/2)
                transform.localScale += new Vector3(0.01f, 0.01f, 0);
            else
                transform.localScale -= new Vector3(0.01f, 0.01f, 0);

            yield return new WaitForSecondsRealtime(time / frame);
        }
    }
}
