using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class MainScript : MonoBehaviour
{
    public List<int> mTime = new List<int>();
    public List<string> mConferenceName = new List<string>();
    public TMP_InputField mInputTime,mInputConferenceName;
    int mMorningShift = 180,mPostLunch=180;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InsertConferenceTime()
    {
        if(mInputTime.text!="" && mInputConferenceName.text != "")
        {
            mConferenceName.Add(mInputConferenceName.text);
            mTime.Add(int.Parse(mInputTime.text));
            mInputTime.text = "";
            mInputConferenceName.text = "";
        }
 
    }

    public void SolutionCheck()
    {
        try
        {
            var maxValues = (int[,])Array.CreateInstance(typeof(int), mTime.Count + 1, mMorningShift + 1);
            var doInclude = (bool[,])Array.CreateInstance(typeof(bool), mTime.Count + 1, mMorningShift + 1);
            for (var i = 0; i < mTime.Count; i++)
            {
                var currentItem = mTime[i];
                var row = i + 1;
                for (var c = 1; c <= mMorningShift; c++)
                {
                    var valueExcluded = maxValues[row - 1, c];
                    var valueIncluded = 0;
                    if (currentItem <= c)
                        valueIncluded = currentItem + maxValues[row - 1, c - currentItem];

                    // Only use the current item if it results in a higher value:
                    if (valueIncluded > valueExcluded)
                    {
                        maxValues[row, c] = valueIncluded;
                        doInclude[row, c] = true;
                    }
                    else
                    {
                        maxValues[row, c] = valueExcluded;
                    }
                }
            }


            // Find out which items that actually constitutes the above found solution value...
            var chosenItems = new List();
            // Iterate backwards, starting from the last item...
            for (var row = mTime.Count; row > 0; row--)
            {
                // If the current item should not be included, just skip it and look at the next one:
                if (!doInclude[row, mMorningShift])
                    continue;

                // Otherwise, add it to the list of chosen items:
                var item = mTime[row - 1];
                chosenItems.Add(item);

                // Now, with that item chosen, the next sub-problem's capacity is correspondingly smaller:
                mMorningShift -= item;
            }
            return chosenItems;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}