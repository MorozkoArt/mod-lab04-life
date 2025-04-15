using Xunit;
using cli_life;

public class CellTests
{
    [Fact]
    public void Cell_StartsDead()
    {
        var cell = new Cell();
        Assert.False(cell.IsAlive);
    }

    [Fact]
    public void DetermineNextLiveState_DeadCellWith3Neighbors_BecomesAlive()
    {
        var cell = new Cell { IsAlive = false };
        cell.neighbors.AddRange(Enumerable.Repeat(new Cell { IsAlive = true }, 3));
        cell.DetermineNextLiveState();
        Assert.True(cell.IsAliveNext);
    }
}

public class BoardTests
{
    [Fact]
    public void Board_InitializesCorrectSize()
    {
        var board = new Board(100, 100, 10);
        Assert.Equal(10, board.Columns);
        Assert.Equal(10, board.Rows);
    }

    [Fact]
    public void Randomize_SetsAliveCells()
    {
        var board = new Board(100, 100, 10);
        board.Randomize(0.5);
        Assert.Contains(board.Cells.Cast<Cell>(), c => c.IsAlive);
    }

    [Fact]
    public void Advance_UpdatesAllCells()
    {
        var board = new Board(100, 100, 10);
        board.Cells[0, 0].IsAlive = true;
        board.Advance();
        Assert.False(board.Cells[0, 0].IsAlive); 
    }
}

public class TextControllerTests
{
    [Fact]
    public void SaveAndRead_LifeFile_MatchesOriginal()
    {
        var board = new Board(30, 30, 1);
        board.Randomize(0.3);
        string filePath = "test_life.txt";

        TextController.Save_life(board.Cells, filePath);
        var newBoard = new Board(30, 30, 1);
        TextController.Read_life(newBoard.Cells, filePath);

        Assert.Equal(board.Cells[0, 0].IsAlive, newBoard.Cells[0, 0].IsAlive);
        File.Delete(filePath);
    }
}

public class JSONControllerTests
{
    [Fact]
    public void DeserializeFromJSON_ValidFile_ReturnsProperties()
    {
        string json = @"{""BoardWidth"": 50, ""BoardHeight"": 50, ""BoardCellSize"": 1, ""LifeDensity"": 0.5}";
        File.WriteAllText("test_props.json", json);

        var props = JSONController.DeserializeFromJSON("test_props.json");
        Assert.Equal(50, props.BoardWidth);
        File.Delete("test_props.json");
    }
}

public class PatternClassifierTests
{
    [Fact]
    public void Classify_UnknownPattern_ReturnsUnknown()
    {
        var classifier = new PatternClassifier();
        var pattern = new bool[,] { { true, false }, { false, true } }; // Неизвестный паттерн
        string name = classifier.Classify(pattern);
        Assert.Equal("Unknown", name);
    }
}



