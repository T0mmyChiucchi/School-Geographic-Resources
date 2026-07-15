import React, { useState } from 'react';
import { Header } from '../components/Header';
import { usePlaces, useDeletePlace, type Place } from '../hooks/usePlaces';
import { GlassCard } from '../components/GlassCard';
import { MapPin, Loader2, AlertCircle, Edit3, Trash2 } from 'lucide-react';
import { PlaceForm } from '../components/PlaceForm';
import { ConfirmModal } from '../components/ConfirmModal';

export function Places() {
  const { data, isLoading, error } = usePlaces();
  const deletePlaceMutation = useDeletePlace();

  const [isModalOpen, setIsModalOpen] = useState(false);
  const [placeToEdit, setPlaceToEdit] = useState<Place | undefined>(undefined);

  const [isConfirmOpen, setIsConfirmOpen] = useState(false);
  const [placeToDelete, setPlaceToDelete] = useState<Place | undefined>(undefined);

  const handleAddNew = () => {
    setPlaceToEdit(undefined);
    setIsModalOpen(true);
  };

  const handleEditClick = (place: Place) => {
    setPlaceToEdit(place);
    setIsModalOpen(true);
  };

  const handleDeleteClick = (place: Place) => {
    setPlaceToDelete(place);
    setIsConfirmOpen(true);
  };

  const confirmDelete = async () => {
    if (placeToDelete) {
      try {
        await deletePlaceMutation.mutateAsync(placeToDelete.id);
        setIsConfirmOpen(false);
        setPlaceToDelete(undefined);
      } catch (err) {
        console.error('Failed to delete place', err);
      }
    }
  };

  if (isLoading) {
    return (
      <>
        <Header title="Places" subtitle="Loading geographical locations..." />
        <div style={{ padding: '2rem', display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100%' }}>
          <Loader2 className="animate-spin" size={32} style={{ color: 'var(--accent-primary)' }} />
        </div>
      </>
    );
  }

  if (error) {
    return (
      <>
        <Header title="Places" subtitle="Error loading locations" />
        <div style={{ padding: '2rem', display: 'flex', gap: '0.5rem', color: 'var(--danger)', alignItems: 'center' }}>
          <AlertCircle />
          <span>Failed to load places: {(error as Error).message}</span>
        </div>
      </>
    );
  }

  const places = data?.items || [];

  return (
    <>
      <Header title="Places" subtitle="Manage geographical locations" />
      <div style={{ padding: '2rem' }}>
        <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '2rem' }}>
          <h1 style={{ margin: 0, fontSize: '2rem', fontWeight: 700 }}>Geographic Places</h1>
          <button className="btn btn-primary" onClick={handleAddNew}>Add Place</button>
        </div>

      {places.length === 0 ? (
        <GlassCard style={{ padding: '3rem', textAlign: 'center', opacity: 0.8 }}>
          <MapPin size={48} style={{ margin: '0 auto 1rem', opacity: 0.5 }} />
          <h3>No places found</h3>
          <p style={{ color: 'var(--text-secondary)' }}>Get started by adding a new geographic place.</p>
        </GlassCard>
      ) : (
        <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fill, minmax(300px, 1fr))', gap: '1.5rem' }}>
          {places.map((place) => (
            <GlassCard key={place.id} style={{ padding: '1.5rem', display: 'flex', flexDirection: 'column' }}>
              <div style={{ display: 'flex', alignItems: 'flex-start', gap: '1rem', marginBottom: '1rem' }}>
                <div style={{ width: '48px', height: '48px', borderRadius: '12px', background: 'var(--accent-secondary)', display: 'flex', alignItems: 'center', justifyContent: 'center', color: 'white', flexShrink: 0 }}>
                  <MapPin size={24} />
                </div>
                <div>
                  <h3 style={{ margin: '0 0 0.25rem', fontSize: '1.1rem' }}>{place.name}</h3>
                  <div style={{ display: 'flex', gap: '0.5rem', fontSize: '0.8rem', color: 'var(--text-secondary)' }}>
                    <span>Lat: {place.latitude.toFixed(4)}</span>
                    <span>•</span>
                    <span>Lng: {place.longitude.toFixed(4)}</span>
                  </div>
                </div>
              </div>
              
              {place.description && (
                <p style={{ fontSize: '0.9rem', color: 'var(--text-secondary)', margin: '0 0 1rem', display: '-webkit-box', WebkitLineClamp: 2, WebkitBoxOrient: 'vertical', overflow: 'hidden' }}>
                  {place.description}
                </p>
              )}
              
              <div style={{ display: 'flex', justifyContent: 'flex-end', gap: '0.5rem', marginTop: 'auto' }}>
                <button onClick={() => handleEditClick(place)} className="btn btn-glass" style={{ padding: '0.5rem', fontSize: '0.85rem' }}>
                  <Edit3 size={18} />
                </button>
                <button onClick={() => handleDeleteClick(place)} className="btn btn-glass" style={{ padding: '0.5rem', fontSize: '0.85rem', color: 'var(--danger)' }}>
                  <Trash2 size={18} />
                </button>
              </div>
            </GlassCard>
          ))}
        </div>
      )}
      </div>
      {isModalOpen && <PlaceForm onClose={() => setIsModalOpen(false)} place={placeToEdit} />}
      <ConfirmModal 
        isOpen={isConfirmOpen}
        title="Delete Place"
        message={`Are you sure you want to delete "${placeToDelete?.name}"? This action cannot be undone.`}
        onConfirm={confirmDelete}
        onClose={() => setIsConfirmOpen(false)}
        isLoading={deletePlaceMutation.isPending}
      />
    </>
  );
}
