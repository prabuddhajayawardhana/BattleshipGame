import Cell from './Cell';

interface BoardProps {
  grid: string[][];
  onCellClick: (x: number, y: number) => void;
}

const Board = ({ grid, onCellClick }: BoardProps) => {
  return (
    <div className="grid">
      {grid.map((row, rowIndex) => (
        <div key={rowIndex} className="row">
          {row.map((cell, colIndex) => (
            <Cell
              key={colIndex}
              value={cell}
              onClick={() => onCellClick(colIndex, rowIndex)}
              className={`cell ${cell === 'H' ? 'hit' : cell === 'M' ? 'miss' : ''}`}
            />
          ))}
        </div>
      ))}
    </div>
  );
};

export default Board;
