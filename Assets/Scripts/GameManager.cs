using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    // The 9 buttons on the board
    public Button[] cells;

    // The text inside each button (X or O)
    private TMP_Text[] cellTexts;

    // UI text to show whose turn it is or who won
    public TMP_Text infoText;

    // Restart button
    public Button restartButton;

    private string currentPlayer = "X"; // Start with X
    private string[] board = new string[9]; // Track the board state
    private bool gameOver = false; // Is the game finished?

    void Start()
    {
        // Set up each button and its text
        cellTexts = new TMP_Text[cells.Length];

        for (int i = 0; i < cells.Length; i++)
        {
            cellTexts[i] = cells[i].GetComponentInChildren<TMP_Text>();
            cellTexts[i].text = ""; // Make sure it's empty at the start

            int index = i; // local copy for the click listener
            cells[i].onClick.AddListener(() => CellClicked(index));
        }

        // Hide restart button at first
        restartButton.gameObject.SetActive(false);
        restartButton.onClick.AddListener(ResetGame);

        // Show initial turn
        infoText.text = "Turn: " + currentPlayer;
    }

    void CellClicked(int index)
    {
        if (board[index] != null || gameOver)
            return; // ignore clicks on filled cells or if game is over

        // Mark the cell
        board[index] = currentPlayer;
        cellTexts[index].text = currentPlayer;
        cells[index].interactable = false; // prevent clicking again

        // Check if someone won
        if (CheckWin())
        {
            infoText.text = currentPlayer + " Wins!";
            gameOver = true;
            restartButton.gameObject.SetActive(true);
            return;
        }

        // Check for draw
        if (IsBoardFull())
        {
            infoText.text = "It's a Draw!";
            gameOver = true;
            restartButton.gameObject.SetActive(true);
            return;
        }

        // Switch turn
        currentPlayer = (currentPlayer == "X") ? "O" : "X";
        infoText.text = "Turn: " + currentPlayer;
    }

    bool CheckWin()
    {
        int[,] wins = new int[,]
        {
            {0,1,2}, {3,4,5}, {6,7,8}, // rows
            {0,3,6}, {1,4,7}, {2,5,8}, // columns
            {0,4,8}, {2,4,6}           // diagonals
        };

        for (int i = 0; i < wins.GetLength(0); i++)
        {
            if (board[wins[i,0]] == currentPlayer &&
                board[wins[i,1]] == currentPlayer &&
                board[wins[i,2]] == currentPlayer)
            {
                return true;
            }
        }
        return false;
    }

    bool IsBoardFull()
    {
        foreach (string cell in board)
            if (cell == null) return false;
        return true;
    }

    void ResetGame()
    {
        for (int i = 0; i < board.Length; i++)
        {
            board[i] = null;
            cellTexts[i].text = "";
            cells[i].interactable = true;
        }

        currentPlayer = "X";
        gameOver = false;
        infoText.text = "Turn: " + currentPlayer;
        restartButton.gameObject.SetActive(false);
    }
}
