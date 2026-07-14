import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { fetchWithAuth } from '../lib/api';

export interface Place {
  id: string;
  name: string;
  latitude: number;
  longitude: number;
  fullAddress: string;
  description: string;
}

export interface PaginatedList<T> {
  items: T[];
  pageNumber: number;
  totalPages: number;
  totalCount: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}

export function usePlaces() {
  return useQuery<PaginatedList<Place>>({
    queryKey: ['places'],
    queryFn: async () => {
      // Using arbitrary large bounding box for global map / fetching all places initially
      return fetchWithAuth('/api/Places?minLat=-90&maxLat=90&minLng=-180&maxLng=180&pageSize=100');
    }
  });
}

export function useCreatePlace() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: async (newPlace: {
      name: string;
      latitude: number;
      longitude: number;
      street: string;
      city: string;
      postalCode: string;
      countryCode: string;
    }) => {
      const response = await fetchWithAuth('/api/Places', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(newPlace),
      });
      return response;
    },
    onSuccess: () => {
      // Invalidate and refetch
      queryClient.invalidateQueries({ queryKey: ['places'] });
    },
  });
}
