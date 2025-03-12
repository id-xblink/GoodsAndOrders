import api from '@/api.js';
import { handleApiErrorToast } from '@/utils/apiErrorHandler';

export const checkServerStatus = async () => {
    try {
      const response = await api.get('/health');
      return response.data;
    } catch (error) {
      throw error;
    }
  };
