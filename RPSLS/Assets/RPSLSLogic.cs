using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum Gesture
{
    Rock,
    Paper,
    Scissors,
    Lizard,
    Spock
}

public class RPSLSLogic : MonoBehaviour
{
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI computerResultText;
    public TextMeshProUGUI playerScore;
    public GameObject RPSLSHolder;
    public Button[] gestureButtons;
    public Button playButton;

    private int score = 0;


    //Using a hash set here, so as to have faster look up times and eliminate the need to perform linear searches
    private static Dictionary<Gesture, HashSet<Gesture>> winConditions = new Dictionary<Gesture, HashSet<Gesture>>()
    {
        { Gesture.Rock, new HashSet<Gesture> { Gesture.Scissors, Gesture.Lizard } },
        { Gesture.Paper, new HashSet<Gesture> { Gesture.Rock, Gesture.Spock } },
        { Gesture.Scissors, new HashSet<Gesture> { Gesture.Paper, Gesture.Lizard } },
        { Gesture.Lizard, new HashSet<Gesture> { Gesture.Paper, Gesture.Spock } },
        { Gesture.Spock, new HashSet<Gesture> { Gesture.Rock, Gesture.Scissors } }
    };

    private void Start()
    {
        score = 0;

        for (int i = 0; i < gestureButtons.Length; i++)
        {
            int index = i;
            gestureButtons[index].onClick.AddListener(() => PlayGesture((Gesture)index));
        }
    }

    private void PlayGesture(Gesture gesture)
    {
        Gesture computerGesture = (Gesture)Random.Range(0, 5);

        DetermineWinner(gesture, computerGesture);
        DisplayComputerGesture(computerGesture);
    }

    private void DetermineWinner(Gesture playerGesture, Gesture computerGesture)
    {
        if (playerGesture == computerGesture)
        {
            resultText.text = "It's a tie!";
        }
        else if (winConditions[playerGesture].Contains(computerGesture))
        {
            resultText.text = "Player wins!";
            score++;
            playerScore.text = score.ToString();
        }
        else
        {
            resultText.text = "Computer wins!";
            StartCoroutine(GoBacktoMenu());
        }
    }

    private void DisplayComputerGesture(Gesture gesture)
    {
        computerResultText.text = "Computer chose: " + gesture.ToString();
    }

    IEnumerator GoBacktoMenu()
    {
        yield return new WaitForSeconds(1);
        score = 0;
        playerScore.text = score.ToString();
        RPSLSHolder.SetActive(false);
        playButton.gameObject.SetActive(true);
    }
}
