export const fetchGridStatus = async () => {
    const response = await fetch('api/game/status');
    if (!response.ok) throw new Error('Failed to fetch grid status');
    return await response.json();
  };
  
  export const handleCellClick = async (x: number, y: number) => {
    const coordinate = String.fromCharCode(65 + x) + (y + 1);
    const response = await fetch('api/game/shoot', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(coordinate),
    });
  
    if (!response.ok) throw new Error('Failed to shoot at cell');
    return await response.text();
  };
  
  export const handleRestart = async () => {
    return await fetch('api/game/restart', { method: 'GET' });
  };
  
  export const handleStart = async () => {
    return await fetch('api/game/start', { method: 'GET' });
  };
  