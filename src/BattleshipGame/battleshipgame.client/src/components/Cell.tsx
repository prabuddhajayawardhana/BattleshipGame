interface CellProps {
  value: string;
  onClick: () => void;
  className?: string;
}

const Cell = ({ value, onClick, className }: CellProps) => {
  return (
    <div className={`cell ${className}`} onClick={onClick}>
      {value}
    </div>
  );
};

export default Cell;
