import { Moon, Sun } from 'lucide-react';
import { useTheme } from '../contexts/ThemeContext';

export function ThemeToggle() {
  const { theme, toggleTheme } = useTheme();

  return (
    <button 
      className="btn btn-glass" 
      style={{ width: '48px', height: '48px', padding: 0, borderRadius: '50%' }}
      onClick={toggleTheme}
      aria-label="Toggle Theme"
    >
      {theme === 'dark' ? <Sun size={20} /> : <Moon size={20} />}
    </button>
  );
}
