using BattleshipGame.Domain.Entities;
using BattleshipGame.Domain.Enums;

namespace BattleshipGame.Application.Interfaces;

public interface IGridService
{
    CellState[][] InitializeGrid(List<Ship> ships);
    bool CheckIfSunk(int x, int y, List<Ship> ships, CellState[][] grid);
    bool CheckGameOver(List<Ship> ships, CellState[][] grid);
}
