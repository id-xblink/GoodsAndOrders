import axios from 'axios';

// baseURL из переменной окружения
const API_BASE_URL = import.meta.env.VITE_API_URL || 'https://localhost:7212/api';

import { useToast } from 'vue-toastification';

// Флаг для проверки выхода
let isLoggingOut = false;

const toast = useToast();
const toastOptions = {
  timeout: 3000,
};

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json'
  }
});

const getToken = () => localStorage.getItem('token');

// Перехватчик запросов для добавления токена
api.interceptors.request.use(
  config => {
    const token = getToken();
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  error => Promise.reject(error)
);

// Обработка ошибок и просроченного токена
const handleErrorResponse = async error => {
  if (!error.response) {
    return Promise.reject(error);
  }

  const { status } = error.response;
  
  if (status === 401) {
    if (isLoggingOut) {
      return Promise.reject(error);
    }

    isLoggingOut = true;

    toast.warning('Необходимо авторизоваться вновь', toastOptions);

    try {
      const { useAuthStore } = await import('@/stores/authStore.js');
      const authStore = useAuthStore();
      authStore.logout();
    } catch {

    } finally {
      isLoggingOut = false;
    }
  }

  return Promise.reject(error);
};

// Перехватчик ответов от сервера
api.interceptors.response.use(
  response => response,
  handleErrorResponse
);

export default api;