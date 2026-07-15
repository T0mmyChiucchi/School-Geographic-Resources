import React from 'react';
import { NavLink, useNavigate } from 'react-router-dom';
import { LayoutDashboard, Map as MapIcon, FolderOpen, Settings, LogOut, Sun, Moon } from 'lucide-react';
import clsx from 'clsx';

import { supabase } from '../lib/supabase';
import { useTheme } from '../contexts/ThemeContext';

export function Sidebar() {
  const navigate = useNavigate();
  const { theme, toggleTheme } = useTheme();

  const handleLogout = async () => {
    await supabase.auth.signOut();
    navigate('/login');
  };

  return (
    <aside className="sidebar glass-panel">
      <div style={{ padding: '2rem 1.5rem', display: 'flex', alignItems: 'center', gap: '0.75rem' }}>
        <div style={{ width: '40px', height: '40px', borderRadius: '10px', background: 'var(--accent-primary)', display: 'flex', alignItems: 'center', justifyContent: 'center', color: 'white' }}>
          <MapIcon size={24} />
        </div>
        <div>
          <h1 style={{ fontSize: '1.25rem', fontWeight: 700, margin: 0, lineHeight: 1.2 }}>GeoResources</h1>
          <span style={{ fontSize: '0.75rem', color: 'var(--text-secondary)' }}>School Dashboard</span>
        </div>
      </div>

      <nav style={{ flex: 1, padding: '0 1rem', display: 'flex', flexDirection: 'column', gap: '0.5rem' }}>
        <SidebarLink to="/" icon={<LayoutDashboard size={20} />} label="Dashboard" />
        <SidebarLink to="/places" icon={<MapIcon size={20} />} label="Places" />
        <SidebarLink to="/collections" icon={<FolderOpen size={20} />} label="Collections" />
        <SidebarLink to="/settings" icon={<Settings size={20} />} label="Settings" />
      </nav>

      <div style={{ padding: '1.5rem', borderTop: '1px solid var(--glass-border)', display: 'flex', flexDirection: 'column', gap: '0.5rem' }}>
        <button 
          className="btn btn-glass" 
          style={{ width: '100%', justifyContent: 'flex-start', border: 'none', background: 'transparent' }}
          onClick={toggleTheme}
        >
          {theme === 'dark' ? <Sun size={20} /> : <Moon size={20} />}
          {theme === 'dark' ? 'Light Mode' : 'Dark Mode'}
        </button>
        <button 
          className="btn btn-glass" 
          style={{ width: '100%', justifyContent: 'flex-start', border: 'none', background: 'transparent' }}
          onClick={handleLogout}
        >
          <LogOut size={20} />
          Sign Out
        </button>
      </div>
    </aside>
  );
}

function SidebarLink({ to, icon, label }: { to: string, icon: React.ReactNode, label: string }) {
  return (
    <NavLink 
      to={to}
      className={({ isActive }) => clsx('btn', 'btn-glass', isActive ? 'active-link' : '')}
      style={({ isActive }) => ({
        width: '100%', 
        justifyContent: 'flex-start',
        border: 'none',
        background: isActive ? 'var(--accent-primary)' : 'transparent',
        color: isActive ? 'white' : 'var(--text-primary)',
        boxShadow: isActive ? '0 4px 12px rgba(59, 130, 246, 0.3)' : 'none',
        textDecoration: 'none'
      })}
    >
      {icon}
      {label}
    </NavLink>
  );
}
