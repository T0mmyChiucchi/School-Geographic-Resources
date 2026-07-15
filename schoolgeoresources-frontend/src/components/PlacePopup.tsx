import { MapPin, Edit3, Trash2 } from 'lucide-react';
import { type Place } from '../hooks/usePlaces';

interface PlacePopupProps {
  place: Place;
  onEdit: (place: Place) => void;
  onDelete: (place: Place) => void;
}

export function PlacePopup({ place, onEdit, onDelete }: PlacePopupProps) {
  // We use a beautiful placeholder image from Unsplash
  const imageUrl = `https://images.unsplash.com/photo-1449844908441-8829872d2607?auto=format&fit=crop&q=80&w=400&h=200`;

  return (
    <div className="place-popup">
      <div className="popup-image" style={{ backgroundImage: `url(${imageUrl})` }}></div>
      <div className="popup-content">
        <h3 className="popup-title">{place.name}</h3>
        
        {place.fullAddress && (
          <div className="popup-detail">
            <MapPin size={14} style={{ marginTop: '2px', flexShrink: 0 }} />
            <span>{place.fullAddress}</span>
          </div>
        )}
        
        <div className="popup-coords">
          {place.latitude.toFixed(4)}, {place.longitude.toFixed(4)}
        </div>
        
        <div className="popup-actions">
          <button onClick={() => onEdit(place)} className="btn btn-glass" style={{ flex: 1, padding: '0.4rem', fontSize: '0.85rem' }}>
            <Edit3 size={16} /> Edit
          </button>
          <button onClick={() => onDelete(place)} className="btn btn-glass" style={{ flex: 1, padding: '0.4rem', fontSize: '0.85rem', color: 'var(--danger)' }}>
            <Trash2 size={16} /> Delete
          </button>
        </div>
      </div>
    </div>
  );
}
