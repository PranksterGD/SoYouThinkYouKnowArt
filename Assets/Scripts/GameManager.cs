using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class GameManager : MonoBehaviour {
    [Header("Painting Info")]
    public TextAsset XMLDocument1;
    public TextAsset XMLDocument2;
    public TextAsset XMLDocument3;
    [SerializeField]
    public PaintingContainer ListOfPaintings = new PaintingContainer();

    [Header("UI Elements")]
    public GameObject MainPainting;
    public GameObject Spectators;
    public GameObject PaintingChoices;
    public GameObject MemeChoices;
    public GameObject ProfessorMeter;
    public GameObject SpectatorMeter;
    public GameObject ChoicePreview;
    public GameObject Professor;
    public GameObject Curtain;
    public GameObject PowerPoint;
    public GameObject CountDownNumber;
    public GameObject ProfessorScoreText;
    public GameObject SpectatorScoreText;

    [Header("Gameplay Variables")]
    public int Counter;
    [Range(0,100)]
    public int ProfessorScore;
    [Range(0, 100)]
    public int SpectatorScore;
    public List<string> ArtistsTested = new List<string>();
    public List<string> PaintingsUsed = new List<string>();
    public int ArtistIndex = 0;

    [Range(10, 60)]
    public int CounterMaxTime;
    public List<Painting> PaintingOptions = new List<Painting>();
    public Painting CurrentPainting;
    public bool isSpeaking;
    public bool isGameOver;
    public int CurrentHint = 0;
    public int currentHintAudio = 1;
    public int randomReaction = 0;
 
    public bool[] HasSpectatorLeft = {false,false,false,false,false,false,false,false };
    
    public static int LevelSelected;
    public static int score;
    public int SubmissionIndex;

    [Header("Spectators")]
    public GameObject Nerd;
    public GameObject Foodie;
    public GameObject AnimalLover;
    public GameObject Normal;
    public GameObject Art;
    public GameObject Immature;
    public GameObject DumbBlond;

    public GameObject spectatorReference;

    private List <GameObject> spectatorArray = new List <GameObject>();
    private List <GameObject> spectatorsOnStageArray = new List<GameObject>();
    private GameObject newSpectator;
   
    private int spectatorsOnStage=0;
    private bool previousAnswerCorrect=false;
    private bool previousAnswerMeme=false;

    private const int totalNumberOfSpectators = 7;
    private const int maxSpectatorsOnStage = 4;
    private const int snowBallValue = 10;
    private const int professorCorrectArtReaction = 25;
    private const int professorWrongArtReaction = -10;
    private const int professorArtMemeReaction = -15;
    private const int professorOtherMemeReaction = -25;
    private const float spectatorPosisitionDivideValue = 300;
    private const int spectatorPoisitionOffset = 2;

    private const int spectator1Position = -575;
    private const int spectator2Position = -210;
    private const int spectator3Position =  230;
    private const int spectator4Position =  650;

    static public bool playerLose = false;

    [Header("Audio")]
    public AudioClip CountDownTick;
    public AudioClip CorrectAnswerBing;
    public AudioClip WrongAnswerBuzz;

    void Start () {
        //This is used for testing purposes if you don't want to have to go through the main menu all he time.
        if (LevelSelected == 0) {
            LevelSelected = 2;
        }

        spectatorArray.Add(Nerd);
        spectatorArray.Add(Foodie);
        spectatorArray.Add(AnimalLover);
        spectatorArray.Add(Normal);
        spectatorArray.Add(Art);
        spectatorArray.Add(Immature);
        spectatorArray.Add(DumbBlond);

        //for (int i = 0; i < totalNumberOfSpectators; i++)
        //{
        //    spectatorArray[i] = spectatorArray[i].GetComponent<Spectator>();
        //}

        spectatorsOnStage = 0;
        ProfessorScore = 50;
        SpectatorScore = 40;
        score = 0;
        switch (LevelSelected){
            case 1:
                ListOfPaintings = PaintingContainer.LoadFromText(XMLDocument1.text);
                break;
            case 2:
                ListOfPaintings = PaintingContainer.LoadFromText(XMLDocument2.text);
                break;
            case 3:
                ListOfPaintings = PaintingContainer.LoadFromText(XMLDocument3.text);
                break;
        }
        Counter = CounterMaxTime;
        PaintingOptions.Capacity = 4;
        SubmissionIndex = 5;
        loadRandomSpectators();
        BeginGame();
	}

    void loadRandomSpectators()
    {
        while(spectatorsOnStage<maxSpectatorsOnStage)
        {
            int randomSpectatorNumber = Random.Range(0, totalNumberOfSpectators);
            GameObject newSpectator= Instantiate(spectatorArray[randomSpectatorNumber]) as GameObject;
            newSpectator.transform.SetParent(spectatorReference.transform);
            newSpectator.transform.localScale = new Vector3(1,1,1);
            newSpectator.transform.localPosition = spectatorArray[randomSpectatorNumber].transform.localPosition;
            spectatorsOnStageArray.Add(newSpectator);
            spectatorsOnStageArray[spectatorsOnStage].GetComponent<Spectator>().transform.GetComponent<Animator>().CrossFadeInFixedTime("Spectator" + (spectatorsOnStage+1) + "_Arrive", 1);
            spectatorsOnStage++;
            }
            
     }

    void removeRandomSpectator()
    {
        Vector3 spectatorPosistion;
        int spectatorRemoveNumber = Random.Range(0, maxSpectatorsOnStage);
        spectatorPosistion= spectatorsOnStageArray[spectatorRemoveNumber].GetComponent<Spectator>().transform.localPosition;
        float spectatorXPosition = spectatorPosistion.x;
        spectatorsOnStageArray[spectatorRemoveNumber].GetComponent<Spectator>().transform.GetComponent<Animator>().CrossFadeInFixedTime("Spectator_Leave", 1);
        GameObject spectatorToDestroy=spectatorsOnStageArray[spectatorRemoveNumber];
        spectatorsOnStageArray.RemoveAt(spectatorRemoveNumber);
        Destroy(spectatorToDestroy);
        spectatorsOnStage--;
        addRandomSpectator(spectatorXPosition);
    }

    void addRandomSpectator(float spectatorXPosistion)
    {
        int spectatorRemoveNumber=0;
        int spectatorPosition = (int)spectatorXPosistion;
        switch (spectatorPosition)
        {
            case spectator1Position:
                spectatorRemoveNumber = 1;
                break;

            case spectator2Position:
                spectatorRemoveNumber = 2;
                break;

            case spectator3Position:
                spectatorRemoveNumber = 3;
                break;

            case spectator4Position:
                spectatorRemoveNumber = 4;
                break;
        }
        int randomSpectatorNumber = Random.Range(0, totalNumberOfSpectators);
        GameObject newSpectator = Instantiate(spectatorArray[randomSpectatorNumber]) as GameObject;
        newSpectator.transform.SetParent(spectatorReference.transform);
        newSpectator.transform.localScale = new Vector3(1, 1, 1);
        newSpectator.transform.localPosition = spectatorArray[randomSpectatorNumber].transform.localPosition;
        spectatorsOnStageArray.Add(newSpectator);
        spectatorsOnStageArray[spectatorsOnStage].GetComponent<Spectator>().transform.GetComponent<Animator>().CrossFadeInFixedTime("Spectator" + (spectatorRemoveNumber) + "_Arrive", 1);
        spectatorsOnStage++;

    }
    
    void Update() {
        ProfessorMeter.GetComponent<Slider>().value = Mathf.Lerp(ProfessorMeter.GetComponent<Slider>().value, ProfessorScore/100f, 0.01f);
        SpectatorMeter.GetComponent<Slider>().value = Mathf.Lerp(SpectatorMeter.GetComponent<Slider>().value, SpectatorScore / 100f, 0.01f);
        ProfessorScoreText.GetComponent<Text>().text = ProfessorScore.ToString();
        SpectatorScoreText.GetComponent<Text>().text = SpectatorScore.ToString();
    }

    void BeginGame(){ 
        for (int i = 0; i < 3; i++) {
            string newArtist;
            do {
                newArtist = ListOfPaintings.Paintings[Random.Range(0, ListOfPaintings.Paintings.Count)].Artist;
            } while (ArtistsTested.Contains(newArtist));
            ArtistsTested.Add(newArtist);
        }

        do {
            CurrentPainting = ListOfPaintings.Paintings[Random.Range(0, ListOfPaintings.Paintings.Count - 1)];
        } while (CurrentPainting.Artist != ArtistsTested[ArtistIndex] || PaintingsUsed.Contains(CurrentPainting.Name));
        PaintingsUsed.Add(CurrentPainting.Name);
        
        for (int i = 0; i < 4; i++) {
            Painting newPainting;
            do {
                newPainting = ListOfPaintings.Paintings[Random.Range(0, ListOfPaintings.Paintings.Count-1)];
            } while (CurrentPainting.Artist.Equals(newPainting.Artist) || PaintingOptions.Contains(newPainting));
            PaintingOptions.Insert(i, newPainting);
        }

        if (!PaintingOptions.Contains(CurrentPainting)) {
            PaintingOptions[Random.Range(0, 3)] = CurrentPainting;
        }

        for(int i = 0; i < 4; i++) {
            PaintingChoices.transform.GetChild(i).GetComponent<Image>().sprite = Resources.Load("Paintings/" + PaintingOptions[i].Name, typeof(Sprite)) as Sprite;
        }

        PaintingChoices.GetComponent<Animator>().Play("PaintingOptions_Up");
        MemeChoices.GetComponent<Animator>().Play("MemeOptions_Up");

        GetComponent<AudioSource>().PlayOneShot(Resources.Load("Hints/Introduction " + ArtistsTested[0], typeof(AudioClip)) as AudioClip);
        StartCoroutine(WriteHint("I will be talking about the artist WhatsHisFace"));

        PowerPoint.GetComponentInChildren<Text>().text += "\n\u2022 " + CurrentPainting.Hints[CurrentHint];
        StartCoroutine("DisplayHint");
    }

    public void SubmitAnswer(int AnswerIndex) {
        if (!isGameOver) {
            CountDownNumber.SetActive(false);
            StopCoroutine("CountDown");
            isGameOver = true;
            PaintingChoices.GetComponent<Animator>().Play("PaintingOptions_Down");
            MemeChoices.GetComponent<Animator>().Play("MemeOptions_Down");
            GetComponent<AudioSource>().Stop();
            if (PaintingOptions[AnswerIndex].Equals(CurrentPainting)){
                randomReaction = Random.Range(1, 4);
                GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Hints/Correct" + randomReaction));
                WinGame();
            } else {
                randomReaction = Random.Range(1, 4);
                GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Hints/Messup" + randomReaction));
                LoseGame();
            }
            MainPainting.GetComponent<Animator>().Play("MainPainting_Reveal");
        }
    }

    public void SubmitMeme(int AnswerIndex) {
        if (!isGameOver) {
            CountDownNumber.SetActive(false);
            StopCoroutine("CountDown");
            PowerPoint.SetActive(false);
            isGameOver = true;
            randomReaction = Random.Range(1, 4);
            PaintingChoices.GetComponent<Animator>().Play("PaintingOptions_Down");
            MemeChoices.GetComponent<Animator>().Play("MemeOptions_Down");
            GetComponent<AudioSource>().Stop();
            GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Hints/Meme " + randomReaction));
            MainPainting.GetComponent<Image>().sprite = Resources.Load("Memes/" + ((MemeType)AnswerIndex).ToString() + Random.Range(1, 13).ToString(), typeof(Sprite)) as Sprite;
            MainPainting.GetComponent<Animator>().Play("MainPainting_Reveal");
            StopCoroutine("WriteHint");
            StopCoroutine("DisplayHint");
            StartCoroutine("WriteHint", "");
            if (!previousAnswerMeme) {
                Professor.GetComponentInChildren<Animator>().CrossFade("surprised", Time.deltaTime);
            } else {
                Professor.GetComponentInChildren<Animator>().CrossFade("negative_B", Time.deltaTime);
            }
            StartCoroutine("ResetGame");
            //SCORING
            //  UpdateScores(-25, 25);
            StartCoroutine(updateMemeScoreSpectator(AnswerIndex));
            StartCoroutine(updateMemeScoreProfessor(AnswerIndex, previousAnswerMeme));
            previousAnswerMeme = true;
        }
    }

    public void WinGame() {
        GetComponent<AudioSource>().PlayOneShot(CorrectAnswerBing);
        PaintingChoices.GetComponent<Animator>().CrossFade("PaintingOptions_Down",Time.deltaTime);
        MemeChoices.GetComponent<Animator>().CrossFade("MemeOptions_Down",Time.deltaTime);
        PowerPoint.SetActive(false);
        MainPainting.GetComponent<Image>().sprite = Resources.Load("Paintings/"+CurrentPainting.Name, typeof (Sprite)) as Sprite;
        StopCoroutine("WriteHint");
        StopCoroutine("DisplayHint");
        StartCoroutine("WriteHint","");
        Professor.GetComponentInChildren<Animator>().CrossFade("positive", Time.deltaTime);
        StartCoroutine("ResetGame");
        //SCORING
        StartCoroutine(updatePaintingScoreSpectator(true));
        StartCoroutine(updatePaintingScoreProfessor(true, previousAnswerCorrect));
        previousAnswerCorrect = true;
        previousAnswerMeme = false;
    }

    public void LoseGame() {
        GetComponent<AudioSource>().PlayOneShot(WrongAnswerBuzz);
        PaintingChoices.GetComponent<Animator>().CrossFade("PaintingOptions_Down", Time.deltaTime);
        MemeChoices.GetComponent<Animator>().CrossFade("MemeOptions_Down", Time.deltaTime);
        PowerPoint.SetActive(false);
        MainPainting.GetComponent<Image>().sprite = Resources.Load("Paintings/" + CurrentPainting.Name, typeof(Sprite)) as Sprite;
        StopCoroutine("WriteHint");
        StopCoroutine("DisplayHint");
        StartCoroutine("WriteHint", "");
        Professor.GetComponentInChildren<Animator>().CrossFade("negative", Time.deltaTime);
        StartCoroutine("ResetGame");
        //SCORING
        StartCoroutine(updatePaintingScoreSpectator(false));
        StartCoroutine(updatePaintingScoreProfessor(false, previousAnswerCorrect));
        previousAnswerCorrect = false;
        previousAnswerMeme = false;
    }

    public void DisplayPreview(int choice) {
        ChoicePreview.GetComponent<Image>().sprite = PaintingChoices.transform.GetChild(choice).GetComponent<Image>().sprite;
    }

    public void MemePreview(int choice) {
        ChoicePreview.GetComponent<Image>().sprite = MemeChoices.transform.GetChild(choice).GetComponent<Image>().sprite;
    }

    IEnumerator updateMemeScoreSpectator(int memeNumber)
    {
        SpectatorScore = 0;
        switch (memeNumber)
        {
            case 0:
                  foreach (GameObject spectator in spectatorsOnStageArray)
                {
                    spectator.GetComponent<Spectator>().score += spectator.GetComponent<Spectator>().animalMemeReaction;
                    SpectatorScore += spectator.GetComponent<Spectator>().score;
                }
                break;

            case 1:
                foreach (GameObject spectator in spectatorsOnStageArray)
                {
                    spectator.GetComponent<Spectator>().score += spectator.GetComponent<Spectator>().nerdMemeReaction;
                    SpectatorScore += spectator.GetComponent<Spectator>().score;
                }
                break;

            case 2:
                foreach (GameObject spectator in spectatorsOnStageArray)
                {
                    spectator.GetComponent<Spectator>().score += spectator.GetComponent<Spectator>().foodMemeReaction;
                    SpectatorScore += spectator.GetComponent<Spectator>().score;
                }
                break;

            case 3:
                foreach (GameObject spectator in spectatorsOnStageArray)
                {
                    spectator.GetComponent<Spectator>().score += spectator.GetComponent<Spectator>().artMemeReaction;
                    SpectatorScore += spectator.GetComponent<Spectator>().score;
                }
                break;
        }

        if (SpectatorScore == 0) {
            Professor.GetComponentInChildren<Animator>().CrossFade("pulling", Time.deltaTime);
            Curtain.GetComponent<Animator>().Play("DropDownCurtain_Down");
            playerLose = true;
            yield return new WaitForSeconds(3);
            SceneManager.LoadScene(2);
        }

    }

    IEnumerator updatePaintingScoreSpectator(bool correctAnswer)
    {
        SpectatorScore = 0;
        switch (correctAnswer)
        {
            case true:
                foreach (GameObject spectator in spectatorsOnStageArray)
                {
                    spectator.GetComponent<Spectator>().score += spectator.GetComponent<Spectator>().correctArtReaction;
                    SpectatorScore += spectator.GetComponent<Spectator>().score;
                }
                break;

            case false:
                foreach (GameObject spectator in spectatorsOnStageArray)
                {
                    spectator.GetComponent<Spectator>().score += spectator.GetComponent<Spectator>().wrongArtReaction;
                    SpectatorScore += spectator.GetComponent<Spectator>().score;
                }
                break;
        }

        if (SpectatorScore == 0) {
            Professor.GetComponentInChildren<Animator>().CrossFade("pulling", Time.deltaTime);
            Curtain.GetComponent<Animator>().Play("DropDownCurtain_Down");
            playerLose = true;
            yield return new WaitForSeconds(3);
            SceneManager.LoadScene(2);
        }
    }

    IEnumerator updatePaintingScoreProfessor(bool CorrectAnswer, bool previousAnswerCorrect)
    {
        if(CorrectAnswer==true && previousAnswerCorrect==true)
        {
            ProfessorScore += professorCorrectArtReaction + snowBallValue;
        }
        else
            if(CorrectAnswer==true && previousAnswerCorrect==false)
        {
            ProfessorScore += professorCorrectArtReaction;
        }
        else 
            if(CorrectAnswer==false)
        {
            ProfessorScore += professorWrongArtReaction;
        }

        if (ProfessorScore > 100)
        {
            ProfessorScore = 100;
        }

        if (ProfessorScore < 0)
        {
            ProfessorScore = 0;
        }

        if (ProfessorScore == 0) {
            Professor.GetComponentInChildren<Animator>().CrossFade("pulling", Time.deltaTime);
            Curtain.GetComponent<Animator>().Play("DropDownCurtain_Down");
            playerLose = true;
            yield return new WaitForSeconds(3);
            SceneManager.LoadScene(2);
        }
    }

   IEnumerator updateMemeScoreProfessor(int AnswerIndex,bool previousAnswerMeme)
    {
        if(AnswerIndex==0)
        {
            ProfessorScore += professorArtMemeReaction;
        }
        else
        {
            ProfessorScore += professorOtherMemeReaction;
        }
        if(previousAnswerMeme==true)
        {
            ProfessorScore -= snowBallValue;
        }

        if (ProfessorScore > 100)
        {
            ProfessorScore = 100;
        }

        if (ProfessorScore < 0)
        {
            ProfessorScore = 0;
        }

        if (ProfessorScore == 0)
        {
            Professor.GetComponentInChildren<Animator>().CrossFade("pulling", Time.deltaTime);
            Curtain.GetComponent<Animator>().Play("DropDownCurtain_Down");
            playerLose = true;
            yield return new WaitForSeconds(3);
            SceneManager.LoadScene(2);
        }
    }

    IEnumerator CountDown(int seconds) {
        yield return new WaitForSeconds(8);
        CountDownNumber.SetActive(true);
        for (int i = seconds; i >= 1; i--) {
            GetComponent<AudioSource>().PlayOneShot(CountDownTick);
            CountDownNumber.GetComponent<Text>().text = i.ToString();
            yield return new WaitForSeconds(1);
        }
        MainPainting.GetComponent<Animator>().Play("MainPainting_Reveal");
        CountDownNumber.SetActive(false);
        LoseGame();
    }

    IEnumerator DisplayHint() {
        PowerPoint.SetActive(true);
        yield return new WaitForSeconds(2);
        if (!isSpeaking) {
            GetComponent<AudioSource>().Stop();
            GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Hints/" + CurrentPainting.Artist + " " + currentHintAudio));
            StartCoroutine("WriteHint", CurrentPainting.Hints[CurrentHint]);
            StartCoroutine("CountDown", 10);
        } else {
            StartCoroutine("DisplayHint");
        }
    }

    IEnumerator WriteHint(string Hint) {
        isSpeaking = true;
        Professor.GetComponentInChildren<Animator>().CrossFade("Talking", Time.deltaTime);

        foreach (char letter in Hint) {
            yield return new WaitForSeconds(0.04f);
        }
        Professor.GetComponentInChildren<Animator>().CrossFade("idle", Time.deltaTime);
        yield return new WaitForSeconds(3);
        isSpeaking = false;
    }

    IEnumerator ResetGame() {
        yield return new WaitForSeconds(3);
        MainPainting.GetComponent<Animator>().Play("MainPainting_Hide");

        yield return new WaitForSeconds(2);

        if (CurrentHint + 1 < 3) {
            CurrentHint++;
            currentHintAudio++;
            PowerPoint.GetComponentInChildren<Text>().text += "\n\n\u2022 " + CurrentPainting.Hints[CurrentHint];
        } else {
            CurrentHint = 0;
            currentHintAudio = 1;
            ArtistIndex++;
            if (ArtistIndex < 3) {
                StartCoroutine("WriteHint","I will be talking about the artist WhatsHisFace");
                GetComponent<AudioSource>().PlayOneShot(Resources.Load("Hints/Introduction " + ArtistsTested[ArtistIndex], typeof(AudioClip)) as AudioClip);
            }
            if (ArtistIndex<3){
                removeRandomSpectator();
            }
        }

        if (ArtistIndex == 3) {
            Professor.GetComponentInChildren<Animator>().CrossFade("pulling", Time.deltaTime);
            Curtain.GetComponent<Animator>().Play("DropDownCurtain_Down");
            yield return new WaitForSeconds(3);
            score = SpectatorScore + ProfessorScore;
            SceneManager.LoadScene(2);
            yield return null;
        }

        do {
            CurrentPainting = ListOfPaintings.Paintings[Random.Range(0, ListOfPaintings.Paintings.Count - 1)];
        } while (CurrentPainting.Artist != ArtistsTested[ArtistIndex] || PaintingsUsed.Contains(CurrentPainting.Name));
        PaintingsUsed.Add(CurrentPainting.Name);

        PaintingOptions.Clear();
        for (int i = 0; i < 4; i++) {
            Painting newPainting;
            do {
                newPainting = ListOfPaintings.Paintings[Random.Range(0, ListOfPaintings.Paintings.Count - 1)];
            } while (CurrentPainting.Artist.Equals(newPainting.Artist) || PaintingOptions.Contains(newPainting));
            PaintingOptions.Insert(i, newPainting);
        }

        if (!PaintingOptions.Contains(CurrentPainting)) {
            PaintingOptions[Random.Range(0, 3)] = CurrentPainting;
        }

        for (int i = 0; i < 4; i++) {
            PaintingChoices.transform.GetChild(i).GetComponent<Image>().sprite = Resources.Load("Paintings/" + PaintingOptions[i].Name, typeof(Sprite)) as Sprite;
        }

        isGameOver = false;
        Counter = CounterMaxTime;
        SubmissionIndex = 5;
        ProfessorMeter.transform.GetChild(0).GetComponent<Image>().fillAmount = 1;
        PaintingChoices.GetComponent<Animator>().Play("PaintingOptions_Up");
        MemeChoices.GetComponent<Animator>().Play("MemeOptions_Up");
        if (CurrentHint == 0) {
            PowerPoint.GetComponentInChildren<Text>().text = "";
            PowerPoint.GetComponentInChildren<Text>().text += "\n\u2022 " + CurrentPainting.Hints[CurrentHint];
        }
        StartCoroutine("DisplayHint");
    }
}

public enum MemeType {
    Animal = 0,
    Nerd = 1,
    Food = 2,
    Art = 3
}

[System.Serializable]
public class Painting {
    [XmlAttribute("name")]
    public string Name;
    public string Period;
    public string Artist;
    public string[] Hints;
    public Painting() {
    }
}

[System.Serializable]
[XmlRoot("PaintingCollection")]
public class PaintingContainer {
    [XmlArray("Paintings")]
    [XmlArrayItem("Painting")]
    [SerializeField]
    public List<Painting> Paintings = new List<Painting>();

    public PaintingContainer() {

    }

    public static PaintingContainer LoadFromText(string text) {
        var serializer = new XmlSerializer(typeof(PaintingContainer));
        return serializer.Deserialize(new StringReader(text)) as PaintingContainer;
    }
}