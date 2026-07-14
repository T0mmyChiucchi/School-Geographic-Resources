import React from 'react';
import { Header } from '../components/Header';
import { GlassCard } from '../components/GlassCard';
import { Map as MapIcon, FolderOpen, Files, Activity } from 'lucide-react';
import { usePlaces } from '../hooks/usePlaces';

import { MapContainer, TileLayer, Marker, Popup } from 'react-leaflet';
import L from 'leaflet';

const customIcon = L.divIcon({
  className: 'custom-leaflet-icon',
  html: `<div style="background-color: var(--accent-primary); width: 100%; height: 100%; border-radius: 50%; border: 2px solid white; box-shadow: 0 2px 4px rgba(0,0,0,0.3);"></div>`,
  iconSize: [20, 20],
  iconAnchor: [10, 10],
});

export function Dashboard() {
  const { data, isLoading } = usePlaces();
  const totalPlaces = data?.totalCount || 0;

  return (
    <>
      <Header title="Overview" subtitle="Welcome back to GeoResources" />
      
      {/* Dashboard Grid */}
      <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fit, minmax(300px, 1fr))', gap: '1.5rem', marginBottom: '2rem' }}>
        <GlassCard style={{ display: 'flex', flexDirection: 'column', gap: '1rem' }}>
          <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
            <h3 style={{ fontSize: '1.1rem', fontWeight: 600 }}>Total Places</h3>
            <div style={{ padding: '0.5rem', background: 'rgba(59, 130, 246, 0.1)', borderRadius: '8px', color: 'var(--accent-primary)' }}>
              <MapIcon size={20} />
            </div>
          </div>
          <p style={{ fontSize: '2.5rem', fontWeight: 700, margin: 0 }}>{isLoading ? '...' : totalPlaces}</p>
          <p style={{ fontSize: '0.85rem', color: 'var(--success)', margin: 0 }}>Active Locations</p>
        </GlassCard>

        <GlassCard style={{ display: 'flex', flexDirection: 'column', gap: '1rem' }}>
          <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
            <h3 style={{ fontSize: '1.1rem', fontWeight: 600 }}>Active Collections</h3>
            <div style={{ padding: '0.5rem', background: 'rgba(16, 185, 129, 0.1)', borderRadius: '8px', color: 'var(--accent-secondary)' }}>
              <FolderOpen size={20} />
            </div>
          </div>
          <p style={{ fontSize: '2.5rem', fontWeight: 700, margin: 0 }}>2</p>
          <p style={{ fontSize: '0.85rem', color: 'var(--text-secondary)', margin: 0 }}>Managing categories</p>
        </GlassCard>
      </div>

      {/* Map Visualization */}
      <GlassCard style={{ flex: 1, display: 'flex', flexDirection: 'column', minHeight: '400px', overflow: 'hidden', padding: 0 }}>
        <div style={{ padding: '1.5rem', borderBottom: '1px solid var(--glass-border)' }}>
          <h3 style={{ fontSize: '1.25rem', fontWeight: 600, margin: 0 }}>Global Map</h3>
        </div>
        
        <div style={{ flex: 1, width: '100%', position: 'relative' }}>
          {!isLoading && data?.items ? (
            <MapContainer 
              center={[41.9028, 12.4964]}
              zoom={6}
              minZoom={5}
              maxBounds={[
                [35.0, 5.0], // South-West Italy
                [48.0, 19.0] // North-East Italy
              ]}
              maxBoundsViscosity={1.0}
              preferCanvas={true}
              style={{ height: '100%', width: '100%', zIndex: 1, minHeight: '400px' }}
            >
              <TileLayer
                url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
                attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
              />
              {data.items.map((place) => (
                <Marker 
                  key={place.id} 
                  position={[place.latitude, place.longitude]}
                  icon={customIcon}
                >
                  <Popup>
                    <strong>{place.name}</strong><br />
                    {place.fullAddress}
                  </Popup>
                </Marker>
              ))}
            </MapContainer>
          ) : (
             <div style={{ padding: '2rem', textAlign: 'center', color: 'var(--text-secondary)' }}>
               Loading Map Data...
             </div>
          )}
        </div>
      </GlassCard>
    </>
  );
}
