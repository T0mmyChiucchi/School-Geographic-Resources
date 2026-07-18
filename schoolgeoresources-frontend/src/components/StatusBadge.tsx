import React from 'react';

interface StatusBadgeProps {
  state: string;
}

export function StatusBadge({ state }: StatusBadgeProps) {
  const getBadgeStyle = () => {
    switch (state) {
      case 'Published':
        return { bg: 'rgba(16, 185, 129, 0.15)', color: 'var(--success)' };
      case 'InReview':
        return { bg: 'rgba(245, 158, 11, 0.15)', color: 'var(--warning)' };
      case 'Rejected':
        return { bg: 'rgba(239, 68, 68, 0.15)', color: 'var(--danger)' };
      case 'Archived':
        return { bg: 'rgba(107, 114, 128, 0.15)', color: 'var(--text-secondary)' };
      case 'Draft':
      default:
        return { bg: 'rgba(156, 163, 175, 0.15)', color: 'var(--text-secondary)' };
    }
  };

  const style = getBadgeStyle();

  return (
    <span style={{
      backgroundColor: style.bg,
      color: style.color,
      padding: '0.2rem 0.5rem',
      borderRadius: '12px',
      fontSize: '0.75rem',
      fontWeight: 600,
      textTransform: 'uppercase',
      letterSpacing: '0.05em',
      border: `1px solid ${style.bg.replace('0.15', '0.3')}`
    }}>
      {state || 'Draft'}
    </span>
  );
}
