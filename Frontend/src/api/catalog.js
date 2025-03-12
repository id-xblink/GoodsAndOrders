import api from '@/api.js';
import { handleApiErrorToast } from '@/utils/apiErrorHandler';

export const getFilteredProducts = async (params) => {
  try {
    const response = await api.get('/products', { params });
    return response.data;
  } catch (error) {
    handleApiErrorToast(error);
  }
};

export const getCategories = async () => {
  try {
    const response = await api.get('/categories');
    return response.data;
  } catch (error) {
    handleApiErrorToast(error);
  }
};

export const addProduct = async (newProduct) => {
  try {
    const response = await api.post('/products', newProduct);
    return response.data;
  } catch (error) {
    handleApiErrorToast(error);
  }
};

export const updateProduct = async (productId, editProduct) => {
  try {
    const response = await api.put(`/products/${productId}`, editProduct);
    return response.data;
  } catch (error) {
    handleApiErrorToast(error);
  }
};

export const deleteProduct = async (productId) => {
  try {
    const response = await api.delete(`/products/${productId}`);
    return response.data;
  } catch (error) {
    handleApiErrorToast(error);
  }
};