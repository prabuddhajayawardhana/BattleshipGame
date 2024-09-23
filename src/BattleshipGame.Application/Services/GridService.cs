using BattleshipGame.Application.Interfaces;
using BattleshipGame.Domain.Entities;
using BattleshipGame.Domain.Enums;
using BattleshipGame.SharedKernel.Constants;

namespace BattleshipGame.Application.Services;
public class GridService : IGridService
{
    private readonly Random _random = new Random();

    public CellState[][] InitializeGrid(List<Ship> ships)
    {
        var grid = new CellState[GameConstants.GridSize][];
        for (int i = 0; i < GameConstants.GridSize; i++)
        {
            grid[i] = new CellState[GameConstants.GridSize];
        }

        foreach (var ship in ships)
        {
            PlaceShip(grid, ship);
        }

        return grid;
    }

    private void PlaceShip(CellState[][] grid, Ship ship)
    {
        bool placed = false;
        while (!placed)
        {
            int direction = _random.Next(2); // 0: horizontal, 1: vertical
            int startX = _random.Next(GameConstants.GridSize);
            int startY = _random.Next(GameConstants.GridSize);

            if (direction == 0) // Horizontal
            {
                if (startX + ship.Size <= GameConstants.GridSize && CanPlaceShip(grid, startX, startY, ship.Size, 0))
                {
                    for (int i = 0; i < ship.Size; i++)
                    {
                        grid[startY][startX + i] = CellState.Ship;
                        ship.Coordinates.Add((startX + i, startY));
                    }
                    placed = true;
                }
            }
            else // Vertical
            {
                if (startY + ship.Size <= GameConstants.GridSize && CanPlaceShip(grid, startX, startY, ship.Size, 1))
                {
                    for (int i = 0; i < ship.Size; i++)
                    {
                        grid[startY + i][startX] = CellState.Ship;
                        ship.Coordinates.Add((startX, startY + i));
                    }
                    placed = true;
                }
            }
        }
    }

    private bool CanPlaceShip(CellState[][] grid, int x, int y, int size, int direction)
    {
        for (int i = 0; i < size; i++)
        {
            if (direction == 0 && grid[y][x + i] == CellState.Ship) return false; // Horizontal
            if (direction == 1 && grid[y + i][x] == CellState.Ship) return false; // Vertical
        }
        return true;
    }

    public bool CheckIfSunk(int x, int y, List<Ship> ships, CellState[][] grid)
    {
        var ship = ships.FirstOrDefault(s => s.Coordinates.Any(c => c.x == x && c.y == y));
        return ship?.IsSunk(grid) == true;
    }

    public bool CheckGameOver(List<Ship> ships, CellState[][] grid)
    {
        return ships.All(ship => ship.IsSunk(grid));
    }
}
