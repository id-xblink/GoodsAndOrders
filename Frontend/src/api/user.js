import api from '@/api.js';
import { handleApiErrorToast } from '@/utils/apiErrorHandler';

export const getFilteredUsers = async (params) => {
  try {
    const response = await api.get('/users/dto', { params });
    return response.data;
  } catch (error) {
    handleApiErrorToast(error);
  }
};

export const getRoles = async () => {
  try {
    const response = await api.get('/roles');
    return response.data;
  } catch (error) {
    handleApiErrorToast(error);
  }
};

export const addUser = async (newUser, password) => {
  try {
    const response = await api.post(`/users?password=${encodeURIComponent(password)}`, newUser);
    return response.data;
  } catch (error) {
    handleApiErrorToast(error);
  }
};

export const updateUser = async (userId, editUser) => {
  try {
    const response = await api.put(`/users/${userId}`, editUser);
    return response.data;
  } catch (error) {
    handleApiErrorToast(error);
  }
};

export const deleteUser = async (userId) => {
  try {
    const response = await api.delete(`/users/${userId}`);
    return response.data;
  } catch (error) {
    handleApiErrorToast(error);
  }
};

// registerView
export const registerUser = async (newUser, password) => {
  try {
    const response = await api.post(`/users/registration?password=${encodeURIComponent(password)}`, newUser);
    return response.data;
  } catch (error) {
    handleApiErrorToast(error);
  }
};

// authStore
export const loginUser = async (login, password) => {
  try {
    const response = await api.post('/auth/login', {login, password});
    return response.data;
  } catch (error) {
    handleApiErrorToast(error);
  }
}