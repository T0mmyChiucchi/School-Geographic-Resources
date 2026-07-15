import React from 'react';
import { GlassCard } from './GlassCard';
import { AlertTriangle, Loader2 } from 'lucide-react';

interface ConfirmModalProps {
  isOpen: boolean;
  title: string;
  message: string;
  onConfirm: () => void;
  onClose: () => void;
  isLoading?: boolean;
}

export function ConfirmModal({ isOpen, title, message, onConfirm, onClose, isLoading }: ConfirmModalProps) {
  if (!isOpen) return null;

  return (
    <div style={{
      position: 'fixed', top: 0, left: 0, right: 0, bottom: 0,
      backgroundColor: 'rgba(15, 23, 42, 0.4)',
      display: 'flex', alignItems: 'center', justifyContent: 'center',
      zIndex: 1100, padding: '1rem',
      backdropFilter: 'blur(12px)',
      WebkitBackdropFilter: 'blur(12px)',
    }}>
      <GlassCard className="modal-animate" style={{ width: '100%', maxWidth: '400px', padding: '2rem', textAlign: 'center' }}>
        <div style={{ display: 'flex', justifyContent: 'center', marginBottom: '1.5rem', color: 'var(--danger)' }}>
          <AlertTriangle size={48} />
        </div>
        <h2 style={{ margin: '0 0 1rem 0', fontSize: '1.5rem', fontWeight: 600 }}>{title}</h2>
        <p style={{ margin: '0 0 2rem 0', color: 'var(--text-secondary)' }}>{message}</p>
        
        <div style={{ display: 'flex', gap: '1rem', justifyContent: 'center' }}>
          <button onClick={onClose} className="btn btn-glass" disabled={isLoading} style={{ flex: 1 }}>
            Cancel
          </button>
          <button 
            onClick={onConfirm} 
            className="btn btn-primary" 
            disabled={isLoading} 
            style={{ flex: 1, backgroundColor: 'var(--danger)' }}
          >
            {isLoading ? <Loader2 className="animate-spin" size={20} /> : 'Delete'}
          </button>
        </div>
      </GlassCard>
    </div>
  );
}
