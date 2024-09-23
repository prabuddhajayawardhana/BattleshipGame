interface CellProps {
  value: string;
  onClick: () => void;
}

enum CellStatus {
  EMPTY = '~', 
  HIT = 'X',     
  MISS = 'O'    
}

const Cell = ({ value, onClick }: CellProps) => {

  const statusMap: { [key: number | string]: string } = {
    0: CellStatus.EMPTY,
    2: CellStatus.HIT,
    3: CellStatus.MISS
  };

  return (
    <div className={`cell ${statusMap[value]}`} onClick={onClick}>
      {statusMap[value]}
    </div>
  );
};

export default Cell;
