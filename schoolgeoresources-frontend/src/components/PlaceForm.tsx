import React, { useState } from 'react';
import { X, Loader2, MapPin, Compass, Building, Map, Hash, Flag } from 'lucide-react';
import { GlassCard } from './GlassCard';
import { useCreatePlace, useUpdatePlace, type Place } from '../hooks/usePlaces';

interface PlaceFormProps {
  onClose: () => void;
  initialCoordinates?: { lat: number; lng: number };
  place?: Place;
}

export function PlaceForm({ onClose, initialCoordinates, place }: PlaceFormProps) {
  const [formData, setFormData] = useState({
    name: place?.name || '',
    latitude: place ? place.latitude.toString() : (initialCoordinates ? initialCoordinates.lat.toString() : ''),
    longitude: place ? place.longitude.toString() : (initialCoordinates ? initialCoordinates.lng.toString() : ''),
    street: place?.street || '',
    city: place?.city || '',
    postalCode: place?.postalCode || '',
    countryCode: place?.countryCode || 'IT'
  });
  const [error, setError] = useState('');

  const createPlace = useCreatePlace();
  const updatePlace = useUpdatePlace();

  const isEditing = !!place;
  const isPending = createPlace.isPending || updatePlace.isPending;

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');

    try {
      const lat = parseFloat(formData.latitude);
      const lng = parseFloat(formData.longitude);
      
      if (isNaN(lat) || isNaN(lng)) {
        throw new Error('Latitude and Longitude must be valid numbers');
      }

      if (isEditing && place) {
        await updatePlace.mutateAsync({
          id: place.id,
          name: formData.name,
          latitude: lat,
          longitude: lng,
          street: formData.street,
          city: formData.city,
          postalCode: formData.postalCode,
          countryCode: formData.countryCode
        });
      } else {
        await createPlace.mutateAsync({
          name: formData.name,
          latitude: lat,
          longitude: lng,
          street: formData.street,
          city: formData.city,
          postalCode: formData.postalCode,
          countryCode: formData.countryCode
        });
      }
      onClose();
    } catch (err: any) {
      setError(err.message || 'Failed to save place');
    }
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value
    });
  };

  return (
    <div style={{
      position: 'fixed', top: 0, left: 0, right: 0, bottom: 0,
      backgroundColor: 'rgba(15, 23, 42, 0.4)',
      display: 'flex', alignItems: 'center', justifyContent: 'center',
      zIndex: 1000, padding: '1rem',
      backdropFilter: 'blur(12px)',
      WebkitBackdropFilter: 'blur(12px)',
    }}>
      <GlassCard className="modal-animate" style={{ width: '100%', maxWidth: '600px', padding: '2.5rem', maxHeight: '90vh', overflowY: 'auto' }}>
        <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '2rem' }}>
          <div>
            <h2 style={{ margin: 0, fontSize: '1.75rem', fontWeight: 700, background: 'var(--bg-gradient)', WebkitBackgroundClip: 'text', color: 'var(--text-primary)' }}>
              {isEditing ? 'Edit Place' : 'Add New Place'}
            </h2>
            <p style={{ margin: '0.25rem 0 0 0', color: 'var(--text-secondary)', fontSize: '0.95rem' }}>
              {isEditing ? 'Modify the details of this geographic resource.' : 'Create a new geographic resource for your school.'}
            </p>
          </div>
          <button onClick={onClose} style={{ 
            background: 'rgba(255,255,255,0.1)', border: '1px solid var(--glass-border)', 
            color: 'var(--text-primary)', cursor: 'pointer', borderRadius: '50%',
            width: '40px', height: '40px', display: 'flex', alignItems: 'center', justifyContent: 'center',
            transition: 'all 0.2s ease'
          }}
          onMouseOver={(e) => e.currentTarget.style.transform = 'rotate(90deg)'}
          onMouseOut={(e) => e.currentTarget.style.transform = 'rotate(0deg)'}
          >
            <X size={20} />
          </button>
        </div>

        {error && (
          <div style={{ padding: '1rem', backgroundColor: 'rgba(239, 68, 68, 0.1)', color: 'var(--danger)', borderRadius: '8px', marginBottom: '1.5rem', border: '1px solid rgba(239, 68, 68, 0.2)' }}>
            <strong>Error:</strong> {error}
          </div>
        )}

        <form onSubmit={handleSubmit} style={{ display: 'flex', flexDirection: 'column', gap: '1.5rem' }}>
          
          <div>
            <label className="form-label">Place Name</label>
            <div className="input-with-icon">
              <MapPin />
              <input name="name" className="input-field" required value={formData.name} onChange={handleChange} placeholder="e.g. Colosseo" />
            </div>
          </div>

          <div style={{ display: 'flex', gap: '1.5rem' }}>
            <div style={{ flex: 1 }}>
              <label className="form-label">Latitude</label>
              <div className="input-with-icon">
                <Compass />
                <input name="latitude" type="number" step="any" className="input-field" required value={formData.latitude} onChange={handleChange} placeholder="41.8902" />
              </div>
            </div>
            <div style={{ flex: 1 }}>
              <label className="form-label">Longitude</label>
              <div className="input-with-icon">
                <Compass />
                <input name="longitude" type="number" step="any" className="input-field" required value={formData.longitude} onChange={handleChange} placeholder="12.4922" />
              </div>
            </div>
          </div>

          <div>
            <label className="form-label">Street Address</label>
            <div className="input-with-icon">
              <Map />
              <input name="street" className="input-field" required value={formData.street} onChange={handleChange} placeholder="Piazza del Colosseo, 1" />
            </div>
          </div>

          <div style={{ display: 'flex', gap: '1.5rem' }}>
            <div style={{ flex: 2 }}>
              <label className="form-label">City</label>
              <div className="input-with-icon">
                <Building />
                <input name="city" className="input-field" required value={formData.city} onChange={handleChange} placeholder="Roma" />
              </div>
            </div>
            <div style={{ flex: 1 }}>
              <label className="form-label">Postal Code</label>
              <div className="input-with-icon">
                <Hash />
                <input name="postalCode" className="input-field" required value={formData.postalCode} onChange={handleChange} placeholder="00184" />
              </div>
            </div>
            <div style={{ flex: 1 }}>
              <label className="form-label">Country</label>
              <div className="input-with-icon">
                <Flag />
                <input name="countryCode" className="input-field" required value={formData.countryCode} onChange={handleChange} maxLength={2} placeholder="IT" />
              </div>
            </div>
          </div>

          <div style={{ display: 'flex', justifyContent: 'flex-end', gap: '1rem', marginTop: '1.5rem', paddingTop: '1.5rem', borderTop: '1px solid var(--glass-border)' }}>
            <button type="button" onClick={onClose} className="btn btn-glass" style={{ padding: '0.75rem 2rem' }}>
              Cancel
            </button>
            <button type="submit" className="btn btn-primary" disabled={isPending} style={{ padding: '0.75rem 2.5rem' }}>
              {isPending ? <Loader2 className="animate-spin" size={20} /> : (isEditing ? 'Save Changes' : 'Save Place')}
            </button>
          </div>
        </form>
      </GlassCard>
    </div>
  );
}
