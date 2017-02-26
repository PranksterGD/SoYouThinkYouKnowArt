using UnityEngine;
using System.Collections;

public class Spectator : MonoBehaviour {

    public int correctArtReaction;
    public int wrongArtReaction;
    public int artMemeReaction;
    public int nerdMemeReaction;
    public int foodMemeReaction;
    public int animalMemeReaction;


    public int score
    {
    get
        {
            return mScore;
        }

       set
        {
            if (value > 25)
            {
                mScore = 25;
            }       
            else
            if( value< 0)
            {
                mScore = 0;
            }
            else
            {
                mScore = value;
            }
        }
    }
    private int mScore;

    public Spectator()
        {
        score = 10;
         
        }
    

   
}
