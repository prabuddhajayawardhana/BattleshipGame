using BattleshipGame.Application.Interfaces;
using BattleshipGame.Domain.Entities;
using BattleshipGame.Domain.Enums;
using BattleshipGame.SharedKernel.Constants;

namespace BattleshipGame.Application.Services;
public class GameService : IGameService
{
    private readonly IGridService _gridService;
    private CellState[][] _grid;
    private List<Ship> _ships;

    public GameService(IGridService gridService)
    {
        _gridService = gridService;
        RestartGame();
    }

    public void RestartGame()
    {
        _ships = new List<Ship>
        {
            new Ship("Battleship", 5),
            new Ship("Destroyer 1", 4),
            new Ship("Destroyer 2", 4)
        };
        _grid = _gridService.InitializeGrid(_ships);
    }

    public GameResult Shoot(string coordinate)
    {
        var column = coordinate[0];
        var row = int.Parse(coordinate.Substring(1)) - 1;

        int x = Array.IndexOf(GameConstants.Columns, column);
        int y = row;

        if (x < 0 || x >= GameConstants.GridSize || y < 0 || y >= GameConstants.GridSize)
        {
            throw new ArgumentException(GameConstants.InvalidCoordinateMessage);
        }

        if (_grid[y][x] == CellState.Ship)
        {
            _grid[y][x] = CellState.Hit;
            if (_gridService.CheckIfSunk(x, y, _ships, _grid))
            {
                if (_gridService.CheckGameOver(_ships, _grid))
                {
                    return GameResult.GameOver;
                }
                return GameResult.Sunk;
            }
            return GameResult.Hit;
        }
        else if (_grid[y][x] == CellState.Hit || _grid[y][x] == CellState.Miss)
        {
            return GameResult.AlreadyTried;
        }
        else
        {
            _grid[y][x] = CellState.Miss;
            return GameResult.Miss;
        }
    }

    public CellState[][] GetVisibleGrid()
    {
        var visibleGrid = new CellState[_grid.Length][];

        for (int i = 0; i < _grid.Length; i++)
        {
            visibleGrid[i] = new CellState[_grid[i].Length];
            for (int j = 0; j < _grid[i].Length; j++)
            {
                visibleGrid[i][j] = _grid[i][j] == CellState.Hit ? CellState.Hit :
                                     _grid[i][j] == CellState.Miss ? CellState.Miss : CellState.Empty;
            }
        }

        return visibleGrid;
    }
}
