using System;

public class GameStarter
{
    public static void Main(String[] args)
    {
        TicTacToe game = new TicTacToe();
        game.play();

    }
}
public class TicTacToe
{
    string nameOfPlayer1;
    string nameOfPlayer2;
    bool shouldGenerateRandomID;
    int boardSize;
    int rowPlacement; // Row index of a single placement
    int colPlacement; // Column index of a single placement
    int numberOfPlacements; // Total number of placements during the game
    int numberOfWinsByPlayer1;
    int numberOfWinsByPlayer2;

    private Player player1;
    private Player player2;
    private GameBoard gameBoard;

    /*
        "The value x of rows[1][1]" indicates:
            - Player 1 has made x placements on row 1
            - So if rows[i][j] = boardSize, we know that player i has won
            - Columns and diagonals have just the same logics
     */
    private int[,] rows;
    private int[,] columns;
    private int[,] diagonals;

    public TicTacToe()
    {

        nameOfPlayer1 = "";
        nameOfPlayer2 = "";
        shouldGenerateRandomID = false;
        boardSize = 3;
        rowPlacement = -1;
        colPlacement = -1;
        numberOfPlacements = 0;
        numberOfWinsByPlayer1 = 0;
        numberOfWinsByPlayer2 = 0;

        this.prepare();
        this.getPlayerNames();

        this.player1 = new Player(nameOfPlayer1);
        this.player2 = new Player(nameOfPlayer2);
        this.gameBoard = new GameBoard();

        this.rows = new int[3, boardSize];
        this.columns = new int[3, boardSize];
        this.diagonals = new int[3, 3];
    }

    /*
        This is the chief method of the class
     */
    public void play()
    {
        int playerID = 0, winnerID = 0;
        shouldGenerateRandomID = true;

        while (true)
        {
            // Only when we start a new game should we decide which player to begin
            if (shouldGenerateRandomID)
            {
                playerID = this.idGenerator();
                shouldGenerateRandomID = false;
            }

            playerID = this.playerTurn(playerID); // Players take turn to play
            ++numberOfPlacements;
            winnerID = checkWinner(playerID);

            if (winnerID == 1)
            {
                NotificationCenter.winnerCongratulations(player2.name);
                ++numberOfWinsByPlayer2;
            }
            else if (winnerID == 2)
            {
                NotificationCenter.winnerCongratulations(player1.name);
                ++numberOfWinsByPlayer1;
            }
            else if ((numberOfPlacements == boardSize * boardSize) && (winnerID == -1))
            {
                NotificationCenter.stalemateAnnouncement();
            }
            else
            {
                continue; // IMPORTANT! If no winner generated, skip the code below.
            }

            this.gameBoard.PrintBoard(); // Update and print the current game board
            playAgainOrExit(); // After a round has finished, decide to play again or exit
        }
    }

    /*
        BEFORE the game -> Display welcome messages and other important notifications
     */
    private void prepare()
    {
       
            NotificationCenter.welcome();
        try
        {

            string DECISION = Console.ReadLine();
            if (DECISION == null)
            {
                throw new ArgumentException("Felaktigt val"); 
            }
            int code = NotificationCenter.startOrExit(DECISION);
            if (code == 0) { Environment.Exit(0); }
        }
        catch(ArgumentException e)
        {
            prepare();
        }
        
    }

    /*
        DURING the game -> Ask for the names of the two players and their preferred board size
     */
    public void getPlayerNames()
    {
        NotificationCenter.namesAndSize(1);
        nameOfPlayer1 = Console.ReadLine();
        NotificationCenter.namesAndSize(2);
        nameOfPlayer2 = Console.ReadLine();
    }

    ///*
    //    DURING the game -> Choose a player to begin at random
    // */
    private int idGenerator()
    {
        Random seed = new Random();
        int id;
        id = seed.Next(1, 3);

        return id;
    }

    /*
        DURING the game -> Let the designated player play
     */
    private int playerTurn(int playerID)
    {
        if (playerID == 1)
        {
            Checker checker1 = new Checker('X');
            this.player1.move(this.gameBoard, checker1);
            rowPlacement = this.player1.rowIndex;
            colPlacement = this.player1.colIndex;
            playerID = 2; // Player should take turns to play
        }
        else
        {
            Checker checker2 = new Checker('O');
            this.player2.move(this.gameBoard, checker2);
            rowPlacement = this.player2.rowIndex;
            colPlacement = this.player2.colIndex;
            playerID = 1; // Player should take turns to play
        }

        return playerID;
    }

    ///*
    //    DURING the game -> Check if there is a winner generated
    // */
    private int checkWinner(int playerID)
    {
        BoardCell[,] board = gameBoard.getBoard();
        string diag = "";
        string diag2 = "";
        for (int i = 0; i < 3; i++)
        {
            string row = "";
            string col = "";
            for (int j = 0; j < 3; j++)
            {
                row += board[i, j].getChecker();
                col += board[j, i].getChecker();
                if (i == j)
                {
                    diag += board[i, j].getChecker();
                }
                //0,2 1,1 2,0
                if (j == 2 - i)
                {
                    diag2 += board[i, j].getChecker();
                }
  
            }

            if (row == "XXX" || row == "OOO" || col == "XXX" || col == "OOO")
            {
                return playerID;
                // Här kan du kolla om rad eller col == 1 -> player 1 vinner, om rad eller col == 8 -> player 2 vinner   
            }


            if (diag == "XXX" || diag == "OOO"||diag2=="XXX"||diag2=="OOO")
            {
                return playerID;
            }
           
        }
        return -1;
    }

    // här kan du kolla om diag == 1 eller diag == 8



    //        if (this.rows[0,0] == playerID && this.rows[0, 1] == playerID && this.rows[0, 2] == playerID) { return playerID; }
    //if (this.rows[1, 0] == playerID && this.rows[1, 1] == playerID && this.rows[1, 2] == playerID) { return playerID; }
    //if (this.rows[2, 0] == playerID && this.rows[2, 1] == playerID && this.rows[2, 2] == playerID) { return playerID; }

    //// check columns
    //if (this.columns[0, 0] == playerID && this.columns[1, 0] == playerID && this.columns[2, 0] == playerID) { return playerID; }
    //if (this.columns[0, 1] == playerID && this.columns[1, 1] == playerID && this.columns[2, 1] == playerID) { return playerID; }
    //if (this.columns[0, 2] == playerID && this.columns[1, 2] == playerID && this.columns[2, 2] == playerID) { return playerID; }

    //// check diags
    //if (this.diagonals[0, 0] == playerID && this.diagonals[1, 1] == playerID && this.diagonals[2, 2] == playerID) { return playerID; }
    //if (this.diagonals[0, 2] == playerID && this.diagonals[1, 1] == playerID && this.diagonals[2, 0] == playerID) { return playerID; }

    //return -1;
    //if (this.rows[playerID,row] == boardSize)
    //{
    //    return playerID;
    //}
    //if (this.columns[playerID,col] == boardSize)
    //{
    //    return playerID;
    //}
    //if (row == col && this.diagonals[playerID,0] == boardSize)
    //{
    //    return playerID;
    //}
    //if ((row + col == boardSize - 1) && this.diagonals[playerID,1] == boardSize)
    //{
    //    return playerID;
    //}

    //return -1;


///*
//    AFTER the game -> Let players decide to have another round or just exit the game
// */
private void playAgainOrExit()
{
    string userDecision;

    NotificationCenter.newGamePrompt();


    userDecision = Console.ReadLine();

    if (userDecision.Equals("Y", StringComparison.InvariantCultureIgnoreCase))
    {
        // Players decide to play again
        NotificationCenter.startOrExit("Y");

        // Reset all the member variables
        shouldGenerateRandomID = true;
        numberOfPlacements = 0;

        this.gameBoard = new GameBoard();
        this.rows = new int[3, boardSize];
        this.columns = new int[3, boardSize];
        this.diagonals = new int[3, 2];
    }
    else
    {
        // Players decide to exit the game
        NotificationCenter.printSummaryResults(numberOfWinsByPlayer1, player1.name,
                                               numberOfWinsByPlayer2, player2.name);
        NotificationCenter.startOrExit("Exit");
        Environment.Exit(0);
    }
}
}

//string display = " ";

//for (int row = 0; row < test.getBoard().GetLength(0); row++)
//{
//    for (int column = 0; column < test.getBoard()[row].Length; column++)
//    {
//        display += test.getBoard()[row][column].ToString() + " ";
//    }
//    display += "\n";
//}
//Console.WriteLine(display);
public class BoardCell
{
    char checker;

    public BoardCell() { }
    public BoardCell(Checker checker)
    {
        this.checker = checker.CheckSign;
    }
    public void setChecker(Checker checker)
    {
        this.checker = checker.CheckSign;
    }
    public char getChecker()
    {
        return checker;
    }
}
public class Checker

{
    public Checker() { }
    public Checker(char checkSign)
    {
        CheckSign = checkSign;
    }

    public char CheckSign { get; private set; }

}
public class GameBoard
{
    public int boardSize { get; private set; } = 3;
    BoardCell[,] board;

    public GameBoard()
    {
        this.board = new BoardCell[3, 3];
        ;
        this.SetBoard(this.boardSize);
    }
   

    public void SetBoard(int boardSize)
    {
        BoardCell[,] row = new BoardCell[boardSize, boardSize];
        for (int i = 0; i < row.GetLength(0); i++)
        {
            for (int j = 0; j < row.GetLength(1); j++)
            {
                this.board[i, j] = new BoardCell(new Checker(' '));

            }
        }

    }
    public BoardCell[,] getBoard()
    {
        return this.board;
    }
    public void PrintBoard()
    {
        Console.WriteLine($"+___+___+___+");
        Console.WriteLine($"| {board[0, 0].getChecker()} | {board[0, 1].getChecker()} | {board[0, 2].getChecker()} |");
        Console.WriteLine($"+___+___+___+");
        Console.WriteLine($"| {board[1, 0].getChecker()} | {board[1, 1].getChecker()} | {board[1, 2].getChecker()} |");
        Console.WriteLine($"+___+___+___+");
        Console.WriteLine($"| {board[2, 0].getChecker()} | {board[2, 1].getChecker()} | {board[2, 2].getChecker()} |");
        Console.WriteLine($"+___+___+___+");

    }
    public bool isValidPosition(int position)
    {
        if (position < 1 || position > (this.boardSize * this.boardSize)) { return false; }

        int count = 1, i = 0, j = 0;
        bool flag = true;

        for (i = 0; i < 3; ++i)
        {
            for (j = 0; j < 3; ++j)
            {
                if (count == position)
                {
                    flag = false; break;
                }
                ++count;
            }

            if (!flag) { break; }
        }

        if (board[i, j].getChecker() != ' ') { return false; }

        return true;

    }
}
public class Player
{
    public string name { get; private set; }
    public int rowIndex { get; private set; } // Row index of a single placement
    public int colIndex { get; private set; } // Column index of a single placement
    public string input = " ";
    private Player() { }

    public Player(string name)
    {
        this.name = name;
        rowIndex = -1;
        colIndex = -1;

    }
    /*
        A player can move a checker to place it on the board
     */
    public void move(GameBoard board, Checker checker)
    {
        int movePos;
        while (true)
        {
            NotificationCenter.boardPlacement(1, this.name, board.boardSize);
            board.PrintBoard();

            if (int.TryParse(Console.ReadLine(), out int input))
            {

                movePos = input;

                if (!board.isValidPosition(movePos))
                {
                    // Input is an integer, but it leads to an illegal position on the board
                    NotificationCenter.boardPlacement(3, this.name, board.boardSize);
                    continue;
                }
                else
                {
                    // Input is a valid integer which points to an open vacancy on the board
                    break;
                }
            }
            else
            {
                // Input is not an integer

                NotificationCenter.boardPlacement(2, this.name, board.boardSize);
            }
        }

        // Place the move to the board
        placeTheMove(board, checker, movePos);
    }

    /*
        Place the move that a player made to the game board
     */
    public void placeTheMove(GameBoard gameBoard, Checker checker, int movePosition)
    {
        int count = 1, i = 0, j = 0;
        bool flag = true;

        for (i = 0; i < gameBoard.getBoard().Length; ++i)
        {
            for (j = 0; j < 3; ++j)
            {
                if (count == movePosition)
                {
                    gameBoard.getBoard()[i, j].setChecker(checker);
                    flag = false;
                    break;
                }
                ++count;
            }

            if (!flag) { break; }
        }

        this.rowIndex = i;
        this.colIndex = j;
    }
}
public class NotificationCenter
{
    private NotificationCenter() { }

    public static void welcome()
    {
        Console.WriteLine("Welcome to Tic Tac Toe! Please be noticed of the followings before our game starts:");

        Console.WriteLine();
        Console.WriteLine("    1. Make sure you are playing this game with one and only one of your friends.");
        Console.WriteLine("    2. The system will randomly decide which one of you two to begin first.");
        Console.WriteLine("    3. You can choose the size of the board from 3x3 to 10x10.");
        Console.WriteLine();

        Console.WriteLine("Hit \"y/Y\" to start the game. Or hit any other key to exit.");
    }

    public static int startOrExit(string message)
    {
        if (message.Equals("y", StringComparison.InvariantCultureIgnoreCase))
        {
            Console.WriteLine("**************************************************");
            Console.WriteLine("Game starts! May the odds be ever in your favor :)");
            Console.WriteLine("**************************************************");
            Console.WriteLine();
            return 1;
        }
        else
        {
            Console.WriteLine("**********************************************");
            Console.WriteLine("Game exits:( Take care and come back anytime!");
            Console.WriteLine("**********************************************");
            return 0;
        }
    }

    public static void namesAndSize(int index)
    {
        switch (index)
        {
            case 1:
                Console.Write("Please enter YOUR name: ");
                break;
            case 2:
                Console.Write("Please enter YOUR FRIEND'S name: ");
                break;
            case 3:
                Console.Write("Please enter your preferred SIZE of the board");
                Console.WriteLine(" (from 3 to 10. 3 -> 3x3; 4 -> 4x4; 10 -> 10x10, etc): ");
                break;
            case 4:
                Console.Write("------------------------------------------------------");
                Console.WriteLine("------------------------------------------");
                Console.Write("Invalid input! Please enter a valid SIZE from 3 to 10! ");
                Console.WriteLine("(3 -> 3x3; 4 -> 4x4; 10 -> 10x10, etc)");
                Console.Write("------------------------------------------------------");
                Console.WriteLine("------------------------------------------");
                Console.WriteLine();
                break;
            case 5:
                Console.WriteLine("----------------------------------");
                Console.WriteLine("Invalid input! Must be an INTEGER!");
                Console.WriteLine("----------------------------------");
                Console.WriteLine();
                break;
            default:
                Console.WriteLine("Index can only be 1/2/3/4/5!");
                break;
        }
    }

    public static void boardPlacement(int index, String playerName, int boardSize)
    {
        switch (index)
        {
            case 1:
                Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
                Console.Write("Player " + playerName + ", ");
                Console.WriteLine("please enter your move. (enter a value from 1 - " + boardSize * boardSize + ")");
                Console.WriteLine("Example: 1 (means: cell[1, 1]); 3 (means: cell[1, 3])");
                Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
                Console.WriteLine();
                break;
            case 2:
                Console.WriteLine("----------------------------------");
                Console.WriteLine("Invalid input! Must be an INTEGER!");
                Console.WriteLine("----------------------------------");
                break;
            case 3:
                Console.WriteLine();
                Console.WriteLine("-------------------------------------------------------");
                Console.WriteLine("Input out of range or position taken! Please try again.");
                Console.WriteLine("-------------------------------------------------------");
                break;
            default:
                Console.WriteLine("Index for boardPlacement() must be 1/2/3!");
                break;
        }
    }

    public static void winnerCongratulations(String winnerName)
    {
        Console.WriteLine();
        Console.WriteLine("***********************************************");
        Console.WriteLine("Congratulations " + winnerName + "! You have won the game!");
        Console.WriteLine("***********************************************");
        Console.WriteLine();
    }

    public static void stalemateAnnouncement()
    {
        Console.WriteLine("*********************************");
        Console.WriteLine("Ops! We have reached a stalemate~");
        Console.WriteLine("*********************************");
    }

    public static void newGamePrompt()
    {
        Console.WriteLine("************************************************************");
        Console.WriteLine("Do you guys want to enjoy another round?");
        Console.WriteLine("Hit \"y/Y\" to kick off again! Or hit any other key to exit.");
        Console.WriteLine("************************************************************");
        Console.WriteLine();
    }

    public static void printSummaryResults(int wins1, String name1, int wins2, String name2)
    {
        String finalChampion;

        Console.WriteLine("√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√");
        Console.WriteLine("Fantastic performance guys! Superb!");
        Console.WriteLine("Player " + name1 + " has won: " + wins1 + " time(s).");
        Console.WriteLine("Player " + name2 + " has won: " + wins2 + " time(s).");

        if (wins1 == wins2)
        {
            Console.WriteLine("Therefore, the final champion is: BOTH YOU GUYS!!!");
        }
        else
        {
            finalChampion = wins1 > wins2 ? name1 : name2;
            Console.WriteLine("Therefore, the final winner is: " + finalChampion + "!!!");
        }

        Console.WriteLine("√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√");
        Console.WriteLine();
    }
}

