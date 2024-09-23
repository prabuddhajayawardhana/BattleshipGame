using BattleshipGame.Domain.Enums;

namespace BattleshipGame.Domain.Entities;
public class Ship
{
    public string Name { get; }
    public int Size { get; }
    public List<(int x, int y)> Coordinates { get; }

    public Ship(string name, int size)
    {
        Name = name;
        Size = size;
        Coordinates = new List<(int x, int y)>();
    }

    public bool IsSunk(CellState[][] grid)
    {
        return Coordinates.All(coord => grid[coord.y][coord.x] == CellState.Hit);
    }
}