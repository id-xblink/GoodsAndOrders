import api from '@/api.js';
import { handleApiErrorToast } from '@/utils/apiErrorHandler';

export const getDetailedOrder = async (orderId) => {
  try {
    const response = await api.get(`/orders/${orderId}`);
    return response.data;
  } catch (error) {
    handleApiErrorToast(error);
  }
};

export const getStatuses = async () => {
  try {
    const response = await api.get('/orderstatus');
    return response.data;
  } catch (error) {
    handleApiErrorToast(error);
  }
};

export const getUserOrders = async (isAdmin, userId, params) => {
  try {
    const response = await api.get(`/orders?customerId=${isAdmin ? '' : userId}`, {params});
    return response.data;
  } catch (error) {
    handleApiErrorToast(error);
  }
};

export const updateOrderToStart = async (orderStart) => {
  try {
    const response = await api.put(`/orders/status`, orderStart);
    return response.data;
  } catch (error) {
    handleApiErrorToast(error);
  }
};

export const updateOrderToFinish = async (orderFinish) => {
  try {
    const response = await api.put(`/orders/status`, orderFinish);
    return response.data;
  } catch (error) {
    handleApiErrorToast(error);
  }
};

export const deleteOrder = async (orderId) => {
  try {
    const response = await api.delete(`/orders/${orderId}`);
    return response.data;
  } catch (error) {
    handleApiErrorToast(error);
  }
};
// cartview
export const createOrder = async (orderData) => {
  try {
    const response = await api.post('/orders', orderData);
    return response.data;
  } catch (error) {
    handleApiErrorToast(error);
  }
};