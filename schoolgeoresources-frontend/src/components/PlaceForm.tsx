import React, { useState } from 'react';
import { X, Loader2 } from 'lucide-react';
import { GlassCard } from './GlassCard';
import { useCreatePlace } from '../hooks/usePlaces';

interface PlaceFormProps {
  onClose: () => void;
}

export function PlaceForm({ onClose }: PlaceFormProps) {
  const [formData, setFormData] = useState({
    name: '',
    latitude: '',
    longitude: '',
    street: '',
    city: '',
    postalCode: '',
    countryCode: 'IT'
  });
  const [error, setError] = useState('');

  const createPlace = useCreatePlace();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');

    try {
      const lat = parseFloat(formData.latitude);
      const lng = parseFloat(formData.longitude);
      
      if (isNaN(lat) || isNaN(lng)) {
        throw new Error('Latitude and Longitude must be valid numbers');
      }

      await createPlace.mutateAsync({
        name: formData.name,
        latitude: lat,
        longitude: lng,
        street: formData.street,
        city: formData.city,
        postalCode: formData.postalCode,
        countryCode: formData.countryCode
      });
      onClose();
    } catch (err: any) {
      setError(err.message || 'Failed to create place');
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
      backgroundColor: 'rgba(0, 0, 0, 0.5)',
      display: 'flex', alignItems: 'center', justifyContent: 'center',
      zIndex: 1000, padding: '1rem',
      backdropFilter: 'blur(5px)'
    }}>
      <GlassCard style={{ width: '100%', maxWidth: '600px', padding: '2rem', maxHeight: '90vh', overflowY: 'auto' }}>
        <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '1.5rem' }}>
          <h2 style={{ margin: 0, fontSize: '1.5rem', fontWeight: 600 }}>Add New Place</h2>
          <button onClick={onClose} style={{ background: 'none', border: 'none', color: 'var(--text-secondary)', cursor: 'pointer' }}>
            <X size={24} />
          </button>
        </div>

        {error && (
          <div style={{ padding: '1rem', backgroundColor: 'rgba(239, 68, 68, 0.1)', color: 'var(--danger)', borderRadius: '8px', marginBottom: '1.5rem' }}>
            {error}
          </div>
        )}

        <form onSubmit={handleSubmit} style={{ display: 'flex', flexDirection: 'column', gap: '1rem' }}>
          <div className="form-group">
            <label>Name</label>
            <input name="name" className="form-input" required value={formData.name} onChange={handleChange} placeholder="e.g. Colosseo" />
          </div>

          <div style={{ display: 'flex', gap: '1rem' }}>
            <div className="form-group" style={{ flex: 1 }}>
              <label>Latitude</label>
              <input name="latitude" type="number" step="any" className="form-input" required value={formData.latitude} onChange={handleChange} placeholder="e.g. 41.8902" />
            </div>
            <div className="form-group" style={{ flex: 1 }}>
              <label>Longitude</label>
              <input name="longitude" type="number" step="any" className="form-input" required value={formData.longitude} onChange={handleChange} placeholder="e.g. 12.4922" />
            </div>
          </div>

          <div className="form-group">
            <label>Street</label>
            <input name="street" className="form-input" required value={formData.street} onChange={handleChange} placeholder="e.g. Piazza del Colosseo, 1" />
          </div>

          <div style={{ display: 'flex', gap: '1rem' }}>
            <div className="form-group" style={{ flex: 2 }}>
              <label>City</label>
              <input name="city" className="form-input" required value={formData.city} onChange={handleChange} placeholder="e.g. Roma" />
            </div>
            <div className="form-group" style={{ flex: 1 }}>
              <label>Postal Code</label>
              <input name="postalCode" className="form-input" required value={formData.postalCode} onChange={handleChange} placeholder="e.g. 00184" />
            </div>
            <div className="form-group" style={{ flex: 1 }}>
              <label>Country</label>
              <input name="countryCode" className="form-input" required value={formData.countryCode} onChange={handleChange} maxLength={2} placeholder="IT" />
            </div>
          </div>

          <div style={{ display: 'flex', justifyContent: 'flex-end', gap: '1rem', marginTop: '1rem' }}>
            <button type="button" onClick={onClose} className="btn btn-glass">Cancel</button>
            <button type="submit" className="btn btn-primary" disabled={createPlace.isPending}>
              {createPlace.isPending ? <Loader2 className="animate-spin" size={20} /> : 'Save Place'}
            </button>
          </div>
        </form>
      </GlassCard>
    </div>
  );
}
