public class GameState
{
    public const int Rows = 8;
    public const int Cols = 8;

    public Player[,] Board { get; }
    public Dictionary<Player, int> DiscCount { get; }
    public Player CurrentPlayer { get; private set; }
    public bool GameOver { get; private set; }
    public Player Winner { get; private set; }
    public Dictionary<Position, List<Position>> LegalMoves { get; private set; }

    public GameState()
    {
        Board = new Player[Rows, Cols];
        Board[3, 3] = Player.White;
        Board[3, 4] = Player.Black;
        Board[4, 3] = Player.Black;
        Board[4, 4] = Player.White;

        DiscCount = new Dictionary<Player, int>()
        {
            {Player.Black, 2 },
            {Player.White, 2 }
        };

        CurrentPlayer = Player.Black;
        LegalMoves = FindLegalMove(CurrentPlayer);
    }

    public bool MakeMove(Position pos, out MoveInfo moveInfo)
    {
        if (!LegalMoves.ContainsKey(pos))
        {
            moveInfo = null;
            return false;
        }

        CurrentPlayer movePlayer = CurrentPlayer;
        List<Position> outflanked = LegalMoves[pos];

        Board[pos.Row, pos.Column] = movePlayer;
        // flip discs
        // update disc counts
        // pass turn

        moveInfo = new MoveInfo { Player = movePlayer, Position = pos, Outflanked = outflanked };
        return true;
    }

    private bool IsInsideBoar(int r, int c)
    {
        return r >= 0 && r < Rows && c >= 0 && c < Cols;
    }

    private List<Position> OutflankedInDir(Position pos, Player player, int rDelta, int cDelta)
    {
        List<Position> outflanked = new List<Position>();
        int r = pos.Row + rDelta;
        int c = pos.Col + cDelta;

        while (IsInsideBoar(r, c) && Board[r, c] != Player.None)
        {
            if (Board[r, c] == player.Opponent())
            {
                outflanked.Add(new Position(r, c));
                r += rDelta;
                c += cDelta;
            }
            else /*if (Board[r, c] == player)*/
            {
                return outflanked;
            }
        }

        return new List<Position>();
    }

    private List<Position> Outflanked(Position pos, Player player)
    {
        List<Position> outflanked = new List<Position>();

        for (int rDelta = -1; rDelta <= 1; rDelta++)
        {
            for (int cDelta = -1; cDelta <= 1; cDelta++)
            {
                if (rDelta == 0 && cDelta == 0)
                {
                    continue;
                }

                outflanked.AddRange(OutflankedInDir(pos, player, rDelta, cDelta));
            }
        }

        return outflanked;
    }

    private bool IsMoveLegal(Player player, Position pos, out List<Position> outflanked)
    {
        if (Board[pos.Row, pos.Col] != Player.None)
        {
            outflanked = null;
            return false;
        }

        outflanked = Outflanked(pos, player);
        return outflanked.Count > 0;
    }

    private Dictionary<Position, List<Position>> FindLegalMove(Player player)
    {
        Dictionary<Position, List<Position>> legalMoves = new Dictionary<Position, List<Position>>();

        for (int r = 0; r < Rows; r++)
        {
            for (int c = 0; c < Cols; c++)
            {
                Position pos = new Position(r, c);

                if (IsMoveLegal(player, pos, out List<Position> outflanked))
                {
                    legalMoves[pos] = outflanked;
                }
            }
        }

        return legalMoves;
    }
}
