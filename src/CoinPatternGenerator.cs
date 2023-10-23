using Godot;
using System;
using Godot.Collections;

public partial class CoinPatternGenerator : Node2D
{
    public int ShiftAmount { get; set; } = 48;
    public override void _Ready()
    {
        GD.Randomize();
        GenerateCoinPatterns();
    }

    private enum CoinArrangement : uint
    {
        Single,
        Line,
        Cluster,
        Diamond,
        SineWave,
        ZigZag,
        MAX
    }
    static private Dictionary<CoinArrangement, Array<Vector2I>> CoinPatterns = new Dictionary<CoinArrangement, Array<Vector2I>>();

    public void GenerateCoinPatterns()
    {
        // Fill out dictionary with generated coin patterns. Run once at startup.
        // Note this can be optimized via caching pre-generated patterns.

        // Note coin positions should be placed relative to the center of the viewport.
        var centerX = GetViewportRect().Size.X;
        var centerY = GetViewportRect().Size.Y / 2;

        // Generate single coin pattern
        var singleCoinPattern = new Array<Vector2I>();
        singleCoinPattern.Add(new Vector2I((int)centerX, (int)centerY));
        singleCoinPattern = ShiftRightOffScreen(singleCoinPattern);
        CoinPatterns.Add(CoinArrangement.Single, singleCoinPattern);

        // Generate line pattern
        var linePattern = new Array<Vector2I>();
        for (int i = 0; i < 5; i++)
        {
            // 2x shift amount between each coin
            linePattern.Add(new Vector2I((int)centerX + i * ShiftAmount * 2, (int)centerY));
        }

        linePattern = ShiftRightOffScreen(linePattern);

        CoinPatterns.Add(CoinArrangement.Line, linePattern);

        // Generate cluster pattern (i.e. a 5x5 grid of coins)
        var clusterPattern = new Array<Vector2I>();
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                clusterPattern.Add(new Vector2I((int)centerX + i * ShiftAmount, (int)centerY + j * ShiftAmount));
            }
        }

        clusterPattern = ShiftUpPositions(clusterPattern);
        clusterPattern = ShiftRightOffScreen(clusterPattern);

        CoinPatterns.Add(CoinArrangement.Cluster, clusterPattern);

        // Generate diamond pattern
        var diamondPattern = new Array<Vector2I>();

        // This is easiest if we just convert an existing 2D array of 0s and 1s
        // into a list of coin positions.

        // 0s represent empty space, 1s represent coins
        int[,] diamondPattern2D = new int[10, 10] {
            {0, 0, 0, 0, 1, 0, 0, 0, 0, 0},
            {0, 0, 0, 1, 1, 1, 0, 0, 0, 0},
            {0, 0, 1, 1, 1, 1, 1, 0, 0, 0},
            {0, 1, 1, 1, 1, 1, 1, 1, 0, 0},
            {1, 1, 1, 1, 1, 1, 1, 1, 1, 0},
            {0, 1, 1, 1, 1, 1, 1, 1, 0, 0},
            {0, 0, 1, 1, 1, 1, 1, 0, 0, 0},
            {0, 0, 0, 1, 1, 1, 0, 0, 0, 0},
            {0, 0, 0, 0, 1, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        };

        for (int i = 0; i < diamondPattern2D.GetLength(0); i++)
        {
            for (int j = 0; j < diamondPattern2D.GetLength(1); j++)
            {
                if (diamondPattern2D[i, j] == 1)
                {
                    diamondPattern.Add(new Vector2I((int)centerX + i * ShiftAmount, (int)centerY + j * ShiftAmount));
                }
            }
        }

        diamondPattern = ShiftUpPositions(diamondPattern);
        diamondPattern = ShiftRightOffScreen(diamondPattern);

        CoinPatterns.Add(CoinArrangement.Diamond, diamondPattern);

        // Generate sine wave pattern
        var sineWavePattern = new Array<Vector2I>();

        // We'll generate a sine wave with 10 peaks and 10 troughs
        for (int i = 0; i < 50; i++)
        {
            sineWavePattern.Add(new Vector2I((int)centerX + i * ShiftAmount, (int)centerY + (int)(Math.Sin(i) * ShiftAmount)));
        }

        sineWavePattern = ShiftUpPositions(sineWavePattern);
        sineWavePattern = ShiftRightOffScreen(sineWavePattern);

        CoinPatterns.Add(CoinArrangement.SineWave, sineWavePattern);

        // Generate zig zag pattern

        // We'll generate a zig zag
        var zigZagPattern = new Array<Vector2I>();

        for (int i = 0; i < 30; i++)
        {
            const int amplitude = 10;
            const int period = 5;
            zigZagPattern.Add(new Vector2I((int)centerX + i * ShiftAmount, (int)centerY + ((amplitude/period) * (period - Math.Abs(i % (2*period) - period) ) * ShiftAmount)));
        }

        zigZagPattern = ShiftUpPositions(zigZagPattern);
        zigZagPattern = ShiftRightOffScreen(zigZagPattern);

        CoinPatterns.Add(CoinArrangement.ZigZag, zigZagPattern);
    }

    private Array<Vector2I> ShiftUpPositions(Array<Vector2I> pattern)
    {
        // Interate through the pattern, finding the median y position
        // and shifting all positions up by that amount.
        int medianY = 0;
        foreach (Vector2I position in pattern)
        {
            medianY += position.Y;
        }
        medianY /= pattern.Count;

        // Calculate distance between median and center of viewport
        var centerY = GetViewportRect().Size.Y / 2;
        int adjustDistance = Math.Abs((int)centerY - medianY);

        var shiftedPattern = new Array<Vector2I>();
        foreach (Vector2I position in pattern)
        {
            shiftedPattern.Add(new Vector2I(position.X, position.Y - adjustDistance));
        }
        return shiftedPattern;
    }

    private Array<Vector2I> ShiftRightOffScreen(Array<Vector2I> pattern)
    {
        // Find the rightmost coin position and shift all positions
        // to the right of that position off the screen.
        int rightmostX = 0;
        foreach (Vector2I position in pattern)
        {
            if (position.X > rightmostX)
            {
                rightmostX = position.X;
            }
        }

        var shiftedPattern = new Array<Vector2I>();
        foreach (Vector2I position in pattern)
        {
            shiftedPattern.Add(new Vector2I(position.X + rightmostX, position.Y));
        }
        return shiftedPattern;
    }

    public Array<Vector2I> GenerateRandomPattern()
    {
        // Returns a random pattern of coins represented by a list of coin positions
        // relative to the center of the viewport.
        uint randomPattern = GD.Randi() % (int)CoinArrangement.MAX;
        return CoinPatterns[(CoinArrangement)randomPattern];
    }
}
