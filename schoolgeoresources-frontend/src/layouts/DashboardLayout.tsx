import { Outlet } from 'react-router-dom';
import { Sidebar } from '../components/Sidebar';

export function DashboardLayout() {
  return (
    <div className="app-container">
      <Sidebar />
      <main className="main-content glass-panel">
        <Outlet />
      </main>
    </div>
  );
}
